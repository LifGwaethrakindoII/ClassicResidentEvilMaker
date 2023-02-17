using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public struct ProbabilityRange
{
	public FloatRange range; 		/// <summary>Probability's Range.</summary>
	private float _repetitions; 	/// <summary>Number of times this range has been randomly chosen.</summary>
	private float _scale; 			/// <summary>Alteration scale dependant on the times this range has been randomly chosen.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets repetitions property.</summary>
	public float repetitions
	{
		get { return _repetitions; }
		set { _repetitions = value; }
	}

	/// <summary>Gets and Sets scale property.</summary>
	public float scale
	{
		get { return _scale; }
		set { _scale = value; }
	}

	/// <summary>Gets and Sets min property.</summary>
	public float min
	{
		get { return range.Min(); }
		set { range.min = Mathf.Max(value, 0.0f); }
	}

	/// <summary>Gets and Sets max property.</summary>
	public float max
	{
		get { return range.Max(); }
		set { range.max = Mathf.Min(value, 1.0f); }
	}
#endregion

	/// <summary>ProbabilityRange's Constructor.</summary>
	/// <param name="_range">Probability's Range.</param>
	public ProbabilityRange(FloatRange _range) : this()
	{
		range = _range;
		Reset();
	}

	/// <summary>ProbabilityRange's Constructor.</summary>
	/// <param name="_range">Probability's Range.</param>
	public ProbabilityRange(float min, float max) : this(new FloatRange(min, max)) { /*...*/ }

	/// <summary>Resets ProbabilityRange.</summary>
	public void Reset()
	{
		repetitions = 0.0f;
		scale = 1.0f;
	}

	/// <returns>String representing this Probability Range.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("Probability Range = { Range: ");
		builder.Append(range.ToString());
		builder.Append(", Repetitions: ");
		builder.Append(repetitions.ToString());
		builder.Append(", Scale: ");
		builder.Append(scale.ToString());
		builder.Append(", Range of Probability Corrected to: ");
		builder.Append(range.GetLength() * scale);
		builder.Append(" }");

		return builder.ToString();
	}
}
}