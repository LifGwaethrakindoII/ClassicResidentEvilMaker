using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VRigidbody
{
#region IVehicleSteeringBehaviors:
	/// <summary>Gets Seek Velocity.</summary>
	/// <param name="_vehicle">IVehicle's implementer.</param>
	/// <param name="_target">Target's point in space.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Seek Velocity.</returns>
	public static Vector3 GetSeekVelocity<T>(this T _vehicle, Vector3 _target, float _weight = 1.0f) where T : MonoBehaviour, IVehicle
	{
		Vector3 desired = ((_target - _vehicle.transform.position).normalized * _vehicle.maxSpeed);
		Vector3 steer = ((desired - _vehicle.body.velocity).normalized * _vehicle.maxForce);

		return (steer * _weight);
	}
	
	/// <summary>Gets Flee Velocity.</summary>
	/// <param name="_vehicle">IVehicle's implementer.</param>
	/// <param name="_target">Target's point in space.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Flee Velocity.</returns>
	public static Vector3 GetFleeVelocity<T>(this T _vehicle, Vector3 _target, float _weight = 1.0f) where T : MonoBehaviour, IVehicle
	{
		Vector3 desired = ((_vehicle.transform.position - _target).normalized * _vehicle.maxSpeed);
		Vector3 steer = ((desired - _vehicle.body.velocity).normalized * _vehicle.maxForce);

		return (steer * _weight);
	}
	
	/// <summary>Gets Pursuit Velocity.</summary>
	/// <param name="_vehicle">IVehicle's implementer.</param>
	/// <param name="_target">Target's point in space.</param>
	/// <param name="_predictionTime">Seconds in advance's target position.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Pursuit Velocity.</returns>
	public static Vector3 GetPursuitVelocity<T>(this T _vehicle, Rigidbody _target, float _predictionTime, float _weight = 1.0f) where T : MonoBehaviour, IVehicle
	{
		Vector3 desired = (((_target.position + (_target.velocity * _predictionTime)) - _vehicle.transform.position).normalized * _vehicle.maxSpeed);
		Vector3 steer = ((desired - _vehicle.body.velocity).normalized * _vehicle.maxForce);

		return (steer * _weight);
	}
	
	/// <summary>Gets Evasion Velocity.</summary>
	/// <param name="_vehicle">IVehicle's implementer.</param>
	/// <param name="_target">Target's point in space.</param>
	/// <param name="_predictionTime">Seconds in advance's target position.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Evasion Velocity.</returns>
	public static Vector3 GetEvasionVelocity<T>(this T _vehicle, Rigidbody _target, float _predictionTime, float _weight = 1.0f) where T : MonoBehaviour, IVehicle
	{
		Vector3 desired = (_vehicle.transform.position - ((_target.position + (_target.velocity * _predictionTime))).normalized * _vehicle.maxSpeed);
		Vector3 steer = ((desired - _vehicle.body.velocity).normalized * _vehicle.maxForce);

		return (steer * _weight);
	}

	/// <summary>Gets Separation Velocity.</summary>
	/// <param name="_body">Vehicle's Rigidbody.</param>
	/// <param name="_targets">Target's points in space.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Separation Velocity.</returns>
	public static Vector3 GetSeparationVelocity<T>(this Rigidbody _body, ICollection<T> _targets, float _maxSpeed, float _maxForce, float _weight = 1.0f) where T : MonoBehaviour, IVehicle
	{
		Vector3 steer = Vector3.zero;

		foreach(T vehicle in _targets)
		{
			steer += _body.GetFleeVelocity(vehicle.transform.position, _maxSpeed, _maxForce);
		}

		return _targets.Count > 0 ? ((steer / _targets.Count) * _weight) : steer;
	}

	/// <summary>Gets Separation Velocity.</summary>
	/// <param name="_vehicle">IVehicle's implementer.</param>
	/// <param name="_targets">Target's points in space.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Separation Velocity.</returns>
	public static Vector3 GetSeparationVelocity<T>(this T _vehicle, ICollection<T> _targets, float _maxSpeed, float _maxForce, float _weight = 1.0f) where T : MonoBehaviour, IVehicle
	{
		Vector3 steer = Vector3.zero;

		foreach(T vehicle in _targets)
		{
			steer += _vehicle.GetFleeVelocity(vehicle.transform.position);
		}

		return _targets.Count > 0 ? ((steer / _targets.Count) * _weight) : steer;
	}

	/// <summary>Gets Seek Velocity.</summary>
	/// <param name="_body">Vehicle's Rigidbody.</param>
	/// <param name="_bodies">Flock of Vegicles' Rigidbodies.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Seek Velocity.</returns>
	public static Vector3 GetAlignmentVelocity<T>(this Rigidbody _body, ICollection<T> _bodies, float _maxSpeed, float _maxForce, float _weight = 1.0f) where T : MonoBehaviour, IVehicle
	{
		Vector3 steer = Vector3.zero;

		foreach(T vehicle in _bodies)
		{
			steer += _body.GetSeekVelocity((vehicle.body.position + vehicle.body.velocity), _maxSpeed, _maxForce);
		}

		return _bodies.Count > 0 ? ((steer / _bodies.Count) * _weight) : steer;
	}

	/// <summary>Gets Seek Velocity.</summary>
	/// <param name="_vehicle">IVehicle's implementer.</param>
	/// <param name="_bodies">Flock of Vegicles' Rigidbodies.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Seek Velocity.</returns>
	public static Vector3 GetAlignmentVelocity<T>(this T _vehicle, ICollection<T> _bodies, float _maxSpeed, float _maxForce, float _weight = 1.0f) where T : MonoBehaviour, IVehicle
	{
		Vector3 steer = Vector3.zero;

		foreach(T vehicle in _bodies)
		{
			steer += _vehicle.GetSeekVelocity((vehicle.body.position + vehicle.body.velocity));
		}

		return _bodies.Count > 0 ? ((steer / _bodies.Count) * _weight) : steer;
	}

	/// <summary>Gets Seek Velocity.</summary>
	/// <param name="_body">Vehicle's Rigidbody.</param>
	/// <param name="_vehicles">Vehicles in space.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Seek Velocity.</returns>
	public static Vector3 GetCohesionVelocity<T>(this Rigidbody _body, ICollection<T> _vehicles, float _maxSpeed, float _maxForce, float _weight = 1.0f) where T : MonoBehaviour, IVehicle
	{
		Vector3 steer = Vector3.zero;

		foreach(T vehicle in _vehicles)
		{
			steer += _body.GetSeekVelocity(vehicle.body.position, _maxSpeed, _maxForce);
		}

		return _vehicles.Count > 0 ? ((steer / _vehicles.Count) * _weight) : steer;
	}

	/// <summary>Gets Seek Velocity.</summary>
	/// <param name="_vehicle">IVehicle's implementer.</param>
	/// <param name="_vehicles">Vehicles in space.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Seek Velocity.</returns>
	public static Vector3 GetCohesionVelocity<T>(this T _vehicle, ICollection<T> _vehicles, float _maxSpeed, float _maxForce, float _weight = 1.0f) where T : MonoBehaviour, IVehicle
	{
		Vector3 steer = Vector3.zero;

		foreach(T vehicle in _vehicles)
		{
			steer += _vehicle.GetSeekVelocity(vehicle.body.position);
		}

		return _vehicles.Count > 0 ? ((steer / _vehicles.Count) * _weight) : steer;
	}
#endregion

#region RigidbodySteeringBehaviors:
	/// <summary>Gets Seek Velocity.</summary>
	/// <param name="_body">Vehicle's Rigidbody.</param>
	/// <param name="_target">Target's point in space.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Seek Velocity.</returns>
	public static Vector3 GetSeekVelocity(this Rigidbody _body, Vector3 _target, float _maxSpeed, float _maxForce, float _weight = 1.0f)
	{
		Vector3 desired = ((_target - _body.position).normalized * _maxSpeed);
		Vector3 steer = ((desired - _body.velocity).normalized * _maxForce);

		return (steer * _weight);
	}

	/// <summary>Gets Flee Velocity.</summary>
	/// <param name="_body">Vehicle's Rigidbody.</param>
	/// <param name="_target">Target's point in space.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Flee Velocity.</returns>
	public static Vector3 GetFleeVelocity(this Rigidbody _body, Vector3 _target, float _maxSpeed, float _maxForce, float _weight = 1.0f)
	{
		Vector3 desired = ((_body.position - _target).normalized * _maxSpeed);
		Vector3 steer = ((desired - _body.velocity).normalized * _maxForce);

		return (steer * _weight);
	}

	/// <summary>Gets Pursuit Velocity.</summary>
	/// <param name="_body">Vehicle's Rigidbody.</param>
	/// <param name="_target">Target's point in space.</param>
	/// <param name="_predictionTime">Seconds in advance's target position.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Pursuit Velocity.</returns>
	public static Vector3 GetPursuitVelocity(this Rigidbody _body, Rigidbody _target, float _predictionTime, float _maxSpeed, float _maxForce, float _weight = 1.0f)
	{
		Vector3 desired = (((_target.position + (_target.velocity * _predictionTime)) - _body.position).normalized * _maxSpeed);
		Vector3 steer = ((desired - _body.velocity).normalized * _maxForce);

		return (steer * _weight);
	}

	/// <summary>Gets Evasion Velocity.</summary>
	/// <param name="_body">Vehicle's Rigidbody.</param>
	/// <param name="_target">Target's point in space.</param>
	/// <param name="_predictionTime">Seconds in advance's target position.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Evasion Velocity.</returns>
	public static Vector3 GetEvasionVelocity(this Rigidbody _body, Rigidbody _target, float _predictionTime, float _maxSpeed, float _maxForce, float _weight = 1.0f)
	{
		Vector3 desired = (_body.position - ((_target.position + (_target.velocity * _predictionTime))).normalized * _maxSpeed);
		Vector3 steer = ((desired - _body.velocity).normalized * _maxForce);

		return (steer * _weight);
	}

	/// <summary>Gets Separation Velocity.</summary>
	/// <param name="_body">Vehicle's Rigidbody.</param>
	/// <param name="_targets">Target's points in space.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Separation Velocity.</returns>
	public static Vector3 GetSeparationVelocity(this Rigidbody _body, ICollection<Vector3> _targets, float _maxSpeed, float _maxForce, float _weight = 1.0f)
	{
		Vector3 steer = Vector3.zero;

		foreach(Vector3 vector in _targets)
		{
			steer += _body.GetFleeVelocity(vector, _maxSpeed, _maxForce);
		}

		return _targets.Count > 0 ? ((steer / _targets.Count) * _weight) : steer;
	}

	/// <summary>Gets Separation Velocity.</summary>
	/// <param name="_body">Vehicle's Rigidbody.</param>
	/// <param name="_bodies">Vehicles' Rigidbodies in space.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Separation Velocity.</returns>
	public static Vector3 GetSeparationVelocity(this Rigidbody _body, ICollection<Rigidbody> _bodies, float _maxSpeed, float _maxForce, float _weight = 1.0f)
	{
		Vector3 steer = Vector3.zero;

		foreach(Rigidbody body in _bodies)
		{
			steer += _body.GetFleeVelocity(body.position, _maxSpeed, _maxForce);
		}

		return _bodies.Count > 0 ? ((steer / _bodies.Count) * _weight) : steer;
	}

	/// <summary>Gets Separation Velocity.</summary>
	/// <param name="_body">Vehicle's Rigidbody.</param>
	/// <param name="_targets">Target's points in space.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Separation Velocity.</returns>
	public static Vector3 GetSeparationVelocity(this Rigidbody _body, ICollection<Transform> _targets, float _maxSpeed, float _maxForce, float _weight = 1.0f)
	{
		Vector3 steer = Vector3.zero;

		foreach(Transform transform in _targets)
		{
			steer += _body.GetFleeVelocity(transform.position, _maxSpeed, _maxForce);
		}

		return _targets.Count > 0 ? ((steer / _targets.Count) * _weight) : steer;
	}

	/// <summary>Gets Seek Velocity.</summary>
	/// <param name="_body">Vehicle's Rigidbody.</param>
	/// <param name="_bodies">Flock of Vegicles' Rigidbodies.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Seek Velocity.</returns>
	public static Vector3 GetAlignmentVelocity(this Rigidbody _body, ICollection<Rigidbody> _bodies, float _maxSpeed, float _maxForce, float _weight = 1.0f)
	{
		Vector3 steer = Vector3.zero;

		foreach(Rigidbody body in _bodies)
		{
			steer += _body.GetSeekVelocity((body.position + body.velocity), _maxSpeed, _maxForce);
		}

		return _bodies.Count > 0 ? ((steer / _bodies.Count) * _weight) : steer;
	}

	/// <summary>Gets Seek Velocity.</summary>
	/// <param name="_body">Vehicle's Rigidbody.</param>
	/// <param name="_targets">Target's points in space.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Seek Velocity.</returns>
	public static Vector3 GetCohesionVelocity(this Rigidbody _body, ICollection<Vector3> _targets, float _maxSpeed, float _maxForce, float _weight = 1.0f)
	{
		Vector3 steer = Vector3.zero;

		foreach(Vector3 vector in _targets)
		{
			steer += _body.GetSeekVelocity(vector, _maxSpeed, _maxForce);
		}

		return _targets.Count > 0 ? ((steer / _targets.Count) * _weight) : steer;
	}

	/// <summary>Gets Seek Velocity.</summary>
	/// <param name="_body">Vehicle's Rigidbody.</param>
	/// <param name="_targets">Target's points in space.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Seek Velocity.</returns>
	public static Vector3 GetCohesionVelocity(this Rigidbody _body, ICollection<Transform> _targets, float _maxSpeed, float _maxForce, float _weight = 1.0f)
	{
		Vector3 steer = Vector3.zero;

		foreach(Transform transform in _targets)
		{
			steer += _body.GetSeekVelocity(transform.position, _maxSpeed, _maxForce);
		}

		return _targets.Count > 0 ? ((steer / _targets.Count) * _weight) : steer;
	}

	/// <summary>Gets Seek Velocity.</summary>
	/// <param name="_body">Vehicle's Rigidbody.</param>
	/// <param name="_bodies">Vehicles' Rigidbodies in space.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Seek Velocity.</returns>
	public static Vector3 GetCohesionVelocity(this Rigidbody _body, ICollection<Rigidbody> _bodies, float _maxSpeed, float _maxForce, float _weight = 1.0f)
	{
		Vector3 steer = Vector3.zero;

		foreach(Rigidbody body in _bodies)
		{
			steer += _body.GetSeekVelocity(body.position, _maxSpeed, _maxForce);
		}

		return _bodies.Count > 0 ? ((steer / _bodies.Count) * _weight) : steer;
	}

	public static float GetArriveForce(this Rigidbody _body, Vector3 _target, float _minRadius, float _maxRadius)
	{
		Vector3 relativeVelocity = (_body.position + _body.velocity);
		Vector3 desiredTarget = _minRadius == 0.0f ? _target : ((relativeVelocity - _target).normalized * _minRadius);
		Vector3 direction = (desiredTarget - relativeVelocity);
		
		float squareDistance = direction.sqrMagnitude;
		_minRadius *= _minRadius;
		_maxRadius *= _maxRadius;

		return Mathf.Clamp((squareDistance / (_maxRadius - _minRadius)), 0.0f, 1.0f);
	}
#endregion

#region DebugMethods:
	/// <summary>Debugs Rigidbody.</summary>
	/// <param name="_body">Rigidbody to debug.</param>
	/// <param name="_color">Debug's Color.</param>
	public static void Debug(this Rigidbody _body, Color _color)
	{
#if UNITY_EDITOR
		UnityEngine.Debug.DrawRay(_body.position, _body.velocity, _color);
#endif
	}

	/// <summary>Debugs IVehicle's implementer.</summary>
	/// <param name="_body">IVehicle's implementer to debug.</param>
	/// <param name="_color">Debug's Color.</param>
	public static void Debug<T>(this T _vehicle, Color _color) where T : MonoBehaviour, IVehicle
	{
#if UNITY_EDITOR
		UnityEngine.Debug.DrawRay(_vehicle.transform.position, _vehicle.body.velocity, _color);
#endif
	}
#endregion

	/// <summary>Cancels Rigidbody's Velocity on the Given Axes.</summary>
	/// <param name="_rigidbody">Rigidbody that extends the method.</param>
	/// <param name="_axes">Axes to cancel.</param>
	public static void CancelForcesOnAxes(this Rigidbody _rigidbody, Axes3D _axes)
	{
		Vector3 cancelScale = new Vector3
		(
			(_axes | Axes3D.X) == _axes ? 0.0f : 1.0f,
			(_axes | Axes3D.Y) == _axes ? 0.0f : 1.0f,
			(_axes | Axes3D.Z) == _axes ? 0.0f : 1.0f
		);
		_rigidbody.velocity = Vector3.Scale(_rigidbody.velocity, cancelScale);
	}
}
}