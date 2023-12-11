using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices.WindowsRuntime;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Voidless
{
	[CreateAssetMenu(menuName = PATH_ROTATIONFOLLOWMODULE + "Default")]
	public class VCameraRotationFollowModule : VCameraFollowModule
	{
		public const string PATH_ROTATIONFOLLOWMODULE = PATH_FOLLOWMODULE + "Rotation Follow / ";

		[Space(5f)]
		[Header("Rotation Following's Attributes:")]
		[SerializeField] private EulerRotation _rotationOffset; /// <summary>Rotation's Offset.</summary>

		/// <summary>Gets and Sets rotationOffset property.</summary>
		public EulerRotation rotationOffset
		{
			get { return _rotationOffset; }
			set { _rotationOffset = value; }
		}

#if UNITY_EDITOR
		/// <summary>Draws Rotation's Gizmos.</summary>
		private void DrawGizmos(VCamera _camera)
		{
			const float RADIUS_ARC = 0.5f;
			const float COLOR_ALPHA = 0.3f;

			Quaternion rotation = (_camera.transform.rotation * rotationOffset);

			Handles.color = Color.red.WithAlpha(COLOR_ALPHA);
			Handles.DrawSolidArc(_camera.transform.position, _camera.transform.right, _camera.transform.forward, Vector3.Angle(_camera.transform.forward, rotation * Vector3.forward) * Mathf.Sign(rotationOffset.eulerAngles.x), RADIUS_ARC);

			Handles.color = Color.green.WithAlpha(COLOR_ALPHA);
			Handles.DrawSolidArc(_camera.transform.position, _camera.transform.up, _camera.transform.right, Vector3.Angle(_camera.transform.right, rotation * Vector3.right) * Mathf.Sign(rotationOffset.eulerAngles.y), RADIUS_ARC);

			Handles.color = Color.blue.WithAlpha(COLOR_ALPHA);
			Handles.DrawSolidArc(_camera.transform.position, _camera.transform.forward, _camera.transform.up, Vector3.Angle(_camera.transform.up, rotation * Vector3.up) * Mathf.Sign(rotationOffset.eulerAngles.z), RADIUS_ARC);
		}
#endif

	    /// <summary>Gets Target rotation.</summary>
		/// <param name="_camera">VCamera that requests the target.</param>
		/// <param name="_target">Target that will have a conversion inside this function.</param>
	    public virtual Quaternion GetTargetRotation(VCamera _camera, Vector3 _target, out Vector3 direction)
	    {
	    	direction = GetFollowingDirection(_camera, _target);
	    	Quaternion rotation = Quaternion.LookRotation(direction) * rotationOffset;
	        float dt = _camera.GetDeltaTime();
	    	
	    	switch(followMode)
	        {
	            case FollowMode.Smooth:
	            	switch(speedScalar)
	            	{
	            		case SpeedScalar.OneScalarForAllAxes:
	            			rotation = Quaternion.RotateTowards(_camera.transform.rotation, rotation, acceleration * dt);
	            		break;

	            		case SpeedScalar.ScalarPerAxis:
	            			rotation = VQuaternion.RotateTowards(
			                	_camera.transform.rotation,
			                	rotation,
			                	xAxisFollowData.acceleration,
			                	yAxisFollowData.acceleration,
			                	zAxisFollowData.acceleration,
			                	dt,
			                	Vector3.up
			                );
	            		break;
	            	}
	            break;
	        }

	        return rotation;
	    }

	    /// <summary>Gets instant-time Target position.</summary>
	    /// <param name="_camera">VCamera that requests the target.</param>
		/// <param name="_target">Target that will have a conversion inside this function.</param>
	    public virtual Quaternion GetInstantTargetRotation(VCamera _camera, Vector3 _target)
	    {
	    	return Quaternion.LookRotation(GetFollowingDirection(_camera, _target)) * rotationOffset;
	    }

	    /// <summary>Gets Following's Direction.</summary>
		/// <returns>Following direction from camera's position towards target's point.</returns>
		public override Vector3 GetFollowingDirection(VCamera _camera, Vector3 _target)
		{
			Vector3 direction = _target - _camera.transform.position;
		
			if(ignoreAxes.HasFlag(Axes3D.X)) direction.x = 0.0f;
			if(ignoreAxes.HasFlag(Axes3D.Y)) direction.y = 0.0f;
			if(ignoreAxes.HasFlag(Axes3D.Z)) direction.z = 0.0f;

			return direction;
		}

		/// <summary>Evaluates Target's Proximity.</summary>
		/// <param name="_camera">VCamera's reference
		/// <param name="_target">Target to evaluate against VCamera.</param>.</param>
		protected override bool TargetProximityEvaluation(VCamera _camera, Vector3 _target)
		{
			Vector3 direction = GetFollowingDirection(_camera, _target);
			return Vector3.Dot(_camera.transform.forward, direction.normalized) >= reachThreshold;
		}
	}
}