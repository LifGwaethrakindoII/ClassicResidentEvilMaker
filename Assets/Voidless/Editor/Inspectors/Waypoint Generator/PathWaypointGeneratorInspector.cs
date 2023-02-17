using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomEditor(typeof(PathWaypointGenerator))]
public class PathWaypointGeneratorInspector : BaseWaypointGeneratorInspector<PathWaypointGenerator, PathWaypoint>
{
	protected static readonly string LABEL_START_TANGENT = "Start Tangent: ";
	protected static readonly string LABEL_CURVE_TYPE = "Curve Type: ";
	protected static readonly string LABEL_MOVEMENT_SPEED = "Movement Speed: ";
	protected static readonly string LABEL_ROTATION_SPEED = "Rotation Speed: ";
#if UNITY_EDITOR
	protected static readonly string LABEL_PATH_LINE_COLOR = "Path Line's Color: ";
	protected static readonly string LABEL_TIME_SPLIT = "Time Split: ";
#endif

	/// <summary>Debugs waypoints with their respective debuggable properties.</summary>
	protected override void DrawDebugWaypoints()
	{
		for(int i = 0; i < waypointGenerator.waypoints.Count; i++)
		{
			if(waypointGenerator.waypoints[i] != null)
			{ /// Show Waypoints' positions:
				EditorGUILayout.LabelField(LABEL_WAYPOINT + i + LABEL_COLON);

				if(waypointGenerator.positionsDebugOption != DebugOption.None)
				{
					DrawDebugWaypointPosition(i);
				}
				if(waypointGenerator.rotationsDebugOption != DebugOption.None)
				{
					DrawDebugWaypointRotation(i);
				}
				EditorGUILayout.Space();
				DrawDebugWaypointSpeeds(i);
				DrawDebugCurveType(i);

				EditorGUILayout.Space();
			}
			else EditorGUILayout.LabelField(LABEL_WAYPOINTS + LABEL_COLON, LABEL_NO_WAYPOINTS);	
		}
	}

	protected virtual void DrawDebugWaypointSpeeds(int _index)
	{
		waypointGenerator.waypoints[_index].movementSpeed = EditorGUILayout.FloatField(LABEL_MOVEMENT_SPEED, waypointGenerator.waypoints[_index].movementSpeed);
		waypointGenerator.waypoints[_index].rotationSpeed = EditorGUILayout.FloatField(LABEL_MOVEMENT_SPEED, waypointGenerator.waypoints[_index].rotationSpeed);
	}

	protected virtual void DrawDebugCurveType(int _index)
	{
		waypointGenerator.waypoints[_index].curveType = (CurveType)EditorGUILayout.EnumPopup(LABEL_CURVE_TYPE, waypointGenerator.waypoints[_index].curveType);
		EditorGUILayout.Space();
		
		switch(waypointGenerator.waypoints[_index].curveType)
		{
			case CurveType.Linear:
			break;

			case CurveType.Cuadratic:
			DrawDebugStartTangent(_index);
			VEditorGUILayout.Spaces(2);
			break;

			case CurveType.Cubic:
			DrawDebugStartTangent(_index);
			DrawDebugEndTangent(_index);
			VEditorGUILayout.Spaces(2);
			break;
		}
	}

	protected virtual void DrawDebugStartTangent(int _index)
	{
		Vector3 tangent = EditorGUILayout.Vector3Field(LABEL_START_TANGENT, waypointGenerator.waypoints[_index].startTangent);
		waypointGenerator.waypoints[_index].startTangent = tangent;
	}

	protected virtual void DrawDebugEndTangent(int _index)
	{
		Vector3 tangent = EditorGUILayout.Vector3Field(LABEL_START_TANGENT, waypointGenerator.waypoints[_index].endTangent);
		waypointGenerator.waypoints[_index].endTangent = tangent;
	}

#if UNITY_EDITOR
	/// <summary>Debugs GizmosWaypoints' properties for the inspector to modify.</summary>
	protected override void DrawDebugGizmosWaypointProperties()
	{
		base.DrawDebugGizmosWaypointProperties();
		waypointGenerator.pathLineColor = EditorGUILayout.ColorField(LABEL_PATH_LINE_COLOR, waypointGenerator.pathLineColor);
		waypointGenerator.timeSplit = EditorGUILayout.IntField(LABEL_TIME_SPLIT, waypointGenerator.timeSplit);
		VEditorGUILayout.Spaces(2);
	}
#endif
}
}