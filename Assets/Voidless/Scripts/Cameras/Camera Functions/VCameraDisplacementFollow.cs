using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// \TODO Fix the Smoothie
/// \TODO Update the position as transform.position + GetFocusDirection()
namespace Voidless
{
public class VCameraDisplacementFollow : VCameraFollow
{
	[Space(5f)]
	[Header("Displacement Following's Attributes:")]
	[SerializeField] private NormalizedVector3 _displacementOffset; 	/// <summary>Displacement's Offset.</summary>
	protected Vector3 followVelocity;  									/// <summary>Following's Velocity.</summary>

	/// <summary>Gets and Sets displacementOffset property.</summary>
	public NormalizedVector3 displacementOffset
	{
		get { return _displacementOffset; }
		set { _displacementOffset = value; }
	}

	/// <summary>Follows target Smoothly towards given target [overrides Target's transform].</summary>
	/// <param name="_target">Target to follow.</param>
	public override void FollowTarget(Vector3 _target)
	{
		transform.position = GetDesiredTarget(_target);
		EvaluateTargetProximity();
	}

	/// <summary>Gets Desired Displacement towards given Target.</summary>
	/// <param name="_target">Target.</param>
	/// <returns>Desired displacement's position towards Target.</returns>
	public Vector3 GetDesiredTarget(Vector3 _target)
	{
		float deltaTime = vCamera.GetDeltaTime();
		Vector3 position = Vector3.zero;
		viewportOffset = GetCenterFocusDirection();
		
		viewportOffset = !viewportOffset.IsNaN() ? viewportOffset : Vector3.zero;
		GetFollowingDirection(_target);

		switch(followMode)
		{
			case FollowMode.Instant:
			position = followingDirection;
			break;

			case FollowMode.Smooth:
			position = Vector3.SmoothDamp(
				transform.position,
				followingDirection,
				ref followVelocity,
				followMode != FollowMode.Instant ? followDuration : SMOOTH_TIME_INSTANT, //deltaTime
				(followMode != FollowMode.Instant && limitFollowingSpeed) ? maxFollowSpeed : Mathf.Infinity,
				deltaTime
			);
			break;
		}

		return position;
	}

	/// <returns>Following's Direction.</returns>
	/// <param name="_target">Target's Point.</param>
	/// <returns>Following direction from given origin towards target's point.</returns>
	protected override Vector3 GetFollowingDirection(Vector3 _target)
	{
		Axes3D axesInside = vCamera.GetAxesWhereTargetIsWithin(_target);
		Vector3 offsetPosition = GetOffsetPositionRelativeToTarget(_target);
		Vector3 position = transform.position;

		desiredNonIgnoredDirection = offsetPosition;

		offsetPosition += viewportOffset;

		if(ignoreAxes.HasFlag(Axes3D.X) && axesInside.HasFlag(Axes3D.X)) offsetPosition.x = position.x;
		if(ignoreAxes.HasFlag(Axes3D.Y) && axesInside.HasFlag(Axes3D.Y)) offsetPosition.y = position.y;
		if(ignoreAxes.HasFlag(Axes3D.Z) && axesInside.HasFlag(Axes3D.Z)) offsetPosition.z = position.z;

		followingDirection = offsetPosition;

		return offsetPosition;
	}

	/// <summary>Evaluates Target's Proximity.</summary>
	protected override void EvaluateTargetProximity()
	{
		reachedTarget = (followingDirection - transform.position).sqrMagnitude <= (reachTolerance * reachTolerance);
	}

	/// <param name="_target">Target's Vector.</param>
	/// <returns>Offseted position from target's position.</returns>
	protected virtual Vector3 GetOffsetPositionRelativeToTarget(Vector3 _target)
	{
		Vector3 scaledOffset = (displacementOffset.normalized * vCamera.distanceAdjuster.distance);
		Vector3 point = _target + (relativeToTarget ? (vCamera.targetRetriever.GetTargetRotation() * scaledOffset) : scaledOffset);

		return point;
	}
}
}