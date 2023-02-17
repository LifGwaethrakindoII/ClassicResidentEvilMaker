using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
/*[CustomEditor(typeof(Transform))]
public class TransformInspector : Editor
{
	private Transform transform; 	/// <summary>Inspector's Target.</summary>

	/// <summary>Sets target property.</summary>
	void OnEnable()
	{
		transform = target as Transform;
	}

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{	
		//DrawDefaultInspector();
		transform.position = EditorGUILayout.Vector3Field("Position: ", transform.position);
		transform.eulerAngles = EditorGUILayout.Vector3Field("Rotation: ", transform.eulerAngles);
		transform.localScale = EditorGUILayout.Vector3Field("Scale: ", transform.localScale);
		VEditorGUILayout.Spaces(3);
		if(GUILayout.Button("Reset Position.")) transform.position = Vector3.zero;
		if(GUILayout.Button("Reset Rotation.")) transform.eulerAngles = Vector3.zero;
		if(GUILayout.Button("Reset Scale.")) transform.localScale = Vector3.one;
		EditorGUILayout.Space();
		if(GUILayout.Button("Reset Transform."))
		{
			transform.position = Vector3.zero;
			transform.eulerAngles = Vector3.zero;
			transform.localScale = Vector3.one;
		}
	}
}*/
}