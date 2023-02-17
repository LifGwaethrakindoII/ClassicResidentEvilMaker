using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless.EditorNodes
{
[CustomEditor(typeof(LevelController))]
public class LevelControllerInspector : Editor
{
	private const string OPEN_LEVEL_EDITOR = "Open Level Flow Editor";
	private const string UPDATE_LEVELFLOW = "Update Level Flow Data";
	//private LevelController levelController;

	/// <summary>Sets target property.</summary>
	void OnEnable()
	{
		//levelController = target as LevelController;
	}

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{
		/// Do your stuff here:
		
		DrawDefaultInspector();
		/*if(levelController.levelFlowData != null)
		{
			if(GUILayout.Button(OPEN_LEVEL_EDITOR))
			{
				if((levelController.levelFlowData is LevelFlowNodeEditorData))
				{
					LevelFlowNodeEditorWindow.OpenNodeEditorWindowWithData(levelController.levelFlowData as LevelFlowNodeEditorData);
				}
				else
				Debug.LogError("[TestLevelControllerInspector] ScriptableObject provided is not of type LevelFlowNodeEditorData");
			}
			if(GUILayout.Button(UPDATE_LEVELFLOW))
			{
				if((levelController.levelFlowData is LevelFlowNodeEditorData))
				{
					LevelFlowNodeEditorData data = levelController.levelFlowData as LevelFlowNodeEditorData;
					levelController.levelFlowRoot = data.CreateLevelFlowTree();

					//levelController.levelFlowRoot.Tick();
				}
				else
				Debug.LogError("[TestLevelControllerInspector] ScriptableObject provided is not of type LevelFlowNodeEditorData");
			}
		}*/
	}
}
}