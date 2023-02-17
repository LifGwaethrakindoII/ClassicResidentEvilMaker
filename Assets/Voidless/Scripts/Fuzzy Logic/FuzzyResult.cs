using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public struct FuzzyResult
{
	public string name; 			/// <summary>Result's Name.</summary>
	public int value; 				/// <summary>Result's Value.</summary>
	public float membershipDegree; 	/// <summary>Result's MembershipDegree.</summary>

	/// <summary>FuzzyResult constructor.</summary>
	/// <param name="_name">Name of the value associated with the input.</param>
	/// <param name="_value">Value associated with the input.</param>
	/// <param name="_mermbershipDegree">Membership's Degree.</param>
	public FuzzyResult(string _name, int _value, float _membershipDegree)
	{
		name = _name;
		value = _value;
		membershipDegree = _membershipDegree;
	}

	/// <summary>FuzzyResult constructor.</summary>
	/// <param name="_value">Value associated with the input.</param>
	/// <param name="_mermbershipDegree">Membership's Degree.</param>
	public FuzzyResult(int _value, float _membershipDegree) : this(string.Empty, _value, _membershipDegree)  { /*...*/ }

	/// <returns>String representing this Fuzzy Subset's result.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("Fuzzy Result = {");
		builder.Append("Name: ");
		builder.Append(name);
		builder.Append(", ");
		builder.Append("Value: ");
		builder.Append(value.ToString());
		builder.Append(", ");
		builder.Append("Membership Degree: ");
		builder.Append(membershipDegree.ToString());
		builder.Append(" }");

		return builder.ToString();
	}
}
}