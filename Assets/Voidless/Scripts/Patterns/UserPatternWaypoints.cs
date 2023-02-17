using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[System.Serializable]
public struct UserPatternWaypoints
{
	[SerializeField] private PatternWaypoint _leftWaypoint; 	/// <summary>Left Hand's Waypoint.</summary>
	[SerializeField] private PatternWaypoint _rightWaypoint; 	/// <summary>Right Hand's Waypoint.</summary>

	/// <summary>Gets and Sets leftWaypoint property.</summary>
	public PatternWaypoint leftWaypoint
	{
		get { return _leftWaypoint; }
		set { _leftWaypoint = value; }
	}

	/// <summary>Gets and Sets rightWaypoint property.</summary>
	public PatternWaypoint rightWaypoint
	{
		get { return _rightWaypoint; }
		set { _rightWaypoint = value; }
	}

	/// <summary>UserPatternWaypoints constructor.</summary>
	/// <param name="_leftWaypoint">Left Hand's Waypoint.</param>
	/// <param name="_rightWaypoint">Right Hand's Waypoint.</param>
	public UserPatternWaypoints(PatternWaypoint _rightWaypoint, PatternWaypoint _leftWaypoint) : this()
	{
		rightWaypoint = _rightWaypoint;
		leftWaypoint = _leftWaypoint;
	}
}
}