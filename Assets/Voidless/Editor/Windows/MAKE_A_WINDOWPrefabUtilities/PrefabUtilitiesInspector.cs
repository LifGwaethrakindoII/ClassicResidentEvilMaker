using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomEditor(typeof(PrefabUtilities))]
public class PrefabUtilitiesInspector : Editor
{
	private PrefabUtilities prefabUtilities; 	/// <summary>Inspector's Target.</summary>

	/// <summary>Sets target property.</summary>
	void OnEnable()
	{
		prefabUtilities = target as PrefabUtilities;
	}

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{	
		DrawDefaultInspector();
		if(GUILayout.Button("Substitute with Prefab.")) prefabUtilities.SubstituteGameObjectsForPrefab();
	}
}
}