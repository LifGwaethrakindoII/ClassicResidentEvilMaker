using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VBounds
{
	/// <summary>Iterates through Bounds' Points.</summary>
	/// <param name="_bounds">Bounds to get the poiunts from.</param>
	public static IEnumerator<Vector3> GetCornerVertices(this Bounds _bounds)
	{
		Vector3 center = _bounds.center;
		float x = center.x + -_bounds.extents.x;
		float y = center.y + _bounds.extents.y;
		float z = center.z + _bounds.extents.z;

		yield return new Vector3(x, y, z);
		yield return new Vector3(x, -y, z);
		yield return new Vector3(x, y, -z);
		yield return new Vector3(x, -y, -z);
		yield return new Vector3(-x, y, z);
		yield return new Vector3(-x, -y, z);
		yield return new Vector3(-x, y, -z);
		yield return new Vector3(-x, -y, -z);
	}

	/// <summary>Gets Cartesian Grid Bounds contained on a given Bound.</summary>
	/// <param name="_bounds">Container Bounds.</param>
	/// <param name="_divisions">Number of divisions made.</param>
	/// <returns>Array Bounds per each division.</returns>
	public static Bounds[] GetCartesianGridBounds(this Bounds _bounds, Vector3 _divisions)
	{
		_divisions = _divisions.Floored();

		int length = (int)(Mathf.Abs(_divisions.x) * Mathf.Abs(_divisions.y) * Mathf.Abs(_divisions.z));

		if(length <= 0) return null;

		int i = 0;
		Bounds[] bounds = new Bounds[length];
		Vector3 dividedBound = VVector3.Division(_bounds.size, _divisions);
		Vector3 origin = _bounds.center - _bounds.extents - (dividedBound * 0.5f);

		for(float x = 1.0f; x < _divisions.x + 1.0f; x++)
		{
			for(float y = 1.0f; y < _divisions.y + 1.0f; y++)
			{
				for(float z = 1.0f; z < _divisions.z + 1.0f; z++)
				{
					bounds[i] = new Bounds(
						origin + Vector3.Scale(dividedBound, new Vector3(x, y, z)),
						dividedBound
					);
					i++;
				}
			}
		}

		return bounds;
	}

	/// <summary>Calculates Bounds that better fit a given set of Bounds.</summary>
	/// <param name="_bounds">Set of Bounds.</param>
	/// <returns>Bounds that fit set of Bounds [it will return default Bounds if the set is empty].</returns>
	public static Bounds GetBoundsToFitSet(params Bounds[] _bounds)
	{
		int length = _bounds.Length;

		if(_bounds == null || length == 0)
		{
#if UNITY_EDITOR
			Debug.Log("[VBounds] No Bounds provided as argument, returning a default Bounds' structure...");
#endif
			return new Bounds();

		} else if(length == 1) return _bounds[0];

		Bounds a = _bounds[0];
		Bounds b = default(Bounds);

		for(int i = 1; i < length; i++)
		{
			b = _bounds[i];
			a = VMath.GetBoundsToFitPair(a, b);
		}

		return a;
	}

	/// <summary>Calculates Bounds that better fit a given set of Bounds contained in a set of Renderers.</summary>
	/// <param name="_renderers">Set of Renderers [which contains Bounds].</param>
	/// <returns>Bounds that fit set of Bounds [it will return default Bounds if the set is empty].</returns>
	public static Bounds GetBoundsToFitSet(params Renderer[] _renderers)
	{
		int length = _renderers.Length;

		if(_renderers == null || length == 0)
		{
#if UNITY_EDITOR
			Debug.Log("[VBounds] No Bounds provided as argument, returning a default Bounds' structure...");
#endif
			return new Bounds();

		} else if(length == 1) return _renderers[0].bounds;

		Bounds a = _renderers[0].bounds;
		Bounds b = default(Bounds);

		for(int i = 1; i < length; i++)
		{
			if(_renderers[i] == null) continue;

			b = _renderers[i].bounds;
			a = VMath.GetBoundsToFitPair(a, b);
		}

		return a;
	}

	/// <summary>Calculates Bounds that better fit a given set of Bounds contained in a set of Colliders.</summary>
	/// <param name="_colliders">Set of Colliders [which contains Bounds].</param>
	/// <returns>Bounds that fit set of Bounds [it will return default Bounds if the set is empty].</returns>
	public static Bounds GetBoundsToFitSet(params Collider[] _colliders)
	{
		int length = _colliders.Length;

		if(_colliders == null || length == 0)
		{
#if UNITY_EDITOR
			Debug.Log("[VBounds] No Bounds provided as argument, returning a default Bounds' structure...");
#endif
			return new Bounds();

		} else if(length == 1) return _colliders[0].bounds;

		Bounds a = _colliders[0].bounds;
		Bounds b = default(Bounds);

		for(int i = 1; i < length; i++)
		{
			if(_colliders[i] == null) continue;

			b = _colliders[i].bounds;
			a = VMath.GetBoundsToFitPair(a, b);
		}

		return a;
	}

	/// DEPRECATED [DO NOT USE UNLESS YOU ARE GAY]
	/*
		- This function does not work given that the center of the bounds must not be the average center of 
		the pair [that is: c = (a + b) / 2].
		- The center must be equal to the component-wise median of both min and max vectors of the pair
		- Where median: m(min, max) = min + [(max - min) / 2]
		- Therefore the proper center of the Bounds ought to be:
			c = {
					m(Min(a.min.x, b.min.x), Max(a.max.x, b.max.x)),
					m(Min(a.min.y, b.min.y), Max(a.max.y, b.max.y)),
					m(Min(a.min.z, b.min.z), Max(a.max.z, b.max.z))
			}
		- Aside from that, the way to calculate the dimensions that better fit the pair is correct...
	*/
	public static Bounds GetBoundsToFitPair(Bounds a, Bounds b)
	{
		Vector3 center = (a.center + b.center) * 0.5f;
		Vector3 size = new Vector3(
			VMath.GetSizeToFitSegments(a.min.x, a.max.x, b.min.x, b.max.x),
			VMath.GetSizeToFitSegments(a.min.y, a.max.y, b.min.y, b.max.y),
			VMath.GetSizeToFitSegments(a.min.z, a.max.z, b.min.z, b.max.z)
		);

		return new Bounds(center, size);
	}
}
}