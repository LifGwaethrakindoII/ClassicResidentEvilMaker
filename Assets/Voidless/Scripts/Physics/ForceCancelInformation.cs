using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public struct ForceCancelInformation
{
	public LayerMask mask; 	/// <summary>Collider's Layer Mask.</summary>
	public Axes3D axes; 	/// <summary>Axes to Cancel.</summary>
}
}