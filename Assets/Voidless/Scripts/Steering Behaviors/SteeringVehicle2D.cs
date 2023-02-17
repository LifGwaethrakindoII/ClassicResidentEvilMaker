using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

/*
Steps:
	- Get and Sum Forces (wighted sum is optional): { GetSeekForce(t), GetFleeForce(t), GetSeparationForce(t), etc. }
	- ApplyForces: { ApplyForces(sum) }
	- Displace: Displace(dt)
	- Optional, Rotate: Rotate(dt)
*/

namespace Voidless
{
public class SteeringVehicle2D : VMonoBehaviour
{
	[SerializeField] private float _maxSpeed; 			/// <summary>Vehicle's Maximum Speed.</summary>
	[SerializeField] private float _maxForce; 			/// <summary>Vehicle's Maximum Steering Force.</summary>
	[SerializeField] private float _mass; 				/// <summary>Vehicle's Mass.</summary>
	[Space(5f)]
	[SerializeField] private float _radius; 			/// <summary>Vehicle's Radius.</summary>
	[Space(5f)]
	[Header("Wander's Attributes:")]
	[SerializeField] private float _offset; 			/// <summary>Wander's Offset [Circle Distance].</summary>
	[SerializeField] private float _wanderRadius; 		/// <summary>Wander's Radius.</summary>
	[SerializeField] private float _angleChange; 		/// <summary>Wander's Angle Change.</summary>
	[Space(5f)]
	[Header("Flocking Attributes:")]
	[SerializeField] private float _separationRadius; 	/// <summary>Separation's Radius.</summary>
	[SerializeField] private float _cohesionRadius; 	/// <summary>Cohesion's Radius.</summary>
	[SerializeField] private float _allignmentRadius; 	/// <summary>Allignment's Radius.</summary>
	[Space(5f)]
	[SerializeField] private float _rotationSpeed; 		/// <summary>Rotation's Speed [for the Vehicle's transform].</summary>
#if UNITY_EDITOR
	[SerializeField] private Color color; 				/// <summary>Gizmos' Color.</summary>
#endif
	private Vector2 velocity; 							/// <summary>Vehicle's Velocity.</summary>
	private float wanderAngle; 							/// <summary>Wander's Angle Reference.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets maxSpeed property.</summary>
	public float maxSpeed
	{
		get { return _maxSpeed; }
		set { _maxSpeed = value; }
	}

	/// <summary>Gets and Sets maxForce property.</summary>
	public float maxForce
	{
		get { return _maxForce; }
		set { _maxForce = value; }
	}

	/// <summary>Gets and Sets mass property.</summary>
	public float mass
	{
		get { return _mass; }
		set { _mass = value; }
	}

	/// <summary>Gets and Sets offset property.</summary>
	public float offset
	{
		get { return _offset; }
		set { _offset = value; }
	}

	/// <summary>Gets and Sets wanderRadius property.</summary>
	public float wanderRadius
	{
		get { return _wanderRadius; }
		set { _wanderRadius = value; }
	}

	/// <summary>Gets and Sets radius property.</summary>
	public float radius
	{
		get { return _radius; }
		set { _radius = value; }
	}

	/// <summary>Gets and Sets angleChange property.</summary>
	public float angleChange
	{
		get { return _angleChange; }
		set { _angleChange = value; }
	}

	/// <summary>Gets and Sets separationRadius property.</summary>
	public float separationRadius
	{
		get { return _separationRadius; }
		set { _separationRadius = value; }
	}

	/// <summary>Gets and Sets cohesionRadius property.</summary>
	public float cohesionRadius
	{
		get { return _cohesionRadius; }
		set { _cohesionRadius = value; }
	}

	/// <summary>Gets and Sets allignmentRadius property.</summary>
	public float allignmentRadius
	{
		get { return _allignmentRadius; }
		set { _allignmentRadius = value; }
	}

	/// <summary>Gets and Sets rotationSpeed property.</summary>
	public float rotationSpeed
	{
		get { return _rotationSpeed; }
		set { _rotationSpeed = value; }
	}
#endregion

	/// <summary>Resets SteeringVehicle2D's instance to its default values.</summary>
	private void Reset()
	{
		velocity = Vector2.zero;
		wanderAngle = 0.0f;
		mass = 1.0f;
#if UNITY_EDITOR
		color = Color.red;
#endif
	}

#if UNITY_EDITOR
	/// <summary>Draws Gizmos on Editor mode.</summary>
	private void OnDrawGizmos()
	{
		Gizmos.color = color;

		if(radius != 0.0f)
		{
			Vector3 circleCenter = transform.position + (Vector3)(offset != 0.0f ? velocity.normalized * offset : Vector2.zero);

			Gizmos.DrawWireSphere(circleCenter, wanderRadius);
		}

		Gizmos.DrawRay(transform.position, velocity);
		Gizmos.DrawWireSphere(transform.position, radius);
	}
#endif

	/// <summary>SteeringVehicle2D's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		velocity = Vector2.zero;
	}

	/// <summary>Sets Velocity.</summary>
	/// <param name="v">New Velocity.</param>
	public void SetVelocity(Vector2 v)
	{
		velocity = v;

		if(velocity.sqrMagnitude > maxSpeed * maxSpeed) Vector2.ClampMagnitude(velocity, maxSpeed);
	}

	/// <summary>Applies force to velocity.</summary>
	/// <param name="force">Force to apply.</param>
	/// <returns>Applied Force.</returns>
	public Vector2 ApplyForce(Vector2 force)
	{
		/*float sqrMagnitude = (maxForce * maxForce);
		
		if(force.sqrMagnitude > sqrMagnitude)
		{
			float magnitude = Mathf.Sqrt(sqrMagnitude);
			//force = Vector2.ClampMagnitude(force, maxForce);
			force *= (maxForce / magnitude);
		}

		velocity += force;
		velocity = Vector2.ClampMagnitude(velocity, maxSpeed);*/
		velocity = ApplyForce(force, ref velocity, maxSpeed, maxForce);

		return velocity;
	}

	/// <summary>Applies force to velocity.</summary>
	/// <param name="force">Force to apply.</param>
	/// <param name="v">Velocity's reference.</param>
	/// <param name="s">Vehicle's Maximum Speed.</param>
	/// <param name="f">Vehicle's Maximum Steering Force.</param>
	/// <returns>Applied Force.</returns>
	public static Vector2 ApplyForce(Vector2 force, ref Vector2 v, float s, float f)
	{
		float sm = (f * f);
		
		if(force.sqrMagnitude > sm)
		{
			float m = Mathf.Sqrt(sm);
			//force = Vector2.ClampMagnitude(force, f);
			force *= (f / m);
		}

		v += force;
		v = Vector2.ClampMagnitude(v, s);

		return v;
	}

	/// <returns>Vehicle's Velocity.</returns>
	public Vector2 GetVelocity()
	{
		if(velocity.sqrMagnitude == 0.0f) velocity = transform.forward * maxSpeed;
		return velocity;
	}

	/// <summary>Resets Velocity.</summary>
	public void ResetVelocity() { velocity = Vector2.zero; }

	/// <returns>Vehicle's Position.</returns>
	public Vector2 GetPosition() { return new Vector2(transform.position.x, transform.position.y); }

	/// <returns>Vehicle's Wander Angle.</returns>
	public float GetWanderAngle() { return wanderAngle; }

	/// <summary>Displaces towards velocity.</summary>
	/// <param name="dt">Delta Time.</param>
	public void Displace(float dt) { transform.position += ((Vector3)velocity * dt).NaNFilter(); }

	/// <summary>Projects the vehicle on given time.</summary>
	/// <param name="t">Time's Projection [1.0f by default].</param>
	public Vector2 Project(float t = 1.0f) { return GetPosition() + (velocity * t); }

	/// <summary>Rotates towards velocity.</summary>
	/// <param name="dt">Delta Time.</param>
	/// <param name="_fix">Fix rotation when direction's x from the transform is less than 0.0f? false by default.</param>
	public void Rotate(float dt, bool _fix = false)
	{
		Rotate((Vector3)velocity, dt, _fix);
	}

	/// <summary>Rotates towards given direction.</summary>
	/// <param name="d">Direction to rotate towards.</param>
	/// <param name="dt">Delta Time.</param>
	/// <param name="_fix">Fix rotation when direction's x from the transform is less than 0.0f? false by default.</param>
	public void Rotate(Vector3 d, float dt, bool _fix = false)
	{
		if(velocity.sqrMagnitude == 0.0f) return;

		Quaternion rotation = _fix ? VQuaternion.LookRotation(d, Vector3.up) : Quaternion.LookRotation(d) ;
		transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * dt);
	}

#region LocalFunctions:
	/// <summary>Gets Seek Steering Force.</summary>
	/// <param name="t">Target's position.</param>
	/// <returns>Seek Steering Force towards target.</returns>
	public Vector2 GetSeekForce(Vector2 t)
	{
		return GetSeekForce(transform.position, t, ref velocity, maxSpeed, maxForce, mass); 
	}

	/// <summary>Gets Flee Steering Force.</summary>
	/// <param name="t">Target's position.</param>
	/// <returns>Flee Steering Force past the target.</returns>
	public Vector2 GetFleeForce(Vector2 t)
	{
		return GetFleeForce(transform.position, t, ref velocity, maxSpeed, maxForce, mass);
	}

	/// <returns>Wandering Point.</returns>
	public Vector2 GetWanderPoint()
	{
		return GetWanderPoint(transform.position, ref velocity, maxSpeed, maxForce, offset, wanderRadius, ref wanderAngle, angleChange, mass);
	}

	/// <returns>Wandering Steering Force.</returns>
	public Vector2 GetWanderForce()
	{
		return GetWanderForce(transform.position, ref velocity, maxSpeed, maxForce, offset, wanderRadius, ref wanderAngle, angleChange, mass);
	}

	/// <summary>Gets Arrival Weight between vehicle and target.</summary>
	/// <param name="p">Vehicle's Position.</param>
	/// <param name="t">Target's Position.</param>
	/// <param name="rMin">Minimum's Radius.</param>
	/// <param name="rMax">Maximum's Radius.</param>
	/// <returns>Normalized Arrival Weight.</returns>
	public float GetArrivalWeight(Vector2 t, float rMin, float rMax)
	{
		return GetArrivalWeight(transform.position, t, rMin, rMax);
	}

	/// <summary>Gets Arrival Weight between vehicle and target.</summary>
	/// <param name="p">Vehicle's Position.</param>
	/// <param name="t">Target's Position.</param>
	/// <param name="r">Arrival's Radius.</param>
	/// <returns>Normalized Arrival Weight.</returns>
	public float GetArrivalWeight(Vector2 t, float r)
	{
		return GetArrivalWeight(transform.position, t, r);
	}
#endregion

#region FlockingBehaviors:
	/// <summary>Gets separation force from neighborhood.</summary>
	/// <param name="_vehicles">Vehicles' neighborhood.</param>
	/// <returns>Separation force.</returns>
	public Vector2 GetSeparationForce(ICollection<SteeringVehicle2D> _vehicles)
	{
		if(_vehicles == null || _vehicles.Count <= 1) return Vector2.zero;

		Vector2 diffSum = Vector2.zero;
		Vector2 direction = Vector2.zero;
		Vector2 n = Vector2.zero;
		float m = 0.0f;
		float r = 0.0f;
		float count = 0.0f;

		foreach(SteeringVehicle2D vehicle in _vehicles)
		{
			if(vehicle == this) continue;

			direction = GetPosition() - vehicle.GetPosition();
			m = direction.magnitude;
			r = separationRadius + radius + _radius;

			/// If farther that the necessary separation, then there is no need for separation with the current ..
			if(m > 0.0f && m < r)
			{
				direction.Normalize();
				direction /= m;
				diffSum += direction;
				count++;

				Debug.DrawRay(transform.position, direction, Color.magenta);
			}
		}

		return count > 0.0f ? GetSeekForce(GetPosition() + (diffSum / count)) : Vector2.zero;
	}

	/// <summary>Gets Cohesion forces as an iterator.</summary>
	/// <param name="_vehicles">Vehicles' neighborhood.</param>
	/// <returns>Cohesion force.</returns>
	public Vector2 GetCohesionForce(ICollection<SteeringVehicle2D> _vehicles)
	{
		if(_vehicles == null || _vehicles.Count <= 1) return Vector2.zero;

		Vector2 sum = Vector2.zero;
		Vector2 direction = Vector2.zero;
		float count = 0.0f;
		float m = 0.0f;
		float r = 0.0f;

		foreach(SteeringVehicle2D vehicle in _vehicles)
		{
			if(vehicle == this) continue;

			direction = vehicle.GetPosition() - GetPosition();
			m = direction.magnitude;
			r = vehicle.cohesionRadius + vehicle.radius + radius;

			float c = m - vehicle.radius;

			if(m > 0.0f && m < r && c > 0.0f)
			{
				direction = direction.normalized * c;

				sum += GetPosition() + direction;
				count++;
			}
		}

		return count > 0.0f ? GetSeekForce(sum / count) : Vector2.zero;
	}

	/// <summary>Gets Allignments as an iterator.</summary>
	/// <param name="_vehicles">Vehicles' neighborhood.</param>
	/// <returns>Average allignment.</returns>
	public Vector2 GetAllignment(ICollection<SteeringVehicle2D> _vehicles)
	{
		if(_vehicles == null || _vehicles.Count <= 1) return Vector2.zero;

		Vector2 sum = Vector2.zero;
		Vector2 direction = Vector2.zero;
		float count = 0.0f;
		float m = 0.0f;
		float r = 0.0f;

		foreach(SteeringVehicle2D vehicle in _vehicles)
		{
			if(vehicle == this) continue;

			direction = vehicle.GetPosition() - GetPosition();
			m = direction.magnitude;
			r = vehicle.allignmentRadius + vehicle.radius + radius;

			if(m > 0.0f && m < r)
			{
				sum += vehicle.velocity;
				count++;
			}
		}

		return count > 0.0f ? GetSeekForce(GetPosition() + (sum / count)) : Vector2.zero;
	}
#endregion

#region StaticFunctions:
	/// <summary>Gets Seek Steering Force.</summary>
	/// <param name="p">Vehicle's Position.</param>
	/// <param name="t">Target's position.</param>
	/// <param name="v">Velocity's reference.</param>
	/// <param name="s">Vehicle's Maximum Speed.</param>
	/// <param name="f">Vehicle's Maximum Steering Force.</param>
	/// <param name="m">Vehicle's Mass [1.0 by default].</param>
	/// <returns>Seek Steering Force towards target.</returns>
	public static Vector2 GetSeekForce(Vector2 p, Vector2 t, ref Vector2 v, float s, float f, float m = 1.0f)
	{
		Vector2 d = t - p;
		d = d.normalized * s;

		Vector2 steering = d - v;
		steering = Vector2.ClampMagnitude(steering, f);

		if(m != 1.0f) steering /= m;

		return steering;
	}

	/// <summary>Gets Flee Steering Force.</summary>
	/// <param name="p">Vehicle's Position.</param>
	/// <param name="t">Target's position.</param>
	/// <param name="v">Velocity's reference.</param>
	/// <param name="s">Vehicle's Maximum Speed.</param>
	/// <param name="f">Vehicle's Maximum Steering Force.</param>
	/// <param name="m">Vehicle's Mass [1.0 by default].</param>
	/// <returns>Flee Steering Force past the target.</returns>
	public static Vector2 GetFleeForce(Vector2 p, Vector2 t, ref Vector2 v, float s, float f, float m = 1.0f)
	{
		Vector2 d = p - t;
		d = d.normalized * s;

		Vector2 steering = d - v;
		steering = Vector2.ClampMagnitude(steering, f);

		if(m != 1.0f) steering /= m;

		return steering;
	}

	/// <summary>Gets Wandering Point.</summary>
	/// <param name="p">Vehicle's Position.</param>
	/// <param name="v">Velocity's reference.</param>
	/// <param name="s">Vehicle's Maximum Speed.</param>
	/// <param name="f">Vehicle's Maximum Steering Force.</param>
	/// <param name="d">Circle's Distance from the position.</param>
	/// <param name="r">Circle's Radius.</param>
	/// <param name="a">Wander Angle's reference.</param>
	/// <param name="m">Vehicle's Mass [1.0 by default].</param>
	/// <returns>Wandering Point [without steering force applied].</returns>
	public static Vector2 GetWanderPoint(Vector2 p, ref Vector2 v, float s, float f, float d, float r, ref float a, float c, float m = 1.0f)
	{
		Vector2 displacement = (v.sqrMagnitude == 0.0f ? Vector2.right : v.normalized) * r;
		Vector2 circleCenter = p + (v.normalized * d);

		a += Random.Range(-c, c);

		/// No need to rotate the vector if there is no angle...
		if(a != 0.0f) displacement = displacement.Rotate(a);

		return (circleCenter + displacement);
	}

	/// <summary>Gets Wandering Steering Force.</summary>
	/// <param name="p">Vehicle's Position.</param>
	/// <param name="v">Velocity's reference.</param>
	/// <param name="s">Vehicle's Maximum Speed.</param>
	/// <param name="f">Vehicle's Maximum Steering Force.</param>
	/// <param name="d">Circle's Distance from the position.</param>
	/// <param name="r">Circle's Radius.</param>
	/// <param name="a">Wander Angle's reference.</param>
	/// <param name="m">Vehicle's Mass [1.0 by default].</param>
	/// <returns>Wandering Steering Force.</returns>
	public static Vector2 GetWanderForce(Vector2 p, ref Vector2 v, float s, float f, float d, float r, ref float a, float c, float m = 1.0f)
	{
		Vector2 target = GetWanderPoint(p, ref v, s, f, d, r, ref a, c, m);

#if UNITY_EDITOR
		Debug.DrawRay(target, Vector3.back * 5.0f, Color.cyan, 5.0f);
		Debug.DrawRay(p, target - p, Color.cyan, 5.0f);
#endif

		return GetSeekForce(p, target, ref v, s, f, m);
	}

	/// <summary>Gets Arrival Weight between vehicle and target.</summary>
	/// <param name="p">Vehicle's Position.</param>
	/// <param name="t">Target's Position.</param>
	/// <param name="rMin">Minimum's Radius.</param>
	/// <param name="rMax">Maximum's Radius.</param>
	/// <returns>Normalized Arrival Weight.</returns>
	public static float GetArrivalWeight(Vector2 p, Vector2 t, float rMin, float rMax)
	{
		if(rMin == rMax) rMin = 0.0f;
		
		float r = rMax - rMin;
		Vector2 d = t - p;

		r *= r;
		
		if(rMin > 0.0f) d = (t + (d.normalized * rMin)) - p;

		return (Mathf.Min(d.sqrMagnitude, r) / r);
	}

	/// <summary>Gets Arrival Weight between vehicle and target.</summary>
	/// <param name="p">Vehicle's Position.</param>
	/// <param name="t">Target's Position.</param>
	/// <param name="r">Arrival's Radius.</param>
	/// <returns>Normalized Arrival Weight.</returns>
	public static float GetArrivalWeight(Vector2 p, Vector2 t, float r)
	{
		return GetArrivalWeight(p, t, 0.0f, r);
	}
#endregion

}
}