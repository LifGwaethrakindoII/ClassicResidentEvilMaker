using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[System.Serializable]
public struct ValueVTuple<T1, T2> where T1 : struct where T2 : struct
{
	public T1 Item1; 	/// <summary>Item 1.</summary>
	public T2 Item2; 	/// <summary>Item 2.</summary>

	/// <summary>Implicit ValueVTuple to VTuple converter.</summary>
	public static implicit operator VTuple<T1, T2>(ValueVTuple<T1, T2> _tuple) { return new VTuple<T1, T2>(_tuple.Item1, _tuple.Item2); }

	/// <summary>VTuple default constructor.</summary>
	/// <param name="_Item1">Item 1.</param>
	/// <param name="_Item2">Item 2.</param>
	public ValueVTuple(T1 _Item1, T2 _Item2)
	{
		Item1 = _Item1;
		Item2 = _Item2;
	}

	/// <summary>Creates a VTuple reference from this ValueVTuple.</summary>
	/// <param name="_Item1">Item 1.</param>
	/// <param name="_Item2">Item 2.</param>
	public VTuple<T1, T2> ToVTuple()
	{
		return new Voidless.VTuple<T1, T2>(Item1, Item2);
	}

	/// <returns>String representign this VTuple.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("ValueVTupple<");
		builder.Append(typeof(T1).Name);
		builder.Append(", ");
		builder.Append(typeof(T2).Name);
		builder.Append(">: { ");
		builder.Append(Item1.ToString());
		builder.Append(", ");
		builder.Append(Item2.ToString());
		builder.Append(" }");

		return builder.ToString();
	}
}
}