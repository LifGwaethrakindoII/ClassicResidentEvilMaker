using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public enum FollowMode 													/// <summary>Follow Modes.</summary>
{
	Smooth, 															/// <summary>Smooth's Follow Mode.</summary>
	Instant, 															/// <summary>Instant's Follow Mode.</summary>
	//PhysicsSmooth 													/// <summary>Physics-Smooth's Follow Mode.</summary>
}

public abstract class VCameraFollow : VCameraComponent
{
	protected const float SMOOTH_TIME_INSTANT = 0.1f; 					/// <summary>Instant's Smooth Time.</summary>

	/*[SerializeField] private VCameraFollowAxisData _xAxisFollowData; 	/// <summary>Follow's Data for the X's Axis.</summary>
	[SerializeField] private VCameraFollowAxisData _yAxisFollowData; 	/// <summary>Follow's Data for the Y's Axis.</summary>
	[SerializeField] private VCameraFollowAxisData _zAxisFollowData; 	/// <summary>Follow's Data for the Z's Axis.</summary>*/

	[Space(5f)]
	[Header("Following's Attributes:")]
	[SerializeField] private FollowMode _followMode; 					/// <summary>Follow's Mode.</summary>
	[SerializeField] private bool _relativeToTarget; 					/// <summary>Make the following relative to the target?.</summary>
	[SerializeField] private bool _limitFollowingSpeed; 				/// <summary>Limit Maximum Following's Speed.</summary>
	[SerializeField] protected Axes3D _ignoreAxes; 						/// <summary>Axes to ignore when following.</summary>
	[SerializeField]
	[Range(0.1f, 1.0f)] private float _followDuration; 					/// <summary>Follow's Duration.</summary>
	[SerializeField] private float _maxFollowSpeed; 					/// <summary>Maximum's Following Speed.</summary>
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _reachTolerance; 					/// <summary>Reach's Tolerance towards target.</summary>
	private Vector3 _followingDirection; 								/// <summary>Current Frame's Following Direction.</summary>
	private Vector3 _desiredNonIgnoredDirection; 						/// <summary>Current Frame's Following Direction without ignoring axes.</summary>
	private Vector3 _viewportOffset; 									/// <summary>Viewport's Offset.</summary>
	private bool _reachedTarget; 										/// <summary>Has this follow component reached its target.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets followMode property.</summary>
	public FollowMode followMode
	{
		get { return _followMode; }
		set { _followMode = value; }
	}

	/// <summary>Gets and Sets relativeToTarget property.</summary>
	public bool relativeToTarget
	{
		get { return _relativeToTarget; }
		set { _relativeToTarget = value; }
	}

	/// <summary>Gets and Sets limitFollowingSpeed property.</summary>
	public bool limitFollowingSpeed
	{
		get { return _limitFollowingSpeed; }
		set { _limitFollowingSpeed = value; }
	}

	/// <summary>Gets and Sets reachedTarget property.</summary>
	public bool reachedTarget
	{
		get { return _reachedTarget; }
		protected set { _reachedTarget = value; }
	}

	/// <summary>Gets and Sets ignoreAxes property.</summary>
	public Axes3D ignoreAxes
	{
		get { return _ignoreAxes; }
		set { _ignoreAxes = value; }
	}

	/// <summary>Gets and Sets followDuration property.</summary>
	public float followDuration
	{
		get { return _followDuration; }
		set { _followDuration = value; }
	}

	/// <summary>Gets and Sets maxFollowSpeed property.</summary>
	public float maxFollowSpeed
	{
		get { return limitFollowingSpeed ? _maxFollowSpeed : Mathf.Infinity; }
		set { _maxFollowSpeed = value; }
	}

	/// <summary>Gets reachTolerance property.</summary>
	public float reachTolerance { get { return _reachTolerance; } }

	/// <summary>Gets and Sets followingDirection property.</summary>
	public Vector3 followingDirection
	{
		get { return _followingDirection; }
		protected set { _followingDirection = value; }
	}

	/// <summary>Gets and Sets desiredNonIgnoredDirection property.</summary>
	public Vector3 desiredNonIgnoredDirection
	{
		get { return _desiredNonIgnoredDirection; }
		set { _desiredNonIgnoredDirection = value; }
	}

	/// <summary>Gets and Sets viewportOffset property.</summary>
	public Vector3 viewportOffset
	{
		get { return _viewportOffset; }
		set { _viewportOffset = value; }
	}
#endregion

	/// <summary>Resets Component.</summary>
	protected virtual void Reset()
	{
		followMode = FollowMode.Smooth;
		relativeToTarget = true;
		limitFollowingSpeed = true;
		ignoreAxes = Axes3D.None;
		followDuration = 1.0f;
		maxFollowSpeed = 100.0f;
	}

	/// <summary>Follows target.</summary>
	public void FollowTarget()
	{
		FollowTarget(vCamera.targetRetriever.GetTargetPosition());
	}

	/// <summary>Follows target [overrides Target's transform].</summary>
	/// <param name="_target">Target to follow.</param>
	public abstract void FollowTarget(Vector3 _target);

	/// <summary>Gets Following's Direction.</summary>
	/// <param name="_target">Target to follow.</param>
	/// <returns>Following direction from camera's position towards target's point.</returns>
	protected Vector3 GetFollowingDirection()
	{
		return GetFollowingDirection(vCamera.targetRetriever.GetTargetPosition());
	}

	/// <summary>Gets Following's Direction.</summary>
	/// <returns>Following direction from camera's position towards target's point.</returns>
	protected abstract Vector3 GetFollowingDirection(Vector3 _target);

	/// <summary>Evaluates Target's Proximity.</summary>
	protected abstract void EvaluateTargetProximity();

	/// <returns>Focus' Direction, ignoring the flagged axes.</returns>
	public Vector3 GetCenterFocusDirection()
	{
		return vCamera.GetCenterFocusDirection();
	}

	/// <summary>Copies this componen's stats into another VCameraFollow component.</summary>
	/// <param name="_cameraFollow">Component that will have the same stats as this component.</param>
	public void CopyStatsTo(VCameraFollow _cameraFollow)
	{
		_cameraFollow.followMode = followMode;
		_cameraFollow.limitFollowingSpeed = limitFollowingSpeed;
		_cameraFollow.followDuration = followDuration;
		_cameraFollow.maxFollowSpeed = maxFollowSpeed;
	}
}
}