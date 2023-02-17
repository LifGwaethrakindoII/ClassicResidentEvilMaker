using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomEditor(typeof(WaypointsContainer))]
public class WaypointsContainerInspector : Editor
{
	private const float LENGTH_SCALE_RAY = 50.0f; 	/// <summary>Scale's Ray Length.</summary>

	private WaypointsContainer waypointsContainer; 	/// <summary>Inspector's Target.</summary>

	/// <summary>Sets target property.</summary>
	private void OnEnable()
	{
		waypointsContainer = target as WaypointsContainer;
		EditorUtility.SetDirty(waypointsContainer);
	}

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{
		string handlesButtonText = (waypointsContainer.showHandles ? "Hide" : "Show") + "Handles";
		string relativeButtonText = (waypointsContainer.relativeToTransform ? "Not-Relative" : "Relative") + " to this Transform";
		
		if(GUILayout.Button(handlesButtonText)) waypointsContainer.showHandles = !waypointsContainer.showHandles;
		if(GUILayout.Button(relativeButtonText)) waypointsContainer.ToggleAddParentToWaypoints();
		
		DrawDefaultInspector();

        serializedObject.ApplyModifiedProperties();
    }

	/// <summary>Enables the Editor to handle an event in the Scene view.</summary>
	private void OnSceneGUI()
	{
		if(!waypointsContainer.showHandles) return;

		TransformData waypoint = default(TransformData);
		Vector3 newPosition = Vector3.zero;
		Vector3 newScale = Vector3.zero;
		Quaternion newRotation = Quaternion.identity;

		for(int i = 0; i < waypointsContainer.waypoints.Count; i++)
		{
			try
			{
				waypoint = waypointsContainer.waypoints[i];

				switch(Tools.current)
				{
					case Tool.Move:
					EditorGUI.BeginChangeCheck();
					newPosition = Handles.PositionHandle(waypoint.localPosition, waypoint.localRotation);
					if(EditorGUI.EndChangeCheck()) waypoint.position = newPosition;
					break;

					case Tool.Rotate:
					EditorGUI.BeginChangeCheck();
					newRotation = Handles.RotationHandle(waypoint.localRotation, waypoint.localPosition);
					if(EditorGUI.EndChangeCheck()) waypoint.rotation = newRotation;
					break;

					case Tool.Scale:
					EditorGUI.BeginChangeCheck();
					newScale = Handles.ScaleHandle(waypoint.localScale, waypoint.localPosition, waypoint.localRotation, waypoint.localScale.GetAverage() * LENGTH_SCALE_RAY);
					if(EditorGUI.EndChangeCheck()) waypoint.scale = newScale;
					break;
				}

				waypointsContainer.waypoints[i] = waypoint;
			}
			catch(Exception exception)
			{
				Debug.LogWarning("[TransformDataDrawer] Catched Exception when trying to reload SceneGUI: " + exception.Message);
			}
		}
	}
}
}