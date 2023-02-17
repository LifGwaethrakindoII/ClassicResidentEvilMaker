using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VPolygonCollider2D
{
	/// <summary>Calculates the center of a PolygonCollider2D's path.</summary>
	/// <param name="_polygonCollider">PolygonCollider2D's reference.</param>
	/// <param name="_index">Path's index [0 by default].</param>
	public static Vector2 GetCenter(this PolygonCollider2D _polygonCollider, int _index = 0)
	{
		Vector2[] path = _polygonCollider.GetPath(_index);
		Vector2 sumCenter = Vector2.zero;
		float sumWeight = 0.0f;
		int length = path.Length;
		int i = 0;

		foreach(Vector2 point in path)
		{
			Vector2 previous = i == 0 ? path[length - 1] : path[i - 1];
			Vector2 next = i < length - 1 ? path[i + 1] : path[0];
			float weight = ((point - next).magnitude + (point - previous).magnitude);

			sumCenter += point * weight;
			sumWeight += weight;
			i++;
		}

		return sumCenter / sumWeight;
	}

	/// <summary>Calculates area of PolygonCollider2D's path.</summary>
	/// <param name="_polygonCollider">PolygonCollider2D's reference.</param>
	/// <param name="_index">Path's index [0 by default].</param>
	public static float GetArea(this PolygonCollider2D _polygonCollider, int _index = 0)
	{
		Vector2[] path = _polygonCollider.GetPath(_index);
		Vector2 c = _polygonCollider.GetCenter(_index);
		float a = 0.0f;
		int length = path.Length;
		int i = 0;

		foreach(Vector2 point in path)
		{
			Vector2 next = i < length - 1 ? path[i + 1] : path[0];
			a += VMath.AreaOfIrregularTriangle(point, next, c);
		}

		return a;
	}
}
}