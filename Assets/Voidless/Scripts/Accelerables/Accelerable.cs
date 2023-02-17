using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public enum DeltaTimeType 														/// <summary>Delta Time's Type.</summary>
{
	DeltaTime, 																	/// <summary>Main Thread's Delta Time.</summary>
	FixedDeltaTime, 															/// <summary>Physics' Thread Delta Time.</summary>
	SmoothDeltaTime 															/// <summary>Smoothed Main Thread's Delta Time.</summary>
}

public enum EquationType 														/// <summary>Equation Type's Enumerator.</summary>
{
	Lineal, 																	/// <summary>Lineal Equation Type.</summary>
	NonLineal 																	/// <summary>Non-Lineal Equation Type.</summary>
}

[Serializable]
public class Accelerable : ISerializationCallbackReceiver
{
	[SerializeField] private DeltaTimeType _deltaTimeCoefficient; 				/// <summary>Delta Time's Coefficient Type.</summary>
	//[Space(5f)]
	//[Header("Speed Attributes:")]
	[SerializeField] private float _minSpeed; 									/// <summary>Minimum speed this Accelerable can have.</summary>
	[SerializeField] private float _maxSpeed; 									/// <summary>Maximim speed this Accelerable can have.</summary>
	//[Space(5f)]
	//[Header("Acceleration Times:")]
	[SerializeField] private float _accelerationDuration; 						/// <summary>Time it takes from speed to reach maxSpeed.</summary>
	[SerializeField] private float _decelerationDuration; 						/// <summary>Time it takes from speed to reach minSpeed.</summary>
	//[Space(5f)]
	//[Header("Equation's Data:")]
	[SerializeField] private EquationType _accelerationEquationType; 			/// <summary>Acceleration's Equation Type.</summary>
	[SerializeField] private EquationType _decelerationEquationType; 			/// <summary>Deceleration's Equation Type.</summary>
	[SerializeField] private NormalizedPropertyFunction _accelerationFunction; 	/// <summary>Acceleration's Function.</summary>
	[SerializeField] private NormalizedPropertyFunction _decelerationFunction; 	/// <summary>Deceleration's Function.</summary>
	private float _speed; 														/// <summary>Accelerable's current speed.</summary>
	private float _linealSpeed; 												/// <summary>Speed if the acceleration was constant.</summary>
	private float _inverseAccelerationDuration; 								/// <summary>Acceleration time multiplier to avoid division operation [1/t].</summary>
	private float _inverseDecelerationDuration; 								/// <summary>Deceleration time multiplier to avoid division operation [1/t].</summary>
	private float _inverseSpeedNormal; 											/// <summary>Speed's Normal.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets deltaTimeCoefficient property.</summary>
	public DeltaTimeType deltaTimeCoefficient
	{
		get { return _deltaTimeCoefficient; }
		set { _deltaTimeCoefficient = value; }
	}

	/// <summary>Gets and Sets speed property.</summary>
	public float speed
	{
		get { return _speed; }
		set { _speed = value; }
	}

	/// <summary>Gets and Sets linealSpeed property.</summary>
	public float linealSpeed
	{
		get { return _linealSpeed; }
		set { _linealSpeed = value; }
	}

	/// <summary>Gets and Sets minSpeed property.</summary>
	public float minSpeed
	{
		get { return _minSpeed; }
		set
		{
			_minSpeed = value;
			UpdateInverseSpeedNormal();
		}
	}

	/// <summary>Gets and Sets maxSpeed property.</summary>
	public float maxSpeed
	{
		get { return _maxSpeed; }
		set
		{
			_maxSpeed = value;
			UpdateInverseSpeedNormal();
		}
	}

	/// <summary>Gets and Sets accelerationDuration property.</summary>
	public float accelerationDuration
	{
		get { return _accelerationDuration; }
		set
		{			
			_accelerationDuration = value;
			UpdateInverseAccelerationDuration();
		}
	}

	/// <summary>Gets and Sets decelerationDuration property.</summary>
	public float decelerationDuration
	{
		get { return _decelerationDuration; }
		set
		{
			_decelerationDuration = value;
			UpdateInverseDecelerationDuration();
		}
	}

	/// <summary>Gets and Sets inverseAccelerationDuration property.</summary>
	public float inverseAccelerationDuration
	{
		get { return _inverseAccelerationDuration; }
		private set { _inverseAccelerationDuration = value; }
	}

	/// <summary>Gets and Sets inverseDecelerationDuration property.</summary>
	public float inverseDecelerationDuration
	{
		get { return _inverseDecelerationDuration; }
		private set { _inverseDecelerationDuration = value; }
	}

	/// <summary>Gets and Sets inverseSpeedNormal property.</summary>
	public float inverseSpeedNormal
	{
		get { return _inverseSpeedNormal; }
		private set { _inverseSpeedNormal = value; }
	}

	/// <summary>Gets and Sets accelerationEquationType property.</summary>
	public EquationType accelerationEquationType
	{
		get { return _accelerationEquationType; }
		set { _accelerationEquationType = value; }
	}

	/// <summary>Gets and Sets decelerationEquationType property.</summary>
	public EquationType decelerationEquationType
	{
		get { return _decelerationEquationType; }
		set { _decelerationEquationType = value; }
	}

	/// <summary>Gets and Sets accelerationFunction property.</summary>
	public NormalizedPropertyFunction accelerationFunction
	{
		get { return _accelerationFunction; }
		set { _accelerationFunction = value; }
	}

	/// <summary>Gets and Sets decelerationFunction property.</summary>
	public NormalizedPropertyFunction decelerationFunction
	{
		get { return _decelerationFunction; }
		set { _decelerationFunction = value; }
	}
#endregion

#region ConstructorOverloads:
	/// <summary>Default Accelerable constructor.</summary>
	/// <param name="_speed">Initial speed.</param>
	/// <param name="_minSpeed">Minimum speed.</param>
	/// <param name="_maxSpeed">Maximum speed.</param>
	/// <param name="_accelerationDuration">Time it takes from initial speed to maximum speed [in seconds].</param>
	/// <param name="_decelerationDuration">Time it takes from maximum speed to initial speed [in seconds].</param>
	public Accelerable(float _speed, float _minSpeed, float _maxSpeed, float _accelerationDuration, float _decelerationDuration)
	{
		speed = _speed;
		linealSpeed = speed;
		minSpeed = _minSpeed;
		maxSpeed = _maxSpeed;
		accelerationDuration = _accelerationDuration;
		decelerationDuration = _decelerationDuration;
	}

	/// <summary>Overload Accelerable constructor.</summary>
	/// <param name="_minSpeed">Minimum speed. It also determines the initial speed value</param>
	/// <param name="_maxSpeed">Maximum speed.</param>
	/// <param name="_accelerationDuration">Time it takes from initial speed to maximum speed [in seconds].</param>
	/// <param name="_decelerationDuration">Time it takes from maximum speed to initial speed [in seconds].</param>
	public Accelerable(float _minSpeed, float _maxSpeed, float _accelerationDuration, float _decelerationDuration) : this(_minSpeed, _minSpeed, _maxSpeed, _accelerationDuration, _decelerationDuration)
	{
		//...
	}

	/// <summary>Overload Accelerable constructor.</summary>
	/// <param name="_minSpeed">Minimum speed.</param>
	/// <param name="_maxSpeed">Maximum speed.</param>
	/// <param name="_duration">Time for both acceleration and deacceleration.</param>
	public Accelerable(float _minSpeed, float _maxSpeed, float _duration) : this(_minSpeed, _minSpeed, _maxSpeed, _duration, _duration)
	{
		//...
	}
#endregion

	/// <summary>Implement this method to receive a callback before Unity serializes your object.</summary>
	public void OnBeforeSerialize()
    {
    	if(!Application.isPlaying)
    	{
    		speed = minSpeed;
    		linealSpeed = speed;
	    	UpdateInverseAccelerationDuration();
	    	UpdateInverseDecelerationDuration();
	    	UpdateInverseSpeedNormal();
    	}	
    }

    /// <summary>Implement this method to receive a callback after Unity deserializes your object.</summary>
    public void OnAfterDeserialize()
    {
		speed = minSpeed;
		linealSpeed = speed;
    	UpdateInverseAccelerationDuration();
    	UpdateInverseDecelerationDuration();
    	UpdateInverseSpeedNormal();	
    }

#region AccelerateOverloads:
	/// <summary>Accelerates speed until it reaches its max speed.</summary>
	/// <returns>Speed value accelerated.</returns>
	public float Accelerate()
	{
		linealSpeed = Mathf.Min(linealSpeed += (GetAverageAcceleration() * GetDeltaTime()), maxSpeed);
		if(AccelerateLinearly()) speed = linealSpeed;
		else speed = Mathf.LerpUnclamped(minSpeed, maxSpeed, accelerationFunction.Evaluate(GetAccelerationProgress()));
		return speed;
	}

	/// <summary>[Overload] Accelerates speed until it reaches its max speed. Call this function if you don't accelerate on consecutive frames.</summary>
	/// <param name="_deltaTime">Delta time reference for acceleration.</param>
	/// <returns>Speed value accelerated.</returns>
	public float Accelerate(float _deltaTime)
	{
		linealSpeed = Mathf.Min(linealSpeed += (GetAverageAcceleration() * _deltaTime), maxSpeed);
		if(AccelerateLinearly()) speed = linealSpeed;
		else speed = Mathf.LerpUnclamped(minSpeed, maxSpeed, accelerationFunction.Evaluate(GetAccelerationProgress()));

		return speed;
	}
#endregion

#region DecelerateOverloads:
	/// <summary>Decelerates speed until it reaches its minimum speed.</summary>
	/// <returns>Speed value deaccelerated.</returns>
	public float Decelerate()
	{
		linealSpeed = Mathf.Max(linealSpeed -= (GetAverageDeceleration() * GetDeltaTime()), minSpeed);
		if(DecelerateLinearly()) speed = linealSpeed;
		else speed = Mathf.LerpUnclamped(maxSpeed, minSpeed, decelerationFunction.Evaluate(GetDecelerationProgress()));
		return speed;
	}

	/// <summary>Decelerates speed until it reaches its minimum speed. Call this function if you don't deaccelerate on consecutive frames.</summary>
	/// <param name="_deltaTime">Delta time reference for deacceleration.</param>
	/// <returns>Speed value deaccelerated.</returns>
	public float Decelerate(float _deltaTime)
	{
		linealSpeed = Mathf.Max(linealSpeed -= (GetAverageDeceleration() * _deltaTime), minSpeed);
		if(DecelerateLinearly()) speed = linealSpeed;
		else speed = Mathf.LerpUnclamped(maxSpeed, minSpeed, decelerationFunction.Evaluate(GetDecelerationProgress()));
		return speed;
	}
#endregion

#region DecelerateToZeroOverloads:
	/// <summary>Decelerates speed until it reaches 0.</summary>
	/// <returns>Speed value deaccelerated.</returns>
	public float DecelerateToZero()
	{
		linealSpeed = Mathf.Max(linealSpeed -= (GetAverageDeceleration() * GetDeltaTime()), 0f);
		if(DecelerateLinearly()) speed = linealSpeed;
		else speed = Mathf.LerpUnclamped(maxSpeed, minSpeed, decelerationFunction.Evaluate(GetDecelerationToZeroProgress()));
		return speed;
	}

	/// <summary>Decelerates speed until it reaches 0.</summary>
	/// <param name="_deltaTime">Delta time reference for deacceleration.</param>
	/// <returns>Speed value deaccelerated.</returns>
	public float DecelerateToZero(float _deltaTime)
	{
		linealSpeed = Mathf.Max(linealSpeed -= (GetAverageDeceleration() * _deltaTime), 0f);
		if(DecelerateLinearly()) speed = linealSpeed;
		else speed = Mathf.LerpUnclamped(maxSpeed, minSpeed, decelerationFunction.Evaluate(GetDecelerationToZeroProgress()));
		return speed;
	}
#endregion

	/// <summary>Gets average acceleration to maximum speed on one frame.</summary>
	/// <returns>Averaga acceleration of maximum speed.</returns>
	public float GetAverageAcceleration()
	{
		return ((maxSpeed - minSpeed) * inverseAccelerationDuration);
	}

	/// <summary>Gets average acceleration to minimum speed on one frame.</summary>
	/// <returns>Averaga acceleration of minimum speed.</returns>
	public float GetAverageDeceleration()
	{
		return ((maxSpeed - minSpeed) * inverseDecelerationDuration);
	}

	/// <summary>Gets average Acceleration's progress.</summary>
	/// <returns>Acceleration's progress.</returns>
	public float GetAccelerationProgress()
	{
		return Mathf.Min((linealSpeed - minSpeed) * inverseSpeedNormal, 1.0f);
	}

	/// <summary>Gets average Deceleration's progress.</summary>
	/// <returns>Deceleration's progress.</returns>
	public float GetDecelerationProgress()
	{
		return Mathf.Max(1.0f - ((linealSpeed - minSpeed) * inverseSpeedNormal), 0.0f);
	}

	/// <summary>Gets Deceleration to zero progress.</summary>
	/// <returns>Deceleration's progress.</returns>
	public float GetDecelerationToZeroProgress()
	{
		return Mathf.Max(1.0f - (linealSpeed / maxSpeed), 0.0f);
	}

	/// <summary>Resets speed to its minimum speed set.</summary>
	public void Reset()
	{
		speed = minSpeed;
		linealSpeed = speed;
	}

	/// <summary>Updates Inverse Acceleration's Duration.</summary>
	private void UpdateInverseAccelerationDuration()
	{
		inverseAccelerationDuration = (1.0f / accelerationDuration);
	}

	/// <summary>Updates Inverse Deceleration's Duration.</summary>
	private void UpdateInverseDecelerationDuration()
	{
		inverseDecelerationDuration = (1.0f / decelerationDuration);
	}

	/// <summary>Updates Inverse Speed's Normal.</summary>
	private void UpdateInverseSpeedNormal()
	{
		inverseSpeedNormal = (1.0f / (maxSpeed - minSpeed));
	}

	/// <returns>Gets Delta Time coefficient from Enum's value.</returns>
	private float GetDeltaTime()
	{
		switch(deltaTimeCoefficient)
		{
			case DeltaTimeType.DeltaTime: return Time.deltaTime;
			case DeltaTimeType.FixedDeltaTime: return Time.fixedDeltaTime;
			case DeltaTimeType.SmoothDeltaTime: return Time.smoothDeltaTime;
			default: return Time.deltaTime;
		}
	}

	/// <returns>True if he conditions to accelerate are met.</returns>
	private bool AccelerateLinearly() { return (accelerationEquationType == EquationType.Lineal || accelerationFunction == null); }

	/// <returns>True if he conditions to decelerate are met.</returns>
	private bool DecelerateLinearly() { return (decelerationEquationType == EquationType.Lineal || decelerationFunction == null); }

	/// <returns>String representing this Accelerable.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.AppendLine("Accelerable:");
		builder.Append("Delta Time's Coefficient: ");
		builder.AppendLine(deltaTimeCoefficient.ToString());
		builder.Append("Speed: { Current: ");
		builder.Append(speed.ToString());
		builder.Append(", Min. Speed: ");
		builder.Append(minSpeed.ToString());
		builder.Append(", Max. Speed: ");
		builder.Append(maxSpeed.ToString());
		builder.AppendLine(" }");
		builder.Append("Acceleration Duration: ");
		builder.AppendLine(accelerationDuration.ToString());
		builder.Append("Acceleration's Multiplicative Inverse: ");
		builder.AppendLine(inverseAccelerationDuration.ToString());
		builder.Append("Deceleration Duration: ");
		builder.AppendLine(decelerationDuration.ToString());
		builder.Append("Deceleration's Multiplicative Inverse: ");
		builder.AppendLine(inverseDecelerationDuration.ToString());
		builder.Append("Acceleration's Equation Type: ");
		builder.AppendLine(accelerationEquationType.ToString());
		if(accelerationEquationType == EquationType.NonLineal)
		{
			builder.Append("Acceleration's Function of Type: ");
			builder.AppendLine(accelerationFunction != null ? accelerationFunction.GetType().Name : "Missing function's reference.");
		}
		builder.Append("Deceleration's Equation Type: ");
		builder.AppendLine(decelerationEquationType.ToString());
		if(decelerationEquationType == EquationType.NonLineal)
		{
			builder.Append("Deceleration's Function of Type: ");
			builder.AppendLine(decelerationFunction != null ? decelerationFunction.GetType().Name : "Missing function's reference.");
		}
		builder.Append("Acceleration's Progress: ");
		builder.AppendLine(GetAccelerationProgress().ToString());
		builder.Append("Deceleration's Progress: ");
		builder.AppendLine(GetDecelerationProgress().ToString());

		return builder.ToString();
	}
}
}