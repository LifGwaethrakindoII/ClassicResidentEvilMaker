using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public struct Line
{
	public LineSegmentType type; 	/// <summary>Line Segment's Type.</summary>
	public Vector3 a; 				/// <summary>Vector3 A.</summary>
	public Vector3 b; 				/// <summary>Vector3 B.</summary>
	public Vector3 point1; 			/// <summary>Middle- Point 1.</summary>
	public Vector3 point2; 			/// <summary>Middle- Point 2.</summary>
#if UNITY_EDITOR
	public bool showHandles; 												/// <summary>Show Handles on Editor Mode?.</summary>
	public bool showForLinePath; 	/// <summary>Show For Line Path?.</summary>
#endif

	/*/// <summary>Implicit Line to Ray Operator.</summary>
	public static implicit operator Ray(Line _line) { return new Ray(_line.a, (_line.b - _line.a)); }

	/// <summary>Implicit Ray to Line Operator.</summary>
	public static implicit operator Line(Ray _ray) { return new Line(_ray.origin, (_ray.origin + _ray.direction)); }*/

	/// <summary>Implicit Line equals Line value bool operator.</summary>
	public static bool operator == (Line lA, Line lB)
	{
		return 	(lA.type == lB.type)
				&& (lA.a == lB.a)
				&& (lA.b == lB.b)
				&& (lA.point1 == lB.point1)
				&& (lA.point2 == lB.point2);
	}

	/// <summary>Implicit Line not-equals Line value bool operator.</summary>
	public static bool operator != (Line lA, Line lB)
	{
		return 	(lA.type != lB.type)
				|| (lA.a != lB.a)
				|| (lA.b != lB.b)
				|| (lA.point1 != lB.point1)
				|| (lA.point2 != lB.point2);
	}

	/// <summary>Line's Constructor.</summary>
	/// <param name="_type">Line Segment's Type.</param>
	/// <param name="_a">Vector A.</param>
	/// <param name="_b">Vector B.</param>
	public Line(LineSegmentType _type, Vector3 _a, Vector3 _b) : this()
	{
		type = _type;
		a = _a;
		b = _b;
		point1 = new Vector3();
		point2 = new Vector3();
	}

	/// <summary>Interpolates Line's points.</summary>
	/// <param name="t">Time parameter.</param>
	/// <returns>Interpolation between Line's points in t time.</returns>
	public Vector3 Lerp(float t)
	{
		switch(type)
		{
			case LineSegmentType.Linear:
			return a + ((b - a) * t);
			break;

			case LineSegmentType.CuadraticBeizer:
			return VMath.CuadraticBeizer(a, b, point1, t);
			break;

			case LineSegmentType.CubicBeizer:
			return VMath.CubicBeizer(a, b, point1, point2, t);
			break;
		}

		return a + ((b - a) * t);
	}
}
}