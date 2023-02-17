using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/*
	1.- Camera as an API:
		- 
*/

/// \TODO Fix the Smoothie
namespace Voidless
{
public class VCameraRotationFollow : VCameraFollow
{
	public const float ANGLE_LOOK_AT_EACH_OTHER = 180.0f; 	/// <summary>Look at each other's angle.</summary>

	[Space(5f)]
	[Header("Rotation Following's Attributes:")]
	[SerializeField]
	[Range(0.0f, 90.0f)] private float _angleTolerance; 	/// <summary>Tolerance of degrees if the Third Person Character is heading towards the camera.</summary>
	[SerializeField] private EulerRotation _rotationOffset; /// <summary>Rotation's Offset.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Gizmos' Attributes (Rotation Follow Component):")]
	[SerializeField] private float gizmosRadius; 			/// <summary>Gizmos' Radius.</summary>
	[SerializeField]
	[Range(0.0f, 1.0f)] private float colorAlpha; 			/// <summary>Color's Alpha.</summary>
#endif
	protected float angularSpeed; 							/// <summary>Angular Speed's Reference.</summary>

	/// <summary>Gets and Sets rotationOffset property.</summary>
	public EulerRotation rotationOffset
	{
		get { return _rotationOffset; }
		set { _rotationOffset = value; }
	}

	/// <summary>Gets and Sets angleTolerance property.</summary>
	public float angleTolerance
	{
		get { return _angleTolerance; }
		set { _angleTolerance = Mathf.Clamp(0.0f, 90.0f, value); }
	}

#if UNITY_EDITOR
	/// <summary>Draws Rotation's Gizmos.</summary>
	private void OnDrawGizmos()
	{
		Quaternion rotation = (transform.rotation * rotationOffset);

		Handles.color = Color.red.WithAlpha(colorAlpha);
		Handles.DrawSolidArc(transform.position, transform.right, transform.forward, Vector3.Angle(transform.forward, rotation * Vector3.forward) * Mathf.Sign(rotationOffset.eulerAngles.x), gizmosRadius);

		Handles.color = Color.green.WithAlpha(colorAlpha);
		Handles.DrawSolidArc(transform.position, transform.up, transform.right, Vector3.Angle(transform.right, rotation * Vector3.right) * Mathf.Sign(rotationOffset.eulerAngles.y), gizmosRadius);

		Handles.color = Color.blue.WithAlpha(colorAlpha);
		Handles.DrawSolidArc(transform.position, transform.forward, transform.up, Vector3.Angle(transform.up, rotation * Vector3.up) * Mathf.Sign(rotationOffset.eulerAngles.z), gizmosRadius);
	}
#endif

	/// <summary>Resets Component.</summary>
	protected override void Reset()
	{
		base.Reset();
#if UNITY_EDITOR
		gizmosRadius = 0.5f;
		colorAlpha = 0.2f;
#endif
	}

	/// <summary>Follows Target [takes into account component's parameters, but target overrides the Target's Transform].</summary>
	/// <param name="_target">Target to follow.</param>
	public override void FollowTarget(Vector3 _target)
	{
		/// if instant, the rotation won't change.
		GetFollowingDirection(_target);
		Quaternion rotation = Quaternion.LookRotation(followingDirection) * rotationOffset;

		switch(followMode)
		{
			case FollowMode.Smooth:
			float deltaAngle = Quaternion.Angle(vCamera.transform.rotation, rotation);

			if(deltaAngle > 0.0f)
			{
				float deltaTime = vCamera.GetDeltaTime();
				float t = Mathf.SmoothDampAngle(
					deltaAngle,
					0.0f,
					ref angularSpeed,
					followDuration, //deltaTime
					limitFollowingSpeed ? maxFollowSpeed : Mathf.Infinity,
					deltaTime
				);
				t = (1.0f - (t / deltaAngle));

				rotation = Quaternion.Lerp(vCamera.transform.rotation, rotation, t);
			}
			break;
		}
		
		vCamera.transform.rotation = (rotation);
		EvaluateTargetProximity();
	}

	/// <summary>Gets Following's Direction.</summary>
	/// <param name="_target">Target's Point.</param>
	/// <returns>Following direction from given origin towards target's point.</returns>
	protected override Vector3 GetFollowingDirection(Vector3 _target)
	{
		Vector3 direction = (_target - transform.position);

		desiredNonIgnoredDirection = direction;
	
		if(ignoreAxes.HasFlag(Axes3D.X)) direction.x = 0.0f;
		if(ignoreAxes.HasFlag(Axes3D.Y)) direction.y = 0.0f;
		if(ignoreAxes.HasFlag(Axes3D.Z)) direction.z = 0.0f;

		followingDirection = direction;

		return direction;
	}

	/// <summary>Evaluates Target's Proximity.</summary>
	protected override void EvaluateTargetProximity()
	{
		reachedTarget = Vector3.Dot(vCamera.transform.forward, followingDirection) >= reachTolerance;
	}

	/// <returns>True if camera and target are faceing each other, considering the tolerance angle, false otherwise or if there is no target.</returns>
	public virtual bool CameraAndTargetLookingAtEachOther()
	{
		//if(target != null)
		{
			/*float angle = Vector3.Angle(transform.forward, target.forward);
			return ((angle <= ANGLE_LOOK_AT_EACH_OTHER) && (angle >= ANGLE_LOOK_AT_EACH_OTHER - angleTolerance));*/
		}
		/*else */return false;
	}
}
}