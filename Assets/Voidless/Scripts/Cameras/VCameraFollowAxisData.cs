using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
	[Serializable]
	public struct VCameraFollowAxisData
	{
		public FollowMode followMode;
		public SpeedChange speedChange;
		public float acceleration;
		[HideInInspector, SerializeField] public float velocity;

		/// <summary>VCameraFollowAxisData's constructor.</summary>
		/// <param name="_acceleration">Acceleration value.</param>
		/// <param name="_followMode">Follow Mode [FollowMode.Smooth by default].</param>
		/// <param name="_speedChange">Speed-change mode [SpeedChange.Acceleration by default].</param>
		public VCameraFollowAxisData(float _acceleration, FollowMode _followMode = FollowMode.Smooth, SpeedChange _speedChange = SpeedChange.Acceleration)
		{
			followMode = _followMode;
			speedChange = _speedChange;
			acceleration = _acceleration;
			velocity = 0.0f;
		}

		/// <summary>Resets internal data.</summary>
		public void Reset()
		{
			velocity = 0.0f;
		}

		/// <summary>Accelerates value towards target.</summary>
		/// <param name="x">Value to accelerate towards the target.</param>
		/// <param name="y">Target value.</param>
		/// <param name="dt">Time's Delta.</param>
		/// <returns>Value accelerated towards target.</returns>
		public float AccelerateTowards(float x, float y, float dt)
		{
			return followMode == FollowMode.Instant ? y : VMath.AccelerateTowards(x, y, ref velocity, acceleration, dt, speedChange, VMath.EPSILON, false);
		}
	}
}