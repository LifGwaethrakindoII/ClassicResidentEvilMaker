using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomEditor(typeof(AnimationCommandSender))]
public class AnimationCommandSenderInspector : Editor
{
	private const float MIN = 0.0f; 						/// <summary>Range's Minimum.</summary>
	private const float MAX = 1.0f; 						/// <summary>Range's Maximum.</summary>

	private AnimationCommandSender animationCommandSender; 	/// <summary>Inspector's Target.</summary>

	/// <summary>Sets target property.</summary>
	private void OnEnable()
	{
		animationCommandSender = target as AnimationCommandSender;
		if(animationCommandSender != null) EditorUtility.SetDirty(animationCommandSender);
	}

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{	
		//DrawDefaultInspector();
		VEditorGUILayout.Spaces(2);
		DrawFlagsAndIDFields();
		VEditorGUILayout.Spaces(2);
		DrawPercentageSliders();
		VEditorGUILayout.Spaces(2);
		animationCommandSender.additionalWindow = EditorGUILayout.FloatField("Additional Window", animationCommandSender.additionalWindow);
		VEditorGUILayout.Spaces(3);
		DrawPercentageDistributionsBar();
		VEditorGUILayout.Spaces(3);
		DrawHipotheticalAnimationStateData();
		
		serializedObject.ApplyModifiedProperties();
	}

	/// <summary>Draws fields for  the Animation Flags and Sub-ID.</summary>
	private void DrawFlagsAndIDFields()
	{
		animationCommandSender.flags = (AnimationFlags)EditorGUILayout.EnumPopup("Animation Flags", animationCommandSender.flags);
		animationCommandSender.subID = EditorGUILayout.IntField("Sub-ID", animationCommandSender.subID);
	}

	/// <summary>Draw Percentages' Sliders.</summary>
	private void DrawPercentageSliders()
	{
		EditorGUILayout.LabelField("Animation Stages' Distribution:");

		animationCommandSender.startupPercentage.value = EditorGUILayout.Slider("Startup's Percentage", animationCommandSender.startupPercentage.value, MIN, MAX);
		animationCommandSender.activePercentage.value = EditorGUILayout.Slider("Active's Percentage", animationCommandSender.activePercentage.value, MIN, MAX);
		animationCommandSender.recoveryPercentage.value = EditorGUILayout.Slider("Recovery's Percentage", animationCommandSender.recoveryPercentage.value, MIN, MAX);

		VMath.ToDependableNumberSet(MIN, MAX, animationCommandSender.startupPercentage, animationCommandSender.activePercentage, animationCommandSender.recoveryPercentage);
	}

	/// <summary>Draws Percentages' Distributions' Bar.</summary>
	private void DrawPercentageDistributionsBar()
	{
		EditorGUILayout.LabelField("Animation Stages' Percentage Distributions:");

		Rect rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
		float width = rect.width;
		float startup = animationCommandSender.startupPercentage;
		float dActiveStartup = animationCommandSender.activePercentage - startup;
		float dRecoveryActive = animationCommandSender.recoveryPercentage - animationCommandSender.activePercentage;
		float residue = 1.0f - animationCommandSender.recoveryPercentage.value;
		Color color = GUI.color;

		rect.width = width * startup;
		GUI.color = Color.magenta;
		EditorGUI.ProgressBar(rect, 1.0f, "Startup");
		rect.x += rect.width;
		rect.width = width * dActiveStartup;
		GUI.color = Color.yellow;
		EditorGUI.ProgressBar(rect, 1.0f, "Active");
		rect.x += rect.width;
		rect.width = width * dRecoveryActive;
		GUI.color = Color.cyan;
		EditorGUI.ProgressBar(rect, 1.0f, "Recovery");
		rect.x += rect.width;
		rect.width = width * residue;
		GUI.color = Color.gray;
		EditorGUI.ProgressBar(rect, 1.0f, "Undefined");
		GUI.color = color;
	}

	/// <summary>Hipothetical AnimationState's Data.</summary>
	private void DrawHipotheticalAnimationStateData()
	{
		if(animationCommandSender.toggleAnimationStateData = EditorGUILayout.BeginFoldoutHeaderGroup(animationCommandSender.toggleAnimationStateData, "Debug w/ Hipothetical AnimationState's Data"))
		{
			VEditorGUILayout.Spaces(1);
			animationCommandSender.clip = EditorGUILayout.ObjectField("Clip", animationCommandSender.clip, typeof(AnimationClip), false) as AnimationClip;

			if(animationCommandSender.clip == null) return;

			animationCommandSender.speed = EditorGUILayout.FloatField("Speed", animationCommandSender.speed);

			float frameRate = animationCommandSender.clip.frameRate;
			float duration = (animationCommandSender.clip.length  / animationCommandSender.speed);
			int frameDuration = (int)(duration * frameRate);
			int additionalWindowFrames = (int)(animationCommandSender.additionalWindow * frameRate);

			EditorGUILayout.LabelField("Duration (unscaled by Speed): ", animationCommandSender.clip.length.ToString());
			EditorGUILayout.LabelField("Duration (scaled by Speed): ", duration.ToString());
			VEditorGUILayout.Spaces(2);
			EditorGUILayout.LabelField("Seconds' Conversion to Frame Data:");
			EditorGUILayout.LabelField("Frame Rate: ", frameRate.ToString());
			EditorGUILayout.LabelField("Duration in Frames: ", frameDuration.ToString());
			EditorGUILayout.LabelField("Window in Frames: ", additionalWindowFrames.ToString());

			int minFrameRate = 0;
			int startupFrames = (int)(duration * animationCommandSender.startupPercentage * frameRate);
			int activeFrames = (int)(duration * animationCommandSender.activePercentage * frameRate);
			int recoveryFrames = (int)(duration * animationCommandSender.recoveryPercentage * frameRate);
			
			VEditorGUILayout.Spaces(2);
			EditorGUILayout.LabelField("Animation Stages' Ranges:");
			EditorGUILayout.LabelField("Startup: ", minFrameRate + " - " + startupFrames);
			minFrameRate = startupFrames;
			EditorGUILayout.LabelField("Active: ", minFrameRate + " - " + activeFrames);
			minFrameRate = activeFrames;
			EditorGUILayout.LabelField("Recovery: ", minFrameRate + " - " + recoveryFrames);
		}

		VEditorGUILayout.Spaces(2);
		EditorGUILayout.EndFoldoutHeaderGroup();
	}
}
}