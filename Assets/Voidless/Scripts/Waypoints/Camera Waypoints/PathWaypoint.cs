using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public enum CurveType 									/// <summary>Curve type that describes how to interpolate towards this Path Waypoint.</summary>
{
	Linear, 											/// <summary>Linear curve type.</summary>
	Cuadratic, 											/// <summary>Cuadratic curve type.</summary>
	Cubic 												/// <summary>Cubic curve type.</summary>
}

/*[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(GizmosSphereWaypoint))]*/
public class PathWaypoint : SphereWaypoint
{
	[Space(5f)]
	[Header("Transition Speed Attributes:")]
	[SerializeField] private float _movementSpeed; 		/// <summary>Movement Speed to this Camera Waypoint.</summary>
	[SerializeField] private float _rotationSpeed; 		/// <summary>Rotation Speed for this Camera Waypoint.</summary>
	[Space(5f)]
	[Header("Beizer Attributes:")]
	[SerializeField] private Vector3 _startTangent; 	/// <summary>Beizer's start tangent.</summary>
	[SerializeField] private Vector3 _endTangent; 		/// <summary>Beizer's end tangent.</summary> 
	[Space(5f)]
	[SerializeField] private CurveType _curveType; 		/// <summary>Waypoint's curve type.</summary>

	/// <summary>Gets and Sets movementSpeed property.</summary>
	public float movementSpeed
	{
		get { return _movementSpeed; }
		set { _movementSpeed = value; }
	}

	/// <summary>Gets and Sets rotationSpeed property.</summary>
	public float rotationSpeed
	{
		get { return _rotationSpeed; }
		set { _rotationSpeed = value; }
	}

	/// <summary>Gets and Sets startTangent property.</summary>
	public Vector3 startTangent
	{
		get { return _startTangent; }
		set { _startTangent = value; }
	}

	/// <summary>Gets and Sets endTangent property.</summary>
	public Vector3 endTangent
	{
		get { return _endTangent; }
		set { _endTangent = value; }
	}

	/// <summary>Gets and Sets curveType property.</summary>
	public CurveType curveType
	{
		get { return _curveType; }
		set { _curveType = value; }
	}
}
}