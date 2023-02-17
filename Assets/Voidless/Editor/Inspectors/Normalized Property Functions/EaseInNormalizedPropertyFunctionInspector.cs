using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CanEditMultipleObjects]
[CustomEditor(typeof(EaseInNormalizedPropertyFunction))]
public class EaseInNormalizedPropertyFunctionInspector : NormalizedPropertyFunctionInspector
{
	private EaseInNormalizedPropertyFunction exponentialNormalizedPropertyFunction; 	/// <summary>Inspector's Target.</summary>

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{	
		base.OnInspectorGUI();
	}
}
}