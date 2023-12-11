using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
	public enum FollowMode
	{
		Smooth,
		Instant,
	}

	public enum SpeedScalar
	{
		OneScalarForAllAxes,
		ScalarPerAxis
	}

	[CreateAssetMenu(menuName = PATH_FOLLOWMODULE)]
	public abstract class VCameraFollowModule : VCameraModule
	{
		public const string PATH_FOLLOWMODULE = PATH_ROOT + " Camera Follow Modules / ";

		[Space(5f)]
		[Header("Following's Attributes:")]
		[SerializeField] private FollowMode _followMode;
		[SerializeField] private SpeedChange _speedChange;
		[SerializeField] private SpeedScalar _speedScalar;
		[SerializeField] private bool _relativeToTarget;
		[SerializeField] protected Axes3D _ignoreAxes;
		[SerializeField] private float _acceleration;
		[SerializeField] [Range(0.0f, 1.0f)] private float _reachThreshold;
		[Space(5f)]
		[SerializeField] protected VCameraFollowAxisData xAxisFollowData;
		[SerializeField] protected VCameraFollowAxisData yAxisFollowData;
		[SerializeField] protected VCameraFollowAxisData zAxisFollowData;
		private Vector3 _viewportOffset;
		private bool _reachedTarget;
		protected Vector3 velocity;

#region Getters/Setters:
		/// <summary>Gets and Sets followMode property.</summary>
		public FollowMode followMode
		{
			get { return _followMode; }
			set { _followMode = value; }
		}

		/// <summary>Gets and Sets speedChange property.</summary>
		public SpeedChange speedChange
		{
			get { return _speedChange; }
			set { _speedChange = value; }
		}

		/// <summary>Gets and Sets speedScalar property.</summary>
		public SpeedScalar speedScalar
		{
			get { return _speedScalar; }
			set { _speedScalar = value; }
		}

		/// <summary>Gets and Sets relativeToTarget property.</summary>
		public bool relativeToTarget
		{
			get { return _relativeToTarget; }
			set { _relativeToTarget = value; }
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

		/// <summary>Gets and Sets acceleration property.</summary>
		public float acceleration
		{
			get { return _acceleration; }
			set { _acceleration = value; }
		}

		/// <summary>Gets and Sets reachThreshold property.</summary>
		public float reachThreshold
		{
			get { return _reachThreshold; }
			set { _reachThreshold = Mathf.Clamp(value, 0.0f, 1.0f); }
		}

		/// <summary>Gets and Sets viewportOffset property.</summary>
		public Vector3 viewportOffset
		{
			get { return _viewportOffset; }
			set { _viewportOffset = value; }
		}
#endregion

		/// <summary>Resets Component.</summary>
		public override void Reset()
		{
			followMode = FollowMode.Smooth;
			relativeToTarget = true;
			ignoreAxes = Axes3D.None;
		}

		/// <returns>Focus' Direction, ignoring the flagged axes.</returns>
		public Vector3 GetFocusDisplacement(VCamera _camera)
		{
			return _camera.GetFocusDisplacement();
		}

		/// <summary>Gets Following's Direction.</summary>
		/// <returns>Following direction from camera's position towards target's point.</returns>
		public abstract Vector3 GetFollowingDirection(VCamera _camera, Vector3 _target);

		/// <summary>Evaluates Target's Proximity.</summary>
		/// <param name="_camera">VCamera's reference
		/// <param name="_target">Target to evaluate against VCamera.</param>.</param>
		protected abstract bool TargetProximityEvaluation(VCamera _camera, Vector3 _target);

		/// <summary>Accelerates Vector A towards Vector B using the Follow-Axis Data for each axis.</summary>
		/// <param name="a">Vector A.</param>
		/// <param name="b">Vector B.</param>
		/// <param name="dt">Time's Delta.</param>
		/// <returns>Vector A accelerated towards target Vector B.</returns>
		public virtual Vector3 AccelerateTowards(Vector3 a, Vector3 b, float dt)
		{
			float x = xAxisFollowData.AccelerateTowards(a.x, b.x, dt);
			float y = yAxisFollowData.AccelerateTowards(a.y, b.y, dt);
			float z = zAxisFollowData.AccelerateTowards(a.z, b.z, dt);

			return new Vector3(x, y, z);
		}
	}
}