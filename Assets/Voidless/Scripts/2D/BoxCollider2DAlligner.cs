using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[RequireComponent(typeof(BoxCollider2D))]
public class BoxCollider2DAlligner : Collider2DAlligner
{
	[SerializeField] private BoxCollider2D _boxCollider; 	/// <summary>BoxCollider2D's Component.</summary>

	/// <summary>Gets boxCollider Component.</summary>
	public BoxCollider2D boxCollider
	{ 
		get
		{
			if(_boxCollider == null) _boxCollider = GetComponent<BoxCollider2D>();
			return _boxCollider;
		}
	}

	/// <summary>Updates Collider2D.</summary>
	protected override void UpdateCollider()
	{
		/*if(transformA == null || transformB == null) return;

		Vector3 pointA = transformA.TransformPoint(a);
		Vector3 pointB = transformB.TransformPoint(b);
		Vector2 d = pointB - pointA;
		float m = d.magnitude;

		boxCollider.size = boxCollider.size.WithX(m);
		transform.position = Vector3.Lerp(pointA, pointB, 0.5f);
		transform.rotation = VQuaternion.RightLookRotation(d);*/
	}
}
}