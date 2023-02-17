using UnityEngine;
using System;
using System.Text;

namespace Voidless
{
[Serializable]
public struct NormalizedVector2
{
	private static readonly string LABEL_OUT_OF_RANGE = "The range is between 0 and 1."; 	/// <summary>IndexOutOfRangeException's additional message.</summary>
	private const float MIN_CONSTRAINT = -1.0f; 											/// <summary>NormalizedVector2's minimum value.</summary>
	private const float MAX_CONSTRAINT = 1.0f; 												/// <summary>NormalizedVector2's minimum value.</summary>

	[SerializeField] [Range(MIN_CONSTRAINT, MAX_CONSTRAINT)] private float _x; 				/// <summary>Vector2 X's Component.</summary>
	[SerializeField] [Range(MIN_CONSTRAINT, MAX_CONSTRAINT)] private float _y; 				/// <summary>Vector2 Y's Component.</summary

	/// <summary>Gets and Sets x property.</summary>
	public float x
	{
		get { return _x; }
		set { _x = Mathf.Clamp(value, MIN_CONSTRAINT, MAX_CONSTRAINT); }
	}

	/// <summary>Gets and Sets y property.</summary>
	public float y
	{
		get { return _y; }
		set { _y = Mathf.Clamp(value, MIN_CONSTRAINT, MAX_CONSTRAINT); }
	}
	

	/// <summary>Gets and Sets Normalized Vector's component by given index [from 0 to 2].</summary>
	public float this[int _index]
	{
		get
		{
			switch(_index)
			{
				case 0: return x;
				case 1: return y;
				default: throw new IndexOutOfRangeException(LABEL_OUT_OF_RANGE);
			}
		}
		set
		{
			switch(_index)
			{
				case 0: x = value; break;
				case 1: y = value; break;
				default: throw new IndexOutOfRangeException(LABEL_OUT_OF_RANGE);
			}
		}
	}

	/// <summary>Implicit NormalizedVector2 to Vector2 operator.</summary>
	public static implicit operator Vector2(NormalizedVector2 _vector) { return new Vector2(_vector.x, _vector.y); }

	/// <summary>Implicit NormalizedVector2 to Vector3 operator.</summary>
	public static implicit operator Vector3(NormalizedVector2 _vector) { return new Vector3(_vector.x, _vector.y, 0.0f); }

	/// <summary>Implicit Vector2 to NormalizedVector2 operator.</summary>
	public static implicit operator NormalizedVector2(Vector2 _vector) { return new NormalizedVector2(_vector.x, _vector.y); }

	/// <summary>Implicit NormalizedVector2 plus Vector2 value math operator.</summary>
	public static Vector2 operator + (NormalizedVector2 _n, Vector2 _vector) { return new Vector2(_n.x, _n.y) + _vector; }

	/// <summary>Implicit NormalizedVector2 minus Vector2 value math operator.</summary>
	public static Vector2 operator - (NormalizedVector2 _n, Vector2 _vector) { return new Vector2(_n.x, _n.y) - _vector; }

	/// <summary>Implicit NormalizedVector2 times Vector2 value math operator.</summary>
	public static Vector2 operator * (NormalizedVector2 _n, Vector2 _vector) { return new Vector2(_n.x * _vector.x, _n.y * _vector.y); }

	/// <summary>Implicit NormalizedVector2 not equals Vector2 value bool operator.</summary>
	public static bool operator != (NormalizedVector2 _n, Vector2 _vector) { return (new Vector2(_n.x, _n.y) != _vector); }

	/// <summary>Implicit NormalizedVector2 equals Vector2 value bool operator.</summary>
	public static bool operator == (NormalizedVector2 _n, Vector2 _vector) { return (new Vector2(_n.x, _n.y) == _vector); }

	/// <summary>Implicit NormalizedVector2 times float value math operator.</summary>
	public static Vector2 operator * (NormalizedVector2 _n, float _number) { return new Vector2(_n.x, _n.y) * _number; }

	/// <summary>Implicit NormalizedVector2 divided by float value math operator.</summary>
	public static Vector2 operator / (NormalizedVector2 _n, float _number) { return new Vector2(_n.x, _n.y) / _number; }

	/// <summary>Gets normalized property.</summary>
	public NormalizedVector2 normalized { get { return new Vector2(x, y).normalized; } }

	/// <summary>NormalizedVector2's constructor.</summary>
	/// <param name="_x">X's component.</param>
	/// <param name="_y">Y's component.</param>
	public NormalizedVector2(float _x, float _y) : this()
	{
		x = _x;
		y = _y;
	}

	/// <returns>Normalized Vector's Magnitude.</returns>
	public float Magnitude()
	{
		return Mathf.Sqrt((x * x) + (y * y));
	}

	/// <summary>Normalizes Vector.</summary>
	public void Normalize()
	{
		float inverseMagnitude = 1.0f / Magnitude();
		x *= inverseMagnitude;
		y *= inverseMagnitude;
	}

	/// <summary>Determines whether two object instances are equal.</summary>
	/// <param name="_object">Object to compare this object against.</param>
	/// <returns>True if both objects are considered equal, false otherwise.</returns>
	public override bool Equals(object _object)
	{
		return (_object.GetType() == typeof(NormalizedVector2) && this == (NormalizedVector2)_object);
	}

	/// <returns>Default hash function.</returns>
	public override int GetHashCode()
	{
		return ((x.GetHashCode() + y.GetHashCode()) / 2);
	}

	/// <summary>Converts Vector's data to a string.</summary>
	/// <returns>String representing NormalizedVector2.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("{ ");
		builder.Append(x.ToString());
		builder.Append(", ");
		builder.Append(y.ToString());
		builder.Append(" }");

		return builder.ToString();
	}
}	
}