using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomEditor(typeof(FOVFrustumColliderData))]
public class FOVFrustumColliderDataInspector : Editor
{
	private FOVFrustumColliderData FOVfrustumColliderData; 	/// <summary>Inspector's Target.</summary>

	/// <summary>Sets target property.</summary>
	private void OnEnable()
	{
		FOVfrustumColliderData = target as FOVFrustumColliderData;
		EditorUtility.SetDirty(FOVfrustumColliderData);
	}

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{	
		//DrawDefaultInspector();
		EditorGUILayout.LabelField(FOVfrustumColliderData.ToString(), GUILayout.Height(100));
		if(GUILayout.Button("Configure"))
		{
			FOVFrustumMeshCreationWindow.CreateFOVFrustumMeshCreationWindow();
			FOVFrustumMeshCreationWindow.LoadFOVData(FOVfrustumColliderData);
		}
		serializedObject.ApplyModifiedProperties();
	}
}
}