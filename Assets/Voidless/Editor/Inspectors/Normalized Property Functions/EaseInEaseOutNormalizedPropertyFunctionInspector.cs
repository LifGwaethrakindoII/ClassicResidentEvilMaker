using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CanEditMultipleObjects]
[CustomEditor(typeof(EaseInEaseOutNormalizedPropertyFunction))]
public class EaseInEaseOutNormalizedPropertyFunctionInspector : NormalizedPropertyFunctionInspector
{
	private EaseInEaseOutNormalizedPropertyFunction easeIneaseOutNormalizedPropertyFunction; 	/// <summary>Inspector's Target.</summary>

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{	
		base.OnInspectorGUI();
	}
}
}