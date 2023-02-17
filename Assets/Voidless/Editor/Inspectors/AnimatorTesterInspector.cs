using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomEditor(typeof(AnimatorTester))]
public class AnimatorTesterInspector : Editor
{
	private AnimatorTester animatorTester; 			/// <summary>Inspector's Target.</summary>
	private List<AnimatorParameter> parameters; 	/// <summary>Parameter's Reference.</summary>

	/// <summary>Sets target property.</summary>
	private void OnEnable()
	{
		animatorTester = target as AnimatorTester;
	}

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{	
		int index = 0;
		parameters = animatorTester.parameters;
		//DrawDefaultInspector();
		animatorTester.animator = EditorGUILayout.ObjectField("Animator: ", animatorTester.animator, typeof(Animator), true) as Animator;
		VEditorGUILayout.ShowListSizeConfiguration(ref parameters);
		animatorTester.parameters = parameters;

		EditorGUILayout.Space();

		for(int i = 0; i < animatorTester.parameters.Count; i++)
		{
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Parameter " + i + ": ");
			AnimatorParameter parameter = animatorTester.parameters[i];
			
			parameter.key.tag = EditorGUILayout.TextField("Key: ", parameter.key.tag);
			parameter.type = (AnimatorParameterType)EditorGUILayout.EnumPopup("Parameter Type: ", parameter.type);

			switch(parameter.type)
			{
				case AnimatorParameterType.Bool:
				parameter.boolValue = EditorGUILayout.Toggle("Value: ", parameter.boolValue);
				break;

				case AnimatorParameterType.Int:
				parameter.intValue = EditorGUILayout.IntField("Value: ", parameter.intValue);
				break;

				case AnimatorParameterType.Float:
				parameter.floatValue = EditorGUILayout.FloatField("Value: ", parameter.floatValue);
				break;

				case AnimatorParameterType.BlendTreeFloat:
				parameter.floatValue = EditorGUILayout.Slider("Value: ", parameter.floatValue, -1.0f, 1.0f);
				break;
			}

			animatorTester.parameters[i] = parameter;
		}

		serializedObject.ApplyModifiedProperties();
	}
}
}