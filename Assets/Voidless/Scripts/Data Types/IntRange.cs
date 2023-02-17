using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public struct IntRange : IRange<int>, ISerializationCallbackReceiver
{
	[SerializeField] private int _min; 	/// <summary>Range's Minimum value.</summary>
	[SerializeField] private int _max; 	/// <summary>Range's maximum value.</summary>

	/// <summary>Gets and Sets min property.</summary>
	public int min
	{
		get { return _min; }
		set { _min = value; }
	}

	/// <summary>Gets and Sets max property.</summary>
	public int max
	{
		get { return _max; }
		set { _max = value; }
	}

	/// <summary>Implicit int value to IntRange operator.</summary>
	public static implicit operator IntRange(int _value) { return new IntRange(_value); }

	/// <summary>IntRange's Constructor.</summary>
	/// <param name="_min">Minimum's value.</param>
	/// <param name="_max">Maximum's value.</param>
	public IntRange(int _min, int _max) : this()
	{
		min = _min;
		max = _max;
	}

	/// <summary>IntRange's constructor.</summary>
	/// <param name="_value">Same minimum and maximum's value for the IntRange.</param>
	public IntRange(int _value) : this(_value, _value) { /*...*/ }

	/// <param name="t">Normalized Time t.</param>
	/// <returns>Interpolation between the Minimum and Maximum Value.</returns>
	public int Lerp(float t)
	{
		int m = Min();
		int M = Max();
		
		return m + (int)((M - m) * t);
	}

	/// <returns>Range's Median.</returns>
	public int GetMedian()
	{
		return (min + (GetLength() / 2));
	}

	/// <returns>Range's Length.</returns>
	public int GetLength()
	{
		return (max - min + 1);
	}

	/// <returns>Maximum Value.</returns>
	public int Max()
	{
		return min < max ? max : min;
	}

	/// <returns>Minimum Value.</returns>
	public int Min()
	{
		return min < max ? min : max;
	}

	/// <summary>Gets a randopm value between the range [if both min and max are equal, no random calculations will be made].</summary>
	/// <param name="_inclusive">Is the random inclusive? true by default.</param>
	/// <returns>Random value between the range.</returns>
	public int Random(bool _inclusive = true)
	{
		return min != max ? UnityEngine.Random.Range(Min(), Max() + (_inclusive ? 1 : 0)) : min;
	}

	/// <summary>Clamps given value between the FloatRange.</summary>
	/// <param name="x">Value to clamp.</param>
	/// <returns>Clamped value.</returns>
	public int Clamp(int x)
	{
		int min = Min();
		int max = Max();

		return x < min ? min : x > max ? max : x;
	}

	/// <summary>Evaluates if given value is inside the range.</summary>
	/// <param name="x">Value to evaluate.</param>
	/// <returns>True if given value is inside the range.</returns>
	public bool ValueInside(int x)
	{
		return x >= Min() && x <= Max();
	}

	/// <summary>Evaluates if given value is outside the range.</summary>
	/// <param name="x">Value to evaluate.</param>
	/// <returns>True if given value is outside the range.</returns>
	public bool ValueOutside(int x)
	{
		return x < Min() || x > Max();
	}

	/// <summary>Implement this method to receive a callback before Unity serializes your object.</summary>
	public void OnBeforeSerialize()
    {
    	/*min = Min();
    	max = Max();*/
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
	}
}
}