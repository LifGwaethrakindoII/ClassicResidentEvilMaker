using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[RequireComponent(typeof(SightSensor))]
public abstract class OcclusionEvaluator : MonoBehaviour
{
	private SightSensor _sightSensor; 	/// <summary>SightSensor's Component.</summary>

	/// <summary>Gets sightSensor Component.</summary>
	public SightSensor sightSensor
	{ 
		get
		{
			if(_sightSensor == null) _sightSensor = GetComponent<SightSensor>();
			return _sightSensor;
		}
	}

	/// <summary>Evaluates occlusions.</summary>
	/// <param name="_origin">Origin of the evaluation.</param>
	/// <param name="_target">Evaluation's target.</param>
	/// <param name="_layerMask">LayerMask to selectively ignore colliders.</param>
	/// <returns>Array of RaycastHits of occluders towards target.</returns>
	public abstract RaycastHit[] EvaluateOcclusions(Vector3 _origin, Vector3 _target, LayerMask _layerMask);
}
}