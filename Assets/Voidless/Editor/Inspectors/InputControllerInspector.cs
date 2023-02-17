using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomEditor(typeof(InputController))]
public class InputControllerInspector : Editor
{
	private InputController inputController; 	/// <summary>Inspector's Target.</summary>

	/// <summary>Sets target property.</summary>
	void OnEnable()
	{
		inputController = target as InputController;
		EditorUtility.SetDirty(inputController);
	}

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{	
		DrawDefaultInspector();
		VEditorGUILayout.Spaces(2);
		if(GUILayout.Button("Load Default File")) LoadDefaultInputMappingFile();

		serializedObject.ApplyModifiedProperties();
	}

	/// <summary>Loads InputMapping's File from default path stored.</summary>
	private void LoadDefaultInputMappingFile()
	{
		string path = VEditorData.LoadString(VString.EDITOR_DATA_KEY_MAPPING_PATH);
		TextAsset file = AssetDatabase.LoadAssetAtPath(path, typeof(TextAsset)) as TextAsset;

		if(file != null)
		{
			InputMapping mapping = VJSONSerializer.DeserializeFromJSONFromTextAsset<InputMapping>(file);
			if(mapping != null) inputController.inputMappingFile = file;
			else EditorUtility.DisplayDialog
			(
				"InputMapping not retrieved from TextAsset's Data!",
				"Could not retrieve any InputMapping from TextAsset's data. Returning null instead.",
				"Ok"
			);
		}
		else EditorUtility.DisplayDialog
		(
			"TextAsset not retrieved from Asset on given path!",
			"There was no TextAsset retrieved on path (" + path + "). Returning null instead.",
			"Ok"
		);
	}
}
}