using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public struct AngleDotProduct : ISerializationCallbackReceiver
{
	public const float EPSILON = 0.01f; 			/// <summary>Default Tolerance's Epsilon.</summary>

	[SerializeField]
	[Range(0.0f, 180.0f)] private float _angle; 	/// <summary>Angle [in degrees].</summary>	
	[SerializeField] private float _dotProduct; 	/// <summary>Angle's Dot Product.</summary>

	/// <summary>Gets and Sets angle property.</summary>
	public float angle
	{
		get { return _angle; }
		set
		{
			_angle = Mathf.Clamp(value, -1.0f, 180.0f);
			dotProduct = Mathf.Cos(_angle * Mathf.Deg2Rad);
		}
	}

	/// <summary>Gets and Sets dotProduct property.</summary>
	public float dotProduct
	{
		get { return _dotProduct; }
		private set { _dotProduct = value; }
	}

	/// <summary>Implicit float to AngleDotProduct.</summary>
	public static implicit operator AngleDotProduct(float _angle) { return new AngleDotProduct(_angle); }

	/// <summary>Implicit AngleDotProduct to float.</summary>
	public static implicit operator float(AngleDotProduct _angleDotProduct) { return _angleDotProduct.dotProduct; }

	/// <summary>AngleDotProduct's Constructor.</summary>
	/// <param name="_angle">Angle.</param>
	public AngleDotProduct(float _angle) : this()
	{
		angle = _angle;
	}

	/// <summary>Implement this method to receive a callback before Unity serializes your object.</summary>
	public void OnBeforeSerialize()
    {
    	angle = angle;
    }

    /// <summary>Implement this method to receive a callback after Unity deserializes your object.</summary>
    public void OnAfterDeserialize()
    {
    	angle = angle;
    }

	/// <summary>Evaluates if Dot Product between 2 vectors is less or equal than this Dot Product.</summary>
	/// <param name="a">Vector A.</param>
	/// <param name="b">Vector B.</param>
	/// <returns>True if Dot Product between 2 vectors is less or equal than this Dot Product.</returns>
	public bool DotProductLEQ(Vector3 a, Vector3 b)
	{
		if(a.sqrMagnitude != 1.0f) a.Normalize();
		if(b.sqrMagnitude != 1.0f) b.Normalize();
	
		return (Vector3.Dot(a, b) <= dotProduct);
	}

	/// <summary>Evaluates if Dot Product between 2 vectors is greater or equal than this Dot Product.</summary>
	/// <param name="a">Vector A.</param>
	/// <param name="b">Vector B.</param>
	/// <returns>True if Dot Product between 2 vectors is greater or equal than this Dot Product.</returns>
	public bool DotProductGEQ(Vector3 a, Vector3 b)
	{
		if(a.sqrMagnitude != 1.0f) a.Normalize();
		if(b.sqrMagnitude != 1.0f) b.Normalize();
	
		return (Vector3.Dot(a, b) >= dotProduct);
	}

	/// <summary>Calculates if the Dot Product between 2 vectors is close by a tolerance.</summary>
	/// <param name="a">Vector A.</param>
	/// <param name="b">Vector B.</param>
	/// <param name="tolerance">Tolerane [Epsilon by default].</param>
	/// <returns>True if dot product is close to the given tolerance.</returns>
	public bool Close(Vector3 a, Vector3 b, float tolerance = EPSILON)
	{
		if(a.sqrMagnitude != 1.0f) a.Normalize();
		if(b.sqrMagnitude != 1.0f) b.Normalize();

		return Mathf.Abs(Vector3.Dot(a, b) - dotProduct) <= tolerance;
	}

	/// <summary>Evaluates if dot product between 2 vectors is within range.</summary>
	/// <param name="a">Vector A.</param>
	/// <param name="b">Vector B.</param>
	/// <returns>True if the dot product between two vectors is within the defined dot product.</returns>
	public bool WithinRange(Vector3 a, Vector3 b)
	{
		if(a.sqrMagnitude != 1.0f) a.Normalize();
		if(b.sqrMagnitude != 1.0f) b.Normalize();

		float dot = Vector3.Dot(a, b);

		if(dotProduct > 0.0f)
		{
			return dot <= dotProduct;

		} else if(dotProduct < 0.0f)
		{
			return dot >= dotProduct;
		}
		else return dot == dotProduct;
	}

	/// <summary>Evaluates if dot product between 2 vectors is below range.</summary>
	/// <param name="a">Vector A.</param>
	/// <param name="b">Vector B.</param>
	/// <returns>True if the dot product between two vectors is below the defined dot product.</returns>
	public bool BelowRange(Vector3 a, Vector3 b)
	{
		if(a.sqrMagnitude != 1.0f) a.Normalize();
		if(b.sqrMagnitude != 1.0f) b.Normalize();

		float dot = Vector3.Dot(a, b);

		if(dotProduct > 0.0f)
		{
			return dot < dotProduct;

		} else if(dotProduct < 0.0f)
		{
			return dot > dotProduct;
		}
		else return dot == dotProduct;
	}

	/// <summary>Evaluates if dot product between 2 vectors is above range.</summary>
	/// <param name="a">Vector A.</param>
	/// <param name="b">Vector B.</param>
	/// <returns>True if the dot product between two vectors is above the defined dot product.</returns>
	public bool AboveRange(Vector3 a, Vector3 b)
	{
		if(a.sqrMagnitude != 1.0f) a.Normalize();
		if(b.sqrMagnitude != 1.0f) b.Normalize();

		float dot = Vector3.Dot(a, b);

		if(dotProduct > 0.0f)
		{
			return dot >= dotProduct;

		} else if(dotProduct < 0.0f)
		{
			return dot <= dotProduct;
		}
		else return dot == dotProduct;
	}

	/// <returns>String representing this structure.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("{ Angle: ");
		builder.Append(angle.ToString("0.00"));
		builder.Append(", Dot Product: ");
		builder.Append(dotProduct.ToString("0.00"));
		builder.Append(" }");

		return builder.ToString();
	}
}
}