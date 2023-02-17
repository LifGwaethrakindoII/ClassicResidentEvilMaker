using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class TransformDeltaCalculator : MonoBehaviour
{
	[SerializeField] private TransformProperties _detectionType; 	/// <summary>Properties that this bodyu will update.</summary>
	[SerializeField] private Distance _magnitudeToChangePosition; 	/// <summary>Minimum magnitude between the velocity to register a change.</summary>
	[SerializeField] private Distance _magnitudeToChangeRotation; 	/// <summary>Minimum magnitude to register a change in rotation.</summary>
	//[SerializeField] private Distance _magnitudeToChangeScale; 		/// <summary>Minimum magnitude to register a change in scale.</summary>
	private Vector3 _velocity; 										/// <summary>TransformDeltaCalculator's Velocity.</summary>
	private Vector3 _angularVelocity; 								/// <summary>TransformDeltaCalculator's Angular Velocity.</summary>
	private Vector3 _lastPosition; 									/// <summary>TransformDeltaCalculator's Last Position.</summary>
	private Vector3 _lastEulerRotation; 							/// <summary>TransformDeltaCalculator's Last Euler Rotation.</summary>
	private Vector3 _deltaPosition; 								/// <summary>Position's Delta.</summary>
	private Vector3 _accumulatedDeltaPosition; 						/// <summary>Accumulated Position's Delta.</summary>
	private Vector3 _deltaRotation; 								/// <summary>Rotation's Delta.</summary>
	private Vector3 _accumulatedDeltaRotation; 						/// <summary>Accumulated Rotation's Delta.</summary>

#if UNITY_EDITOR
	[Space(5f)]
	[Header("Debug Options:")]
	[SerializeField] private bool debug; 							/// <summary>Debug Velocities?.</summary>
	[SerializeField] private Color positionRayColor; 				/// <summary>Position Ray's Color.</summary>
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _rotationColorAlpha; 			/// <summary>Rotation Color's Alpha.</summary>
	[SerializeField] private float _gizmosRadius; 					/// <summary>Gizmos' Radius.</summary>
	[SerializeField] private float rayDuration; 					/// <summary>Ray's Duration when debugging.</summary>

	/// <summary>Gets rotationColorAlpha property.</summary>
	public float rotationColorAlpha { get { return _rotationColorAlpha; } }

	/// <summary>Gets gizmosRadius property.</summary>
	public float gizmosRadius { get { return _gizmosRadius; } }
#endif

#region Getters/Setters:
	/// <summary>Gets and Sets detectionType property.</summary>
	public TransformProperties detectionType
	{
		get { return _detectionType; }
		set { _detectionType = value; }
	}

	/// <summary>Gets and Sets magnitudeToChangePosition property.</summary>
	public Distance magnitudeToChangePosition
	{
		get { return _magnitudeToChangePosition; }
		set { _magnitudeToChangePosition = value * value; }
	}

	/// <summary>Gets and Sets magnitudeToChangeRotation property.</summary>
	public Distance magnitudeToChangeRotation
	{
		get { return _magnitudeToChangeRotation; }
		set { _magnitudeToChangeRotation = value; }
	}

	/*/// <summary>Gets and Sets magnitudeToChangeScale property.</summary>
	public Distance magnitudeToChangeScale
	{
		get { return _magnitudeToChangeScale; }
		set { _magnitudeToChangeScale = value; }
	}*/

	/// <summary>Gets and Sets velocity property.</summary>
	public Vector3 velocity
	{
		get { return _velocity; }
		private set { _velocity = value; }
	}

	/// <summary>Gets and Sets angularVelocity property.</summary>
	public Vector3 angularVelocity
	{
		get { return _angularVelocity; }
		set { _angularVelocity = value; }
	}

	/// <summary>Gets and Sets lastPosition property.</summary>
	public Vector3 lastPosition
	{
		get { return _lastPosition; }
		private set { _lastPosition = value; }
	}

	/// <summary>Gets and Sets lastEulerRotation property.</summary>
	public Vector3 lastEulerRotation
	{
		get { return _lastEulerRotation; }
		private set { _lastEulerRotation = value; }
	}

	/// <summary>Gets and Sets deltaPosition property.</summary>
	public Vector3 deltaPosition
	{
		get { return _deltaPosition; }
		private set { _deltaPosition = value; }
	}

	/// <summary>Gets and Sets accumulatedDeltaPosition property.</summary>
	public Vector3 accumulatedDeltaPosition
	{
		get { return _accumulatedDeltaPosition; }
		private set { _accumulatedDeltaPosition = value; }
	}

	/// <summary>Gets and Sets deltaRotation property.</summary>
	public Vector3 deltaRotation
	{
		get { return _deltaRotation; }
		private set { _deltaRotation = value; }
	}

	/// <summary>Gets and Sets accumulatedDeltaRotation property.</summary>
	public Vector3 accumulatedDeltaRotation
	{
		get { return _accumulatedDeltaRotation; }
		private set { _accumulatedDeltaRotation = value; }
	}
#endregion

	/// <summary>Resets TransformDeltaCalculator's Data.</summary>
	private void Reset()
	{
		lastPosition = transform.localPosition;
		lastEulerRotation = transform.localRotation.eulerAngles;
		accumulatedDeltaPosition = Vector3.zero;
		accumulatedDeltaRotation = Vector3.zero;
	}

	/// <summary>Callback invoked when TransformDeltaCalculator's instance is disabled.</summary>
	private void OnDisable()
	{
		Reset();
	}

	/// <summary>TransformDeltaCalculator's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		Reset();
	}
	/// <summary>Updates TransformDeltaCalculator's instance at each frame.</summary>
	private void LateUpdate()
	{
		if(detectionType.HasFlag(TransformProperties.Position)) UpdateVelocity();
		if(detectionType.HasFlag(TransformProperties.Rotation)) UpdateAngularVelocity();
#if UNITY_EDITOR
		if(debug) DebugTransformDeltaCalculator();
#endif
	}

	/// <summary>Updates Velocity's Data.</summary>
	private void UpdateVelocity()
	{
		deltaPosition = (transform.localPosition - lastPosition);
		if(deltaPosition.sqrMagnitude >= magnitudeToChangePosition * Time.deltaTime)
		accumulatedDeltaPosition += deltaPosition;
		else accumulatedDeltaPosition = Vector3.zero;
		lastPosition = transform.localPosition;
		velocity = deltaPosition * Application.targetFrameRate;
	}

	/// <summary>Updates Angular velocity's Data.</summary>
	private void UpdateAngularVelocity()
	{
		deltaRotation = (transform.localRotation.eulerAngles - lastEulerRotation);
		if(velocity.sqrMagnitude >= magnitudeToChangeRotation * Time.deltaTime)
		accumulatedDeltaRotation = deltaRotation;
		else accumulatedDeltaRotation = Vector3.zero;
		lastEulerRotation = transform.localRotation.eulerAngles;
		angularVelocity = deltaRotation * Application.targetFrameRate;
	}

	/// <summary>Projects Position given this component's velocity.</summary>
	/// <param name="t">Projection's Time.</param>
	public Vector3 ProjectPosition(float t)
	{
		return transform.position + (velocity * t);
	}

#if UNITY_EDITOR
	/// <summary>Debug TransformDeltaCalculator's Velocities [Only in Editor Mode].</summary>
	private void DebugTransformDeltaCalculator()
	{
		if(detectionType.HasFlag(TransformProperties.Position)) Debug.DrawRay(transform.position, deltaPosition, positionRayColor, rayDuration);
	}
#endif
}
}