using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[RequireComponent(typeof(SphereCollider))]
public class SphereWaypoint : Waypoint
{
	private SphereCollider _sphereCollider; 	/// <summary>SphereCollider's Component.</summary>

	/// <summary>Gets and Sets SphereCollider Component.</summary>
	public SphereCollider sphereCollider
	{ 
		get
		{
			if(_sphereCollider == null) _sphereCollider = GetComponent<SphereCollider>();
			return _sphereCollider;
		}
	}

	/// <summary>Resizes Waypoint's collider.</summary>
	public override void ResizeCollider()
	{
		sphereCollider.radius = dimensions.GetMaxVectorProperty();
	}

#if UNITY_EDITOR
	/// <summary>Draws Waypoint's Gizmos.</summary>
	protected override void DrawWaypoint()
	{
		Gizmos.color = color;
		switch(drawType)
		{
			case DrawTypes.Wired:
			Gizmos.DrawWireSphere(Vector3.zero, dimensions.GetMaxVectorProperty());
			break;

			case DrawTypes.Solid:
			Gizmos.DrawSphere(Vector3.zero, dimensions.GetMaxVectorProperty());
			break;
		}
		ResizeCollider();
	}
#endif

}
}