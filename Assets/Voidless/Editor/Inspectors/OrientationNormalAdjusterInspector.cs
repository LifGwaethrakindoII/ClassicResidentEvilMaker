using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomEditor(typeof(OrientationNormalAdjuster))]
public class OrientationNormalAdjusterInspector : Editor
{
	private OrientationNormalAdjuster orientationNormalAdjuster; 	/// <summary>Inspector's Target.</summary>

	/// <summary>Sets target property.</summary>
	private void OnEnable()
	{
		orientationNormalAdjuster = target as OrientationNormalAdjuster;
		EditorUtility.SetDirty(orientationNormalAdjuster);
	}

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{
		orientationNormalAdjuster.canChangeRelativeNormal = EditorGUILayout.Toggle("Can Change Relative Normal", orientationNormalAdjuster.canChangeRelativeNormal);
		orientationNormalAdjuster.normalToAdjust = (IdentityVector)EditorGUILayout.EnumPopup("Normal To Adjust", orientationNormalAdjuster.normalToAdjust);
		DrawRelativeNormalSettings();
		EditorGUILayout.Space();
		orientationNormalAdjuster.normalLength = EditorGUILayout.FloatField("Normal Length", orientationNormalAdjuster.normalLength);
		
		serializedObject.ApplyModifiedProperties();
	}

	/// <summary>Draws Normal's Settings.</summary>
	private void DrawRelativeNormalSettings()
	{
		string fieldLabel = "Relative Normal";
		int relativeNormalIndex = 0;

		EditorGUILayout.BeginHorizontal();
		switch(orientationNormalAdjuster.normalToAdjust)
		{
			case IdentityVector.Right:
			relativeNormalIndex =
			(int)(RightRelativeIdentityVector)EditorGUILayout.EnumPopup(fieldLabel, (RightRelativeIdentityVector)orientationNormalAdjuster.relativeNormal);
			break;

			case IdentityVector.Up:
			relativeNormalIndex =
			(int)(UpRelativeIdentityVector)EditorGUILayout.EnumPopup(fieldLabel, (UpRelativeIdentityVector)orientationNormalAdjuster.relativeNormal);
			break;

			case IdentityVector.Forward:
			relativeNormalIndex =
			(int)(ForwardRelativeIdentityVector)EditorGUILayout.EnumPopup(fieldLabel, (ForwardRelativeIdentityVector)orientationNormalAdjuster.relativeNormal);
			break;
		}

		orientationNormalAdjuster.relativeNormal = (IdentityVector)relativeNormalIndex;
		
		switch(orientationNormalAdjuster.normalToAdjust)
		{
			case IdentityVector.Right:
			if(orientationNormalAdjuster.relativeNormal == IdentityVector.Up)
			orientationNormalAdjuster.up = EditorGUILayout.Vector3Field(string.Empty, orientationNormalAdjuster.up);
			else if(orientationNormalAdjuster.relativeNormal == IdentityVector.Forward)
			orientationNormalAdjuster.forward = EditorGUILayout.Vector3Field(string.Empty, orientationNormalAdjuster.forward);
			break;

			case IdentityVector.Up:
			if(orientationNormalAdjuster.relativeNormal == IdentityVector.Right)
			orientationNormalAdjuster.right = EditorGUILayout.Vector3Field(string.Empty, orientationNormalAdjuster.right);
			else if(orientationNormalAdjuster.relativeNormal == IdentityVector.Forward)
			orientationNormalAdjuster.forward = EditorGUILayout.Vector3Field(string.Empty, orientationNormalAdjuster.forward);
			break;

			case IdentityVector.Forward:
			if(orientationNormalAdjuster.relativeNormal == IdentityVector.Right)
			orientationNormalAdjuster.right = EditorGUILayout.Vector3Field(string.Empty, orientationNormalAdjuster.right);
			else if(orientationNormalAdjuster.relativeNormal == IdentityVector.Up)
			orientationNormalAdjuster.up = EditorGUILayout.Vector3Field(string.Empty, orientationNormalAdjuster.up);
			break;
		}
		EditorGUILayout.EndHorizontal();
	}
}
}