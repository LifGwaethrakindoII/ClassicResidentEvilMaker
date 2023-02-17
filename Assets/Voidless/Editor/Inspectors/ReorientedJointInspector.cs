using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomEditor(typeof(ReorientedJoint))]
public class ReorientedJointInspector : Editor
{
	private ReorientedJoint reorientedJoint; 	/// <summary>Inspector's Target.</summary>

	/// <summary>Sets target property.</summary>
	private void OnEnable()
	{
		reorientedJoint = target as ReorientedJoint;
	}

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{	
		DrawDefaultInspector();
		if(reorientedJoint.reference != null && GUILayout.Button("Update Reference Offset"))
		reorientedJoint.UpdateRotationOffset();
		serializedObject.ApplyModifiedProperties();
	}
}
}