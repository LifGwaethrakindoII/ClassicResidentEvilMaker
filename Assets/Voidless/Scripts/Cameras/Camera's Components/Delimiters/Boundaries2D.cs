using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public struct Boundaries2D
{
	public Vector3 size; 		/// <summary>Size.</summary>
	public Vector3 center; 		/// <summary>Center.</summary>

	/// <summary>Boundaries 2D's Constructor.</summary>
	public Boundaries2D(Vector3 _size, Vector3 _center = default(Vector3))
	{
		size = _size;
		center = _center;
	}

	/// <summary>Lerps Between Boundares2D A and B.</summary>
	/// <param name="a">Boundaries2D A.</param>
	/// <param name="b">Boundaries2D B.</param>
	/// <param name="t">Normalized Time t [internally clamped].</param>
	/// <returns>Interpolation between A and B.</returns>
	public static Boundaries2D Lerp(Boundaries2D a, Boundaries2D b, float t)
	{
		t = Mathf.Clamp(t, 0.0f, 1.0f);

		return new Boundaries2D(
			Vector3.Lerp(a.size, b.size, t),
			Vector3.Lerp(a.center, b.center, t)
		);
	}
}
}