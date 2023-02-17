using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class SensorSystem3D : MonoBehaviour
{
	private const float RADIUS_SPHERE = 0.025f; 				/// <summary>Gizmos's sphere Radius.</summary>

	[SerializeField] private SensorSubsystem3D[] _subsystems; 	/// <summary>Boundaries' Sensors.</summary>

	/// <summary>Gets and Sets subsystems property.</summary>
	public SensorSubsystem3D[] subsystems
	{
		get { return _subsystems; }
		set { _subsystems = value; }
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
		foreach(SensorSubsystem3D subsystem in subsystems)
		{
			Gizmos.color = subsystem.color;
			foreach(SensorData3D sensorData in subsystem)
			{
				Ray sensorRay = ConvertToRay(subsystem, sensorData);
				Vector3 origin = transform.position + (transform.rotation * subsystem.offset);
				Vector3 sensorDirection = sensorRay.direction * sensorData.distance;
				Gizmos.DrawSphere(origin, RADIUS_SPHERE);
				Gizmos.DrawSphere(sensorRay.origin, RADIUS_SPHERE); 

				switch(sensorData.sensorType)
				{
					case SensorType.Ray:
					Gizmos.DrawRay(sensorRay.origin, sensorDirection);
					break;

					case SensorType.Box:
					Gizmos.DrawRay(sensorRay.origin, sensorDirection);
					VGizmos.DrawWireBox(sensorRay.origin, sensorData.dimensions, transform.rotation);
					VGizmos.DrawWireBox(sensorRay.origin + sensorDirection, sensorData.dimensions, transform.rotation);
					break;

					case SensorType.Sphere:
					/*Gizmos.DrawRay(sensorRay.origin, sensorDirection);
					Gizmos.DrawWireSphere(sensorRay.origin, sensorData.radius);
					Gizmos.DrawWireSphere(sensorRay.origin + sensorDirection, sensorData.radius);*/
					VGizmos.DrawCapsule(sensorRay.origin, sensorDirection, sensorData.radius, sensorData.distance);
					break;

					case SensorType.Capsule:
					VGizmos.DrawCapsule(sensorRay.origin, sensorDirection, sensorData.radius, sensorData.distance);
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
		if(!subsystems.CheckIfIndexBetweenBounds(_subsystemIndex))
		{
			Debug.LogError("[SensorSystem3D] Index provided " + _subsystemIndex + " is out of bounds. Returning false.");
			return false;
		}

		SensorSubsystem3D subsystem = subsystems[_subsystemIndex];

		foreach(SensorData3D sensorData in subsystem)
		{
			Ray ray = ConvertToRay(subsystem, sensorData);

			switch(sensorData.sensorType)
			{
				case SensorType.Ray:
				return Physics.Raycast(ray, sensorData.distance, subsystem.layerMask);

				case SensorType.Box:
				return VPhysics.BoxCast(ray.origin, sensorData.dimensions, ray.direction, transform.rotation, sensorData.distance, subsystem.layerMask);

				case SensorType.Sphere:
				return VPhysics.SphereCast(ray, sensorData.radius, sensorData.distance, subsystem.layerMask);

				case SensorType.Capsule:
				Vector3 destinyPoint = ray.origin + (ray.direction * sensorData.distance);
				return VPhysics.CapsuleCast(ray.origin, destinyPoint, sensorData.radius, ray.direction, sensorData.distance, subsystem.layerMask);
			}
		}

		return false;
	}

	/// <summary>Evaluates if the sensor subsystem detects a hit.</summary>
	/// <param name="_subsystemIndex">Subsystem's Index.</param>
	/// <param name="_hit">RaycastHit information.</param>
	/// <returns>True if hit with something, false otherwise.</returns>
	public bool GetSubsystemDetection(int _subsystemIndex, out RaycastHit _hit)
	{
		_hit = default(RaycastHit);

		if(!subsystems.CheckIfIndexBetweenBounds(_subsystemIndex))
		{
			Debug.LogError("[SensorSystem3D] Index provided " + _subsystemIndex + " is out of bounds. Returning false.");
			return false;
		}

		SensorSubsystem3D subsystem = subsystems[_subsystemIndex];

		foreach(SensorData3D sensorData in subsystem)
		{
			Ray ray = ConvertToRay(subsystem, sensorData);

			switch(sensorData.sensorType)
			{
				case SensorType.Ray:
				return Physics.Raycast(ray, out _hit, sensorData.distance, subsystem.layerMask);

				case SensorType.Box:
				return VPhysics.BoxCast(ray.origin, sensorData.dimensions, ray.direction, out _hit, transform.rotation, sensorData.distance, subsystem.layerMask);

				case SensorType.Sphere:
				return VPhysics.SphereCast(ray, sensorData.radius, out _hit, sensorData.distance, subsystem.layerMask);

				case SensorType.Capsule:
				Vector3 destinyPoint = ray.origin + (ray.direction * sensorData.distance);
				return VPhysics.CapsuleCast(ray.origin, destinyPoint, sensorData.radius, ray.direction, out _hit, sensorData.distance, subsystem.layerMask);
			}
		}

		return false;
	}

	/// <summary>Evaluates if the sensor subsystems contained in a given index detects a hit.</summary>
	/// <param name="_subsystemIndex">Subsystem's Index.</param>
	/// <param name="_hit">RaycastHit information.</param>
	/// <returns>True if hit with something, false otherwise.</returns>
	public bool GetSubsystemDetections(int _subsystemIndex, out RaycastHit _hit)
	{
		_hit = default(RaycastHit);

		if(!subsystems.CheckIfIndexBetweenBounds(_subsystemIndex))
		{
			Debug.LogError("[SensorSystem3D] Index provided " + _subsystemIndex + " is out of bounds. Returning false.");
			return false;
		}

		SensorSubsystem3D subsystem = subsystems[_subsystemIndex];

		foreach(SensorData3D sensorData in subsystem)
		{
			Ray ray = ConvertToRay(subsystem, sensorData);

			switch(sensorData.sensorType)
			{
				case SensorType.Ray:
				if(!Physics.Raycast(ray, out _hit, sensorData.distance, subsystem.layerMask)) return false;
				else break;

				case SensorType.Box:
				if(!VPhysics.BoxCast(ray.origin, sensorData.dimensions, ray.direction, out _hit, transform.rotation, sensorData.distance, subsystem.layerMask)) return false;
				else break;

				case SensorType.Sphere:
				if(!VPhysics.SphereCast(ray, sensorData.radius, out _hit, sensorData.distance, subsystem.layerMask)) return false;
				else break;

				case SensorType.Capsule:
				Vector3 destinyPoint = ray.origin + (ray.direction * sensorData.distance);
				if(!VPhysics.CapsuleCast(ray.origin, destinyPoint, sensorData.radius, ray.direction, out _hit, sensorData.distance, subsystem.layerMask)) return false;
				else break;
			}
		}

		return true;
	}

	/// <summary>Evaluates if the sensor subsystem detects a hit.</summary>
	/// <param name="_subsystemIndex">Subsystem's Index.</param>
	/// <param name="_hits">Array of RaycastHit information.</param>
	/// <returns>True if hit with something, false otherwise.</returns>
	public bool GetSubsystemDetection(int _subsystemIndex, out RaycastHit[] _hits)
	{
		_hits = null;

		if(!subsystems.CheckIfIndexBetweenBounds(_subsystemIndex))
		{
			Debug.LogError("[SensorSystem3D] Index provided " + _subsystemIndex + " is out of bounds. Returning false.");
			return false;
		}

		SensorSubsystem3D subsystem = subsystems[_subsystemIndex];

		foreach(SensorData3D sensorData in subsystem)
		{
			Ray ray = ConvertToRay(subsystem, sensorData);

			switch(sensorData.sensorType)
			{
				case SensorType.Ray:
				_hits = Physics.RaycastAll(ray, sensorData.distance, subsystem.layerMask);
				break;

				case SensorType.Box:
				_hits = VPhysics.BoxCastAll(ray.origin, sensorData.dimensions, ray.direction, transform.rotation, sensorData.distance, subsystem.layerMask);
				break;

				case SensorType.Sphere:
				_hits = VPhysics.SphereCastAll(ray, sensorData.radius, sensorData.distance, subsystem.layerMask);
				break;

				case SensorType.Capsule:
				Vector3 destinyPoint = ray.origin + (ray.direction * sensorData.distance);
				_hits = VPhysics.CapsuleCastAll(ray.origin, destinyPoint, sensorData.radius, ray.direction, sensorData.distance, subsystem.layerMask);
				break;
			}

			if(_hits.Length > 0) return true;
		}

		return false;
	}

	/// <summary>Evaluates if the sensor subsystem detects a hit.</summary>
	/// <param name="_subsystemIndex">Subsystem's Index.</param>
	/// <param name="_colliders">Array of potential Collider.</param>
	/// <returns>True if hit with something, false otherwise.</returns>
	public bool GetSubsystemDetection(int _subsystemIndex, out Collider[] _colliders)
	{
		_colliders = null;

		if(!subsystems.CheckIfIndexBetweenBounds(_subsystemIndex))
		{
			Debug.LogError("[SensorSystem3D] Index provided " + _subsystemIndex + " is out of bounds. Returning false.");
			return false;
		}

		SensorSubsystem3D subsystem = subsystems[_subsystemIndex];

		foreach(SensorData3D sensorData in subsystem)
		{
			Ray ray = ConvertToRay(subsystem, sensorData);

			switch(sensorData.sensorType)
			{
				case SensorType.Ray:
				Debug.LogError("[SensorSystem3D] There is no method to provide colliders with a Ray sensor. Returning false");
				continue;

				case SensorType.Box:
				_colliders = Physics.OverlapBox(ray.origin, sensorData.dimensions, transform.rotation, subsystem.layerMask);
				break;

				case SensorType.Sphere:
				_colliders = Physics.OverlapSphere(ray.origin, sensorData.radius, subsystem.layerMask);
				break;

				case SensorType.Capsule:
				Vector3 destinyPoint = ray.origin + (ray.direction * sensorData.distance);
				_colliders = Physics.OverlapCapsule(ray.origin, destinyPoint, sensorData.radius, subsystem.layerMask);
				break;
			}

			if(_colliders.Length > 0) return true;
		}

		return false;
	}
#endregion

	/// <summary>Converts Subsystem's and Sensor's Data into a Ray.</summary>
	/// <param name="_subsystem">Subsystem's Data.</param>
	/// <param name="_sensorData">Sensor's Data.</param>
	/// <returns>Ray interpreted from both Subsystem and Sensor Data.</returns>
	private Ray ConvertToRay(SensorSubsystem3D _subsystem, SensorData3D _sensorData)
	{
		Ray ray = default(Ray);

		ray.origin = transform.position + (transform.rotation * (_subsystem.offset + _sensorData.origin));
		ray.direction = transform.rotation * (_sensorData.direction.normalized * _sensorData.distance);

		return ray;
	}
}
}