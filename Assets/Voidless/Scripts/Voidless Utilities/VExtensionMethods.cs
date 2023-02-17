using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Voidless
{
public static class VExtensionMethods
{
	private static Vector3 _CENTER_VIEWPORT_VECTOR_3;
	private static Vector2 _CENTER_VIEWPORT_VECTOR_2;

	public static Vector3 CENTER_VIEWPORT_VECTOR_3
	{
		get
		{
			if(_CENTER_VIEWPORT_VECTOR_3 == default(Vector3)) _CENTER_VIEWPORT_VECTOR_3 = new Vector3(0.5f, 0.5f);
			return _CENTER_VIEWPORT_VECTOR_3;
		}
	}

	public static Vector2 CENTER_VIEWPORT_VECTOR_2
	{
		get
		{
			if(_CENTER_VIEWPORT_VECTOR_2 == default(Vector2)) _CENTER_VIEWPORT_VECTOR_2 = new Vector3(0.5f, 0.5f);
			return _CENTER_VIEWPORT_VECTOR_2;
		}
	}

	/// <returns>Current's Frame-Rate [as integer].</returns>
	public static int GetFrameRate()
	{
		return (int)(1.0f / Time.smoothDeltaTime);
	}

	/// <summary>Creates a ValueTuple from a Tuple [only if both generic types are value types].</summary>
	/// <param name="_tuple">Tuple to retreive values from.</param>
	/// <returns>ValueTuple from Tuple's values.</returns>
	public static ValueVTuple<A, B> ToValueTuple<A, B>(this VTuple<A, B> _tuple) where A : struct where B : struct
	{
		return new ValueVTuple<A, B>(_tuple.Item1, _tuple.Item2);
	} 

	/// <summary>Sets Active collection of GameObjects to the desired bool.</summary>
	/// <param name="_active">Determines whether all GameObjects will be or not active.</param>
	public static void SetAllActive(this List<GameObject> _gameObjects, bool _active)
	{
		for(int i = 0; i < _gameObjects.Count; i++)
		{
			_gameObjects[i].SetActive(_active);
		}
	}

	/// <summary>Sets Active collection of GameObjects to the desired bool.</summary>
	/// <param name="_mono">List of MonoBehaviours.</param>
	/// <param name="_active">Determines whether all GameObjects will be or not active.</param>
	public static void SetAllActive<C>(this List<C> _list, bool _active) where C : Component
	{
		for(int i = 0; i < _list.Count; i++)
		{
			_list[i].gameObject.SetActive(_active);
		}
	}

	///\ TODO: Adapt this function to be a library function (with parameters overload) \\\
	/// <summary>Gets the second minium value of a float list.</summary>
	/// <param name="_list">The list of floats from where the second least value will be given.</param>
	/// <returns>Second minimum value of the list.</returns>
	public static float GetSecondMinimum(this List<float> _list)
	{
		//So they enter by default on minimum range.
		float least = Mathf.Infinity;
		float secondLeast = Mathf.Infinity;

		foreach(float number in  _list)
		{
			if(number <= least) //If current number is lower than the least value, then the prior least value passes as the secondLeast, and the least updates.
			{
				secondLeast = least;
				least = number;
			}
			else if(number < secondLeast) //If at least the current number is lower than the current second, update the second.
			secondLeast = number;
		}

		return secondLeast;
	}

	/// <summary>Dispatches Class.</summary>
	/// <param name="_class">Class to dispatch.</param>
	public static void Dispatch<T>(ref T _class) where T : class
	{
		if(_class != null)
		_class = null;
	}

	/// <summary>Checks if the Transform is being visible on viewport.</summary>
	/// <param name="_transform">The Transform that will check if it's on viewport.</param>
	/// <returns>Transform being seen on viewport (bool).</returns>
	public static bool IsVisibleToCamera(this Transform _transform)
	{
		Vector3 transformView = Camera.main.WorldToViewportPoint(_transform.position);

		return (transformView.x > 0.0f && transformView.x < 1.0f && transformView.y > 0.0f && transformView.y < 1.0f);
	}

	/// <summary>Checks if the GameObject is being visible on viewport.</summary>
	/// <param name="_gameObject">The GameObject that will check if it's on viewport.</param>
	/// <returns>GameObject being seen on viewport (bool).</returns>
	public static bool IsVisibleToCamera(this GameObject _gameObject)
	{
		return _gameObject.transform.IsVisibleToCamera();
	}

	/// <summary>Gets If GameObject has Component attached.</summary>
	/// <param name="_object">The GameObject that will check if has T Componentn attached.</param>
	/// <returns>GameObject has Component T (bool).</returns>
	public static bool Has<T>(this GameObject _object) where T : UnityEngine.Object
	{
		return (_object.GetComponent<T>() != null);
	}

	public static void DestroyOnEditor(this GameObject _object)
	{
		UnityEngine.Object.DestroyImmediate(_object);
	}

	/// <summary>Updates Viewport Plane.</summary>
	/// <param name="_transform">Transform reference.</param>
	/// <param name="_viewportPlane">Viewport Plane to update.</param>
	/// <param name="_fov">Field of View.</param>
	/// <param name="_distance">Distance from the transform.</param>
	/// <param name="_aspect">Aspect Ratio.</param>
	public static void UpdateViewportPlane(this Transform _transform, ref CameraViewportPlane _viewportPlane, float _fov, float _distance, float _aspect)
	{
		float z = _distance;
		float x = (Mathf.Tan(_fov * 0.5f) * z);
		float y = (x / _aspect);

		_viewportPlane.centerPoint = (_transform.position + (_transform.forward * _distance));
		_viewportPlane.topLeftPoint = _viewportPlane.centerPoint + new Vector3(-x, y, 0f);
		_viewportPlane.topRightPoint = _viewportPlane.centerPoint + new Vector3(x, y, 0f);
		_viewportPlane.bottomLeftPoint = _viewportPlane.centerPoint + new Vector3(-x, -y, 0f);
		_viewportPlane.bottomRightPoint = _viewportPlane.centerPoint + new Vector3(x, -y, 0f);
	}

	/// <summary>Updates Camera's Viewport Plane.</summary>
	/// <param name="_camera">Camera's reference.</param>
	/// <param name="_viewportPlane">Viewport Plane to update.</param>
	/// <returns>Updated Viewport Plane.</returns>
	public static CameraViewportPlane UpdateViewportPlane(this Camera _camera, ref CameraViewportPlane _viewportPlane)
	{
		float z = _camera.nearClipPlane;
		float x = (Mathf.Tan(_camera.fieldOfView * 0.5f) * z);
		float y = (x / _camera.aspect);

		_viewportPlane.centerPoint = (_camera.transform.position + (_camera.transform.forward * z));
		_viewportPlane.topLeftPoint = _viewportPlane.centerPoint + new Vector3(-x, y, 0f);
		_viewportPlane.topRightPoint = _viewportPlane.centerPoint + new Vector3(x, y, 0f);
		_viewportPlane.bottomLeftPoint = _viewportPlane.centerPoint + new Vector3(-x, -y, 0f);
		_viewportPlane.bottomRightPoint = _viewportPlane.centerPoint + new Vector3(x, -y, 0f);
		
		return _viewportPlane;
	}

	/// <summary>Sets Component's GameObject Active depending on the bool provided.</summary>
	/// <param name="_active">Set GameObject as Active?.</param>
	public static void SetActive(this Component _component, bool _active)
	{
		_component.gameObject.SetActive(_active);
	}

	/// <summary>Checks if a GameObject is in a LayerMask</summary>
	/// <param name="_object">GameObject to test</param>
	/// <param name="_layerMask">LayerMask with all the layers to test against</param>
	/// <returns>True if in any of the layers in the LayerMask</returns>
	public static bool IsInLayerMask(this GameObject _object, LayerMask _layerMask)
	{
	    // Convert the object's layer to a bitfield for comparison
	    int objLayerMask = (1 << _object.layer);
	    return ((_layerMask.value & objLayerMask) > 0);  // Extra round brackets required!
	}

	/// <summary>Checks if point is between viewport's threshold.</summary>
	/// <param name="_point">Point to evaluate.</param>
	/// <param name="_threshold">Threshold's limit.</param>
	/// <param name="_camera">Camera to calculate the viewport point [null as default, if null, it will get the main camera].</param>
	/// <param name="_calculateViewportPoint">Calculate viewport point? false by default.</param>
	public static bool BetweenViewportPoint(this Vector3 _point, Vector3 _threshold, Camera _camera = null, bool _calculateViewportPoint = false)
	{
		_threshold = _threshold.Clamped(0.0f, 0.5f) * 0.5f;
		if(_calculateViewportPoint)
		{
			if(_camera == null) _camera = Camera.main;
			_point = _camera.WorldToViewportPoint(_point);
		}

		return ((_point.x >= (CENTER_VIEWPORT_VECTOR_3.x - _threshold.x))
				&& (_point.x <= (CENTER_VIEWPORT_VECTOR_3.x + _threshold.x))
				&& (_point.y >=  (CENTER_VIEWPORT_VECTOR_3.y - _threshold.y))
				&& (_point.y <= (CENTER_VIEWPORT_VECTOR_3.y + _threshold.y)));
	}

	/// <summary>Checks if point is between viewport's threshold.</summary>
	/// <param name="_point">Point to evaluate.</param>
	/// <param name="_threshold">Threshold's limit.</param>
	/// <param name="_camera">Camera to calculate the viewport point [null as default, if null, it will get the main camera].</param>
	/// <param name="_calculateViewportPoint">Calculate viewport point? false by default.</param>
	public static bool BetweenViewportPoint(this Vector2 _point, Vector2 _threshold, Camera _camera = null, bool _calculateViewportPoint = false)
	{
		_threshold = _threshold.Clamped(0.0f, 0.5f) * 0.5f;
		if(_calculateViewportPoint)
		{
			if(_camera == null) _camera = Camera.main;
			_point = _camera.WorldToViewportPoint(_point);
		}

		return ((_point.x >= (CENTER_VIEWPORT_VECTOR_2.x - _threshold.x))
				&& (_point.x <= (CENTER_VIEWPORT_VECTOR_2.x + _threshold.x))
				&& (_point.y >=  (CENTER_VIEWPORT_VECTOR_2.y - _threshold.y))
				&& (_point.y <= (CENTER_VIEWPORT_VECTOR_2.y + _threshold.y)));
	}

	/// <summary>Checks if point is abovr viewport's threshold.</summary>
	/// <param name="_point">Point to evaluate.</param>
	/// <param name="_threshold">Threshold's limit.</param>
	/// <param name="_camera">Camera to calculate the viewport point [null as default, if null, it will get the main camera].</param>
	/// <param name="_calculateViewportPoint">Calculate viewport point? false by default.</param>
	public static bool AboveViewportPoint(this Vector3 _point, Vector3 _threshold, Camera _camera = null, bool _calculateViewportPoint = false)
	{
		_threshold = _threshold.Clamped(0.0f, 0.5f) * 0.5f;
		if(_calculateViewportPoint)
		{
			if(_camera == null) _camera = Camera.main;
			_point = _camera.WorldToViewportPoint(_point);
		}

		return ((_point.x <= (CENTER_VIEWPORT_VECTOR_3.x - _threshold.x))
				|| (_point.x >= (CENTER_VIEWPORT_VECTOR_3.x + _threshold.x))
				|| (_point.y <=  (CENTER_VIEWPORT_VECTOR_3.y - _threshold.y))
				|| (_point.y >= (CENTER_VIEWPORT_VECTOR_3.y + _threshold.y)));
	}

	/// <summary>Checks if point is abovr viewport's threshold.</summary>
	/// <param name="_point">Point to evaluate.</param>
	/// <param name="_threshold">Threshold's limit.</param>
	/// <param name="_camera">Camera to calculate the viewport point [null as default, if null, it will get the main camera].</param>
	/// <param name="_calculateViewportPoint">Calculate viewport point? false by default.</param>
	public static bool AboveViewportPoint(this Vector2 _point, Vector2 _threshold, Camera _camera = null, bool _calculateViewportPoint = false)
	{
		_threshold = _threshold.Clamped(0.0f, 0.5f) * 0.5f;
		if(_calculateViewportPoint)
		{
			if(_camera == null) _camera = Camera.main;
			_point = _camera.WorldToViewportPoint(_point);
		}

		return ((_point.x <= (CENTER_VIEWPORT_VECTOR_2.x - _threshold.x))
				|| (_point.x >= (CENTER_VIEWPORT_VECTOR_2.x + _threshold.x))
				|| (_point.y <=  (CENTER_VIEWPORT_VECTOR_2.y - _threshold.y))
				|| (_point.y >= (CENTER_VIEWPORT_VECTOR_2.y + _threshold.y))); 
	}

	/// <summary>Subscribes Sight Sensor Listener to Sight Sensor's Events.</summary>
	/// <param name="_listener">Sight Sensor's Listener to subscribe.</param>
	/// <param name="_sightSensor">Sight Sensor.</param>
	public static void SubscribeToSightSensorEvents(this ISightSensorListener _listener, SightSensor _sightSensor)
	{
		_sightSensor.onColliderSighted += _listener.OnColliderSighted;
		_sightSensor.onColliderOccluded += _listener.OnColliderOccluded; 
		_sightSensor.onColliderOutOfSight += _listener.OnColliderOutOfSight;
	}

	/// <summary>Unsubscribes Sight Sensor Listener to Sight Sensor's Events.</summary>
	/// <param name="_listener">Sight Sensor's Listener to unsubscribe.</param>
	/// <param name="_sightSensor">Sight Sensor.</param>
	public static void UnsubscribeToSightSensorEvents(this ISightSensorListener _listener, SightSensor _sightSensor)
	{
		_sightSensor.onColliderSighted -= _listener.OnColliderSighted;
		_sightSensor.onColliderOccluded -= _listener.OnColliderOccluded; 
		_sightSensor.onColliderOutOfSight -= _listener.OnColliderOutOfSight;
	}

	/// <returns>Gets Delta Time coefficient from Enum's value.</returns>
	/// <param name="_type">Type of Time's Delta.</param>
	public static float GetDeltaTime(DeltaTimeType _type)
	{
		switch(_type)
		{
			case DeltaTimeType.DeltaTime: return Time.deltaTime;
			case DeltaTimeType.FixedDeltaTime: return Time.fixedDeltaTime;
			case DeltaTimeType.SmoothDeltaTime: return Time.smoothDeltaTime;
			default: return Time.deltaTime;
		}
	}

	/// <returns>All GameObjects on Scene.</returns>
	public static GameObject[] GetAllGameObjectsInScene()
	{
		return UnityEngine.Object.FindObjectsOfType<GameObject>();
	}

	/// <summary>Retreives component from this object or its parents.</summary>
	/// <param name="obj">Main GameObject's reference.</param>
	/// <returns>Retreived component, if existing.</returns>
	public static T GetComponentHereOrInParent<T>(this GameObject obj) where T : class
	{
		T result = null;

		while(obj != null)
		{
			result = obj.GetComponent<T>();

			if(result != null) return result;

			obj = obj.transform.parent.gameObject;
		}

		return null;
	}
}
}
