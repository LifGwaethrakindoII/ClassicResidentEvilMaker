using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Voidless
{
public class SensorSystem2D : SerializedMonoBehaviour
{
	private const float RADIUS_SPHERE = 0.025f; 				/// <summary>Gizmos's sphere Radius.</summary>

	[SerializeField] private SensorSubsystem2D[] _subsystems; 	/// <summary>Sensor Subsystems.</summary>
	private SpriteRenderer _renderer; 							/// <summary>SpriteRenderer's Component.</summary>
	private Collider2D _collider; 								/// <summary>Collider2D's Component.</summary>

	/// <summary>Gets and Sets subsystems property.</summary>
	public SensorSubsystem2D[] subsystems
	{
		get { return _subsystems; }
		set { _subsystems = value; }
	}

	/// <summary>Gets and Sets renderer Component.</summary>
	public new SpriteRenderer renderer
	{ 
		get
		{
			if(_renderer == null) _renderer = GetComponent<SpriteRenderer>();
			return _renderer;
		}
	}

	/// <summary>Gets and Sets collider Component.</summary>
	public new Collider2D collider
	{ 
		get
		{
			if(_collider == null) _collider = GetComponent<Collider2D>();
			return _collider;
		}
	}

	/// <summary>Draws Gizmos when this GameObject is selected.</summary>
	private void OnDrawGizmosSelected()
	{
		DrawSensors();
	}

	/// <summary>Draws Sensor's Gizmos.</summary>
	private void DrawSensors()
	{
#if UNITY_EDITOR
		if(subsystems != null && subsystems.Length > 0)
		foreach(SensorSubsystem2D subsystem in subsystems)
		{
			Gizmos.color = subsystem.color;
			foreach(SensorData2D sensorData in subsystem)
			{
				Ray2D sensorRay2D = ConvertToRay2D(subsystem, sensorData);
				Vector2 origin = GetRelativeOriginPoint(subsystem, sensorData);
				Vector2 sensorDirection = sensorRay2D.direction * sensorData.distance;
				Gizmos.DrawSphere(origin, RADIUS_SPHERE);
				Gizmos.DrawSphere(sensorRay2D.origin, RADIUS_SPHERE); 

				switch(sensorData.sensorType)
				{
					case SensorType.Ray:
					Gizmos.DrawRay(sensorRay2D.origin, sensorDirection);
					break;

					case SensorType.Box:
					Gizmos.DrawRay(sensorRay2D.origin, sensorDirection);
					VGizmos.DrawWireBox(sensorRay2D.origin + sensorDirection, sensorData.dimensions, transform.rotation);
					break;

					case SensorType.Sphere:
					Gizmos.DrawRay(sensorRay2D.origin, sensorDirection);
					Gizmos.DrawWireSphere(sensorRay2D.origin + sensorDirection, sensorData.radius);
					break;

					case SensorType.Capsule:
					VGizmos.DrawCapsule(sensorRay2D.origin, sensorDirection, sensorData.radius, sensorData.distance);
					break;
				}
			}
		}
#endif
	}

#region SensorDetectionMethods:
	/// <summary>Evaluates if the sensor subsystem detects a hit.</summary>
	/// <param name="_subsystemIndex">Subsystem's Index.</param>
	/// <returns>True if hit with something, false otherwise.</returns>
	public bool GetSubsystemDetection(int _subsystemIndex)
	{
		RaycastHit2D hit = default(RaycastHit2D);
		return GetSubsystemDetection(_subsystemIndex, out hit);
	}

	/// <summary>Evaluates if the sensor subsystem detects a hit.</summary>
	/// <param name="_subsystemIndex">Subsystem's Index.</param>
	/// <param name="_hit">Hit information.</param>
	/// <returns>True if hit with something, false otherwise.</returns>
	public bool GetSubsystemDetection(int _subsystemIndex, out RaycastHit2D _hit)
	{
		_hit = default(RaycastHit2D);
		Vector2 sensorOrigin = Vector2.zero;

		return GetSubsystemDetection(_subsystemIndex, out _hit, out sensorOrigin);
	}

	/// <summary>Evaluates if the sensor subsystem detects a hit.</summary>
	/// <param name="_subsystemIndex">Subsystem's Index.</param>
	/// <param name="_hit">Hit information passed by reference.</param>
	/// <param name="_sensorOrigin">Sensor Origin's passed by reference.</param>
	/// <returns>True if hit with something, false otherwise.</returns>
	public bool GetSubsystemDetection(int _subsystemIndex, out RaycastHit2D _hit, out Vector2 _sensorOrigin)
	{
		_hit = default(RaycastHit2D);
		_sensorOrigin = Vector2.zero;

		if(!subsystems.CheckIfIndexBetweenBounds(_subsystemIndex))
		{
			Debug.LogError("[SensorSystem2D] Index provided " + _subsystemIndex + " is out of bounds. Returning false.");
			return false;
		}

		SensorSubsystem2D subsystem = subsystems[_subsystemIndex];

		foreach(SensorData2D sensorData in subsystem)
		{
			Ray2D ray = ConvertToRay2D(subsystem, sensorData);
			_sensorOrigin = ray.origin;

			switch(sensorData.sensorType)
			{
				case SensorType.Ray:
				_hit = Physics2D.Raycast(ray.origin, ray.direction, sensorData.distance, subsystem.layerMask);
				break;

				case SensorType.Box:
				_hit = VPhysics2D.BoxCast(ray.origin, sensorData.dimensions, transform.rotation.eulerAngles.z, ray.direction, sensorData.distance, subsystem.layerMask);
				break;

				case SensorType.Sphere:
				_hit = VPhysics2D.CircleCast(ray.origin, sensorData.radius, ray.direction, sensorData.distance, subsystem.layerMask);
				break;

				case SensorType.Capsule:
				_hit = VPhysics2D.CapsuleCast(ray.origin, sensorData.dimensions, CapsuleDirection2D.Horizontal, transform.rotation.eulerAngles.z, ray.direction, sensorData.distance, subsystem.layerMask);
				break;
			}

			if(_hit.transform != null) return true;
		}

		return false;
	}

	/// <summary>Evaluates if the sensor subsystem detects a hit.</summary>
	/// <param name="_subsystemIndex">Subsystem's Index.</param>
	/// <param name="_hits">Array of Hit informations.</param>
	/// <returns>True if hit with something, false otherwise.</returns>
	public bool GetSubsystemDetection(int _subsystemIndex, out RaycastHit2D[] _hits)
	{
		_hits = null;

		if(!subsystems.CheckIfIndexBetweenBounds(_subsystemIndex))
		{
			Debug.LogError("[SensorSystem2D] Index provided " + _subsystemIndex + " is out of bounds. Returning false.");
			return false;
		}

		SensorSubsystem2D subsystem = subsystems[_subsystemIndex];

		foreach(SensorData2D sensorData in subsystem)
		{
			Ray2D ray = ConvertToRay2D(subsystem, sensorData);

			switch(sensorData.sensorType)
			{
				case SensorType.Ray:
				_hits = Physics2D.RaycastAll(ray.origin, ray.direction, sensorData.distance, subsystem.layerMask);
				break;

				case SensorType.Box:
				_hits = VPhysics2D.BoxCastAll(ray.origin, sensorData.dimensions, transform.rotation.eulerAngles.z, ray.direction, sensorData.distance, subsystem.layerMask);
				break;

				case SensorType.Sphere:
				_hits = VPhysics2D.CircleCastAll(ray.origin, sensorData.radius, ray.direction, sensorData.distance, subsystem.layerMask);
				break;

				case SensorType.Capsule:
				_hits = VPhysics2D.CapsuleCastAll(ray.origin, sensorData.dimensions, CapsuleDirection2D.Horizontal, transform.rotation.eulerAngles.z, ray.direction, sensorData.distance, subsystem.layerMask);
				break;
			}

			if(_hits.Length > 0) return true;
		}

		return false;
	}

	/// <summary>Evaluates if the sensor subsystem detects a hit.</summary>
	/// <param name="_subsystemIndex">Subsystem's Index.</param>
	/// <param name="_collider">Reference to store potential Collider.</param>
	/// <returns>True if hit with something, false otherwise.</returns>
	public bool GetSubsystemDetection(int _subsystemIndex, out Collider2D _collider)
	{
		_collider = null;

		if(!subsystems.CheckIfIndexBetweenBounds(_subsystemIndex))
		{
			Debug.LogError("[SensorSystem2D] Index provided " + _subsystemIndex + " is out of bounds. Returning false.");
			return false;
		}

		SensorSubsystem2D subsystem = subsystems[_subsystemIndex];

		foreach(SensorData2D sensorData in subsystem)
		{
			Ray2D ray = ConvertToRay2D(subsystem, sensorData);

			switch(sensorData.sensorType)
			{
				case SensorType.Ray:
				Debug.LogError("[SensorSystem2D] There is no method to provide collider with a Ray2D sensor. Returning false");
				continue;

				case SensorType.Box:
				_collider = Physics2D.OverlapBox(ray.origin, sensorData.dimensions, transform.rotation.eulerAngles.z, subsystem.layerMask);
				break;

				case SensorType.Sphere:
				_collider = Physics2D.OverlapCircle(ray.origin, sensorData.radius, subsystem.layerMask);
				break;

				case SensorType.Capsule:
				_collider = Physics2D.OverlapCapsule(ray.origin, sensorData.dimensions, CapsuleDirection2D.Horizontal, transform.rotation.eulerAngles.z, subsystem.layerMask);
				break;
			}

			if(collider != null) return true;
		}

		return false;
	}

	/// <summary>Evaluates if the sensor subsystem detects a hit.</summary>
	/// <param name="_subsystemIndex">Subsystem's Index.</param>
	/// <param name="_colliders">Array of potential Collider.</param>
	/// <returns>True if hit with something, false otherwise.</returns>
	public bool GetSubsystemDetection(int _subsystemIndex, out Collider2D[] _colliders)
	{
		_colliders = null;

		if(!subsystems.CheckIfIndexBetweenBounds(_subsystemIndex))
		{
			Debug.LogError("[SensorSystem2D] Index provided " + _subsystemIndex + " is out of bounds. Returning false.");
			return false;
		}

		SensorSubsystem2D subsystem = subsystems[_subsystemIndex];

		foreach(SensorData2D sensorData in subsystem)
		{
			Ray2D ray = ConvertToRay2D(subsystem, sensorData);

			switch(sensorData.sensorType)
			{
				case SensorType.Ray:
				Debug.LogError("[SensorSystem2D] There is no method to provide colliders with a Ray2D sensor. Returning false");
				continue;

				case SensorType.Box:
				_colliders = Physics2D.OverlapBoxAll(ray.origin, sensorData.dimensions, transform.rotation.eulerAngles.z, subsystem.layerMask);
				break;

				case SensorType.Sphere:
				_colliders = Physics2D.OverlapCircleAll(ray.origin, sensorData.radius, subsystem.layerMask);
				break;

				case SensorType.Capsule:
				_colliders = Physics2D.OverlapCapsuleAll(ray.origin, sensorData.dimensions, CapsuleDirection2D.Horizontal, transform.rotation.eulerAngles.z, subsystem.layerMask);
				break;
			}

			if(_colliders != null && _colliders.Length > 0) return true;
		}

		return false;
	}
#endregion

	/// <summary>Gets origin relative to Subsystem's data and Sensor's data.</summary>
	/// <param name="_subsystem">Subsystem's Data.</param>
	/// <param name="_sensorData">Sensor's Data.</param>
	/// <returns>Origin point relative to subsystem and sensor's data.</returns>
	private Vector2 GetRelativeOriginPoint(SensorSubsystem2D _subsystem, SensorData2D _sensorData)
	{
		switch(_subsystem.relativeTo)
		{
			case RelativeTo.Transform: 		return transform.TransformPoint(_subsystem.relativeOrigin * _subsystem.originDistance);
			case RelativeTo.RendererBounds: return transform.TransformPoint(Vector2.Scale(_subsystem.relativeOrigin, renderer.bounds.extents) * _subsystem.originDistance);
			case RelativeTo.ColliderBounds: return transform.TransformPoint(Vector2.Scale(_subsystem.relativeOrigin, collider.bounds.extents) * _subsystem.originDistance);
			default: 						return Vector2.zero;
		}
	}

	/// <summary>Converts Subsystem's and Sensor's Data into a Ray2D.</summary>
	/// <param name="_subsystem">Subsystem's Data.</param>
	/// <param name="_sensorData">Sensor's Data.</param>
	/// <returns>Ray2D interpreted from both Subsystem and Sensor Data.</returns>
	private Ray2D ConvertToRay2D(SensorSubsystem2D _subsystem, SensorData2D _sensorData)
	{
		Ray2D ray = default(Ray2D);

		ray.origin = transform.TransformDirection(_sensorData.origin);
		ray.direction = (transform.TransformDirection(_sensorData.direction) * _sensorData.distance);

		switch(_subsystem.relativeTo)
		{
			case RelativeTo.Transform:
			ray.origin += (Vector2)transform.TransformPoint(_subsystem.relativeOrigin * _subsystem.originDistance);
			break;

			case RelativeTo.RendererBounds:
			ray.origin += (Vector2)transform.TransformPoint(Vector2.Scale(_subsystem.relativeOrigin, renderer.bounds.extents) * _subsystem.originDistance);
			break;

			case RelativeTo.ColliderBounds:
			ray.origin += (Vector2)transform.TransformPoint(Vector2.Scale(_subsystem.relativeOrigin, collider.bounds.extents) * _subsystem.originDistance);
			break;
		}

		return ray;
	}

	/// <returns>Subsystem's Origin.</returns>
	public Vector2 GetSubsystemOrigin(int index)
	{
		return transform.TransformDirection(subsystems[index].relativeOrigin);
	}
}
}