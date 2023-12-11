using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
	[CreateAssetMenu(menuName = PATH_FOLLOWMODULE + "Default")]
	public class VCameraDisplacementFollowModule : VCameraFollowModule
	{
		[Space(5f)]
		[Header("Displacement Following's Attributes:")]
		[SerializeField] private Vector3 _offset;

		/// <summary>Gets and Sets offset property.</summary>
		public Vector3 offset
		{
			get { return _offset; }
			set { _offset = value; }
		}

		/// <summary>Gets Target Position.</summary>
		/// <param name="_camera">VCamera that requests the target.</param>
		/// <param name="_target">Target that will have a conversion inside this function.</param>
		public virtual Vector3 GetTargetPosition(VCamera _camera, Vector3 _target)
		{
			float dt = _camera.GetDeltaTime();
			Vector3 cameraPosition = _camera.transform.position;
			Vector3 direction = GetFollowingDirection(_camera, _target);
			Vector3 position = cameraPosition + direction;

			switch(followMode)
			{
				case FollowMode.Smooth:
					switch(speedScalar)
					{
						case SpeedScalar.OneScalarForAllAxes:
							position = VVector3.AccelerateTowards(cameraPosition, position, ref velocity, acceleration, dt, speedChange);
						break;

						case SpeedScalar.ScalarPerAxis:
							position = AccelerateTowards(cameraPosition, position, dt);
						break;
					}
				break;
			}

			return position;
		}

		/// <summary>Gets instant-time Target position.</summary>
		/// <param name="_camera">VCamera's reference.</param>
		/// <param name="_target">VCamera's target.</param>
		public virtual Vector3 GetInstantTargetPosition(VCamera _camera, Vector3 _target)
		{
			return _camera.transform.position + GetFollowingDirection(_camera, _target);
		}

		/// <returns>Following's Direction.</returns>
		/// <param name="_target">Target's Point.</param>
		/// <returns>Following direction from given origin towards target's point.</returns>
		public override Vector3 GetFollowingDirection(VCamera _camera, Vector3 _target)
		{
			Axes3D axesInside = _camera.GetAxesWhereTargetIsWithin(_target);
			Vector3 offsetPosition = GetOffsetPositionRelativeToTarget(_camera, _target);
			Vector3 position = _camera.transform.position;
			/*Vector3 viewportOffset = _camera.GetFocusDisplacement().NaNFilter();

			offsetPosition += viewportOffset;*/

			/// If an axis is marked to ignore, pass 0.0f instead.
			if(ignoreAxes.HasFlag(Axes3D.X) && axesInside.HasFlag(Axes3D.X)) offsetPosition.x = position.x;
			if(ignoreAxes.HasFlag(Axes3D.Y) && axesInside.HasFlag(Axes3D.Y)) offsetPosition.y = position.y;
			if(ignoreAxes.HasFlag(Axes3D.Z) && axesInside.HasFlag(Axes3D.Z)) offsetPosition.z = position.z;

			return offsetPosition - position;
		}

		/// <summary>Evaluates Target's Proximity.</summary>
		/// <param name="_camera">VCamera's reference
		/// <param name="_target">Target to evaluate against VCamera.</param>.</param>
		protected override bool TargetProximityEvaluation(VCamera _camera, Vector3 _target)
		{
			Vector3 position = GetInstantTargetPosition(_camera, _target);
			return (position - _camera.transform.position).sqrMagnitude <= (reachThreshold * reachThreshold);
		}

		/// <summary>Gets offseted position relative to VCamera's target.</summary>
		/// <param name="_camera">VCamera's reference.</param>
		/// <param name="_target">Target's Vector.</param>
		/// <returns>Offseted position from target's position.</returns>
		protected virtual Vector3 GetOffsetPositionRelativeToTarget(VCamera _camera, Vector3 _target)
		{
			Vector3 scaledOffset = (offset.normalized * _camera.GetTargetDistance());

			if (_camera.targetRetrieverModule != null && relativeToTarget)
			scaledOffset = _camera.targetRetrieverModule.GetTargetRotation(_camera) * scaledOffset;

			return _target + scaledOffset;
		}
	}
}