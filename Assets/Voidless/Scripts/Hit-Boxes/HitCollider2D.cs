using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
/// <summary>Event invoked when this Hit Collider2D intersects with another GameObject.</summary>
/// <param name="_collider">Collider2D that was involved on the Hit Event.</param>
/// <param name="_eventType">Type of the event.</param>
/// <param name="_hitColliderID">Optional ID of the HitCollider2D.</param>
public delegate void OnTriggerEvent2D(Collider2D _collider, HitColliderEventTypes _eventType, int _hitColliderID = 0);

/// <summary>Event invoked when this Hit Collider2D intersects with another GameObject.</summary>
/// <param name="_hitCollider">HitCollider2D that invoked the event.</param>
/// <param name="_collider">Collider2D that was involved on the Hit Event.</param>
/// <param name="_eventType">Type of the event.</param>
public delegate void OnHitColliderTriggerEvent2D(HitCollider2D _hitCollider, Collider2D _collider, HitColliderEventTypes _eventType);

/// <summary>Event invoked when this Hit Collider2D intersects with another GameObject.</summary>
/// <param name="a">Collider2D A that sent Hit Event.</param>
/// <param name="b">Collider2D B that was involved on the Hit Event.</param>
/// <param name="_eventType">Type of the event.</param>
/// <param name="_hitColliderID">Optional ID of the HitCollider2D.</param>
public delegate void OnTriggerInstanceEvent2D(Collider2D a, Collider2D b, HitColliderEventTypes _eventType, int _hitColliderID = 0);

/// <summary>Event invoked when this Hit Collider2D collides with another GameObject.</summary>
/// <param name="_collision">Collision's Data.</param>
/// <param name="_eventType">Type of the event.</param>
/// <param name="_hitColliderID">Optional ID of the HitCollider2D.</param>
public delegate void OnCollisionEvent2D(Collision2D _collision, HitColliderEventTypes _eventType, int _hitColliderID = 0);

[RequireComponent(typeof(Rigidbody2D))]
public class HitCollider2D : MonoBehaviour
{
	public event OnHitColliderTriggerEvent2D onHitColliderTriggerEvent; 	/// <summary>OnHitColliderTriggerEvent2D's event delegate.</summary>
	public event OnTriggerEvent2D onTriggerEvent2D; 						/// <summary>OnTriggerEvent2D event delegate.</summary>
	public event OnTriggerInstanceEvent2D onTriggerInstanceEvent2D; 		/// <summary>OnTriggerInstanceEvent2D event delegate.</summary>
	public event OnCollisionEvent2D onCollisionEvent2D; 					/// <summary>OnCollisionEvent2D event delegate.</summary>

	[SerializeField] private int _ID; 										/// <summary>Hit Collider's ID.</summary>
	[SerializeField] private HitColliderEventTypes _detectableHitEvents; 	/// <summary>Detectablie Hit's Events.</summary>
	private Collider2D _collider; 											/// <summary>Collider2D's Component.</summary>
	private Rigidbody2D _rigidbody; 										/// <summary>Rigidbody's Component.</summary>

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
	public new Collider2D collider
	{ 
		get
		{
			if(_collider == null) _collider = GetComponent<Collider2D>();
			return _collider;
		}
	}

	/// <summary>Gets rigidbody Component.</summary>
	public Rigidbody2D rigidbody
	{ 
		get
		{
			if(_rigidbody == null) _rigidbody = GetComponent<Rigidbody2D>();
			return _rigidbody;
		}
	}
#endregion

	/// <summary>Resets HitCollider2D's instance to its default values.</summary>
	public void Reset()
	{
		rigidbody.isKinematic = true;
		rigidbody.simulated = false;
	}

	/// <summary>Sets the Collider2D either as trigger or as a collision collider.</summary>
	/// <param name="_trigger">Set collider as trigger? [true by default].</param>
	public void SetTrigger(bool _trigger = true)
	{
		collider.isTrigger = _trigger;
		rigidbody.isKinematic = _trigger;
		rigidbody.useFullKinematicContacts = _trigger;
	}

	/// <summary>Activates HitCollider2D.</summary>
	/// <param name="_activate">Activate? [true by default].</param>
	public void Activate(bool _activate = true)
	{
		collider.enabled = _activate;
		rigidbody.simulated = _activate;
	}

#region TriggerCallbakcs:
	/// <summary>Event triggered when this Collider2D enters another Collider2D trigger.</summary>
	/// <param name="col">The other Collider2D involved in this Event.</param>
	private void OnTriggerEnter2D(Collider2D col)
	{
		if(!detectableHitEvents.HasFlag(HitColliderEventTypes.Enter)) return;
		if(onHitColliderTriggerEvent != null) onHitColliderTriggerEvent(this, col, HitColliderEventTypes.Enter);
		if(onTriggerEvent2D != null) onTriggerEvent2D(col, HitColliderEventTypes.Enter, ID);
		if(onTriggerInstanceEvent2D != null) onTriggerInstanceEvent2D(collider, col, HitColliderEventTypes.Enter, ID);
	}

	/// <summary>Event triggered when this Collider2D stays with another Collider2D trigger.</summary>
	/// <param name="col">The other Collider2D involved in this Event.</param>
	private void OnTriggerStay2D(Collider2D col)
	{
		if(!detectableHitEvents.HasFlag(HitColliderEventTypes.Stays)) return;
		if(onHitColliderTriggerEvent != null) onHitColliderTriggerEvent(this, col, HitColliderEventTypes.Stays);
		if(onTriggerEvent2D != null) onTriggerEvent2D(col, HitColliderEventTypes.Stays, ID);
		if(onTriggerInstanceEvent2D != null) onTriggerInstanceEvent2D(collider, col, HitColliderEventTypes.Stays, ID);
	}

	/// <summary>Event triggered when this Collider2D exits another Collider2D trigger.</summary>
	/// <param name="col">The other Collider2D involved in this Event.</param>
	private void OnTriggerExit2D(Collider2D col)
	{
		if(!detectableHitEvents.HasFlag(HitColliderEventTypes.Exit)) return;
		if(onHitColliderTriggerEvent != null) onHitColliderTriggerEvent(this, col, HitColliderEventTypes.Exit);
		if(onTriggerEvent2D != null) onTriggerEvent2D(col, HitColliderEventTypes.Exit, ID);
		if(onTriggerInstanceEvent2D != null) onTriggerInstanceEvent2D(collider, col, HitColliderEventTypes.Exit, ID);
	}
#endregion

#region CollisionCallbacks:
	/// <summary>Event triggered when this Collider/Rigidbody begun having contact with another Collider/Rigidbody.</summary>
	/// <param name="col">The Collision2D data associated with this collision Event.</param>
	private void OnCollisionEnter2D(Collision2D col)
	{
		if(!detectableHitEvents.HasFlag(HitColliderEventTypes.Enter)) return;
		if(onCollisionEvent2D != null) onCollisionEvent2D(col, HitColliderEventTypes.Enter, ID);
	}

	/// <summary>Event triggered when this Collider/Rigidbody begun having contact with another Collider/Rigidbody.</summary>
	/// <param name="col">The Collision2D data associated with this collision Event.</param>
	private void OnCollisionStay2D(Collision2D col)
	{
		if(!detectableHitEvents.HasFlag(HitColliderEventTypes.Stays)) return;
		if(onCollisionEvent2D != null) onCollisionEvent2D(col, HitColliderEventTypes.Stays, ID);
	}

	/// <summary>Event triggered when this Collider/Rigidbody began having contact with another Collider/Rigidbody.</summary>
	/// <param name="col">The Collision2D data associated with this collision Event.</param>
	private void OnCollisionExit2D(Collision2D col)
	{
		if(!detectableHitEvents.HasFlag(HitColliderEventTypes.Exit)) return;
		if(onCollisionEvent2D != null) onCollisionEvent2D(col, HitColliderEventTypes.Exit, ID);
	}
#endregion
}
}