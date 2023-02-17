using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
/// <summary>Event invoked when this FOVCollider collides with an GameObject.</summary>
/// <param name="_object">GameObject this FOVCollider collided with.</param>
public delegate void OnCollidedWithGameObject(GameObject _object);

/// <summary>Event invoked when this FOVCollider triggers with an GameObject.</summary>
/// <param name="_object">GameObject this FOVCollider triggered with.</param>
public delegate void OnTriggeredWithGameObject(GameObject _object);


public class FOVSight : MonoBehaviour
{
	private static readonly int Z_AXIS = 2; 								/// <summary>Z-Axis Direction's ID.</summary>

	//public event OnCollidedWithGameObject onCollidedWithGameObject; 		/// <summary>Event invoked when an GameObject collides with this Collider.</summary>
	public event OnTriggeredWithGameObject onTriggeredWithGameObject; 		/// <summary>Event invoked when an GameObject triggers with this Collider.</summary>
	//public event OnHitColliderEvent onHitColliderEvent; 					/// <summary>OnHitColliderEvent event delegate.</summary>

	[SerializeField] private HitColliderEventTypes _detectableHitEvents; 	/// <summary>Detectablie Hit's Events.</summary>
	[SerializeField] private float _updateCycle; 							/// <summary>Update's Cycle.</summary>
	[Space(5f)]
	[Header("Collider Interests:")]
	[SerializeField] private string[] _tags; 								/// <summary>Tags to selectively ignore other tags.</summary>
	[SerializeField] private LayerMask _masks; 								/// <summary>Layer Masks to selectively ignore other Layer Masks.</summary>
	[Space(5f)]
	[Header("FOV's Data")]
	[SerializeField] private float _radius; 								/// <summary>FOV's radius.</summary>
	[SerializeField] private float _length; 								/// <summary>FOV's length.</summary>
	//[SerializeField] private FOVFrustum _frustum; 						/// <summary>FOV's frustum.</summary>
	private Collider _collider; 											/// <summary>Collider's Component.</summary>
	private float _currentTime; 											/// <summary>Current Cycle's Time.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets detectableHitEvents property.</summary>
	public HitColliderEventTypes detectableHitEvents
	{
		get { return _detectableHitEvents; }
		set { _detectableHitEvents = value; }
	}

	/// <summary>Gets and Sets updateCycle property.</summary>
	public float updateCycle
	{
		get { return _updateCycle; }
		set { _updateCycle = value; }
	}

	/// <summary>Gets and Sets radius property.</summary>
	public float radius
	{
		get { return _radius; }
		set { _radius = value; }
	}

	/// <summary>Gets and Sets length property.</summary>
	public float length
	{
		get { return _length; }
		set { _length = value; }
	}

	/// <summary>Gets and Sets tags property.</summary>
	public string[] tags
	{
		get { return _tags; }
		set { _tags = value; }
	}

	/// <summary>Gets and Sets masks property.</summary>
	public LayerMask masks
	{
		get { return _masks; }
		set { _masks = value; }
	}

	/*/// <summary>Gets and Sets frustum property.</summary>
	public FOVFrustum frustum
	{
		get { return _frustum; }
		set { _frustum = value; }
	}*/

	/// <summary>Gets and Sets collider Component.</summary>
	public new Collider collider
	{ 
		get
		{
			if(_collider == null) _collider = GetComponent<Collider>();
			return _collider;
		}
	}

	/// <summary>Gets and Sets currentTime property.</summary>
	public float currentTime
	{
		get { return _currentTime; }
		set { _currentTime = value; }
	}
#endregion

#region UnityMethods:
	void OnDrawGizmosSelected()
	{
		ResizeCollider();
	}

	/// <summary>FOVSight's' instance initialization.</summary>
	void Awake()
	{
		//frustum.onChildTriggerEnter += OnTriggerEnter;
	}
	
	/// <summary>FOVSight's tick at each frame.</summary>
	void Update()
	{
		/*if(currentTime < updateCycle)
		{
			//frustum.meshCollider.enabled = false;
			collider.enabled = false;
			currentTime += Time.deltaTime;
		}
		else
		{
			//frustum.meshCollider.enabled = true;
			collider.enabled = true;
			currentTime = 0.0f;
		}*/
	}

	/// <summary>Event triggered when this Collider enters another Collider trigger.</summary>
	/// <param name="col">The other Collider involved in this Event.</param>
	void OnTriggerEnter(Collider col)
	{
		if(!detectableHitEvents.HasFlag(HitColliderEventTypes.Enter)) return;

		GameObject obj = col.gameObject;

		/*if(obj.layer == masks)
		{
			if(onTriggeredWithGameObject != null) onTriggeredWithGameObject(obj);
			if(onHitColliderEvent != null) onHitColliderEvent(obj, HitColliderEventTypes.Enter);
			return;
		}
		for(int i = 0; i < tags.Length; i++)
		{
			if(obj.CompareTag(tags[i]))
			{
				if(onTriggeredWithGameObject != null) onTriggeredWithGameObject(obj);
				if(onHitColliderEvent != null) onHitColliderEvent(obj, HitColliderEventTypes.Enter);
				return;
			}
		}*/
	}

	/// <summary>Event triggered when this Collider exits another Collider trigger.</summary>
	/// <param name="col">The other Collider involved in this Event.</param>
	void OnTriggerExit(Collider col)
	{
		if(!detectableHitEvents.HasFlag(HitColliderEventTypes.Exit)) return;

		GameObject obj = col.gameObject;
	
		/*if(obj.layer == masks)
		{
			if(onTriggeredWithGameObject != null) onTriggeredWithGameObject(obj);
			if(onHitColliderEvent != null) onHitColliderEvent(obj, HitColliderEventTypes.Exit);
			return;
		}
		for(int i = 0; i < tags.Length; i++)
		{
			if(obj.CompareTag(tags[i]))
			{
				if(onTriggeredWithGameObject != null) onTriggeredWithGameObject(obj);
				if(onHitColliderEvent != null) onHitColliderEvent(obj, HitColliderEventTypes.Exit);
				return;
			}
		}*/
	}
#endregion

	/// <summary>Resizes collider, by first evaluating the type of collider.</summary>
	public void ResizeCollider()
	{
		if(collider is CapsuleCollider)
		{
			CapsuleCollider capsuleCollider = collider as CapsuleCollider;

			capsuleCollider.radius = radius;
			capsuleCollider.height = length;
			capsuleCollider.direction = Z_AXIS;
			capsuleCollider.center = new Vector3
			(
				capsuleCollider.center.x,
				capsuleCollider.center.y,
				length * 0.5f
			);
		}
	}
}
}