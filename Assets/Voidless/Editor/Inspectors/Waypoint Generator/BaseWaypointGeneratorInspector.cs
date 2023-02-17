using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
//[CustomEditor(typeof(T))]
public abstract class BaseWaypointGeneratorInspector<T, W> : Editor where T : BaseWaypointGenerator<W> where W : Waypoint
{
	protected static readonly string LABEL_SHOW_WAYPOINTS = "Show Waypoints List? "; 					/// <summary>Show Waypoints List label.</summary>
	protected static readonly string LABEL_WAYPOINT_PREFAB = "Waypoint Prefab: "; 						/// <summary>Waypoint prefab label.</summary>
	protected static readonly string LABEL_QUANTITY = "Quantity: "; 									/// <summary>Quantity label.</summary>
	protected static readonly string LABEL_PRIOR_QUANTITY = "Prior Quantity: "; 						/// <summary>Prior Quantity label.</summary>
	//protected static readonly string LABEL_OVERRIDE_PROPERTIES = "Override Properties: "; 				/// <summary>Override properties label.</summary>
	protected static readonly string LABEL_DRAW_TYPE_OVERRIDE = "Draw Type: "; 							/// <summary>Draw Type override label.</summary>
	protected static readonly string LABEL_COLOR_OVERRIDE = "Color: "; 									/// <summary>Color override label.</summary>
	protected static readonly string LABEL_DIMENSIONS_OVERRIDE = "Dimensions: "; 						/// <summary>Dimensions override label.</summary>
	protected static readonly string LABEL_UPDATE_GENERATOR = "Update Generator.";  					/// <summary>Update Generator button label.</summary>
	protected static readonly string LABEL_POSITIONS_DEBUG_OPTION = "Positions Debug Mode: "; 			/// <summary>Waypoints' positions debug option label.</summary>
	protected static readonly string LABEL_ROTATIONS_DEBUG_OPTION = "Rotations Debug Mode: "; 			/// <summary>Waypoints' rotations debug option label.</summary>
	protected static readonly string LABEL_WAYPOINT = "Waypoint "; 										/// <summary>Waypoint label.</summary>
	protected static readonly string LABEL_WAYPOINTS = "Waypoints: "; 									/// <summary>Waypoints label.</summary>
	protected static readonly string LABEL_NO_WAYPOINTS = "There are not waypoints!"; 					/// <summary>Label letting know that there are not waypoints.</summary>
	protected static readonly string LABEL_POSITIONS = "Positions: "; 									/// <summary>Waypoints' positions label.</summary>
	protected static readonly string LABEL_ROTATIONS = "Rotations: "; 									/// <summary>Waypoints' rotations label.</summary>
	protected static readonly string LABEL_POSITION = "Position "; 										/// <summary>Waypoints' position label.</summary>
	protected static readonly string LABEL_ROTATION = "Rotation "; 										/// <summary>Waypoints' rotation label.</summary>
	protected static readonly string LABEL_COLON = ": "; 												/// <summary>Colon label.</summary>
#if UNITY_EDITOR
	protected static readonly string LABEL_GIZMOS_ATTRIBUTES = "Gizmos Attributes: "; 					/// <summary>Gizmos Attributes Label field.</summary>
	protected static readonly string LABEL_DRAW_WHEN_SELECTED = "Draw When Selected? "; 				/// <summary>Label for drawWhenSelectedProperty [used for Inspector classes].</summary>
	protected static readonly string LABEL_OVERRIDE_PROPERTIES = "Override Waypoints' Properties: "; 	/// <summary>Override Waypoint's properties label.</summary>
	protected static readonly string LABEL_DRAW_TYPE = "Draw Type: "; 									/// <summary>Draw Type label.</summary>
	protected static readonly string LABEL_COLOR = "Color: "; 											/// <summary>Color label.</summary>
	protected static readonly string LABEL_DIMENSIONS = "Dimensions: "; 								/// <summary>Dimensions label.</summary>
	protected static readonly string LABEL_NORMAL_PROJECTION = "Normal Projection: "; 					/// <summary>Normal projection label.</summary>
	protected static readonly string LABEL_UPDATE_PROPERTIES = "Update Waypoints' Properties."; 		/// <summary>Update Waypoint's properties label.</summary>
	protected static readonly string LABEL_DEBUG_GUI_TEXT = "Debug GUI Text: "; 						/// <summary>Debug GUI Text label.</summary>
	protected static readonly string LABEL_TEXT_LABEL = "Text Label: "; 								/// <summary>Text label's label.</summary>
	protected static readonly string LABEL_TEXT_COLOR = "Text Color: "; 								/// <summary>Text color label.</summary>
	protected static readonly string LABEL_TEXT_OFFSET = "Text Offset: "; 								/// <summary>Text Offset label.</summary>
#endif

	protected T waypointGenerator; 																		/// <summary>Inspector's Target.</summary>

	/// <summary>Sets target property.</summary>
	void OnEnable()
	{
		waypointGenerator = target as T;
	}

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{
		DrawListInquiry();
		DrawWaypointUpdateInquiry();
		DrawDebugOptions();
		if(waypointGenerator.waypoints.Count > 0 && (waypointGenerator.positionsDebugOption != DebugOption.None || waypointGenerator.rotationsDebugOption != DebugOption.None))
		{
			EditorGUILayout.LabelField(LABEL_WAYPOINTS);
			EditorGUILayout.Space();
			DrawDebugWaypoints();
		}
#if UNITY_EDITOR
		OnGizmosInspectorGUI();
#endif
	}

	/// <summary>Draws List toggle that determines if WaypointController's waypoints list will be shown on Inspector.</summary>
	protected virtual void DrawListInquiry()
	{
		if(waypointGenerator.showWaypointsList = EditorGUILayout.Toggle(LABEL_SHOW_WAYPOINTS, waypointGenerator.showWaypointsList))
		{
			waypointGenerator.waypoints = VEditorGUILayout.ListField(LABEL_WAYPOINTS, waypointGenerator.waypoints, true);
			if(waypointGenerator.waypoints.Count == 0)
			{
				waypointGenerator.quantity = 0;
				waypointGenerator.UpdateWaypointGenerator();
			}
			VEditorGUILayout.Spaces(3);
		}
	}

	/// <summary>Draws the pertinent fields to update the Waypoint Generator.</summary>
	protected virtual void DrawWaypointUpdateInquiry()
	{
		waypointGenerator.waypointPrefab = EditorGUILayout.ObjectField(LABEL_WAYPOINT_PREFAB, waypointGenerator.waypointPrefab, typeof(W), true) as W;
		EditorGUILayout.Space();
		waypointGenerator.quantity = EditorGUILayout.IntField(LABEL_QUANTITY, waypointGenerator.quantity);
		EditorGUILayout.LabelField(LABEL_PRIOR_QUANTITY, waypointGenerator.priorQuantity.ToString());
		VEditorGUILayout.Spaces(1);
		if(GUILayout.Button(LABEL_UPDATE_GENERATOR)) waypointGenerator.UpdateWaypointGenerator();
		VEditorGUILayout.Spaces(3);
	}

	/// <summary>Draws the debug options for the debuggable properties from the waypoints.</summary>
	protected virtual void DrawDebugOptions()
	{
		waypointGenerator.positionsDebugOption = (DebugOption)EditorGUILayout.EnumPopup(LABEL_POSITIONS_DEBUG_OPTION, waypointGenerator.positionsDebugOption);
		waypointGenerator.rotationsDebugOption = (DebugOption)EditorGUILayout.EnumPopup(LABEL_ROTATIONS_DEBUG_OPTION, waypointGenerator.rotationsDebugOption);
		EditorGUILayout.Space();
	}

	/// <summary>Debugs waypoints with their respective debuggable properties.</summary>
	protected virtual void DrawDebugWaypoints()
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
			}
			else EditorGUILayout.LabelField(LABEL_WAYPOINTS + LABEL_COLON, LABEL_NO_WAYPOINTS);	
		}

		VEditorGUILayout.Spaces(2);
	}

	/// <summary>Debugs waypoint position from the waypoint located at the given index.</summary>
	/// <param name="_index">Index of the waypoint on waypoints list.</param>
	protected virtual void DrawDebugWaypointPosition(int _index)
	{
		Vector3 position = EditorGUILayout.Vector3Field(LABEL_POSITION + LABEL_COLON, waypointGenerator.waypoints[_index].position);
		if(waypointGenerator.positionsDebugOption == DebugOption.DebugAndModifyOnInspectorOnly) waypointGenerator.waypoints[_index].position = position;
	}

	/// <summary>Debugs waypoint rotation from the waypoint located at the given index.</summary>
	/// <param name="_index">Index of the waypoint on waypoints list.</param>
	protected virtual void DrawDebugWaypointRotation(int _index)
	{
		Vector3 rotation = EditorGUILayout.Vector3Field(LABEL_ROTATION + LABEL_COLON, waypointGenerator.waypoints[_index].eulerAngles);
		if(waypointGenerator.rotationsDebugOption == DebugOption.DebugAndModifyOnInspectorOnly) waypointGenerator.waypoints[_index].eulerAngles = rotation;
	}

#if UNITY_EDITOR
	protected virtual void OnGizmosInspectorGUI()
	{
		EditorGUILayout.LabelField(LABEL_GIZMOS_ATTRIBUTES);
		EditorGUILayout.Space();
		waypointGenerator.drawWhenSelected = EditorGUILayout.Toggle("Draw When Selecte?", waypointGenerator.drawWhenSelected);
		VEditorGUILayout.Spaces(2);
		if(waypointGenerator.overrideProperties = EditorGUILayout.Foldout(waypointGenerator.overrideProperties, LABEL_OVERRIDE_PROPERTIES))
		{
			DrawDebugGizmosWaypointProperties();
		}
		if(waypointGenerator.debugGUIText = EditorGUILayout.Toggle(LABEL_DEBUG_GUI_TEXT, waypointGenerator.debugGUIText))
		{
			DrawDebugGUITextProperties();
		}
	}

	/// <summary>Debugs GizmosWaypoints' properties for the inspector to modify.</summary>
	protected virtual void DrawDebugGizmosWaypointProperties()
	{
		waypointGenerator.overrideDrawWhenSelected = EditorGUILayout.Toggle("Draw When Selecte?", waypointGenerator.overrideDrawWhenSelected);
		waypointGenerator.overrideDrawType = (DrawTypes)EditorGUILayout.EnumPopup(LABEL_DRAW_TYPE, waypointGenerator.overrideDrawType);
		waypointGenerator.overrideColor = EditorGUILayout.ColorField(LABEL_COLOR, waypointGenerator.overrideColor);
		waypointGenerator.overrideDimensions = EditorGUILayout.Vector3Field(LABEL_DIMENSIONS, waypointGenerator.overrideDimensions);
		waypointGenerator.overrideNormalProjection = EditorGUILayout.FloatField(LABEL_NORMAL_PROJECTION, waypointGenerator.overrideNormalProjection);
		EditorGUILayout.Space();
		if(GUILayout.Button(LABEL_UPDATE_PROPERTIES))
		{
			waypointGenerator.UpdateWaypointsProperties();
		}
		VEditorGUILayout.Spaces(2);
	}

	/// <summary>Draws GUI's text properties.</summary>
	protected virtual void DrawDebugGUITextProperties()
	{
		waypointGenerator.textLabel = EditorGUILayout.TextField(LABEL_TEXT_LABEL, waypointGenerator.textLabel);
		waypointGenerator.textColor = EditorGUILayout.ColorField(LABEL_TEXT_COLOR, waypointGenerator.textColor);
		waypointGenerator.textOffset = EditorGUILayout.Vector2Field(LABEL_TEXT_OFFSET, waypointGenerator.textOffset);
	}
#endif
}
}