using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*===========================================================================
**
** Class:  VCameraDistanceAdjuster
**
** Purpose: This component contains many possible
** VCameraDistanceCalculatorModules from wich it calculates the best distance
** for the Camera in that moment.
**
**
** Author: Lîf Gwaethrakindo
**
===========================================================================*/
namespace Voidless
{
	public class VCameraDistanceAdjuster : VCameraComponent
	{
		[Space(5f)]
		[Header("Distance-Calculator Modules:")]
		[SerializeField] private List<VCameraDistanceCalculatorModule> _distanceCalculatorModules;
		[Space(5f)]
		[SerializeField] private FloatRange _distanceRange;
		[SerializeField] private float _acceleration;
		[SerializeField] private SpeedChange _speedChange;
		private float _distance;
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
		public virtual void UpdateDistance(VCamera _camera, Vector3 _targetPosition)
		{
			Vector3 target = _targetPosition;
			float currentDistance = distance;
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

			bestDistance = distanceRange.Clamp(bestDistance);
			distance = VMath.AccelerateTowards(currentDistance, bestDistance, ref velocity, acceleration, _camera.GetDeltaTime(), speedChange);
			//return distance;
		}
	}
}