using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class MiddlePointBetweenTransformsTargetRetriever : VCameraTargetRetriever
{
	[Space(5f)]
	[SerializeField] private bool _weightedBounds; 		/// <summary>Take the targets' weights into consideration for their bounds?.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Gizmos' Attributes:")]
	[SerializeField] private Color gizmosColor; 		/// <summary>Gizmos' Color.</summary>
#endif
	private Dictionary<int, VCameraTarget> _targets; 	/// <summary>Targets' Transforms.</summary>
	private List<Bounds> _boundsList; 					/// <summary>Targets' Bounds' List.</summary>

	/// <summary>Gets and Sets weightedBounds property.</summary>
	public bool weightedBounds
	{
		get { return _weightedBounds; }
		set { _weightedBounds = value; }
	}

	/// <summary>Gets and Sets target property.</summary>
	public override VCameraTarget target
	{
		get { return _target; }
		set
		{
			_target = value;
			
			if(targets == null) targets = new Dictionary<int, VCameraTarget>();

			int ID = _target.GetInstanceID();
			bool contains = targets.ContainsKey(ID);

			if(_target != null && !contains)
			{
				targets.Add(ID, _target);
			
			} else if(_target == null && contains)
			{
				targets.Remove(ID);
			}
		}
	}

	/// <summary>Gets and Sets targets property.</summary>
	public Dictionary<int, VCameraTarget> targets
	{
		get
		{
#if UNITY_EDITOR
			if(_targets == null) _targets = new Dictionary<int, VCameraTarget>(); 
#endif
			return _targets;
		}
		private set { _targets = value; }
	}

	/// <summary>Gets and Sets boundsList property.</summary>
	public List<Bounds> boundsList
	{
		get { return _boundsList; }
		private set { _boundsList = value; }
	}

#if UNITY_EDITOR
	/// <summary>Draws Gizmos on Editor mode.</summary>
	private void OnDrawGizmos()
	{
		if(!Application.isPlaying) return;
		
		Gizmos.color = gizmosColor;
		Bounds bounds = GetTargetBounds();
		VGizmos.DrawBounds(bounds);

		Vector3 p1 = new Vector3(
			bounds.min.x,
			bounds.min.y + bounds.extents.y,
			bounds.center.z
		);
		Vector3 p2 = new Vector3(
			bounds.max.x,
			bounds.min.y + bounds.extents.y,
			bounds.center.z
		);
		Vector3 p3 = new Vector3(
			bounds.min.x + bounds.extents.x,
			bounds.min.y,
			bounds.center.z
		);
		Vector3 p4 = new Vector3(
			bounds.min.x + bounds.extents.x,
			bounds.max.y,
			bounds.center.z
		);
		Gizmos.DrawLine(p1, p2);
		Gizmos.DrawLine(p3, p4);
	}
#endif

	/// <summary>MiddlePointBetweenTransformsTargetRetriever's instance initialization.</summary>
	protected override void Awake()
	{
		if(targets == null) targets = new Dictionary<int, VCameraTarget>();
		if(boundsList == null) boundsList = new List<Bounds>();

		if(target == null) return;

		int ID = target.GetInstanceID();

		if(!targets.ContainsKey(ID)) targets.Add(ID, target);
	}

	/// <summary>Clears all targets' references.</summary>
	public void ClearTargets()
	{
		targets.Clear();
	}

	/// <summary>Adds Target's Transform into the internal dictionary.</summary>
	/// <param name="_target">Target Transform to add.</param>
	public void AddTarget(VCameraTarget _target)
	{
		if(_target == null) return;

		int instanceID = _target.GetInstanceID();

		if(!targets.ContainsKey(instanceID))
		targets.Add(instanceID, _target);
	}

	/// <summary>Removes Target's Transform into the internal dictionary.</summary>
	/// <param name="_target">Target Transform to add.</param>
	public void RemoveTarget(VCameraTarget _target)
	{
		if(_target == null) return;
		
		int instanceID = _target.GetInstanceID();

		if(targets.ContainsKey(instanceID))
		targets.Remove(instanceID);
	}

	/// <returns>True if has any active targets.</returns>
	public bool HasActiveTargets()
	{
		if(targets == null || targets.Count == 0) return false;

		foreach(VCameraTarget target in targets.Values)
		{
			if(targets != null && target.gameObject.activeSelf)
			return true;
		}

		return false;
	}

	/// <returns>Camera's Target.</returns>
	public override Vector3 GetTargetPosition()
	{
		if(targets == null) return Vector3.zero;

		Vector3 position = transform.position;
		Vector3 positions = Vector3.zero;
		float w = 0.0f;
		float count = 0.0f;

		foreach(VCameraTarget target in targets.Values)
		{
			if(target == null || !target.gameObject.activeSelf) continue;

			w = target.weight;

			if(w <= 0.0f) continue;

			positions += Vector3.Lerp(position, (position +  (target.GetPosition() - position)), w);
			count++;
		}

		if(count == 0.0f) positions = transform.position;

		return count > 1.0f ? positions / count : positions;
	}

	/// <returns>Target's Rotation.</returns>
	public override Quaternion GetTargetRotation()
	{
		return Quaternion.identity;
	}

	/// <returns>Target's Bounds.</returns>
	public override Bounds GetTargetBounds()
	{
		if(targets == null) return target != null ? target.GetBounds() : default(Bounds);

		boundsList.Clear();
		/*Bounds[] targetsBounds = new Bounds[targets.Count];
		int i = 0;*/

		foreach(VCameraTarget target in targets.Values)
		{
			Bounds bounds = target.GetBounds();

			if(weightedBounds)
			{
				Vector3 p = transform.position;
				float w = target.weight;

				if(w <= 0.0f) continue;

				bounds.center = Vector3.Lerp(p, (p + (target.GetBoundsCenter() - p)), w);
				boundsList.Add(bounds);
			}
			else boundsList.Add(bounds);

			/*targetsBounds[i] = bounds;
			i++;*/
		}

		return VBounds.GetBoundsToFitSet(boundsList.ToArray());
		//return VBounds.GetBoundsToFitSet(targetsBounds);
	}
}
}