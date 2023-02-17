using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[System.Serializable]
public class VTuple<T1, T2>
{
	public T1 Item1; 	/// <summary>Item 1.</summary>
	public T2 Item2; 	/// <summary>Item 2.</summary>

	/// <summary>Parameterless VTuple's Constructor.</summary>
	public VTuple()
	{
		Item1 = default(T1);
		Item2 = default(T2);
	}

	/// <summary>Tuple default constructor.</summary>
	/// <param name="_Item1">Item 1.</param>
	/// <param name="_Item2">Item 2.</param>
	public VTuple(T1 _Item1, T2 _Item2)
	{
		Item1 = _Item1;
		Item2 = _Item2;
	}

	/// <returns>String representign this VTuple.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("VTupple<");
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