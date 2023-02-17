using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public struct ForceInformation
{
	public Vector3 force; 			/// <summary>Force's Vector.</summary>
	public float duration; 			/// <summary>Force's Duration.</summary>
	public ForceMode forceMode; 	/// <summary>Force's Mode.</summary>

	/// <summary>ForceInformation's Constructor.</summary>
	/// <param name="_force">Force's Vector.</param>
	/// <param name="_duration">Force's Duration.</param>
	/// <param name="_forceMode">Force's Mode [ForceMode.Force by default].</param>
	public ForceInformation(Vector3 _force, float _duration, ForceMode _forceMode = ForceMode.Force)
	{
		force = _force;
		duration = _duration;
		forceMode = _forceMode;	
	}
}
}