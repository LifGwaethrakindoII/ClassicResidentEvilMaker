using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class RaycastOcclusionEvaluator : OcclusionEvaluator
{
	/// <summary>Evaluates occlusions.</summary>
	/// <param name="_origin">Origin of the evaluation.</param>
	/// <param name="_target">Evaluation's target.</param>
	/// <param name="_layerMask">LayerMask to selectively ignore colliders.</param>
	/// <returns>Array of RaycastHits of occluders towards target.</returns>
	public override RaycastHit[] EvaluateOcclusions(Vector3 _origin, Vector3 _target, LayerMask _layerMask)
	{
		Vector3 direction = _target - _origin;
		Ray ray = new Ray(_origin, direction);

		return Physics.RaycastAll(ray, direction.magnitude, _layerMask);
	}	
}
}