using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class WorldSpaceCollider2DOrienter : MonoBehaviour
{
	[SerializeField] private Collider2D[] _colliders; 	/// <summary>Colliders to orientate.</summary>

	/// <summary>Gets and Sets colliders property.</summary>
	public Collider2D[] colliders
	{
		get { return _colliders; }
		set { _colliders = value; }
	}

	/// <summary>Updates WorldSpaceCollider2DOrienter's instance at each Physics Thread's frame.</summary>
	private void Update()
	{
		if(colliders == null) return;

		foreach(Collider2D collider in colliders)
		{
			if(collider == null) return;

			collider.transform.rotation = Quaternion.identity;
		}
	}
}
}