using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VPhysics
{
	/// <summary>Calculates resulting velocity of 2 colliding Rigidbodies.</summary>
	/// <param name="va">Velocity A.</param>
	/// <param name="vb">Velocity B.</param>
	/// <param name="ma">A's Mass [1.0f by default].</param>
	/// <param name="mb">B's Mass [1.0f by default].</param>
	/// <returns>Resulting velocity for each Rigidbody [as a Vector3 Tuple].</returns>
	public static ValueVTuple<Vector3, Vector3> CollisionResolution(Vector3 va, Vector3 vb, float ma = 1.0f, float mb = 1.0f)
	{
		Vector3 vaf = (va * ((ma - mb) / (ma + mb))) + (vb * ((2.0f * mb) / (ma + mb)));
		Vector3 vbf = (va * ((2.0f * ma) / (ma + mb))) + (vb * ((mb - ma) / (ma + mb)));

		return new ValueVTuple<Vector3, Vector3>(vaf, vbf);
	}

	/// <summary>Calculates resulting velocity of 2 colliding Rigidbodies.</summary>
	/// <param name="a">Rigidbody A.</param>
	/// <param name="b">Rigidbody B.</param>
	/// <returns>Resulting velocity for each Rigidbody [as a Vector3 Tuple].</returns>
	public static ValueVTuple<Vector3, Vector3> CollisionResolution(Rigidbody a, Rigidbody b)
	{
		return CollisionResolution(a.velocity, b.velocity, a.mass, b.mass);
	}

	/// <summary>Gets a list of Components inside an overlapping Sphere.</summary>
	/// <param name="_origin">Sphere's Origin.</param>
	/// <param name="_radius">Sphere's Radius.</param>
	/// <param name="action">Ection to do for each Component.</param>
	/// <param name="_mask">LayerMask to selectively ignore certain Colliders.</param>
	/// <param name="_queryTriggerInteractions">Hit Interactions to Allow.</param>
	/// <returns>List of Components inside an overlapping Sphere.</returns>
	public static List<T> GetAllComponentsInOverLapSphere<T>(Vector3 _origin, float _radius, int _mask = Physics.AllLayers, QueryTriggerInteraction _queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) where T : Component
	{
		T component = null;
		Collider[] colliders = Physics.OverlapSphere(_origin, _radius, _mask, _queryTriggerInteraction);

		if(colliders.Length > 1)
		{
			List<T> list = new List<T>();

			foreach(Collider collider in colliders)
			{
				component = collider.gameObject.GetComponent<T>();
				if(component != null) list.Add(component);
			}

			return list;
		}
		else return null;
	}

	/// <summary>Dictates actions to do for each component inside an overlapping Sphere.</summary>
	/// <param name="_origin">Sphere's Origin.</param>
	/// <param name="_radius">Sphere's Radius.</param>
	/// <param name="action">Ection to do for each Component.</param>
	/// <param name="_mask">LayerMask to selectively ignore certain Colliders.</param>
	/// <param name="_queryTriggerInteractions">Hit Interactions to Allow.</param>
	public static void ForEachComponentInOverlapSphere<T>(Vector3 _origin, float _radius, Action<T> action, int _mask = Physics.AllLayers, QueryTriggerInteraction _queryTriggerInteraction = QueryTriggerInteraction.UseGlobal) where T : UnityEngine.Object
	{
		T component = null;
		Collider[] colliders = Physics.OverlapSphere(_origin, _radius, _mask, _queryTriggerInteraction);

		for(int i = 0; i < colliders.Length; i++)
		{
			component = colliders[i].gameObject.GetComponent<T>();
			if(component != null) action(component);	
		}
	}

	/// <summary>Calculates the time of intersection between 2 points moving at exactly opposite directions of each other.</summary>
	/// <param name="a">Point A.</param>
	/// <param name="b">Point B.</param>
	/// <param name="speedA">Speed of Point A.</param>
	/// <param name="speedB">Speed of Point B.</param>
	/// <returns>Time of intersection between point A and B.</returns>
	public static float TimeOfIntersectionBetween2Points(float a, float b, float speedA, float speedB)
	{
		float relativeSpeed = speedA + speedB;
		float distance = Mathf.Abs(a - b);
		/// float distanceCovered = t * relativeSpeed;

		return distance / relativeSpeed;
	}

	/// <summary>Gets a converted force given a ForceMode argument.</summary>
	/// <param name="_body">Rigidbody.</param>
	/// <param name="_force">Force.</param>
	/// <param name="_forceMode">Force Mode.</param>
	/// <returns>Converted Force.</returns>
	public static Vector3 ConvertedForce(this Rigidbody _body, Vector3 _force, ForceMode _forceMode)
	{
		switch(_forceMode)
		{
			case ForceMode.Force: 			return (_force / _body.mass) * Time.fixedDeltaTime;
			case ForceMode.Acceleration: 	return _force * Time.fixedDeltaTime;
			case ForceMode.Impulse: 		return (_force / _body.mass);
			default: 						return _force;
		}
	}

	/// <summary>Calculates a projectile's projection given a time t [pf = (g * (t^2/2)) + (v0 * t) + p0].</summary>
	/// <param name="t">Time t.</param>
	/// <param name="v0">Initial Velocity.</param>
	/// <param name="p0">Initial Position.</param>
	/// <param name="g">Gravity.</param>
	/// <returns>Projectile's Projection given time t.</returns>
	public static Vector3 ProjectileProjection(float t, Vector3 v0, Vector3 p0, Vector3 g)
	{
		return g * (0.5f * t * t) + (v0 * t) + p0;
	}

	/// <summary>Calculates desired Projectile's velocity to reach a point pf on time t.</summary>
	/// <param name="t">Time t.</param>
	/// <param name="p0">Initial Position.</param>
	/// <param name="pf">Desired Position.</param>
	/// <param name="g">Gravity.</param>
	/// <param name="a">Gravity Accelerates? true by default.</param>
	/// <returns>Desired Projectile's Initial Velocity to reach pf on time t.</returns>
	public static Vector3 ProjectileDesiredVelocity(float t, Vector3 p0, Vector3 pf, Vector3 g, bool a = true)
	{
		g = a ? g * (0.5f * t * t) : g * t;
		return (pf - (g + p0)) / t;
	}

	/// <summary>Gets projected velocity considering the ForceMode.</summary>
	/// <param name="v">Velocity to project.</param>
	/// <param name="t">Time's Projection.</param>
	/// <param name="f">Force's Mode [ForceMode.Force by default].</param>
	public static Vector3 GetVelocityAtTime(Vector3 v, float t = 1.0f, ForceMode f = ForceMode.Force)
	{
		switch(f)
		{
			case ForceMode.Force:
			case ForceMode.Acceleration: 	return v * t * Time.fixedDeltaTime;

			case ForceMode.Impulse:
			case ForceMode.VelocityChange: 	return v * t;

			default: 						return v;
		}
	}

#region SphereCast:
	/// <summary>Cast a Sphere along a direction and stores the result to a given hit information.</summary>
	/// <param name="ray">Ray.</param>
	/// <param name="r">Sphere's Radius.</param>
	/// <param name="hit">Hit Information.</param>
	/// <param name="l">Ray's Length.</param>
	/// <param name="mask">LayerMask to selectively ignore certain Colliders.</param>
	/// <param name="interactions">Hit Interactions to Allow.</param>
	/// <returns>True if the Sphere cast detected a Collider.</returns>
	public static bool SphereCast(Ray ray, float r, out RaycastHit hit, float l = Mathf.Infinity, int mask = Physics.DefaultRaycastLayers, QueryTriggerInteraction interactions = QueryTriggerInteraction.UseGlobal)
	{
		ray.origin = ray.origin - (ray.direction * r);

		return Physics.SphereCast(ray, r, out hit, l, mask, interactions);
	}

	/// <summary>Cast a Sphere along a direction and stores the result to a given hit information.</summary>
	/// <param name="o">Ray's Origin.</param>
	/// <param name="r">Sphere's Radius.</param>
	/// <param name="d">Cast's Direction.</param>
	/// <param name="hit">Hit Information.</param>
	/// <param name="l">Ray's Length.</param>
	/// <param name="mask">LayerMask to selectively ignore certain Colliders.</param>
	/// <param name="interactions">Hit Interactions to Allow.</param>
	/// <returns>True if the Sphere cast detected a Collider.</returns>
	public static bool SphereCast(Vector3 o, float r, Vector3 d, out RaycastHit hit, float l = Mathf.Infinity, int mask = Physics.DefaultRaycastLayers, QueryTriggerInteraction interactions = QueryTriggerInteraction.UseGlobal)
	{
		Vector3 origin = o - (d * r);
		Ray ray = new Ray(origin, d);

		return Physics.SphereCast(ray, r, out hit, l, mask, interactions);
	}

	/// <summary>Cast a Sphere along a direction.</summary>
	/// <param name="o">Ray's Origin.</param>
	/// <param name="r">Sphere's Radius.</param>
	/// <param name="d">Cast's Direction.</param>
	/// <param name="l">Ray's Length.</param>
	/// <param name="mask">LayerMask to selectively ignore certain Colliders.</param>
	/// <param name="interactions">Hit Interactions to Allow.</param>
	/// <returns>True if the Sphere cast detected a Collider.</returns>
	public static bool SphereCast(Ray ray, float r, float l = Mathf.Infinity, int mask = Physics.DefaultRaycastLayers, QueryTriggerInteraction interactions = QueryTriggerInteraction.UseGlobal)
	{
		ray.origin = ray.origin - (ray.direction * r);

		return Physics.SphereCast(ray, r, l, mask, interactions);
	}

	/// <summary>Cast a Sphere along a direction and stores the result to a given hit information.</summary>
	/// <param name="o">Ray's Origin.</param>
	/// <param name="r">Sphere's Radius.</param>
	/// <param name="d">Cast's Direction.</param>
	/// <param name="hit">Hit Information.</param>
	/// <param name="l">Ray's Length.</param>
	/// <param name="mask">LayerMask to selectively ignore certain Colliders.</param>
	/// <param name="interactions">Hit Interactions to Allow.</param>
	/// <param name="additionalDirections">Additional Directions to cast the ray along.</param>
	/// <returns>True if the Sphere cast detected a Collider.</returns>
	public static bool SphereCast(Vector3 o, float r, Vector3 d, out RaycastHit hit, Quaternion q, float maxD = Mathf.Infinity, int mask = Physics.DefaultRaycastLayers, QueryTriggerInteraction i = QueryTriggerInteraction.UseGlobal, params Vector3[] additionalDirections)
	{
		d.Normalize();

		float diameter = r * 2.0f;
		Vector3 offsetOrigin = o - (q * d * diameter);
		if(maxD == Mathf.Infinity) maxD = diameter;

		if(Physics.SphereCast(offsetOrigin, r, d, out hit, maxD, mask, i)) return true;

		if(additionalDirections != null && additionalDirections.Length > 0)
		{
			foreach(Vector3 direction in additionalDirections)
			{
				direction.Normalize();
				offsetOrigin = o - (q * direction * diameter);
				if(Physics.SphereCast(offsetOrigin, r, d, out hit, maxD, mask, i)) return true;
			}
		}
		return false;
	}

	/// <summary>Cast a Sphere along a direction to get all the RaycastHits intersected.</summary>
	/// <param name="o">Ray's Origin.</param>
	/// <param name="r">Sphere's Radius.</param>
	/// <param name="d">Cast's Direction.</param>
	/// <param name="l">Ray's Length.</param>
	/// <param name="mask">LayerMask to selectively ignore certain Colliders.</param>
	/// <param name="interactions">Hit Interactions to Allow.</param>
	/// <returns>RaycastHits intersected on the cast.</returns>
	public static RaycastHit[] SphereCastAll(Ray ray, float r, float l = Mathf.Infinity, int mask = Physics.DefaultRaycastLayers, QueryTriggerInteraction interactions = QueryTriggerInteraction.UseGlobal)
	{
		ray.origin = ray.origin - (ray.direction * r);

		return Physics.SphereCastAll(ray, r, l, mask, interactions);
	}
#endregion

#region BoxCast:
	/// <summary>Cast a Box along a direction and stores the result to a given hit information.</summary>
	/// <param name="o">Ray's Origin.</param>
	/// <param name="s">Box's Size.</param>
	/// <param name="d">Cast's Direction.</param>
	/// <param name="hit">Hit Information.</param>
	/// <param name="r">Box's Rotation.</param>
	/// <param name="l">Ray's Length.</param>
	/// <param name="mask">LayerMask to selectively ignore certain Colliders.</param>
	/// <param name="interactions">Hit Interactions to Allow.</param>
	/// <returns>True if the Box cast detected a Collider.</returns>
	public static bool BoxCast(Vector3 o, Vector3 s, Vector3 d, out RaycastHit hit, Quaternion r, float l = Mathf.Infinity, int mask = Physics.DefaultRaycastLayers, QueryTriggerInteraction interactions = QueryTriggerInteraction.UseGlobal)
	{
		Vector3 origin = o - (d * s.z * 0.5f);
		Ray ray = new Ray(origin, d);

		return Physics.BoxCast(ray.origin, s, ray.direction, out hit, r, l, mask, interactions);
	}

	/// <summary>Cast a Box along a direction.</summary>
	/// <param name="o">Ray's Origin.</param>
	/// <param name="s">Box's Size.</param>
	/// <param name="d">Cast's Direction.</param>
	/// <param name="r">Box's Rotation.</param>
	/// <param name="l">Ray's Length.</param>
	/// <param name="mask">LayerMask to selectively ignore certain Colliders.</param>
	/// <param name="interactions">Hit Interactions to Allow.</param>
	/// <returns>True if the Box cast detected a Collider.</returns>
	public static bool BoxCast(Vector3 o, Vector3 s, Vector3 d, Quaternion r, float l = Mathf.Infinity, int mask = Physics.DefaultRaycastLayers, QueryTriggerInteraction interactions = QueryTriggerInteraction.UseGlobal)
	{
		Vector3 origin = o - (d * s.z * 0.5f);
		Ray ray = new Ray(origin, d);

		return Physics.BoxCast(ray.origin, s, ray.direction, r, l, mask, interactions);
	}

	/// <summary>Cast a Box along a direction to get all the RaycastHits intersected.</summary>
	/// <param name="o">Ray's Origin.</param>
	/// <param name="s">Box's Size.</param>
	/// <param name="d">Cast's Direction.</param>
	/// <param name="r">Box's Rotation.</param>
	/// <param name="l">Ray's Length.</param>
	/// <param name="mask">LayerMask to selectively ignore certain Colliders.</param>
	/// <param name="interactions">Hit Interactions to Allow.</param>
	/// <returns>RaycastHits intersected on the cast.</returns>
	public static RaycastHit[] BoxCastAll(Vector3 o, Vector3 s, Vector3 d, Quaternion r, float l = Mathf.Infinity, int mask = Physics.DefaultRaycastLayers, QueryTriggerInteraction interactions = QueryTriggerInteraction.UseGlobal)
	{
		Vector3 origin = o - (d * s.z * 0.5f);
		Ray ray = new Ray(origin, d);

		return Physics.BoxCastAll(ray.origin, s, ray.direction, r, l, mask, interactions);
	}
#endregion

#region CapsuleCast:
	/// <summary>Cast a Capsule along a direction and stores the result to a given hit information.</summary>
	/// <param name="a">Point A [Ray's Origin].</param>
	/// <param name="b">Point B [Ray's end point].</param>
	/// <param name="r">Capsule's Radius.</param>
	/// <param name="d">Cast's Direction.</param>
	/// <param name="hit">Hit Information.</param>
	/// <param name="l">Ray's Length.</param>
	/// <param name="mask">LayerMask to selectively ignore certain Colliders.</param>
	/// <param name="interactions">Hit Interactions to Allow.</param>
	/// <returns>True if the Capsule cast detected a Collider.</returns>
	public static bool CapsuleCast(Vector3 a, Vector3 b, float r, Vector3 d, out RaycastHit hit, float l = Mathf.Infinity, int mask = Physics.DefaultRaycastLayers, QueryTriggerInteraction interactions = QueryTriggerInteraction.UseGlobal)
	{
		Vector3 origin = a - (d * r);
		Ray ray = new Ray(origin, d);

		return Physics.CapsuleCast(a, b, r, ray.direction, out hit, l, mask, interactions);
	}

	/// <summary>Cast a Capsule along a direction.</summary>
	/// <param name="a">Point A [Ray's Origin].</param>
	/// <param name="b">Point B [Ray's end point].</param>
	/// <param name="r">Capsule's Radius.</param>
	/// <param name="d">Cast's Direction.</param>
	/// <param name="l">Ray's Length.</param>
	/// <param name="mask">LayerMask to selectively ignore certain Colliders.</param>
	/// <param name="interactions">Hit Interactions to Allow.</param>
	/// <returns>True if the Capsule cast detected a Collider.</returns>
	public static bool CapsuleCast(Vector3 a, Vector3 b, float r, Vector3 d, float l = Mathf.Infinity, int mask = Physics.DefaultRaycastLayers, QueryTriggerInteraction interactions = QueryTriggerInteraction.UseGlobal)
	{
		Vector3 origin = a - (d * r);
		Ray ray = new Ray(origin, d);

		return Physics.CapsuleCast(a, b, r, ray.direction, l, mask, interactions);
	}

	/// <summary>Cast a Capsule along a direction to get all the RaycastHits intersected.</summary>
	/// <param name="a">Point A [Ray's Origin].</param>
	/// <param name="b">Point B [Ray's end point].</param>
	/// <param name="r">Capsule's Radius.</param>
	/// <param name="d">Cast's Direction.</param>
	/// <param name="l">Ray's Length.</param>
	/// <param name="mask">LayerMask to selectively ignore certain Colliders.</param>
	/// <param name="interactions">Hit Interactions to Allow.</param>
	/// <returns>RaycastHits intersected on the cast.</returns>
	public static RaycastHit[] CapsuleCastAll(Vector3 a, Vector3 b, float r, Vector3 d, float l = Mathf.Infinity, int mask = Physics.DefaultRaycastLayers, QueryTriggerInteraction interactions = QueryTriggerInteraction.UseGlobal)
	{
		Vector3 origin = a - (d * r);
		Ray ray = new Ray(origin, d);

		return Physics.CapsuleCastAll(a, b, r, ray.direction, l, mask, interactions);
	}
#endregion

	/*public static Vector3 ProjectileProjection(float t, Vector3 v0, Vector3 p0, params Vector3[] G, params float T)
	{
		if(G == null || T == null) return Vector3.zero;

		int n = Mathf.Min(G.Length, T.Length);
		Vector3 gravitySum = Vector3.zero;
		float tSum = 0.0f;

		for(int i = 0; i < n; i++)
		{
			if(t < tSum) break;

			Vector3 gx = G[i];
			float tx = T[i];
			float time = 

			gravitySum += ((i == 0) ? gx * (t * t * 0.05f) : )
			tSum += tx;
		}

		return gravitySum + (v0 * t) + p0;
	}*/
}
}