using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Voidless
{
public enum ContactEvent { Enter, Exit, Stay }

/// <summary>Event Invoked when a TriggerZone enters an event.</summary>
/// <param name="_triggerZone">TriggerZone that invoked the event.</param>
/// <param name="_event">Event's type.</param>
public delegate void OnTriggerZoneEvent<T>(TriggerZone<T> _triggerZone, ContactEvent _event);

[RequireComponent(typeof(BoxCollider))]
public abstract class TriggerZone<T> : MonoBehaviour
{
	public static event OnTriggerZoneEvent<T> onTriggerZoneEvent; 	/// <summary>OnTriggerZoneEvent's Delegate.</summary>

	protected static HashSet<TriggerZone<T>> triggerZones; 			/// <summary>TriggerZone's static registry.</summary>

	[SerializeField] private GameObjectTag[] _tags; 				/// <summary>GameObject Tags that invoke the trigger.</summary>
	[Space(5f)]
	[SerializeField] protected Color gizmosColor; 					/// <summary>Gizmos' Color.</summary>
	private BoxCollider _boxCollider; 								/// <summary>BoxCollider's Component.</summary>
	private Rigidbody _rigidbody; 									/// <summary>Optional Rigidbody's Component.</summary>
	private bool _entered; 											/// <summary>Has an Object entered inside this TriggerZone?.</summary>

	/// <summary>Gets tags property.</summary>
	public GameObjectTag[] tags { get { return _tags; } }

	/// <summary>Gets and Sets entered property.</summary>
	public bool entered
	{
		get { return _entered; }
		protected set { _entered = value; }
	}

	/// <summary>Gets boxCollider Component.</summary>
	public BoxCollider boxCollider
	{ 
		get
		{
			if(_boxCollider == null) _boxCollider = GetComponent<BoxCollider>();
			return _boxCollider;
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

	/// <summary>Static TriggerZone<T>'s Constructor.</summary>
	static TriggerZone()
	{
		triggerZones = new HashSet<TriggerZone<T>>();
	}

	/// <summary>Draws Gizmos on Editor mode.</summary>
	protected virtual void OnDrawGizmos()
	{
#if UNITY_EDITOR
		Gizmos.color = gizmosColor;
		Gizmos.DrawCube(transform.position + (transform.rotation * boxCollider.center), boxCollider.size);
#endif
	}

	/// <summary>Resets TriggerZone's instance to its default values.</summary>
	protected virtual void Reset()
	{
		boxCollider.isTrigger = true;

		if(rigidbody != null)
		{
			rigidbody.isKinematic = true;
			rigidbody.useGravity = false;
		}
	}

	/// <summary>TriggerZone's instance initialization.</summary>
	protected virtual void Awake()
	{
		entered = false;
	}

	/// <summary>Invokes event.</summary>
	/// <param name="_eventType">Interaction Event Type.</param>
	protected void InvokeEvent(ContactEvent _eventType)
	{
		if(onTriggerZoneEvent != null) onTriggerZoneEvent(this, _eventType);
	}

	/// <summary>Clears Trigger-Zones' Mapping.</summary>
	public static void ClearTriggerZonesMapping()
	{
		triggerZones.Clear();
	}

	/// <summary>Event triggered when this Collider enters another Collider trigger.</summary>
	/// <param name="col">The other Collider involved in this Event.</param>
	private void OnTriggerEnter(Collider col)
	{
		if(entered || tags == null) return;

		GameObject obj = col.gameObject;
		
		foreach(GameObjectTag tag in tags)
		{
			if(obj.CompareTag(tag))
			{
				//triggerZones.Add(this);
				entered = true;
				OnEnter(col);
				return;
			}
		}
	}

	/// <summary>Event triggered when this Collider exits another Collider trigger.</summary>
	/// <param name="col">The other Collider involved in this Event.</param>
	private void OnTriggerExit(Collider col)
	{
		if(!entered || tags == null) return;

		GameObject obj = col.gameObject;
		
		foreach(GameObjectTag tag in tags)
		{
			if(obj.CompareTag(tag))
			{
				//triggerZones.Remove(this);
				entered = false;
				OnExit(col, triggerZones.Count > 0 ? triggerZones.First() : null);
				return;
			}
		}
	}

	/// <summary>Callback internally invoked when a GameObject's Collider enters the TriggerZone.</summary>
	/// <param name="_collider">Collider that Enters.</param>
	protected virtual void OnEnter(Collider _collider)
	{
		if(triggerZones.Contains(this)) return;

		triggerZones.Add(this);

		InvokeEvent(ContactEvent.Enter);
	}

	/// <summary>Callback internally invoked when a GameObject's Collider exits the TriggerZone.</summary>
	/// <param name="_collider">Collider that Exits.</param>
	/// <param name="_nextTriggerZone">Next Trigger that ought to be attended.</param>
	protected virtual void OnExit(Collider _collider, TriggerZone<T> _nextTriggerZone)
	{
		if(!triggerZones.Contains(this)) return;

		triggerZones.Remove(this);

		InvokeEvent(ContactEvent.Exit);
	}

	/// <summary>Adds TriggerZone to HashSet.</summary>
	public static void AddTriggerZone(TriggerZone<T> _triggerZone)
	{
		triggerZones.Add(_triggerZone);
	}

	/// <summary>Removes TriggerZone to HashSet.</summary>
	public static void RemoveTriggerZone(TriggerZone<T> _triggerZone)
	{
		triggerZones.Remove(_triggerZone);
	}

	/// <returns>HashSet's Count.</returns>
	public static int GetTriggersCount()
	{
		return triggerZones.Count;
	}
}
}