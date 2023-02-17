using System;
using System.Text;
using System.Collections.Generic;

namespace Voidless
{
[Serializable]
public struct Range<T> where T : struct, IComparable<T>
{
	public T min; 	/// <summary>Range's Minimum value.</summary>
	public T max; 	/// <summary>Range's maximum value.</summary>

	/// <summary>Implicit T value to Range operator.</summary>
	public static implicit operator Range<T>(T _value) { return new Range<T>(_value); }

	/// <summary>Range's Constructor.</summary>
	/// <param name="_min">Minimum's value.</param>
	/// <param name="_max">Maximum's value.</param>
	public Range(T _min, T _max)
	{
		min = _min;
		max = _max;
	}

	/// <summary>Range's constructor.</summary>
	/// <param name="_value">Same minimum and maximum's value for the Range.</param>
	public Range(T _value) : this(_value, _value) { /*...*/ }

	/// <summary>Returns the value who gets closer to the Infinite.</summary>
	/// <returns>Maximum value.</returns>
	public T MaxValue()
	{
		switch(min.CompareTo(max))
		{
			case -1:
			return max;

			case 0:
			case 1:
			return min;
		}

		return min;
	}

	/// <summary>Returns the value who gets closer to the Negative Infinite.</summary>
	/// <returns>Minimum value.</returns>
	public T MinValue()
	{
		switch(min.CompareTo(max))
		{
			case -1:
			case 0:
			return min;
			
			case 1:
			return max;
		}

		return min;
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
		builder.Append("]");

		return builder.ToString();
	}
}
}