using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomEditor(typeof(FOVFrustumSightSensor))]
[CanEditMultipleObjects]
public class FOVFrustumSightSensorInspector : Editor
{
	private FOVFrustumSightSensor FOVfrustum; 	/// <summary>Inspector's Target.</summary>

	/// <summary>Sets target property.</summary>
	private void OnEnable()
	{
		FOVfrustum = target as FOVFrustumSightSensor;
	}

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{	
		DrawDefaultInspector();
		if(FOVfrustum.FOVFrustumData != null)
		{
			EditorGUILayout.Space();
			EditorGUILayout.LabelField(FOVfrustum.FOVFrustumData.ToString(), GUILayout.Height(100));
			if(GUILayout.Button("Update FOV's Frustum")) FOVfrustum.UpdateFOVFrustum();
		}
		serializedObject.ApplyModifiedProperties();	
	}
}
}