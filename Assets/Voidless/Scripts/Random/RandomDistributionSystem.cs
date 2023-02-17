using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

using Random = UnityEngine.Random;

namespace Voidless
{
[Serializable]
public class RandomDistributionSystem
{
	private const float MIN = 0.0f; 									/// <summary>Range's Minimum Value.</summary>
	private const float MAX = 1.0f; 									/// <summary>Range's Maximum Value.</summary>

	[InfoBox("@ToString()")]
	[SerializeField] private ProbabilityRange[] _probabilityRanges; 	/// <summary>Set of ProbabilityRanges.</summary>

	/// <summary>Gets and Sets probabilityRanges property.</summary>
	public ProbabilityRange[] probabilityRanges
	{
		get { return _probabilityRanges; }
		set { _probabilityRanges = value; }
	}

	/// <summary>RandomDistributionSystem default constructor.</summary>
	/// <param name="_probabilityRanges">Set of ProbabilityRanges.</param>
	public RandomDistributionSystem(ProbabilityRange[] _probabilityRanges)
	{
		probabilityRanges = _probabilityRanges;
	}

	/// <summary>Creates a copy of this instance.</summary>
	/// <param name="_default">Set probability ranges to default? true by default.</param>
	/// <returns>Copy of Random Distribution system.</returns>
	public RandomDistributionSystem Copy(bool _default = true)
	{
		RandomDistributionSystem system = new RandomDistributionSystem(new ProbabilityRange[probabilityRanges.Length]);

		for(int i = 0; i < probabilityRanges.Length; i++)
		{
			system.probabilityRanges[i] = probabilityRanges[i];
		}

		if(_default) system.Reset();

		return system;
	}

	[OnInspectorGUI]
	/// <summary>Redistributes the Probability Ranges' Set.</summary>
	public void Redistribute()
	{
		float min = 0.0f;

		for(int i = 0; i < probabilityRanges.Length; i++)
		{
			ProbabilityRange probabilityRange = probabilityRanges[i];

			probabilityRange.min = Mathf.Clamp(probabilityRange.min, min, MAX);
			probabilityRange.max = Mathf.Clamp(probabilityRange.max, probabilityRange.min, MAX);
			min = probabilityRange.max;

			probabilityRanges[i] = probabilityRange;
		}
	}

	[Button("Reset")]
	/// <summary>Resets Probability Ranges.</summary>
	public void Reset()
	{
		for(int i = 0; i < probabilityRanges.Length; i++)
		{
			probabilityRanges[i].Reset();
		}
	}

	[Button("Test for Errors")]
	/// <summary>Tests system [10,000 consecutive iterations].</summary>
	private void TestForErrors()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("Test Random-Chain (10,000 iterations): { ");

		for(int i = 0; i < 10000; i++)
		{
			builder.Append(GetRandomIndex());
		}

		builder.Append(" }");

		Debug.Log(builder.ToString());
	}

	[Button("Test Random")]
	/// <returns>Random Index.</returns>
	public int GetRandomIndex()
	{
		ProbabilityRange probabilityRange = default(ProbabilityRange);
		int length = probabilityRanges.Length;
		float min = Mathf.Infinity;
		float max = 0.0f;
		float random = 0.0f;

		/// Get the Minimum repetitions' value:
		for(int i = 0; i < length; i++)
		{
			probabilityRange = probabilityRanges[i];
			if(probabilityRange.repetitions < min) min = probabilityRange.repetitions;
		}

		/// Update each ProbabilityRange's scale and modify the Maximum value's reference:
		for(int i = 0; i < length; i++)
		{
			probabilityRange = probabilityRanges[i];
			probabilityRange.scale = (min + 1.0f) / (probabilityRange.repetitions + 1.0f);
			max += (probabilityRange.range.GetLength() * probabilityRange.scale);
			probabilityRanges[i] = probabilityRange;
		}

		random = Random.Range(0.0f, max);
		min = 0.0f;

		/// Evaluate if the random value is within any of the ProbabilityRanges:
		for(int i = 0; i < length; i++)
		{
			probabilityRange = probabilityRanges[i];
			float scale = probabilityRange.scale;
			max = (min + (probabilityRange.range.GetLength() * scale));

			if(random >= min && random <= max)
			{
				probabilityRange.repetitions++;
				probabilityRanges[i] = probabilityRange;
				return i;
			}
		
			min = max;
		}

		Debug.LogError("[RandomDistributionSystem] Bad Calculations. Code will return -1 (The code shouldn't get to this point...)");
		return -1;
	}

	/// <returns>String representing set of Probability Ranges.</returns>
	public override string ToString()
	{
		if(probabilityRanges == null) return string.Empty;

		StringBuilder builder = new StringBuilder();
		int i = 0;
		int length = probabilityRanges.Length;

		if(length == 0) return string.Empty;

		builder.AppendLine("Set of Probability Ranges: ");
		builder.AppendLine();

		foreach(ProbabilityRange probabilityRange in probabilityRanges)
		{
			builder.Append(i.ToString());
			builder.Append(": ");
			builder.Append(probabilityRange.ToString());
			
			if(i < length) builder.AppendLine();
			i++;
		}

		return builder.ToString();
	}
}
}