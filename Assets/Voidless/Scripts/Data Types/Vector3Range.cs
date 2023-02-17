using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

namespace Voidless
{
[Serializable]
public struct Vector3Range// : IRange<Vector3>, ISerializationCallbackReceiver
{
	/*[SerializeField] private Vector3 _min; 	/// <summary>Range's Minimum value.</summary>
	[SerializeField] private Vector3 _max; 	/// <summary>Range's maximum value.</summary>

	/// <summary>Gets and Sets min property.</summary>
	public Vector3 min
	{
		get { return _min; }
		set { _min = value; }
	}

	/// <summary>Gets and Sets max property.</summary>
	public Vector3 max
	{
		get { return _max; }
		set { _max = value; }
	}

	/// <summary>Implicit Vector3 value to Vector3Range operator.</summary>
	public static implicit operator Vector3Range(Vector3 _value) { return new Vector3Range(_value); }

	/// <summary>Vector3Range's Constructor.</summary>
	/// <param name="_min">Minimum's value.</param>
	/// <param name="_max">Maximum's value.</param>
	public Vector3Range(Vector3 _min, Vector3 _max) : this()
	{
		min = _min;
		max = _max;
	}

	/// <summary>Vector3Range's constructor.</summary>
	/// <param name="_value">Same minimum and maximum's value for the Vector3Range.</param>
	public Vector3Range(Vector3 _value) : this(_value, _value) {  }

	/// <returns>Range's Median.</returns>
	public Vector3 GetMedian()
	{
		return (min + (GetLength() * 0.5f));
	}

	/// <returns>Range's Length.</returns>
	public Vector3 GetLength()
	{
		return (max - min);
	}

	/// <returns>Maximum Value.</returns>
	public Vector3 Max()
	{
		return min < max ? max : min;
	}

	/// <returns>Minimum Value.</returns>
	public Vector3 Min()
	{
		return min < max ? min : max;
	}

	/// <returns>Random value between the range [if both min and max are equal, no random calculations will be made].</returns>
	public Vector3 Random()
	{
		return min != max ? new Vector3
		(
			Random.Range(min.x, max.x),
			Random.Range(min.y, max.y),
			Random.Range(min.z, max.z)

		) : min;
	}

	/// <summary>Clamps given value between the Vector3Range.</summary>
	/// <param name="x">Value to clamp.</param>
	/// <returns>Clamped value.</returns>
	public Vector3 Clamp(Vector3 x)
	{
		Vector3 min = Min();
		Vector3 max = Max();

		return x < min ? min : x > max ? max : x;
	}

	/// <summary>Evaluates if given value is inside the range.</summary>
	/// <param name="x">Value to evaluate.</param>
	/// <returns>True if given value is inside the range.</returns>
	public bool ValueInside(Vector3 x)
	{
		return x >= Min() && x <= Max();
	}

	/// <summary>Evaluates if given value is outside the range.</summary>
	/// <param name="x">Value to evaluate.</param>
	/// <returns>True if given value is outside the range.</returns>
	public bool ValueOutside(Vector3 x)
	{
		return x < Min() || x > Max();
	}

	/// <summary>Implement this method to receive a callback before Unity serializes your object.</summary>
	public void OnBeforeSerialize()
    {
    	//min = Min();
    	//max = Max();
    }

    /// <summary>Implement this method to receive a callback after Unity deserializes your object.</summary>
    public void OnAfterDeserialize()
    {
    	
    }

	/// <returns>String representing Range.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("[ ");
		builder.Append("Min: ");
		builder.Append(min.ToString());
		builder.Append(", Max: ");
		builder.Append(max.ToString());
		builder.Append(", Length: ");
		builder.Append(GetLength().ToString());
		builder.Append(", Median: ");
		builder.Append(GetMedian().ToString());
		builder.Append("]");

		return builder.ToString();
	}*/
}
}