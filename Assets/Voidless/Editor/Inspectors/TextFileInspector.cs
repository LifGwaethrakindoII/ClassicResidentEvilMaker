using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomEditor(typeof(TextFile))]
public class TextFileInspector : Editor
{
	private TextFile textFile; 	/// <summary>Inspector's Target.</summary>

	/// <summary>Sets target property.</summary>
	void OnEnable()
	{
		textFile = target as TextFile;
	}

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{	
		DrawDefaultInspector();
		if(textFile.textAsset != null)
		{
			if(!string.IsNullOrEmpty(textFile.textAssetPath))
			{
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Text Asset's Path: ");
				GUILayout.Label(textFile.textAssetPath);
				EditorGUILayout.EndHorizontal();
			}
			if(GUILayout.Button("Get Text Asset's Path")) textFile.textAssetPath = AssetDatabase.GetAssetPath(textFile.textAsset);
		}
		serializedObject.ApplyModifiedProperties();	
	}
}
}