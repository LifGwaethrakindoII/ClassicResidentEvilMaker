using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CanEditMultipleObjects]
[CustomEditor(typeof(EaseOutNormalizedPropertyFunction))]
public class EaseOutNormalizedPropertyFunctionInspector : NormalizedPropertyFunctionInspector
{
	private EaseOutNormalizedPropertyFunction exponentialNormalizedPropertyFunction; 	/// <summary>Inspector's Target.</summary>

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{	
		base.OnInspectorGUI();
	}
}
}