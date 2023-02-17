using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public abstract class NormalizedPropertyFunction : ScriptableObject
{
	protected const string PATH_NORMALIZED_PROPERTY_FUNCTIONS = VString.PATH_SCRIPTABLE_OBJECTS + " / Normalized Property Functions"; 	/// <summary>Normalized Property Functions' Base Path.</summary>

	/// <summary>Evaluates Time t.</summary>
	/// <param name="t">Normalized property to evaluate.</param>
	/// <returns>Time t evaluated.</returns>
	public abstract float Evaluate(float t);
}
}