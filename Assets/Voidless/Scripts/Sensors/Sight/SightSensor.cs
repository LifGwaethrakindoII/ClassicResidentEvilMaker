using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 	Rules for Callbacks/Methods to be invoked:
		1.- OnColliderOnSight:
			a) Collider must have entered FOV (being registered on inFOV's dictionary).
			b) Collider must have no occluders whatsoever.
		2.- OnColliderOccluded:
			a) Collider must have been previously on Sighted state (being registered on inSight's dictionary).
			b) Collider must have at least one occluder.
		3.- OnColliderOutOfSight:
			a) Collider must have been previously on Sighted state (being registered on inSight's dictionary).
			b) Collider must be out of FOV's Collider.
		4.- Unregister Collider from all dictionaries:
			a) Collider must have entered FOV (being registered on inFOV's dictionary).
			b) Collider must be out of FOV's Collider.
			c) In case of being registered in a ViewconeSightSensor, Collider must be out of Viewcone's area (Not necessarily out of SphereCollider).
*/

namespace Voidless
{
/// <summary>Event invoked when a collider is sighted.</summary>
/// <param name="_collider">Collider argument.</param>
public delegate void OnSightEvent(Collider _collider);

//[RequireComponent(typeof(OcclusionEvaluator))]
public abstract class SightSensor : Sensor
{
	public const float INFLATION = 0.01f; 						/// <summary>Inflation's Scalar to apply to Gizmos' Boxes.</summary>

	public event OnSightEvent onColliderSighted; 				/// <summary>On Collider Sighted's Event Delegate.</summary>
	public event OnSightEvent onColliderOccluded; 				/// <summary>On Collider Occluded's Event Delegate.</summary>
	public event OnSightEvent onColliderOutOfSight; 			/// <summary>On Collider Non-Sighted's Event Delegate.</summary>

	protected Dictionary<int, Collider> inFOV; 					/// <summary>Colliders inside Field of Vision.</summary>
	protected Dictionary<int, Collider> inSight; 				/// <summary>Colliders On Sight.</summary>
	protected Dictionary<int, RaycastHit[]> occluders; 			/// <summary>Dictionary containing a set of Colliders occluding the object with the key ID.</summary>
	private OcclusionEvaluator _occlusionEvaluator; 			/// <summary>OcclusionEvaluator's Component.</summary>

	/// <summary>Gets occlusionEvaluator Component.</summary>
	public OcclusionEvaluator occlusionEvaluator
	{ 
		get
		{
			if(_occlusionEvaluator == null) _occlusionEvaluator = GetComponent<OcclusionEvaluator>();
			return _occlusionEvaluator;
		}
	}

	/// <summary>Gets nearPlane property.</summary>
	public virtual float nearPlane { get { return 0.0f; } }

	/// <summary>Gets Far Plane property.</summary>
	public virtual float farPlane { get { return 0.0f; } }

#if UNITY_EDITOR
	/// <summary>Draws Gizmos on Editor mode when SightSensor's instance is selected.</summary>
	private void OnDrawGizmos()
	{
		if(!Application.isPlaying) return;

		Gizmos.color = color.WithAlpha(1.0f);
		Vector3 inflation = Vector3.one * INFLATION;
		int instanceID = 0;

		if(inFOV != null)
		foreach(Collider collider in inFOV.Values)
		{
			instanceID = collider.gameObject.GetInstanceID();
			Gizmos.DrawWireCube(collider.transform.position, collider.bounds.size + inflation);
			
			if(occluders.ContainsKey(instanceID))
			foreach(RaycastHit hitInfo in occluders[instanceID])
			{
				Gizmos.DrawLine(collider.transform.position, hitInfo.transform.position);
			}
		}

		Gizmos.color = color.WithAlpha(0.5f);

		if(inSight != null)
		foreach(Collider collider in inSight.Values)
		{
			instanceID = collider.gameObject.GetInstanceID();
			Gizmos.DrawCube(collider.transform.position, collider.bounds.size + inflation);
		
			if(occluders.ContainsKey(instanceID))
			foreach(RaycastHit hitInfo in occluders[instanceID])
			{
				Gizmos.DrawLine(collider.transform.position, hitInfo.transform.position);
			}
		}
	}
#endif

#region UnityMethods:
	/// <summary>SightSensor's instance initialization.</summary>
	private void Awake()
	{
		Reset();
	}

	/// <summary>Event triggered when this Collider enters another Collider trigger.</summary>
	/// <param name="col">The other Collider involved in this Event.</param>
	private void OnTriggerEnter(Collider col)
	{
		if(!col.gameObject.IsOnLayerMask(detectableMask)) return;

		int instanceID = col.gameObject.GetInstanceID();

		if(inFOV.ContainsKey(instanceID)) inFOV[instanceID] = col;
		else inFOV.Add(instanceID, col);
	}

	/// <summary>Event triggered when this Collider exits another Collider trigger.</summary>
	/// <param name="col">The other Collider involved in this Event.</param>
	private void OnTriggerExit(Collider col)
	{
		if(!col.gameObject.IsOnLayerMask(detectableMask)) return;

		int instanceID = col.gameObject.GetInstanceID();

		if(inSight.ContainsKey(instanceID)) ColliderOutOfSight(col);
		else UnregisterCollider(col);
	}

	/// <summary>Resets SightSensor's instance to its default values.</summary>
	public override void Reset()
	{
		base.Reset();

		if(inFOV == null) inFOV = new Dictionary<int, Collider>();
		else inFOV.Clear();

		if(inSight == null) inSight = new Dictionary<int, Collider>();
		else inSight.Clear();

		if(occluders == null) occluders = new Dictionary<int, RaycastHit[]>();
		else occluders.Clear();
	}
#endregion

	/// <summary>Updates Sensor.</summary>
	protected override void UpdateSensor()
	{
		Collider collider = null;
		HashSet<int> evaluatedIDs = new HashSet<int>();
		int instanceID = 0;

		foreach(int key in inFOV.Keys.ToList())
		{
			if(!inFOV.ContainsKey(key)) continue;

			collider = inFOV[key];
			instanceID = collider.gameObject.GetInstanceID();
			evaluatedIDs.Add(instanceID);

			/// If the Collider inside the FOV is visible, pass to inSight's dictionary and notify it has been seen.
			if(IsVisible(collider)) ColliderSighted(collider);
		}

		foreach(int key in inSight.Keys.ToList())
		{
			if(!inSight.ContainsKey(key)) continue;

			collider = inSight[key];
			instanceID = collider.gameObject.GetInstanceID();

			/// If the collider [not previously on inFOV] is not visible, notify the Collider as Occluded.
			if(!evaluatedIDs.Contains(instanceID) && !IsVisible(collider)) ColliderOccluded(collider);
		}
	}

	/// <summary>Invokes OnColliderSighted's Event and marks the collider in sight.</summary>
	/// <param name="_collider">Collider Sighted.</param>
	protected void ColliderSighted(Collider _collider)
	{
		int instanceID = _collider.gameObject.GetInstanceID();

		if(inFOV.ContainsKey(instanceID)) inFOV.Remove(instanceID);
		if(!inSight.ContainsKey(instanceID))
		{ /// Just invoke the event once if it was not registered on inSight's Dictionary.
			inSight.Add(instanceID, _collider);
			if(onColliderSighted != null) onColliderSighted(_collider);
		}
	}

	/// <summary>Invokes OnColliderOccluded's Event and marks the collider in FOV [but occluded].</summary>
	/// <param name="_collider">Collider Occluded.</param>
	protected void ColliderOccluded(Collider _collider)
	{
		int instanceID = _collider.gameObject.GetInstanceID();

		if(inSight.ContainsKey(instanceID)) inSight.Remove(instanceID);
		if(!inFOV.ContainsKey(instanceID))
		{ /// Just invoke the event once if it was not registered on inFOV's Dictionary.
			inFOV.Add(instanceID, _collider);
			if(onColliderOccluded != null) onColliderOccluded(_collider);
		}
	}

	/// <summary>Invokes OnColliderOutOfSight's Event and marks the collider out of sight.</summary>
	/// <param name="_collider">Collider Sighted.</param>
	/// <param name="_registerOnFOV">Register on inFOV's Dictionary.</param>
	protected void ColliderOutOfSight(Collider _collider, bool _registerOnFOV = false)
	{
		int instanceID = _collider.gameObject.GetInstanceID();

		if(occluders.ContainsKey(instanceID)) occluders.Remove(instanceID);
		if(_registerOnFOV && !inFOV.ContainsKey(instanceID)) inFOV.Add(instanceID, _collider);
		if(inSight.ContainsKey(instanceID))
		{ /// Just invoke the event once if it was registered on inSight's Dictionary.
			inSight.Remove(instanceID);
			if(onColliderOutOfSight != null) onColliderOutOfSight(_collider);
		}
	}

	/// <summary>Unregisters Collider from both inSight and inFOV Dictionaries.</summary>
	/// <param name="_collider">Collider to unregister.</param>
	protected void UnregisterCollider(Collider _collider)
	{
		int instanceID = _collider.gameObject.GetInstanceID();

		inFOV.Remove(instanceID);
		inSight.Remove(instanceID);
		occluders.Remove(instanceID);
	}

	/// <summary>Evaluates if Collider is not occluded by a set of RaycastHits.</summary>
	/// <param name="_collider">Collider to Evaluate.</param>
	/// <returns>True if the Collider is not occluded by any other Collider.</returns>
	protected virtual bool IsVisible(Collider _collider)
	{
		Vector3 origin = transform.position + (transform.forward * nearPlane);
		int instanceID = _collider.gameObject.GetInstanceID();
		RaycastHit[] hitsInfo = null;

		/// Add Occluders' Entry to Dictionary if non-existent.
		if(!occluders.ContainsKey(instanceID)) occluders.Add(instanceID, null);
		
		hitsInfo = occlusionEvaluator.EvaluateOcclusions(origin, _collider.transform.position, detectableMask);
		occluders[instanceID] = hitsInfo;

		/* The Collider is Visible [not occluded] if:
			a) hitsInfo's Array its null.
			b) hitsInfo's Length is equal to 0.
			c) hitsInfo's Length is less than 2 and the first collider registered on the array is the same evaluated collider.
		*/
		return hitsInfo != null ? (hitsInfo.Length == 0 || hitsInfo.Length < 2 && hitsInfo[0].collider == _collider) : true;
	}

	/// <summary>Gets Collider's Occluders.</summary>
	/// <param name="_collider">Collider to get Occluders from.</param>
	/// <returns>Collider's Occluders [if Collider is registered and if it has Occluders].</returns>
	public RaycastHit[] GetColliderOccluders(Collider _collider)
	{
		int instanceID = _collider.gameObject.GetInstanceID();

		return occluders.ContainsKey(instanceID) ? occluders[instanceID] : null;
	}
}
}