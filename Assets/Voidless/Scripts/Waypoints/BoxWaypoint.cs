using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[RequireComponent(typeof(BoxCollider))]
public class BoxWaypoint : Waypoint
{
	private BoxCollider _boxCollider; 	/// <summary>BoxCollider's Component.</summary>

	/// <summary>Gets and Sets boxCollider Component.</summary>
	public BoxCollider boxCollider
	{ 
		get
		{
			if(_boxCollider == null) _boxCollider = GetComponent<BoxCollider>();
			return _boxCollider;
		}
	}

	/// <summary>Resizes Waypoint's collider.</summary>
	public override void ResizeCollider()
	{
		boxCollider.size = dimensions;
	}

#if UNITY_EDITOR
	/// <summary>Draws Waypoint's Gizmos.</summary>
	protected override void DrawWaypoint()
	{
		Gizmos.color = color;
		switch(drawType)
		{
			case DrawTypes.Wired:
			Gizmos.DrawWireCube(Vector3.zero, dimensions);
			break;

			case DrawTypes.Solid:
			Gizmos.DrawCube(Vector3.zero, dimensions);
			break;
		}
		ResizeCollider();
	}
#endif

}
}