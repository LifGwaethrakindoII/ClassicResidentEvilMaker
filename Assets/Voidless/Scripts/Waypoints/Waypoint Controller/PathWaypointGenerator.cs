using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[ExecuteInEditMode]
public class PathWaypointGenerator : BaseWaypointGenerator<PathWaypoint>
{
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Path Lines Attributes:")]
	[SerializeField] private Color _pathLineColor; 	/// <summary>Path line's color.</summary>
	[SerializeField] private int _timeSplit; 		/// <summary>Beizer curve's time split.</summary>
#endif

#if UNITY_EDITOR
	/// <summary>Gets and Sets pathLineColor property.</summary>
	public Color pathLineColor
	{
		get { return _pathLineColor; }
		set { _pathLineColor = value; }
	}

	/// <summary>Gets and Sets timeSplit property.</summary>
	public int timeSplit
	{
		get { return _timeSplit; }
		set { _timeSplit = value; }
	}
#endif

#if UNITY_EDITOR
	/// <summary>Draws Gizmos.</summary>
	protected override void DrawGizmos()
	{
		if(waypoints.Count > 0)
		{
			for(int i = 0; i < waypoints.Count; i++)
			{
				if(waypoints[i] != null)
				{
					if(debugGUIText) DrawDebugGUIText(i);
					if(i < (waypoints.Count - 1)) DrawDebugCurvePath(i);
				}
			}
		}
	}

	protected virtual void DrawDebugCurvePath(int _index)
	{
		int nextIndex = _index + 1;

		switch(waypoints[_index].curveType)
		{
			case CurveType.Linear:
			Gizmos.DrawLine(waypoints[_index].position, waypoints[nextIndex].position);
			break;

			case CurveType.Cuadratic:
			VGizmos.DrawCuadraticBeizerCurve(waypoints[_index].position, waypoints[nextIndex].position, waypoints[nextIndex].startTangent, timeSplit, pathLineColor);
			break;

			case CurveType.Cubic:
			VGizmos.DrawCubicBeizerCurve(waypoints[_index].position, waypoints[nextIndex].position, waypoints[nextIndex].startTangent, waypoints[nextIndex].endTangent, timeSplit, pathLineColor);
			break;
		}
	}
#endif
}
}