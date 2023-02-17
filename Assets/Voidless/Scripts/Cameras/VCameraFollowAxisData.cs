using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public struct VCameraFollowAxisData
{
	public FollowMode followMode; 						/// <summary>Follow's Mode.</summary>
	public bool limitFollowingSpeed; 					/// <summary>Limit Following's Speed?.</summary>	
	[Range(0.0f, 1.0f)] public float followduration; 	/// <summary>Follow's Duration.</summary>
	public float maxFollowSpeed; 						/// <summary>Maximum's following Speed [if the following speed is marked as limited].</summary>	
}
}