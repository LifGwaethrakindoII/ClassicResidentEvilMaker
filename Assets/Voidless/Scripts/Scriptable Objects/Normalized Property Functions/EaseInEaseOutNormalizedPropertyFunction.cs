using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[CreateAssetMenu(menuName = PATH_NORMALIZED_PROPERTY_FUNCTIONS + "/Ease-In Ease-Out")]
public class EaseInEaseOutNormalizedPropertyFunction : NormalizedPropertyFunction
{
	private const float LIMIT_SIN = 180.0f * Mathf.Deg2Rad;

	[SerializeField] private float _easeInExponent; 	/// <summary>Description.</summary>
	[SerializeField] private float _easeOutExponent; 	/// <summary>Ease-Out's Exponent.</summary>

	/// <summary>Gets and Sets easeInExponent property.</summary>
	public float easeInExponent
	{
		get { return _easeInExponent; }
		set { _easeInExponent = value; }
	}

	/// <summary>Gets and Sets easeOutExponent property.</summary>
	public float easeOutExponent
	{
		get { return _easeOutExponent; }
		set { _easeOutExponent = value; }
	}

	/// <summary>Evaluates Time t.</summary>
	/// <param name="t">Normalized property to evaluate.</param>
	/// <returns>Time t evaluated.</returns>
	public override float Evaluate(float t)
	{
		return VMath.EaseInEaseOut(t, easeInExponent, _easeOutExponent);
	}
}
}