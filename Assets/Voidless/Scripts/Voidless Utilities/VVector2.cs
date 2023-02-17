using UnityEngine;
using System;

namespace Voidless
{
public static class VVector2
{
	private static readonly Vector2[] normals2D; 	/// <summary>Normal direction vectors along the Second-Dimension.</summary>

	/// <summary>Static VVector2's Constructor.</summary>
	static VVector2()
	{
		normals2D = new []
		{
			new Vector2(1f, 0f),
			new Vector2(-1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(0f, -1f),
			new Vector2(1f, 1f),
			new Vector2(-1f, -1f),
			new Vector2(1f, -1f),
			new Vector2(-1f, 1f)
		};
	}

	/// <summary>Rotates Vector Counter-Clockwise by given angle.</summary>
	/// <param name="v">Vector to rotate.</param>
	/// <param name="a">Rotation's Angle [in degrees].</param>
	/// <returns>Vector Rotates by a degrees.</returns>
	public static Vector2 Rotate(this Vector2 v, float a)
	{
		a *= VMath.DEG_TO_RAD;

		float sin = Mathf.Sin(a);
		float cos = Mathf.Cos(a);

		return new Vector2(
			(v.x * cos) - (v.y * sin),
			(v.x * sin) + (v.y * cos)
		);
	}

	/// <summary>Rotates Vector Counter-Clockwise by given angle.</summary>
	/// <param name="v">Vector to rotate.</param>
	/// <param name="a">Rotation's Angle [in degrees].</param>
	/// <returns>Vector Rotates by a degrees.</returns>
	public static Vector2 Rotate(this Vector3 v, float a)
	{
		a *= VMath.DEG_TO_RAD;

		float sin = Mathf.Sin(a);
		float cos = Mathf.Cos(a);

		return new Vector2(
			(v.x * cos) - (v.y * sin),
			(v.x * sin) + (v.y * cos)
		);
	}

#region ComponentFunctions:
	/// <summary>Sets Vector2 X.</summary>
	/// <param name="_vector">The Vector2 that will have its X modified.</param>
	/// <param name="_x">Updated Vector2 X Component.</param>
	public static Vector2 WithX(this Vector2 _vector, float _x)
	{
		return _vector = new Vector2(_x, _vector.y);
	}

	/// <summary>Sets Vector2 Y.</summary>
	/// <param name="_vector">The Vector2 that will have its Y modified.</param>
	/// <param name="_x">Updated Vector2 Y Component.</param>
	public static Vector2 WithY(this Vector2 _vector, float _y)
	{
		return _vector = new Vector2(_vector.x, _y);
	}
#endregion

#region DistanceFunctions:
	/// <summary>Calculates the Manhattan distance (sum of the component-wise differences) of 2 vectors.</summary>
	/// <param name="a">Vector A.</param>
	/// <param name="b">Vector B.</param>
	/// <returns>Manhattan distance between Vectors A and B.</returns>
	public static float ManhattanDistance(Vector2 a, Vector2 b)
	{
		return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
	}

	/// <summary>Calculates the Chebysher distance (maximum of the component-wise differences) of 2 vectors.</summary>
	/// <param name="a">Vector A.</param>
	/// <param name="b">Vector B.</param>
	/// <returns>Chebysher distance between Vectors A and B.</returns>
	public static float ChebysherDistance(Vector2 a, Vector2 b)
	{
		return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y));
	}
#endregion 

	/// <summary>Calculates a component-wise Vector division.</summary>
	/// <param name="a">Vector A.</param>
	/// <param name="b">Vector B.</param>
	/// <returns>Component-wise division between two vectors.</returns>
	public static Vector2 Division(Vector2 a, Vector2 b)
	{
		return new Vector2
		(
			(a.x / b.x),
			(a.y / b.y)
		);
	}

	/// <summary>Gets the direction vector towards target position.</summary>
	/// <param name="_fromPosition">The position from where de direction points.</param>
	/// <param name="_targetPosition">The position where the _fromPosition heads to.</param>
	/// <returns>Direction towards target point (Vector2).</returns>
	public static Vector2 GetDirectionTowards(this Vector2 _fromPosition, Vector2 _targetPosition)
	{
		return (_targetPosition - _fromPosition);
	}

	/// <summary>Gets the Vector2 property with the highest value.</summary>
	/// <param name="_vector">The Vector2 that will compare its components.</param>
	/// <returns>Highest value between Vector2 components.</returns>
	public static float GetMaxVectorProperty(this Vector2 _vector)
	{
		return Mathf.Max(_vector.x, _vector.y);
	}

	/// <summary>Gets the Vector2 property with the lowest value.</summary>
	/// <param name="_vector">The Vector2 that will compare its components.</param>
	/// <returns>Lowest value between Vector2 components.</returns>
	public static float GetMinVectorProperty(this Vector2 _vector)
	{
		return Mathf.Min(_vector.x, _vector.y);
	}

	/// <summary>Generates a regular vector, with all components being given value.</summary>
	/// <param name="_value">Value to give to all Vector's Components.</param>
	/// <returns>Regular Vector2 with all components equal.</returns>
	public static Vector2 Regular(float _value)
	{
		return new Vector2(_value, _value);
	}

	/// <summary>Generates a vector with random component values.</summary>
	/// <param name="_randomRange">Random's Range.</param>
	/// <returns>Vector with random ranged component's values.</returns>
	public static Vector2 Random(FloatRange _randomRange)
	{
		return new Vector2
		(
			UnityEngine.Random.Range(_randomRange.Min(), _randomRange.Max()),
			UnityEngine.Random.Range(_randomRange.Min(), _randomRange.Max())
		);
	}

	/// <summary>Gets Hick Opposite by given angles.</summary>
	/// <param name="_angle">Angle.</param>
	/// <returns>Hick Opposite.</returns>
	public static float GetHickOppositeGivenAngle(float _angle)
	{
		return (Mathf.Sin(_angle * Mathf.Deg2Rad) /* (_angle > 180.0f ? -1.0f : 1.0f)*/);
	}

	/// <summary>Gets Adyacent Leg by given angles.</summary>
	/// <param name="_angle">Angle.</param>
	/// <returns>Adyacent Leg.</returns>
	public static float GetAdyacentLegByGivenAngle(float _angle)
	{
		return (Mathf.Cos(_angle * Mathf.Deg2Rad) /* (_angle > 180.0f ? -1.0f : 1.0f)*/);
	}

	/// <summary>Clamps Vector to given range.</summary>
	/// <param name="_vector">Vector to clamp.</param>
	/// <param name="_min">Minimum's Value.</param>
	/// <param name="_max">Maximum's Value.</param>
	/// <returns>Clamped Vector by given range.</returns>
	public static Vector2 Clamped(this Vector2 _vector, float _min, float _max)
	{
		return new Vector2
		(
			Mathf.Clamp(_vector.x, Mathf.Min(_min), Mathf.Max(_max)),
			Mathf.Clamp(_vector.y, Mathf.Min(_min), Mathf.Max(_max))
		); 
	}

	/// <summary>Rounds Vector2 components.</summary>
	/// <param name="_vector">The Vector2 that will have its components rounded.</param>
	/// <returns>Vector2 with components rounded (0 or 1).</returns>
	public static Vector2 Round(this Vector2 _vector)
	{
		return _vector = new Vector2(Mathf.Round(_vector.x), Mathf.Round(_vector.y));
	}

	/// <summary>Calculates a vector with each component evaluated by given function.</summary>
	/// <param name="_vector">Vector to calculate.</param>
	/// <param name="function">Function to evaluate on Vector's Components.</param>
	/// <returns>Vector with each component evaluated by given function.</returns>
	public static Vector2 WithFunctionPerComponent(this Vector2 _vector, Func<float, float> function)
	{
		return new Vector2
		(
			function(_vector.x),
			function(_vector.y)
		);
	}

	/// <summary>Turns Vector2's components into absolute values.</summary>
	/// <param name="_vector">Extended Vector2.</param>
	/// <returns>Vector2 with Absolute Values components.</returns>
	public static Vector2 Abs(this Vector2 _vector)
	{
		return new Vector2
		(
			Mathf.Abs(_vector.x),
			Mathf.Abs(_vector.y)
		);
	}

	/// <summary>Turns Vector2's components into negative absolute values.</summary>
	/// <param name="_vector">Extended Vector2.</param>
	/// <returns>Vector2 with Negative Absolute Values components.</returns>
	public static Vector2 NegativeAbs(this Vector2 _vector)
	{
		return new Vector2
		(
			VMath.NegativeAbs(_vector.x),
			VMath.NegativeAbs(_vector.y)
		);
	}

	/// <summary>Calculates Scalar projection of Vector A into B.</summary>
	/// <param name="a">Vector A.</param>
	/// <param name="b">Vector B.</param>
	/// <returns>Scalar projection.</returns>
	public static float ScalarProjection(Vector2 a, Vector2 b)
	{
		float aDistance = a.magnitude;
		float bDistance = b.magnitude;
		float dot = ((a.x * b.x) + (a.y * b.y));
		float x = dot / (aDistance * bDistance);
		float angle = Mathf.Acos(x);

		return aDistance * Mathf.Cos(angle);
	}

	/// <summary>Calculates Vector projection of Vector A into B.</summary>
	/// <param name="a">Vector A.</param>
	/// <param name="b">Vector B.</param>
	/// <returns>Vector projection.</returns>
	public static Vector2 VectorProjection(Vector2 a, Vector2 b)
	{
		return (b.normalized * ScalarProjection(a, b));
	}

	/// <returns>True if Vector equals Vector2.zero.</returns>
	public static bool IsZero(this Vector2 v)
	{
		return v.x == 0.0f && v.y == 0.0f;
	}

	/// <summary>Gets identity vector of given orientation semantic.</summary>
	/// <param name="_orientation">Orientation's Semantics.</param>
	/// <returns>Identity vector interpreted from orientation semantic.</returns>
	public static Vector2 GetOrientation(OrientationSemantics _orientation)
	{
		Vector2 orientation = Vector2.zero;

		if(_orientation.HasFlag(OrientationSemantics.Right)) orientation += Vector2.right;
		if(_orientation.HasFlag(OrientationSemantics.Left)) orientation += Vector2.left;
		if(_orientation.HasFlag(OrientationSemantics.Up)) orientation += Vector2.up;
		if(_orientation.HasFlag(OrientationSemantics.Down)) orientation += Vector2.down;

		return orientation.normalized;
	}

	/// <summary>Gets direction relative to a transform, given an orientation semantic.</summary>
	/// <param name="_transform">Target Transform.</param>
	/// <param name="_orientation">Orientation's Semantics.</param>
	/// <returns>Relative Direction interpreted from Orientation Semantic.</returns>
	public static Vector2 GetOrientationDirection(this Transform _transform, OrientationSemantics _orientation)
	{
		return _transform.TransformDirection(GetOrientation(_orientation));
	}

	/// <summary>Calculates the average of Vector's Components.</summary>
	/// <param name="_vector">Vector to calculate average from.</param>
	public static float GetAverage(this Vector2 _vector)
	{
		return ((_vector.x + _vector.y) / 2.0f);
	}

	/// <summary>Converts Vector2 to Vector3 format.</summary>
	/// <param name="_vector">The Vector2 that will be converted to Vector3.</param>
	/// <returns>Vector2 converted to Vector3 format.</returns>
	public static Vector3 ToVector3(this Vector2 _vector)
	{
		return new Vector3(_vector.x, _vector.y, 0f);
	}

	/// <summary>Gets a counter-clockwise version of given vector [+90 degrees].</summary>
	/// <param name="v">Original Vector.</param>
	/// <returns>Counter-clockwise vector.</returns>
	public static Vector2 CounterClockwise(this Vector2 v)
	{
		return new Vector2(-v.y, v.x);
	}

	/// <summary>Gets a clockwise version of given vector [-90 degrees].</summary>
	/// <param name="v">Original Vector.</param>
	/// <returns>Clockwise vector.</returns>
	public static Vector2 Clockwise(this Vector2 v)
	{
		return new Vector2(v.y, -v.x);
	}

	/// <summary>Gets a reflected version of given vector [+/-180 degrees].</summary>
	/// <param name="v">Original Vector.</param>
	/// <returns>Reflected vector.</returns>
	public static Vector2 Reflect(this Vector2 v)
	{
		return new Vector2(-v.x, -v.y);
	}
}
}