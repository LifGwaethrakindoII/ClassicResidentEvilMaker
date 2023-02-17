using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public struct PerceptronData
{
	[HideInInspector] public string ID; 	/// <summary>Node's ID.</summary>
	[HideInInspector] public float value; 	/// <summary>Node's Value.</summary>

	/// <summary>PerceptronData' constructor.</summary>
	/// <param name="_ID">Node's ID.</param>
	/// <param name="_value">Node's Value.</param>
	public PerceptronData(string _ID, float _value)
	{
		ID = _ID;
		value = _value;
	}

	/// <summary>PerceptronData' constructor.</summary>
	/// <param name="_ID">Node's ID.</param>
	public PerceptronData(string _ID) : this (_ID, 0.0f) { /**/ }

	/// <summary>PerceptronData' constructor.</summary>
	/// <param name="_value">Node's Value.</param>
	public PerceptronData(float _value) : this ("", _value) { /**/ }

	/// <returns>String representing this PerceptronData.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		if(!string.IsNullOrEmpty(ID))
		{
			builder.Append("{ ");
			builder.Append(ID);
			builder.Append(": ");
			builder.Append(value.ToString());
			builder.Append(" }");
		}
		else builder.Append(value.ToString());

		return builder.ToString();
	}
}
}