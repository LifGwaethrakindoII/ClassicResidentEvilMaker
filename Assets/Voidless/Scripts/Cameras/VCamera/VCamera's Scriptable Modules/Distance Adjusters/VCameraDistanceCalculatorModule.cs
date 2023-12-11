using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
	public abstract class VCameraDistanceCalculatorModule : VCameraModule
	{
		public const string PATH_DISTANCECALCULATOR = PATH_ROOT + "Distance Calculators / ";

		/// <summary>Gets Calculated distance towards given target.</summary>
		/// <param name="_target">Target.</param>
		/// <returns>Calculated distance towards given target.</returns>
		public abstract float GetCalculatedDistance(Vector3 _target);
	}
}