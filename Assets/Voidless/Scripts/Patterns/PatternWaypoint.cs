using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[System.Serializable]
public struct PatternWaypoint
{
	[SerializeField] private Vector3 _offsetPoint; 		/// <summary>Pattern's Offset Point.</summary>
	[SerializeField] private float _toleranceRadius; 	/// <summary>Tolerance's Radius.</summary>

	/// <summary>Gets and Sets offsetPoint property.</summary>
	public Vector3 offsetPoint
	{
		get { return _offsetPoint; }
		set { _offsetPoint = value; }
	}

	/// <summary>Gets and Sets toleranceRadius property.</summary>
	public float toleranceRadius
	{
		get { return _toleranceRadius; }
		set { _toleranceRadius = value; }
	}

	/// <summary>PatternWaypoint constructor.</summary>
	/// <param name="_offsetPoint">Offset's Point.</param>
	/// <param name="_toleranceRadius">Tolerance's Radius.</param>
	public PatternWaypoint(Vector3 _offsetPoint, float _toleranceRadius) : this()
	{
		offsetPoint = _offsetPoint;
		toleranceRadius = _toleranceRadius;
	}
}
}