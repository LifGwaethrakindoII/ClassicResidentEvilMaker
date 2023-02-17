using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[CreateAssetMenu(menuName = PATH_NORMALIZED_PROPERTY_FUNCTIONS + "/Ease-In")]
public class EaseInNormalizedPropertyFunction : NormalizedPropertyFunction
{
	[SerializeField] private float _exponent; 	/// <summary>Function's Exponent.</summary>

	/// <summary>Gets and Sets exponent property.</summary>
	public float exponent
	{
		get { return _exponent; }
		set { _exponent = value; }
	}

	/// <summary>Evaluates Time t.</summary>
	/// <param name="t">Normalized property to evaluate.</param>
	/// <returns>Time t evaluated.</returns>
	public override float Evaluate(float t)
	{
		return VMath.EaseIn(t, exponent);
	}
}
}