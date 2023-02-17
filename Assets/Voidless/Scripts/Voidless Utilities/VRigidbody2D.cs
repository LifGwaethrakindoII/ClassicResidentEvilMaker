using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VRigidbody2D
{
	/// <summary>Configures Rigidbody2D for Dynamic body.</summary>
	/// <param name="_body">RigidbodyDD's Reference.</param>
	public static void SetForDynamicBody(this Rigidbody2D _body)
	{
		_body.bodyType = RigidbodyType2D.Dynamic;
		_body.simulated = true;
	}
	
	/// <summary>Configures Rigidbody2D for body with trigger Collider2D.</summary>
	/// <param name="_body">RigidbodyDD's Reference.</param>
	public static void SetForTrigger(this Rigidbody2D _body)
	{
		_body.bodyType = RigidbodyType2D.Kinematic;
		_body.useFullKinematicContacts = true;
		_body.simulated = true;
	}

	/// <summary>Moves Rigidbody2D considering also the Z-Axis.</summary>
	/// <param name="_rigidbody">Rigidbody to move.</param>
	/// <param name="d">Displacement Vector3.</param>
	public static void MoveIn3D(this Rigidbody2D _rigidbody, Vector3 d)
	{
		Vector3 tP = _rigidbody.transform.position;
		Vector3 rP = _rigidbody.position;

		_rigidbody.transform.position = new Vector3(
			rP.x + d.x,
			rP.y + d.y,
			tP.z + d.z
		).NaNFilter();
	}

#region IVehicle2DSteeringBehaviors:
	/// <summary>Gets Seek Velocity.</summary>
	/// <param name="_vehicle">IVehicle2D's implementer.</param>
	/// <param name="_target">Target's point in space.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Seek Velocity.</returns>
	public static Vector2 GetSeekVelocity<T>(this T _vehicle, Vector2 _target, float _weight = 1.0f) where T : MonoBehaviour, IVehicle2D
	{
		Vector2 desired = ((_target - (Vector2)_vehicle.transform.position).normalized * _vehicle.maxSpeed);
		Vector2 steer = ((desired - _vehicle.body.velocity).normalized * _vehicle.maxForce);

		return (steer * _weight);
	}
	
	/// <summary>Gets Flee Velocity.</summary>
	/// <param name="_vehicle">IVehicle2D's implementer.</param>
	/// <param name="_target">Target's point in space.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Flee Velocity.</returns>
	public static Vector2 GetFleeVelocity<T>(this T _vehicle, Vector2 _target, float _weight = 1.0f) where T : MonoBehaviour, IVehicle2D
	{
		Vector2 desired = (((Vector2)_vehicle.transform.position - _target).normalized * _vehicle.maxSpeed);
		Vector2 steer = ((desired - _vehicle.body.velocity).normalized * _vehicle.maxForce);

		return (steer * _weight);
	}
	
	/// <summary>Gets Pursuit Velocity.</summary>
	/// <param name="_vehicle">IVehicle2D's implementer.</param>
	/// <param name="_target">Target's point in space.</param>
	/// <param name="_predictionTime">Seconds in advance's target position.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Pursuit Velocity.</returns>
	public static Vector2 GetPursuitVelocity<T>(this T _vehicle, Rigidbody2D _target, float _predictionTime, float _weight = 1.0f) where T : MonoBehaviour, IVehicle2D
	{
		Vector2 desired = (((_target.position + (_target.velocity * _predictionTime)) - (Vector2)_vehicle.transform.position).normalized * _vehicle.maxSpeed);
		Vector2 steer = ((desired - _vehicle.body.velocity).normalized * _vehicle.maxForce);

		return (steer * _weight);
	}
	
	/// <summary>Gets Evasion Velocity.</summary>
	/// <param name="_vehicle">IVehicle2D's implementer.</param>
	/// <param name="_target">Target's point in space.</param>
	/// <param name="_predictionTime">Seconds in advance's target position.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Evasion Velocity.</returns>
	public static Vector2 GetEvasionVelocity<T>(this T _vehicle, Rigidbody2D _target, float _predictionTime, float _weight = 1.0f) where T : MonoBehaviour, IVehicle2D
	{
		Vector2 desired = ((Vector2)_vehicle.transform.position - ((_target.position + (_target.velocity * _predictionTime))).normalized * _vehicle.maxSpeed);
		Vector2 steer = ((desired - _vehicle.body.velocity).normalized * _vehicle.maxForce);

		return (steer * _weight);
	}

	/// <summary>Gets Separation Velocity.</summary>
	/// <param name="_body">Vehicle's Rigidbody2D.</param>
	/// <param name="_targets">Target's points in space.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Separation Velocity.</returns>
	public static Vector2 GetSeparationVelocity<T>(this Rigidbody2D _body, ICollection<T> _targets, float _maxSpeed, float _maxForce, float _weight = 1.0f) where T : MonoBehaviour, IVehicle2D
	{
		Vector2 steer = Vector2.zero;

		foreach(T vehicle in _targets)
		{
			steer += _body.GetFleeVelocity(vehicle.transform.position, _maxSpeed, _maxForce);
		}

		return _targets.Count > 0 ? ((steer / _targets.Count) * _weight) : steer;
	}

	/// <summary>Gets Separation Velocity.</summary>
	/// <param name="_vehicle">IVehicle2D's implementer.</param>
	/// <param name="_targets">Target's points in space.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Separation Velocity.</returns>
	public static Vector2 GetSeparationVelocity<T>(this T _vehicle, ICollection<T> _targets, float _maxSpeed, float _maxForce, float _weight = 1.0f) where T : MonoBehaviour, IVehicle2D
	{
		Vector2 steer = Vector2.zero;

		foreach(T vehicle in _targets)
		{
			steer += _vehicle.GetFleeVelocity((Vector2)vehicle.transform.position);
		}

		return _targets.Count > 0 ? ((steer / _targets.Count) * _weight) : steer;
	}

	/// <summary>Gets Seek Velocity.</summary>
	/// <param name="_body">Vehicle's Rigidbody2D.</param>
	/// <param name="_bodies">Flock of Vegicles' Rigidbodies.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Seek Velocity.</returns>
	public static Vector2 GetAlignmentVelocity<T>(this Rigidbody2D _body, ICollection<T> _bodies, float _maxSpeed, float _maxForce, float _weight = 1.0f) where T : MonoBehaviour, IVehicle2D
	{
		Vector2 steer = Vector2.zero;

		foreach(T vehicle in _bodies)
		{
			steer += _body.GetSeekVelocity((vehicle.body.position + vehicle.body.velocity), _maxSpeed, _maxForce);
		}

		return _bodies.Count > 0 ? ((steer / _bodies.Count) * _weight) : steer;
	}

	/// <summary>Gets Seek Velocity.</summary>
	/// <param name="_vehicle">IVehicle2D's implementer.</param>
	/// <param name="_bodies">Flock of Vegicles' Rigidbodies.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Seek Velocity.</returns>
	public static Vector2 GetAlignmentVelocity<T>(this T _vehicle, ICollection<T> _bodies, float _maxSpeed, float _maxForce, float _weight = 1.0f) where T : MonoBehaviour, IVehicle2D
	{
		Vector2 steer = Vector2.zero;

		foreach(T vehicle in _bodies)
		{
			steer += _vehicle.GetSeekVelocity((vehicle.body.position + vehicle.body.velocity));
		}

		return _bodies.Count > 0 ? ((steer / _bodies.Count) * _weight) : steer;
	}

	/// <summary>Gets Seek Velocity.</summary>
	/// <param name="_body">Vehicle's Rigidbody2D.</param>
	/// <param name="_vehicles">Vehicles in space.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Seek Velocity.</returns>
	public static Vector2 GetCohesionVelocity<T>(this Rigidbody2D _body, ICollection<T> _vehicles, float _maxSpeed, float _maxForce, float _weight = 1.0f) where T : MonoBehaviour, IVehicle2D
	{
		Vector2 steer = Vector2.zero;

		foreach(T vehicle in _vehicles)
		{
			steer += _body.GetSeekVelocity(vehicle.body.position, _maxSpeed, _maxForce);
		}

		return _vehicles.Count > 0 ? ((steer / _vehicles.Count) * _weight) : steer;
	}

	/// <summary>Gets Seek Velocity.</summary>
	/// <param name="_vehicle">IVehicle2D's implementer.</param>
	/// <param name="_vehicles">Vehicles in space.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Seek Velocity.</returns>
	public static Vector2 GetCohesionVelocity<T>(this T _vehicle, ICollection<T> _vehicles, float _maxSpeed, float _maxForce, float _weight = 1.0f) where T : MonoBehaviour, IVehicle2D
	{
		Vector2 steer = Vector2.zero;

		foreach(T vehicle in _vehicles)
		{
			steer += _vehicle.GetSeekVelocity(vehicle.body.position);
		}

		return _vehicles.Count > 0 ? ((steer / _vehicles.Count) * _weight) : steer;
	}
#endregion

#region Rigidbody2DSteeringBehaviors:
	/// <summary>Gets Seek Velocity.</summary>
	/// <param name="_body">Vehicle's Rigidbody2D.</param>
	/// <param name="_target">Target's point in space.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Seek Velocity.</returns>
	public static Vector2 GetSeekVelocity(this Rigidbody2D _body, Vector2 _target, float _maxSpeed, float _maxForce, float _weight = 1.0f)
	{
		Vector2 desired = ((_target - _body.position).normalized * _maxSpeed);
		Vector2 steer = ((desired - _body.velocity).normalized * _maxForce);

		return (steer * _weight);
	}

	/// <summary>Gets Flee Velocity.</summary>
	/// <param name="_body">Vehicle's Rigidbody2D.</param>
	/// <param name="_target">Target's point in space.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Flee Velocity.</returns>
	public static Vector2 GetFleeVelocity(this Rigidbody2D _body, Vector2 _target, float _maxSpeed, float _maxForce, float _weight = 1.0f)
	{
		Vector2 desired = ((_body.position - _target).normalized * _maxSpeed);
		Vector2 steer = ((desired - _body.velocity).normalized * _maxForce);

		return (steer * _weight);
	}

	/// <summary>Gets Pursuit Velocity.</summary>
	/// <param name="_body">Vehicle's Rigidbody2D.</param>
	/// <param name="_target">Target's point in space.</param>
	/// <param name="_predictionTime">Seconds in advance's target position.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Pursuit Velocity.</returns>
	public static Vector2 GetPursuitVelocity(this Rigidbody2D _body, Rigidbody2D _target, float _predictionTime, float _maxSpeed, float _maxForce, float _weight = 1.0f)
	{
		Vector2 desired = (((_target.position + (_target.velocity * _predictionTime)) - _body.position).normalized * _maxSpeed);
		Vector2 steer = ((desired - _body.velocity).normalized * _maxForce);

		return (steer * _weight);
	}

	/// <summary>Gets Evasion Velocity.</summary>
	/// <param name="_body">Vehicle's Rigidbody2D.</param>
	/// <param name="_target">Target's point in space.</param>
	/// <param name="_predictionTime">Seconds in advance's target position.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Evasion Velocity.</returns>
	public static Vector2 GetEvasionVelocity(this Rigidbody2D _body, Rigidbody2D _target, float _predictionTime, float _maxSpeed, float _maxForce, float _weight = 1.0f)
	{
		Vector2 desired = (_body.position - ((_target.position + (_target.velocity * _predictionTime))).normalized * _maxSpeed);
		Vector2 steer = ((desired - _body.velocity).normalized * _maxForce);

		return (steer * _weight);
	}

	/// <summary>Gets Separation Velocity.</summary>
	/// <param name="_body">Vehicle's Rigidbody2D.</param>
	/// <param name="_targets">Target's points in space.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Separation Velocity.</returns>
	public static Vector2 GetSeparationVelocity(this Rigidbody2D _body, ICollection<Vector2> _targets, float _maxSpeed, float _maxForce, float _weight = 1.0f)
	{
		Vector2 steer = Vector2.zero;

		foreach(Vector2 vector in _targets)
		{
			steer += _body.GetFleeVelocity(vector, _maxSpeed, _maxForce);
		}

		return _targets.Count > 0 ? ((steer / _targets.Count) * _weight) : steer;
	}

	/// <summary>Gets Separation Velocity.</summary>
	/// <param name="_body">Vehicle's Rigidbody2D.</param>
	/// <param name="_bodies">Vehicles' Rigidbodies in space.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Separation Velocity.</returns>
	public static Vector2 GetSeparationVelocity(this Rigidbody2D _body, ICollection<Rigidbody2D> _bodies, float _maxSpeed, float _maxForce, float _weight = 1.0f)
	{
		Vector2 steer = Vector2.zero;

		foreach(Rigidbody2D body in _bodies)
		{
			steer += _body.GetFleeVelocity(body.position, _maxSpeed, _maxForce);
		}

		return _bodies.Count > 0 ? ((steer / _bodies.Count) * _weight) : steer;
	}

	/// <summary>Gets Separation Velocity.</summary>
	/// <param name="_body">Vehicle's Rigidbody2D.</param>
	/// <param name="_targets">Target's points in space.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Separation Velocity.</returns>
	public static Vector2 GetSeparationVelocity(this Rigidbody2D _body, ICollection<Transform> _targets, float _maxSpeed, float _maxForce, float _weight = 1.0f)
	{
		Vector2 steer = Vector2.zero;

		foreach(Transform transform in _targets)
		{
			steer += _body.GetFleeVelocity(transform.position, _maxSpeed, _maxForce);
		}

		return _targets.Count > 0 ? ((steer / _targets.Count) * _weight) : steer;
	}

	/// <summary>Gets Seek Velocity.</summary>
	/// <param name="_body">Vehicle's Rigidbody2D.</param>
	/// <param name="_bodies">Flock of Vegicles' Rigidbodies.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Seek Velocity.</returns>
	public static Vector2 GetAlignmentVelocity(this Rigidbody2D _body, ICollection<Rigidbody2D> _bodies, float _maxSpeed, float _maxForce, float _weight = 1.0f)
	{
		Vector2 steer = Vector2.zero;

		foreach(Rigidbody2D body in _bodies)
		{
			steer += _body.GetSeekVelocity((body.position + body.velocity), _maxSpeed, _maxForce);
		}

		return _bodies.Count > 0 ? ((steer / _bodies.Count) * _weight) : steer;
	}

	/// <summary>Gets Seek Velocity.</summary>
	/// <param name="_body">Vehicle's Rigidbody2D.</param>
	/// <param name="_targets">Target's points in space.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Seek Velocity.</returns>
	public static Vector2 GetCohesionVelocity(this Rigidbody2D _body, ICollection<Vector2> _targets, float _maxSpeed, float _maxForce, float _weight = 1.0f)
	{
		Vector2 steer = Vector2.zero;

		foreach(Vector2 vector in _targets)
		{
			steer += _body.GetSeekVelocity(vector, _maxSpeed, _maxForce);
		}

		return _targets.Count > 0 ? ((steer / _targets.Count) * _weight) : steer;
	}

	/// <summary>Gets Seek Velocity.</summary>
	/// <param name="_body">Vehicle's Rigidbody2D.</param>
	/// <param name="_targets">Target's points in space.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Seek Velocity.</returns>
	public static Vector2 GetCohesionVelocity(this Rigidbody2D _body, ICollection<Transform> _targets, float _maxSpeed, float _maxForce, float _weight = 1.0f)
	{
		Vector2 steer = Vector2.zero;

		foreach(Transform transform in _targets)
		{
			steer += _body.GetSeekVelocity(transform.position, _maxSpeed, _maxForce);
		}

		return _targets.Count > 0 ? ((steer / _targets.Count) * _weight) : steer;
	}

	/// <summary>Gets Seek Velocity.</summary>
	/// <param name="_body">Vehicle's Rigidbody2D.</param>
	/// <param name="_bodies">Vehicles' Rigidbodies in space.</param>
	/// <param name="_maxSpeed">Vehicle's Maximum Speed.</param>
	/// <param name="_maxForce">Vehicle's Maximum Force.</param>
	/// <param name="_weight">Weight's multiplier.</param>
	/// <returns>Seek Velocity.</returns>
	public static Vector2 GetCohesionVelocity(this Rigidbody2D _body, ICollection<Rigidbody2D> _bodies, float _maxSpeed, float _maxForce, float _weight = 1.0f)
	{
		Vector2 steer = Vector2.zero;

		foreach(Rigidbody2D body in _bodies)
		{
			steer += _body.GetSeekVelocity(body.position, _maxSpeed, _maxForce);
		}

		return _bodies.Count > 0 ? ((steer / _bodies.Count) * _weight) : steer;
	}

	public static float GetArriveForce(this Rigidbody2D _body, Vector2 _target, float _minRadius, float _maxRadius)
	{
		Vector2 relativeVelocity = (_body.position + _body.velocity);
		Vector2 desiredTarget = _minRadius == 0.0f ? _target : ((relativeVelocity - _target).normalized * _minRadius);
		Vector2 direction = (desiredTarget - relativeVelocity);
		
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
	public static void Debug(this Rigidbody2D _body, Color _color)
	{
#if UNITY_EDITOR
		UnityEngine.Debug.DrawRay(_body.position, _body.velocity, _color);
#endif
	}

	/// <summary>Debugs IVehicle2D's implementer.</summary>
	/// <param name="_body">IVehicle2D's implementer to debug.</param>
	/// <param name="_color">Debug's Color.</param>
	public static void Debug<T>(this T _vehicle, Color _color) where T : MonoBehaviour, IVehicle2D
	{
#if UNITY_EDITOR
		UnityEngine.Debug.DrawRay(_vehicle.transform.position, _vehicle.body.velocity, _color);
#endif
	}
#endregion
}
}