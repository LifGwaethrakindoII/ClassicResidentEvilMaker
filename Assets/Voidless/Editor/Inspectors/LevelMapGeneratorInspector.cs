using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomEditor(typeof(LevelMapGenerator))]
public class LevelMapGeneratorInspector : Editor
{
	private LevelMapGenerator levelMapGenerator; 	/// <summary>Inspector's Target.</summary>

	/// <summary>Sets target property.</summary>
	void OnEnable()
	{
		levelMapGenerator = target as LevelMapGenerator;
	}

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{	
		DrawDefaultInspector();

		if(GUILayout.Button("Generate Map"))
		{
			levelMapGenerator.GenerateMap();
		}
		if(GUILayout.Button("Reset"))
		{
			levelMapGenerator.Reset();
		}
	}
}
}