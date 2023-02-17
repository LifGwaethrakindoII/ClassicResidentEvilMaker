using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomEditor(typeof(FiniteStateAudioClip))]
public class FiniteStateAudioClipInspector : Editor
{
	public const float MIN = 0.0f; 						/// <summary>Range's Mininum.</summary>
	public const float MAX = 1.0f; 						/// <summary>Range's Maxinum.</summary>

	private FiniteStateAudioClip finiteStateAudioClip; 	/// <summary>Inspector's Target.</summary>
	private SerializedProperty rangesProperty;

	/// <summary>Sets target property.</summary>
	private void OnEnable()
	{
		finiteStateAudioClip = target as FiniteStateAudioClip;
		rangesProperty = serializedObject.FindProperty("_statesRanges");
	}

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{	
		DrawDefaultInspector();
		DrawSliders();

		serializedObject.ApplyModifiedProperties();
	}

	/// <summary>Draws States' Ranges' Sliders.</summary>
	private void DrawSliders()
	{
		AudioClip clip = finiteStateAudioClip.clip;

		if(clip == null) return;
		if(finiteStateAudioClip.statesRanges == null) finiteStateAudioClip.statesRanges = new FloatWrapper[1];

		FloatWrapper[] ranges = finiteStateAudioClip.statesRanges;
		float length = clip.length;

		VEditorGUILayout.Spaces(2);
		EditorGUILayout.LabelField("Clip's Length: " + length.ToString("0.00") + " Seconds.");
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("States Ranges' Distribution:");

		VEditorGUILayout.ShowSerializedPropertySizeConfiguration(ref rangesProperty, "States' ");

		if(ranges != null) for(int i = 0; i < ranges.Length; i++)
		{
			ranges[i].value = EditorGUILayout.Slider("State " + i, ranges[i].value, 0.0f, length);
		}

		VMath.ToDependableNumberSet(0.0f, length, finiteStateAudioClip.statesRanges);
	}

	/*private void B0ss()
	{
		AudioClip clip = finiteStateAudioClip.clip;

		if(clip == null) return;

		if(GUILayout.Button("X")) AudioUtility.PlayClip(clip);
	}*/
}
}