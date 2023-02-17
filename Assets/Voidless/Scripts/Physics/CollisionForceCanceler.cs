using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[RequireComponent(typeof(Rigidbody))]
public class CollisionForceCanceler : MonoBehaviour
{
	[SerializeField] private ForceCancelInformation[] _information; 	/// <summary>Information.</summary>
	private Rigidbody _rigidbody; 										/// <summary>Rigidbody's Component.</summary>

	/// <summary>Gets information property.</summary>
	public ForceCancelInformation[] information { get { return _information; } }

	/// <summary>Gets rigidbody Component.</summary>
	public Rigidbody rigidbody
	{ 
		get
		{
			if(_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
			return _rigidbody;
		}
	}

	/// <summary>Event triggered when this Collider/Rigidbody began having contact with another Collider/Rigidbody.</summary>
	/// <param name="col">The Collision data associated with this collision Event.</param>
	private void OnCollisionExit(Collision col)
	{
		GameObject obj = col.gameObject;
	
		foreach(ForceCancelInformation info in information)
		{
			if(obj.IsInLayerMask(info.mask))
			rigidbody.CancelForcesOnAxes(info.axes);
		}
	}
}
}