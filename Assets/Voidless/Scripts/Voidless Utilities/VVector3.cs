using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VVector3
{
	private static readonly Vector3[] normals3D; 		/// <summary>Normal direction vectors along the Third-Dimension.</summary>

	/// <summary>Static VVector3's Constructor.</summary>
	static VVector3()
	{
		normals3D = new []
		{
			new Vector3(1f, 0f, 0f),
			new Vector3(-1f, 0f, 0f),
			new Vector3(0f, 1f, 0f),
			new Vector3(0f, -1f, 0f),
			new Vector3(0f, 0f, 1f),
			new Vector3(0f, 0f, -1f),
			new Vector3(1f, 1f, 0f),
			new Vector3(-1f, 1f, 0f),
			new Vector3(1f, -1f, 0f),
			new Vector3(-1f, -1f, 0f),
			new Vector3(1f, 0f, 1f),
			new Vector3(-1f, 0f, 1f),
			new Vector3(1f, 0f, -1f),
			new Vector3(-1f, 0f, -1f),
			new Vector3(0f, 1f, 1f),
			new Vector3(0f, -1f, 1f),
			new Vector3(0f, 1f, -1f),
			new Vector3(0f, -1f, -1f),
			new Vector3(1f, 1f, 1f),
			new Vector3(-1f, 1f, 1f),
			new Vector3(1f, -1f, 1f),
			new Vector3(-1f, -1f, 1f),
			new Vector3(1f, 1f, -1f),
			new Vector3(-1f, 1f, -1f),
			new Vector3(1f, -1f, -1f),
			new Vector3(-1f, -1f, -1f)
		};
	}

#region ComponentFunctions:
	/// <summary>Calculates a constrained direction given an Axes3D's flag.</summary>
	/// <param name="a">Vector A.</param>
	/// <param name="b">Vector B.</param>
	/// <param name="_constraints">Constraints on the Delta calculation [None by default].</param>
	/// <returns>Direction with constrained Axes.</returns>
	public static Vector3 ConstrainedDirection(Vector3 a, Vector3 b, Axes3D _constraints = Axes3D.None, bool _zeroIgnoredAxes = true)
	{
		return new Vector3
		(
			(_constraints | Axes3D.X) != _constraints ? a.x - b.x : (_zeroIgnoredAxes ? 0.0f : b.x),
			(_constraints | Axes3D.Y) != _constraints ? a.y - b.y : (_zeroIgnoredAxes ? 0.0f : b.y),
			(_constraints | Axes3D.Z) != _constraints ? a.z - b.z : (_zeroIgnoredAxes ? 0.0f : b.z)
		);
	}

	/// <summary>Calculates a direction with Axes given an Axes3D's flag.</summary>
	/// <param name="a">Vector A.</param>
	/// <param name="b">Vector B.</param>
	/// <param name="_axes">Axes to include [All by default].</param>
	/// <returns>Direction with constrained Axes.</returns>
	public static Vector3 DirectionWithAxes(Vector3 a, Vector3 b, Axes3D _axes = Axes3D.All, bool _zeroIgnoredAxes = true)
	{
		return new Vector3
		(
			(_axes | Axes3D.X) == _axes ? a.x - b.x : (_zeroIgnoredAxes ? 0.0f : b.x),
			(_axes | Axes3D.Y) == _axes ? a.y - b.y : (_zeroIgnoredAxes ? 0.0f : b.y),
			(_axes | Axes3D.Z) == _axes ? a.z - b.z : (_zeroIgnoredAxes ? 0.0f : b.z)
		);
	}

	/// <summary>Gives a vector with axes cancelled given an enumerator.</summary>
	/// <param name="v">Vector to modify.</param>
	/// <param name="_ignore">Axes to ignore [None by default].</param>
	/// <returns>Modified Vector.</returns>
	public static Vector3 WithIgnoredAxes(this Vector3 v, Axes3D _ignore = Axes3D.None)
	{
		if((_ignore | Axes3D.X) == _ignore) v.x = 0.0f;
		if((_ignore | Axes3D.Y) == _ignore) v.y = 0.0f;
		if((_ignore | Axes3D.Z) == _ignore) v.z = 0.0f;

		return v;
	}

	/// <summary>Gives a vector with axes cancelled given an enumerator.</summary>
	/// <param name="v">Vector to modify.</param>
	/// <param name="_axes">Axes to consider [All by default].</param>
	/// <returns>Modified Vector.</returns>
	public static Vector3 WithAxes(this Vector3 v, Axes3D _axes = Axes3D.All)
	{
		if((_axes | Axes3D.X) != _axes) v.x = 0.0f;
		if((_axes | Axes3D.Y) != _axes) v.y = 0.0f;
		if((_axes | Axes3D.Z) != _axes) v.z = 0.0f;

		return v;
	}

	/// <summary>Sets Vector3 X.</summary>
	/// <param name="_vector">The Vector3 that will have its X modified.</param>
	/// <param name="_x">Updated Vector3 X Component.</param>
	public static Vector3 WithX(this Vector3 _vector, float _x)
	{
		return _vector = new Vector3(_x, _vector.y, _vector.z);
	}

	/// <summary>Sets Vector3 Y.</summary>
	/// <param name="_vector">The Vector3 that will have its Y modified.</param>
	/// <param name="_x">Updated Vector3 Y Component.</param>
	public static Vector3 WithY(this Vector3 _vector, float _y)
	{
		return _vector = new Vector3(_vector.x, _y, _vector.z);
	}

	/// <summary>Sets Vector3 Z.</summary>
	/// <param name="_vector">The Vector3 that will have its Z modified.</param>
	/// <param name="_x">Updated Vector3 Z Component.</param>
	public static Vector3 WithZ(this Vector3 _vector, float _z)
	{
		return _vector = new Vector3(_vector.x, _vector.y, _z);
	}

	/// <summary>Sets Vector3 X and Y.</summary>
	/// <param name="_vector">The Vector3 that will have its X and Y modified.</param>
	/// <param name="_x">Updated Vector3 X Component.</param>
	/// <param name="_y">Updated Vector3 Y Component.</param>
	public static Vector3 WithXAndY(this Vector3 _vector, float _x, float _y)
	{
		return _vector = new Vector3(_x, _y, _vector.z);
	}

	/// <summary>Sets Vector3 X and Z.</summary>
	/// <param name="_vector">The Vector3 that will have its X and Z modified.</param>
	/// <param name="_x">Updated Vector3 X Component.</param>
	/// <param name="_z">Updated Vector3 Z Component.</param>
	public static Vector3 WithXAndZ(this Vector3 _vector, float _x, float _z)
	{
		return _vector = new Vector3(_x, _vector.y, _z);
	}

	/// <summary>Sets Vector3 Y and Z.</summary>
	/// <param name="_vector">The Vector3 that will have its Y and Z modified.</param>
	/// <param name="_y">Updated Vector3 Y Component.</param>
	/// <param name="_z">Updated Vector3 Z Component.</param>
	public static Vector3 WithYAndZ(this Vector3 _vector, float _y, float _z)
	{
		return _vector = new Vector3(_vector.x, _y, _z);
	}

	/// <summary>Adds value to Vector3 X component.</summary>
	/// <param name="_vector">The Vector3 that will have its X subtracted by value.</param>
	/// <param name="_addedX">Added value to Vector3 X Component.</param>
	/// <returns>Vector with subtracted X component by value.</summary>
	public static Vector3 WithAddedX(this Vector3 _vector, float _addedX)
	{
		return _vector = new Vector3((_vector.x + _addedX), _vector.y, _vector.z);
	}

	/// <summary>Adds value to Vector3 Y component.</summary>
	/// <param name="_vector">The Vector3 that will have its Y subtracted by value.</param>
	/// <param name="_addedY">Added value to Vector3 Y Component.</param>
	/// <returns>Vector with subtracted Y component by value.</summary>
	public static Vector3 WithAddedY(this Vector3 _vector, float _addedY)
	{
		return _vector = new Vector3(_vector.x, (_vector.y + _addedY), _vector.z);
	}

	/// <summary>Adds value to Vector3 Z component.</summary>
	/// <param name="_vector">The Vector3 that will have its Z subtracted by value.</param>
	/// <param name="_addedZ">Added value to Vector3 Z Component.</param>
	/// <returns>Vector with subtracted Z component by value.</summary>
	public static Vector3 WithAddedZ(this Vector3 _vector, float _addedZ)
	{
		return _vector = new Vector3(_vector.x, _vector.y, (_vector.z + _addedZ));
	}

	/// <summary>Adds value to Vector3 X and Y components.</summary>
	/// <param name="_vector">The Vector3 that will have its X and Y subtracted by values.</param>
	/// <param name="_addedX">Added value to Vector3 X Component.</param>
	/// <param name="_addedY">Added value to Vector3 Y Component.</param>
	/// <returns>Vector with subtracted X and Y components by values.</summary>
	public static Vector3 WithAddedXAndY(this Vector3 _vector, float _addedX, float _addedY)
	{
		return _vector = new Vector3((_vector.x + _addedX), (_vector.y + _addedY), _vector.z);
	}

	/// <summary>Adds value to Vector3 X and Z components.</summary>
	/// <param name="_vector">The Vector3 that will have its X and Z subtracted by values.</param>
	/// <param name="_addedX">Added value to Vector3 X Component.</param>
	/// <param name="_addedZ">Added value to Vector3 Z Component.</param>
	/// <returns>Vector with subtracted X and Z components by values.</summary>
	public static Vector3 WithAddedXAndZ(this Vector3 _vector, float _addedX, float _addedZ)
	{
		return _vector = new Vector3((_vector.x + _addedX), _vector.y, (_vector.z + _addedZ));
	}

	/// <summary>Adds value to Vector3 Y and Z components.</summary>
	/// <param name="_vector">The Vector3 that will have its Y and Z subtracted by values.</param>
	/// <param name="_addedY">Added value to Vector3 Y Component.</param>
	/// <param name="_addedZ">Added value to Vector3 Z Component.</param>
	/// <returns>Vector with subtracted Y and Z components by values.</summary>
	public static Vector3 WithAddedYAndZ(this Vector3 _vector, float _addedY, float _addedZ)
	{
		return _vector = new Vector3(_vector.x, (_vector.y + _addedY), (_vector.z + _addedZ));
	}

	/// <summary>Inverts Vector3's X component.</summary>
	/// <param name="_vector">The Vector3 that will have its X inverted.</param>
	/// <returns>Vector3 with X component inverted.</returns>
	public static Vector3 InvertX(this Vector3 _vector)
	{
		return _vector = new Vector3(-_vector.x, _vector.y, _vector.z);
	}

	/// <summary>Inverts Vector3's Y component.</summary>
	/// <param name="_vector">The Vector3 that will have its Y inverted.</param>
	/// <returns>Vector3 with Y component inverted.</returns>
	public static Vector3 InvertY(this Vector3 _vector)
	{
		return _vector = new Vector3(_vector.x, -_vector.y, _vector.z);
	}

	/// <summary>Inverts Vector3's Z component.</summary>
	/// <param name="_vector">The Vector3 that will have its Z inverted.</param>
	/// <returns>Vector3 with Z component inverted.</returns>
	public static Vector3 InvertZ(this Vector3 _vector)
	{
		return _vector = new Vector3(_vector.x, _vector.y, -_vector.z);
	}

	/// <summary>Inverts Vector3's X and Y component.</summary>
	/// <param name="_vector">The Vector3 that will have its X and Y inverted.</param>
	/// <returns>Vector3 with X and Y component inverted.</returns>
	public static Vector3 InvertXAndY(this Vector3 _vector)
	{
		return _vector = new Vector3(-_vector.x, -_vector.y, _vector.z);
	}

	/// <summary>Inverts Vector3's X and Z component.</summary>
	/// <param name="_vector">The Vector3 that will have its X and Z inverted.</param>
	/// <returns>Vector3 with X and Z component inverted.</returns>
	public static Vector3 InvertXAndZ(this Vector3 _vector)
	{
		return _vector = new Vector3(-_vector.x, _vector.y, -_vector.z);
	}

	/// <summary>Inverts Vector3's Y and Z component.</summary>
	/// <param name="_vector">The Vector3 that will have its Y and Z inverted.</param>
	/// <returns>Vector3 with Y and Z component inverted.</returns>
	public static Vector3 InvertYAndZ(this Vector3 _vector)
	{
		return _vector = new Vector3(_vector.x, -_vector.y, -_vector.z);
	}
#endregion

#region DistanceFunctions:
	/// <summary>Calculates the Manhattan distance (sum of the component-wise differences) of 2 vectors.</summary>
	/// <param name="a">Vector A.</param>
	/// <param name="b">Vector B.</param>
	/// <returns>Manhattan distance between Vectors A and B.</returns>
	public static float ManhattanDistance(Vector3 a, Vector3 b)
	{
		return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z);
	}

	/// <summary>Calculates the Chebysher distance (maximum of the component-wise differences) of 2 vectors.</summary>
	/// <param name="a">Vector A.</param>
	/// <param name="b">Vector B.</param>
	/// <returns>Chebysher distance between Vectors A and B.</returns>
	public static float ChebysherDistance(Vector3 a, Vector3 b)
	{
		return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y), Mathf.Abs(a.z - b.z));
	}
#endregion 

/// \TODO Update to return/take ICollections in a generic way instead?
#region CollectionMethods:
	/// <summary>Gets the position (Vector3) from a list of Transforms.</summary>
	/// <param name="_transforms">The list of Transforms from where the Vector3 list will be created.</param>
	/// <returns>List of the Transform positions (Vector3).</returns>
	public static List<Vector3> GetPositions(this List<Transform> _transforms)
	{
		List<Vector3> newList = new List<Vector3>(_transforms.Count);

		for(int i = 0; i < _transforms.Count; i++)
		{
			newList.Add(_transforms[i].position);
		}

		return newList;
	}

	/// <summary>Gets the position (Vector3) from a list of GameObjects.</summary>
	/// <param name="_gameObjects">The list of GameObjects from where the Vector3 list will be created.</param>
	/// <returns>List of the GameObject's positions (Vector3).</returns>
	public static List<Vector3> GetPositions(this List<GameObject> _gameObjects)
	{
		List<Vector3> newList = new List<Vector3>(_gameObjects.Count);

		for(int i = 0; i < _gameObjects.Count; i++)
		{
			if(_gameObjects[i] != null) newList.Add(_gameObjects[i].transform.position);
		}
		
		return newList;
	}

	/// <summary>Gets the distances (float) between List of Vector3 and Vector3.</summary>
	/// <param name="_list">The list of Vector3 from where the distance will be measured.</param>
	/// <param name="_targetPoint">The Vector3 from where the List of Vector3 will measure the distance.</param>
	/// <returns>List of the distances (float).</returns>
	public static List<float> GetDistances(this List<Vector3> _list, Vector3 _targetPoint)
	{
		List<float> newList = new List<float>();

		foreach(Vector3 point in _list)
		{
			newList.Add(Vector3.Distance(point, _targetPoint));
		}

		return newList;
	}

	/// <summary>Gets the distances (float) between List of Vector3 and Vector3.</summary>
	/// <param name="_list">The list of Vector3 from where the distance will be measured.</param>
	/// <param name="_targetPoint">The Vector3 from where the List of Vector3 will measure the distance.</param>
	/// <returns>List of the distances (float).</returns>
	public static List<float> GetDistances(this List<Transform> _list, Vector3 _targetPoint)
	{
		List<float> newList = new List<float>();

		foreach(Transform _transform in _list)
		{
			newList.Add(Vector3.Distance(_transform.position, _targetPoint));
		}

		return newList;
	}

	/// <summary>Gets the distances (float) between List of Vector3 and Vector3.</summary>
	/// <param name="_list">The list of Vector3 from where the distance will be measured.</param>
	/// <param name="_targetPoint">The Vector3 from where the List of Vector3 will measure the distance.</param>
	/// <returns>List of the distances (float).</returns>
	public static List<float> GetDistances(this List<GameObject> _list, Vector3 _targetPoint)
	{
		List<float> newList = new List<float>();

		foreach(GameObject _gameObject in _list)
		{
			newList.Add(Vector3.Distance(_gameObject.transform.position, _targetPoint));
		}

		return newList;
	}

	/// <summary>Gets the Square Magnitudes between List of Vector3 and Target Vector3.</summary>
	/// <param name="List">the List of Vector3 from where the Square Magnitudes will be measured.</param>
	/// <param name="_targetPoint">The Vector3 from where the List of Vector3 will measure the Square Distances.</param>
	/// <returns>List of Square Magnitudes (float).</returns>
	public static List<float> GetSquareMagnitudes(this List <Vector3> _list, Vector3 _targetPoint)
	{
		List<float> newList = new List<float>();

		foreach(Vector3 point in _list)
		{
			newList.Add(point.GetDirectionTowards(_targetPoint).sqrMagnitude);
		}

		return newList;
	}

	/// <summary>Gets the Square Magnitudes between List of Vector3 and Target Vector3.</summary>
	/// <param name="List">the List of Vector3 from where the Square Magnitudes will be measured.</param>
	/// <param name="_targetPoint">The Vector3 from where the List of Vector3 will measure the Square Distances.</param>
	/// <returns>List of Square Magnitudes (float).</returns>
	public static List<float> GetSquareMagnitudes(this List <Transform> _list, Vector3 _targetPoint)
	{
		List<float> newList = new List<float>();

		foreach(Transform _transform in _list)
		{
			newList.Add(_transform.position.GetDirectionTowards(_targetPoint).sqrMagnitude);
		}

		return newList;
	}

	/// <summary>Gets the Square Magnitudes between List of Vector3 and Target Vector3.</summary>
	/// <param name="List">the List of Vector3 from where the Square Magnitudes will be measured.</param>
	/// <param name="_targetPoint">The Vector3 from where the List of Vector3 will measure the Square Distances.</param>
	/// <returns>List of Square Magnitudes (float).</returns>
	public static List<float> GetSquareMagnitudes(this List <GameObject> _list, Vector3 _targetPoint)
	{
		List<float> newList = new List<float>();

		foreach(GameObject _gameObject in _list)
		{
			newList.Add(_gameObject.transform.position.GetDirectionTowards(_targetPoint).sqrMagnitude);
		}

		return newList;
	}
#endregion

	/// <summary>Calculates a component-wise Vector division.</summary>
	/// <param name="a">Vector A.</param>
	/// <param name="b">Vector B.</param>
	/// <returns>Component-wise division between two vectors.</returns>
	public static Vector3 Division(Vector3 a, Vector3 b)
	{
		return new Vector3
		(
			(a.x / b.x),
			(a.y / b.y),
			(a.z / b.z)
		);
	}

	/// <summary>Gets the direction vector towards target position.</summary>
	/// <param name="_fromPosition">The position from where de direction points.</param>
	/// <param name="_targetPosition">The position where the _fromPosition heads to.</param>
	/// <returns>Direction towards target point (Vector3).</returns>
	public static Vector3 GetDirectionTowards(this Vector3 _fromPosition, Vector3 _targetPosition)
	{
		return (_targetPosition - _fromPosition);
	}

	/// <summary>Gets the Vector3 property with the highest value.</summary>
	/// <param name="_vector">The Vector3 that will compare its components.</param>
	/// <returns>Highest value between Vector3 components.</returns>
	public static float GetMaxVectorProperty(this Vector3 _vector)
	{
		return Mathf.Max(_vector.x, _vector.y, _vector.z);
	}

	/// <summary>Gets the Vector3 property with the lowest value.</summary>
	/// <param name="_vector">The Vector3 that will compare its components.</param>
	/// <returns>Lowest value between Vector3 components.</returns>
	public static float GetMinVectorProperty(this Vector3 _vector)
	{
		return Mathf.Min(_vector.x, _vector.y, _vector.z);
	}

	/// <summary>Generates a regular vector, with all components being given value.</summary>
	/// <param name="_value">Value to give to all Vector's Components.</param>
	/// <returns>Regular Vector3 with all components equal.</returns>
	public static Vector3 Regular(float _value)
	{
		return new Vector3(_value, _value, _value);
	}

	/// <summary>Generates a vector with random component values.</summary>
	/// <param name="_randomRange">Random's Range.</param>
	/// <returns>Vector with random ranged component's values.</returns>
	public static Vector3 Random(FloatRange _randomRange)
	{
		return new Vector3
		(
			UnityEngine.Random.Range(_randomRange.Min(), _randomRange.Max()),
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

	/// <summary>Rounds Vector3 components.</summary>
	/// <param name="_vector">The Vector3 that will have its components rounded.</param>
	/// <returns>Vector3 with components rounded (0 or 1).</returns>
	public static Vector3 Round(this Vector3 _vector)
	{
		return _vector = new Vector3(Mathf.Round(_vector.x), Mathf.Round(_vector.y), Mathf.Round(_vector.z));
	}

	/// <summary>Clamps Vector to given range.</summary>
	/// <param name="_vector">Vector to clamp.</param>
	/// <param name="_min">Minimum's Value.</param>
	/// <param name="_max">Maximum's Value.</param>
	/// <returns>Clamped Vector by given range.</returns>
	public static Vector3 Clamped(this Vector3 _vector, float _min, float _max)
	{
		return new Vector3
		(
			Mathf.Clamp(_vector.x, Mathf.Min(_min), Mathf.Max(_max)),
			Mathf.Clamp(_vector.y, Mathf.Min(_min), Mathf.Max(_max)),
			Mathf.Clamp(_vector.z, Mathf.Min(_min), Mathf.Max(_max))
		); 
	}

	/// <summary>Floors all Vector components.</summary>
	/// <param name="v">Vector to floor.</param>
	/// <returns>Vector with floored components.</returns>
	public static Vector3 Floored(this Vector3 v)
	{
		return new Vector3(
			Mathf.Floor(v.x),
			Mathf.Floor(v.y),
			Mathf.Floor(v.z)
		);
	}

	/// <summary>Ceils all Vector components.</summary>
	/// <param name="v">Vector to floor.</param>
	/// <returns>Vector with floored components.</returns>
	public static Vector3 Ceiled(this Vector3 v)
	{
		return new Vector3(
			Mathf.Ceil(v.x),
			Mathf.Ceil(v.y),
			Mathf.Ceil(v.z)
		);
	}

	/// <summary>Calculates a vector with each component evaluated by given function.</summary>
	/// <param name="_vector">Vector to calculate.</param>
	/// <param name="function">Function to evaluate on Vector's Components.</param>
	/// <returns>Vector with each component evaluated by given function.</returns>
	public static Vector3 WithFunctionPerComponent(this Vector3 _vector, Func<float, float> function)
	{
		return new Vector3
		(
			function(_vector.x),
			function(_vector.y),
			function(_vector.z)
		);
	}

	/// <summary>Turns Vector3's components into absolute values.</summary>
	/// <param name="_vector">Extended Vector3.</param>
	/// <returns>Vector3 with Absolute Values components.</returns>
	public static Vector3 Abs(this Vector3 _vector)
	{
		return new Vector3
		(
			Mathf.Abs(_vector.x),
			Mathf.Abs(_vector.y),
			Mathf.Abs(_vector.z)
		);
	}

	/// <summary>Turns Vector3's components into negative absolute values.</summary>
	/// <param name="_vector">Extended Vector3.</param>
	/// <returns>Vector3 with Negative Absolute Values components.</returns>
	public static Vector3 NegativeAbs(this Vector3 _vector)
	{
		return new Vector3
		(
			VMath.NegativeAbs(_vector.x),
			VMath.NegativeAbs(_vector.y),
			VMath.NegativeAbs(_vector.z)
		);
	}

	/// <summary>Calculates Scalar projection of Vector A into B.</summary>
	/// <param name="a">Vector A.</param>
	/// <param name="b">Vector B.</param>
	/// <returns>Scalar projection.</returns>
	public static float ScalarProjection(Vector3 a, Vector3 b)
	{
		float aDistance = a.magnitude;
		float bDistance = b.magnitude;
		float dot = ((a.x * b.x) + (a.y * b.y) + (a.z * b.z));
		float angle = (Mathf.Acos(dot / (aDistance * bDistance)));

		/*NOTES:
			- Acos returns value in radians.
			- Cos receives value in radians, so no conversions must be made.
		*/
		return aDistance * Mathf.Cos(angle);
	}

	/// <summary>Calculates Vector projection of Vector A into B.</summary>
	/// <param name="a">Vector A.</param>
	/// <param name="b">Vector B.</param>
	/// <returns>Vector projection.</returns>
	public static Vector3 VectorProjection(Vector3 a, Vector3 b)
	{
		return (b.normalized * ScalarProjection(a, b));
	}

	/// <summary>Gets identity vector of given orientation semantic.</summary>
	/// <param name="_orientation">Orientation's Semantics.</param>
	/// <returns>Identity vector interpreted from orientation semantic.</returns>
	public static Vector3 GetOrientation(OrientationSemantics _orientation)
	{
		Vector3 orientation = Vector3.zero;

		if(_orientation.HasFlag(OrientationSemantics.Right)) orientation += Vector3.right;
		if(_orientation.HasFlag(OrientationSemantics.Left)) orientation += Vector3.left;
		if(_orientation.HasFlag(OrientationSemantics.Up)) orientation += Vector3.up;
		if(_orientation.HasFlag(OrientationSemantics.Down)) orientation += Vector3.down;
		if(_orientation.HasFlag(OrientationSemantics.Forward)) orientation += Vector3.forward;
		if(_orientation.HasFlag(OrientationSemantics.Back)) orientation += Vector3.back;

		return orientation.normalized;
	}

	/// <returns>Random normalized vector in any direction.</returns>
	public static Vector3 RandomDirection()
	{
		return VQuaternion.Random() * Vector3.one;
	}

	/// <summary>Gets a random point on an spherical area.</summary>
	/// <param name="p">Origin's Point.</param>
	/// <param name="r">Sphere's Radius.</param>
	/// <returns>Random point on given spherical area.</returns>
	public static Vector3 RandomPointOnSphere(Vector3 p, float r)
	{
		return p + (RandomDirection() * r);
	}

	/// <summary>Gets direction relative to a transform, given an orientation semantic.</summary>
	/// <param name="_transform">Target Transform.</param>
	/// <param name="_orientation">Orientation's Semantics.</param>
	/// <returns>Relative Direction interpreted from Orientation Semantic.</returns>
	public static Vector3 GetOrientationDirection(this Transform _transform, OrientationSemantics _orientation)
	{
		return _transform.TransformDirection(GetOrientation(_orientation));
	}

	/// <summary>Calculates the average of Vector's Components.</summary>
	/// <param name="_vector">Vector to calculate average from.</param>
	public static float GetAverage(this Vector3 _vector)
	{
		return ((_vector.x + _vector.y + _vector.z) / 3.0f);
	}

	/// <summary>Rotates a point towards another vector along given axis at an angular speed and keeping a distance.</summary>
	/// <param name="v">Point to rotate around.</param>
	/// <param name="o">Origin reference to rotate around.</param>
	/// <param name="axis">Rotation's Axis.</param>
	/// <param name="a">Angular Speed [as degrees].</param>
	/// <param name="r">Distance's Radius.</param>
	/// <returns>Point rotated around origin at 'r' distance.</returns>
	public static Vector3 RotatedAround(this Vector3 v, Vector3 o, Vector3 axis, float a, float r)
	{
		Quaternion rotation = Quaternion.Euler(axis * a);
		Vector3 d = (v - o).normalized;

		return o + (rotation * (d * r));
	}

	/// <summary>Evaluates if any of the given Vector's components is NaN.</summary>
	/// <param name="v">Vector to evaluate.</param>
	/// <returns>True if any of the vector components is NaN.</returns>
	public static bool IsNaN(this Vector3 v)
	{
		if(float.IsNaN(v.x)) return true;
		if(float.IsNaN(v.y)) return true;
		if(float.IsNaN(v.z)) return true;
		return false;
	}

	/// <summary>Evaluates if a vector has any NaN component, if it does it returns a given default vector.</summary>
	/// <param name="v">Vector to evaluate.</param>
	/// <param name="defaultVector">Default vector to return if given vector has any NaN component.</param>
	/// <returns>Filtered Vector.</returns>
	public static Vector3 NaNFilter(this Vector3 v, Vector3 defaultVector = default(Vector3))
	{
		return v.IsNaN() ? defaultVector : v;
	}

	/// <returns>True if Vector equals Vector3.zero.</returns>
	public static bool IsZero(this Vector3 v)
	{
		return v.x == 0.0f && v.y == 0.0f && v.z == 0.0f;
	}

	/// <summary>Converts Vector2 to Vector3 format.</summary>
	/// <param name="_vector">The Vector2 that will be converted to Vector3.</param>
	/// <returns>Vector2 converted to Vector3 format.</returns>
	public static Vector2 ToVector2(this Vector3 _vector)
	{
		return new Vector2(_vector.x, _vector.y);
	}

	/// \TODO Clean these functions:
	// For finite lines:
	public static Vector3 GetClosestPointOnFiniteLine(Vector3 point, Vector3 line_start, Vector3 line_end)
	{
	    Vector3 line_direction = line_end - line_start;
	    float line_length = line_direction.magnitude;
	    line_direction.Normalize();
	    float project_length = Mathf.Clamp(Vector3.Dot(point - line_start, line_direction), 0f, line_length);
	    return line_start + line_direction * project_length;
	}

	// For infinite lines:
	public static Vector3 GetClosestPointOnInfiniteLine(Vector3 point, Vector3 line_start, Vector3 line_end)
	{
	    return line_start + Vector3.Project(point - line_start, line_end - line_start);
	}
}
}