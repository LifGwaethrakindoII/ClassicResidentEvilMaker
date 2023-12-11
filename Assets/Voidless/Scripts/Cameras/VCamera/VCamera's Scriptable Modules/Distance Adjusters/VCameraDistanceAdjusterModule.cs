using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
	[CreateAssetMenu(menuName = PATH_DISTANCEADJUSTER + "Default")]
	public class VCameraDistanceAdjusterModule : VCameraModule
	{
		public const string PATH_DISTANCEADJUSTER = PATH_ROOT + "Distance Adjusters / ";

		[Space(5f)]
		[SerializeField] private List<VCameraDistanceCalculatorModule> _distanceCalculatorModules;
		[Space(5f)]
		[SerializeField] private FloatRange _distanceRange;
		[SerializeField] private float _acceleration;
		[SerializeField] private SpeedChange _speedChange;
		private float _distance;
		protected float currentDistance;
		protected float velocity;

		/// <summary>Gets and Sets distanceCalculatorModules property.</summary>
		public List<VCameraDistanceCalculatorModule> distanceCalculatorModules
		{
			get { return _distanceCalculatorModules; }
			set { _distanceCalculatorModules = value; }
		}

		/// <summary>Gets and Sets distanceRange property.</summary>
		public FloatRange distanceRange
		{
			get { return _distanceRange; }
			set { _distanceRange = value; }
		}

		/// <summary>Gets and Sets acceleration property.</summary>
		public float acceleration
		{
			get { return _acceleration; }
			set { _acceleration = value; }
		}

		/// <summary>Gets and Sets distance property.</summary>
		public float distance
		{
			get { return _distance; }
			set { _distance = Mathf.Clamp(value, distanceRange.Min(), distanceRange.Max()); }
		}

		/// <summary>Gets and Sets speedChange property.</summary>
		public SpeedChange speedChange
		{
			get { return _speedChange; }
			set { _speedChange = value; }
		}

		/// <summary>Updates Distance defined by occlusion's handling.</summary>
		public virtual float UpdateDistance(VCamera _camera, Vector3 _targetPosition)
		{
			Vector3 target = _targetPosition;
			float bestDistance = distanceRange.Min();

			if(distanceCalculatorModules.Count > 0)
			{
				float min = Mathf.Infinity;

				foreach(VCameraDistanceCalculatorModule calculator in distanceCalculatorModules)
				{
					float d = calculator.GetCalculatedDistance(target);
					if(d < min) min = d;
				}

				bestDistance = min;
			}

			float x = bestDistance;
			float y = distanceRange.Clamp(bestDistance);

			distance = VMath.AccelerateTowards(x, y, ref velocity, acceleration, _camera.GetDeltaTime(), speedChange);
			return distance;
		}
	}
}