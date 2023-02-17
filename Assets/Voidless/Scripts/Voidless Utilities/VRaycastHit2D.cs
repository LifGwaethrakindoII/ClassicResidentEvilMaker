using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VRaycastHit2D
{
	/// <summary>Evaluates whether given RaycastHitInfo2D has any information.</summary>
	/// <param name="_hit">Hit Information to evaluate.</param>
	/// <returns>True if the RaycastHit2D has information [having a Transform reference].</returns>
	public static bool HasInfo(this RaycastHit2D _hit)
	{
		return _hit.transform != null;
	}
}
}