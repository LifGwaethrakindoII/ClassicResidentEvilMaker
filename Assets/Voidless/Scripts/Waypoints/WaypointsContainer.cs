using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class WaypointsContainer : MonoBehaviour, IEnumerable<TransformData>
{
#if UNITY_EDITOR
	[Header("Gizmos' Attributes:")]
	[SerializeField] private Mesh _waypointMesh; 				/// <summary>Waypoint's Mesh.</summary>
	[SerializeField] private Vector3 _meshOffset; 				/// <summary>Mesh's Offset.</summary>
	[SerializeField] private DrawTypes _drawType; 				/// <summary>Gizmos' Draw Type.</summary>
	[SerializeField] private Color _color; 						/// <summary>Gizmos' Color.</summary>
	[Space(5f)]
	[HideInInspector] public bool showHandles; 					/// <summary>Global showHandles value for all waypoints.</summary>
#endif
	[SerializeField] private List<TransformData> _waypoints; 	/// <summary>Waypoints.</summary>
	private bool _relativeToTransform; 							/// <summary>Position the Waypoints relative to this Transform?.</summary>

#if UNITY_EDITOR
	/// <summary>Gets waypointMesh property.</summary>
	public Mesh waypointMesh { get { return _waypointMesh; } }

	/// <summary>Gets meshOffset property.</summary>
	public Vector3 meshOffset { get { return _meshOffset; } }

	/// <summary>Gets drawType property.</summary>
	public DrawTypes drawType { get { return _drawType; } }

	/// <summary>Gets and Sets color property.</summary>
	public Color color
	{
		get { return _color; }
		set { _color = value; }
	}
#endif

	/// <summary>Gets and Sets relativeToTransform property.</summary>
	public bool relativeToTransform
	{
		get { return _relativeToTransform; }
		set { _relativeToTransform = value; }
	}

	/// <summary>Gets and Sets waypoints property.</summary>
	public List<TransformData> waypoints
	{
		get { return _waypoints; }
		set { _waypoints = value; }
	}

#if UNITY_EDITOR
	/// <summary>Draws Gizmos on Editor mode.</summary>
	private void OnDrawGizmos()
	{
		if(waypoints == null || waypointMesh == null) return;

		Gizmos.color = color;

		foreach(TransformData waypoint in this)
		{
			Vector3 localPosition = waypoint.localPosition + (waypoint.localRotation * meshOffset);
			
			switch(drawType)
			{
				case DrawTypes.Wired:
				Gizmos.DrawWireMesh(waypointMesh, localPosition, waypoint.localRotation, waypoint.localScale);
				break;

				case DrawTypes.Solid:
				Gizmos.DrawMesh(waypointMesh, localPosition, waypoint.localRotation, waypoint.localScale);
				break;
			}
		}
	}
#endif

	/// <summary>Resets WaypointsContainer's instance to its default values.</summary>
	private void Reset()
	{
#if UNITY_EDITOR
		showHandles = true;
		color = Color.cyan.WithAlpha(0.5f);
#endif
		relativeToTransform = false;
		waypoints = new List<TransformData>();
		waypoints.Add(TransformData.Default());
	}

	/// <summary>Toggles the parenting of this transform to all waypoints contained.</summary>
	public void ToggleAddParentToWaypoints()
	{
		relativeToTransform = !relativeToTransform;

		for(int i = 0; i < waypoints.Count; i++)
		{
			TransformData waypoint = waypoints[i];
			waypoint.parent = relativeToTransform ? transform : null;
			waypoints[i] = waypoint;
		}
	}

	/// <returns>Returns an enumerator that iterates through the List's items.</returns>
	public IEnumerator<TransformData> GetEnumerator()
	{
		foreach(TransformData waypoint in waypoints)
		{
			yield return waypoint;
		}
	}
	
	/// <returns>Returns an enumerator that iterates through the List's items.</returns>
	IEnumerator IEnumerable.GetEnumerator()
	{
		yield return GetEnumerator();
	}

}
}