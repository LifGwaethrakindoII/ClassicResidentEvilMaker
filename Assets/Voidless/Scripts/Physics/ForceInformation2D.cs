using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public struct ForceInformation2D
{
	public Vector2 force; 			/// <summary>Force's Vector.</summary>
	public float duration; 			/// <summary>Force's Duration.</summary>
	public ForceMode forceMode; 	/// <summary>Force's Mode.</summary>

	/// <summary>ForceInformation2D's Constructor.</summary>
	/// <param name="_force">Force's Vector.</param>
	/// <param name="_duration">Force's Duration.</param>
	/// <param name="_forceMode">Force's Mode [ForceMode.Force by default].</param>
	public ForceInformation2D(Vector2 _force, float _duration, ForceMode _forceMode = ForceMode.Force)
	{
		force = _force;
		duration = _duration;
		forceMode = _forceMode;	
	}

	/// <returns>String representing this ForceInformation2D structure.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("[ Force: ");
		builder.Append(force.ToString());
		builder.Append(", Duration: ");
		builder.Append(duration.ToString());
		builder.Append(", Force Mode: ");
		builder.Append(forceMode.ToString());
		builder.Append(" ]");

		return builder.ToString();
	}
}
}