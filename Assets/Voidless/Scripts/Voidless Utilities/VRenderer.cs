using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VRenderer
{
	/// <returns>Bounds scaled by the Transform's scale.</returns>
	public static Vector3 GetScaledBoundsSize(this Renderer _renderer)
	{
		return Vector3.Scale(_renderer.bounds.size, _renderer.transform.localScale);
	}
}
}