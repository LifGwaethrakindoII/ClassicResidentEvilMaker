using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
/// <summary>Event Invoked when this Hit Collider Triggers with another Collider.</summary>
/// <param name="_collider">Collider involved on the Trigger Event.</param>
/// <param name="_eventType">Type of the event.</param>
/// <param name="_ID">Optional ID of the HitCollider2D.</param>
public delegate void OnColliderEvent(Collider _collider, HitColliderEventTypes _eventType, int _ID = 0);

/// <summary>Event Invoked when this Hit Collider Triggers with another Collider.</summary>
/// <param name="a">Collider A that invoked on the Trigger Event.</param>
/// <param name="b">Collider involved on the Trigger Event.</param>
/// <param name="_eventType">Type of the event.</param>
/// <param name="_ID">Optional ID of the HitCollider2D.</param>
public delegate void OnColliderInstanceEvent(Collider a, Collider b, HitColliderEventTypes _eventType, int _ID = 0);

/// <summary>Event invoked when this Hit Collider Collides with another Collider.</summary>
/// <param name="_collision">Collision Data.</param>
/// <param name="_eventType">Type of the event.</param>
/// <param name="_ID">Optional ID of the HitCollider2D.</param>
public delegate void OnCollisionEvent(Collision _collision, HitColliderEventTypes _eventType, int _ID = 0);

[RequireComponent(typeof(Rigidbody))]
public class HitCollider : MonoBehaviour
{
	public event OnColliderEvent onTriggerEvent; 							/// <summary>OnColliderEvent's Event Delegate.</summary>
	public event OnColliderInstanceEvent onTriggerInstanceEvent; 			/// <summary>OnColliderInstanceEvent's Event Delegate.</summary>
	public event OnCollisionEvent onCollisionEvent; 						/// <summary>OnCollisionEnter's Event Delegate.</summary>

	[SerializeField] private int _ID; 										/// <summary>Optional ID.</summary>
	[SerializeField] private HitColliderEventTypes _detectableHitEvents; 	/// <summary>Detectablie Hit's Events.</summary>
	private Collider _collider; 											/// <summary>Collider's Component.</summary>
	private Rigidbody _rigidbody; 											/// <summary>Rigidbody's Component.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets ID property.</summary>
	public int ID
	{
		get { return _ID; }
		set { _ID = value; }
	}

	/// <summary>Gets and Sets detectableHitEvents property.</summary>
	public HitColliderEventTypes detectableHitEvents
	{
		get { return _detectableHitEvents; }
		set { _detectableHitEvents = value; }
	}

	/// <summary>Gets and Sets collider Component.</summary>
	public new Collider collider
	{ 
		get
		{
			if(_collider == null) _collider = GetComponent<Collider>();
			return _collider;
		}
	}

	/// <summary>Gets rigidbody Component.</summary>
	public Rigidbody rigidbody
	{ 
		get
		{
			if(_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
			return _rigidbody;
		}
	}
#endregion

	/// <summary>Activates Hit Collider.</summary>
	public void Activate()
	{
		gameObject.SetActive(true);
	}

	/// <summary>Deactivates Hit Collider.</summary>
	public void Deactivate()
	{
		gameObject.SetActive(false);
	}

	/// <summary>Sets the Collider either as trigger or as a collision collider.</summary>
	/// <param name="_trigger">Set collider as trigger?.</param>
	public void SetTrigger(bool _trigger)
	{
		collider.isTrigger = _trigger;
		rigidbody.isKinematic = _trigger;
	}

#region TriggerCallbakcs:
	/// <summary>Event triggered when this Collider enters another Collider trigger.</summary>
	/// <param name="col">The other Collider involved in this Event.</param>
	private void OnTriggerEnter(Collider col)
	{
		if(!detectableHitEvents.HasFlag(HitColliderEventTypes.Enter)) return;
		if(onTriggerEvent != null) onTriggerEvent(col, HitColliderEventTypes.Enter, ID);
		if(onTriggerInstanceEvent != null) onTriggerInstanceEvent(collider, col, HitColliderEventTypes.Enter, ID);
	}

	/// <summary>Event triggered when this Collider stays with another Collider trigger.</summary>
	/// <param name="col">The other Collider involved in this Event.</param>
	private void OnTriggerStay(Collider col)
	{
		if(!detectableHitEvents.HasFlag(HitColliderEventTypes.Stays)) return;
		if(onTriggerEvent != null) onTriggerEvent(col, HitColliderEventTypes.Stays, ID);
		if(onTriggerInstanceEvent != null) onTriggerInstanceEvent(collider, col, HitColliderEventTypes.Stays, ID);
	}

	/// <summary>Event triggered when this Collider exits another Collider trigger.</summary>
	/// <param name="col">The other Collider involved in this Event.</param>
	private void OnTriggerExit(Collider col)
	{
		if(!detectableHitEvents.HasFlag(HitColliderEventTypes.Exit)) return;
		if(onTriggerEvent != null) onTriggerEvent(col, HitColliderEventTypes.Exit, ID);
		if(onTriggerInstanceEvent != null) onTriggerInstanceEvent(collider, col, HitColliderEventTypes.Exit, ID);
	}
#endregion

#region CollisionCallbacks:
	/// <summary>Event triggered when this Collider/Rigidbody begun having contact with another Collider/Rigidbody.</summary>
	/// <param name="col">The Collision data associated with this collision Event.</param>
	private void OnCollisionEnter(Collision col)
	{
		if(!detectableHitEvents.HasFlag(HitColliderEventTypes.Enter)) return;
		if(onCollisionEvent != null) onCollisionEvent(col, HitColliderEventTypes.Enter, ID);
	}

	/// <summary>Event triggered when this Collider/Rigidbody begun having contact with another Collider/Rigidbody.</summary>
	/// <param name="col">The Collision data associated with this collision Event.</param>
	private void OnCollisionStay(Collision col)
	{
		if(!detectableHitEvents.HasFlag(HitColliderEventTypes.Stays)) return;
		if(onCollisionEvent != null) onCollisionEvent(col, HitColliderEventTypes.Stays, ID);
	}

	/// <summary>Event triggered when this Collider/Rigidbody began having contact with another Collider/Rigidbody.</summary>
	/// <param name="col">The Collision data associated with this collision Event.</param>
	private void OnCollisionExit(Collision col)
	{
		if(!detectableHitEvents.HasFlag(HitColliderEventTypes.Exit)) return;
		if(onCollisionEvent != null) onCollisionEvent(col, HitColliderEventTypes.Exit, ID);
	}
#endregion
}
}