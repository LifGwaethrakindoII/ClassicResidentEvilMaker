using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VRay
{
	/*/// <summary>Converts Ray to Line.</summary>
	/// <param name="_ray">Reference Ray.</param>
	/// <returns>Line from given Ray.</returns>
	public static Line ToLine(this Ray _ray)
	{
		return new Line(_ray.origin, _ray.origin + _ray.direction);
	}*/

	/// <summary>Interpolates ray towards direction, given a time t.</summary>
	/// <param name="_ray">Ray to interpolate.</param>
	/// <param name="t">Time reference.</param>
	/// <returns>Interpolation between Ray's origin and direction on t time, as a Vector2.</returns>
	public static Vector3 Lerp(this Ray _ray, float t)
	{
		return (_ray.origin + (_ray.direction * t));
	}

	/// <param name="_ray">Ray's Direction to return.</param>
	/// <returns>Ray's Direction relative to its origin.</returns>
	public static Vector3 RelativeDirection(this Ray _ray)
	{
		return _ray.origin + _ray.direction;
	}
}
}