using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[RequireComponent(typeof(Rigidbody))]
public class DisplacementAccumulator : MonoBehaviour
{
	private Vector3 _velocity; 		/// <summary>Accumulator's Velocity.</summary>
	//private Vector3 _acceleration; 	/// <summary>Accumulator's Acceleration.</summary>
	private Rigidbody _rigidbody; 	/// <summary>Rigidbody's Component.</summary>

	/// <summary>Gets and Sets velocity property.</summary>
	public Vector3 velocity
	{
		get { return _velocity; }
		set { _velocity = value; }
	}

	/*/// <summary>Gets and Sets acceleration property.</summary>
	public Vector3 acceleration
	{
		get { return _acceleration; }
		set { _acceleration = value; }
	}*/

	/// <summary>Gets rigidbody Component.</summary>
	public Rigidbody rigidbody
	{ 
		get
		{
			if(_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
			return _rigidbody;
		}
	}

	/// <summary>DisplacementAccumulator's instance initialization.</summary>
	private void Awake()
	{
		velocity = Vector3.zero;
		this.StartCoroutine(WaitForEndOfPhysicsThread());
	}

	/// <summary>Callback invoked at the end of the Physics Step.</summary>
	private void OnEndOfPhysicsStep()
	{
		if(velocity.sqrMagnitude == 0.0f) return;

		rigidbody.MovePosition(rigidbody.position + (velocity * Time.fixedDeltaTime));
		velocity *= 0.0f;
	}

	/// <summary>Adds displacement to this accumulator.</summary>
	/// <param name="_displacement">Displacement to accumulate.</param>
	public void AddDisplacement(Vector3 _displacement)
	{
		velocity += _displacement;
	}

	/// <summary>Waits for the end of the Physics's Step and invokes a callback.</summary>
	private IEnumerator WaitForEndOfPhysicsThread()
	{
		while(true)
		{
			yield return VCoroutines.WAIT_PHYSICS_THREAD;
			OnEndOfPhysicsStep();
		}
	}
}
}