using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VPhysics2D
{
	/// <summary>Calculates resulting velocity of 2 colliding Rigidbodies.</summary>
	/// <param name="va">Velocity A.</param>
	/// <param name="vb">Velocity B.</param>
	/// <param name="ma">A's Mass [1.0f by default].</param>
	/// <param name="mb">B's Mass [1.0f by default].</param>
	/// <returns>Resulting velocity for each Rigidbody [as a Vector2 Tuple].</returns>
	public static ValueVTuple<Vector2, Vector2> CollisionResolution(Vector2 va, Vector2 vb, float ma = 1.0f, float mb = 1.0f)
	{
		Vector2 vaf = (va * ((ma - mb) / (ma + mb))) + (vb * ((2.0f * mb) / (ma + mb)));
		Vector2 vbf = (va * ((2.0f * ma) / (ma + mb))) + (vb * ((mb - ma) / (ma + mb)));

		return new ValueVTuple<Vector2, Vector2>(vaf, vbf);
	}

	/// <summary>Calculates resulting velocity of 2 colliding Rigidbodies.</summary>
	/// <param name="a">Rigidbody A.</param>
	/// <param name="b">Rigidbody B.</param>
	/// <returns>Resulting velocity for each Rigidbody [as a Vector2 Tuple].</returns>
	public static ValueVTuple<Vector2, Vector2> CollisionResolution(Rigidbody2D a, Rigidbody2D b)
	{
		return CollisionResolution(a.velocity, b.velocity, a.mass, b.mass);
	}

	/// <summary>Gets a converted force given a ForceMode argument.</summary>
	/// <param name="_body">Rigidbody2D.</param>
	/// <param name="_force">Force.</param>
	/// <param name="_forceMode">Force Mode.</param>
	/// <returns>Converted Force.</returns>
	public static Vector2 ConvertedForce(this Rigidbody2D _body, Vector2 _force, ForceMode _forceMode)
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
	public static Vector2 ProjectileProjection(float t, Vector2 v0, Vector2 p0, Vector2 g)
	{
		return g * (0.5f * t * t) + (v0 * t) + p0;
	}

	/// <summary>Calculates desired Projectile's velocity to reach a point pf on time t.</summary>
	/// <param name="t">Time t.</param>
	/// <param name="p0">Initial Position.</param>
	/// <param name="pf">Desired Position.</param>
	/// <param name="g">Gravity.</param>
	/// <returns>Desired Projectile's Initial Velocity to reach pf on time t.</returns>
	public static Vector2 ProjectileDesiredVelocity(float t, Vector2 p0, Vector2 pf, Vector2 g)
	{
		return (pf - (g * (0.5f * t * t) + p0)) / t;
	}

	/// <summary>Gets projected velocity considering the ForceMode.</summary>
	/// <param name="v">Velocity to project.</param>
	/// <param name="t">Time's Projection.</param>
	/// <param name="f">Force's Mode [ForceMode.Force by default].</param>
	public static Vector2 GetVelocityAtTime(Vector2 v, float t = 1.0f, ForceMode f = ForceMode.Force)
	{
		switch(f)
		{
			case ForceMode.Force:
			case ForceMode.Acceleration: 	return v * t * t * 0.5f;

			case ForceMode.Impulse:
			case ForceMode.VelocityChange: 	return v * t;

			default: 						return v * t;
		}
	}

#region CircleCast:
	/// <summary>Cast a Circle along a direction and stores the result to a given hit information.</summary>
	/// <param name="o">Origin.</param>
	/// <param name="r">Circle's Radius.</param>
	/// <param name="d">Direction.</param>
	/// <param name="l">Ray2D's Length.</param>
	/// <param name="mask">LayerMask to selectively ignore certain Colliders.</param>
	/// <param name="minD">Only include objects with a Z coordinate (depth) greater than or equal to this value [Mathf.NegativeInfinity by default].</param>
	/// <param name="maxD">Only include objects with a Z coordinate (depth) less than or equal to this value [Mathf.Infinity by default].</param>
	/// <returns>RaycastHit2D information.</returns>
	public static RaycastHit2D CircleCast(Vector2 o, float r, Vector2 d, float l = Mathf.Infinity, int mask = Physics.DefaultRaycastLayers, float minD = Mathf.NegativeInfinity, float maxD = Mathf.Infinity)
	{
		Vector2 origin = o - (d * r);

		return Physics2D.CircleCast(origin, r, d, l, mask, minD, maxD);
	}

	/// <summary>Cast a Circle along a direction to get all the RaycastHit2Ds intersected.</summary>
	/// <param name="o">Origin.</param>
	/// <param name="r">Circle's Radius.</param>
	/// <param name="d">Direction.</param>
	/// <param name="l">Ray2D's Length.</param>
	/// <param name="mask">LayerMask to selectively ignore certain Colliders.</param>
	/// <param name="minD">Only include objects with a Z coordinate (depth) greater than or equal to this value [Mathf.NegativeInfinity by default].</param>
	/// <param name="maxD">Only include objects with a Z coordinate (depth) less than or equal to this value [Mathf.Infinity by default].</param>
	/// <returns>RaycastHit2Ds intersected on the cast.</returns>
	public static RaycastHit2D[] CircleCastAll(Vector2 o, float r, Vector2 d, float l = Mathf.Infinity, int mask = Physics.DefaultRaycastLayers, float minD = Mathf.NegativeInfinity, float maxD = Mathf.Infinity)
	{
		Vector2 origin = o - (d * r);

		return Physics2D.CircleCastAll(origin, r, d, l, mask, minD, maxD);
	}
#endregion

#region BoxCast:
	/// <summary>Cast a Box along a direction and stores the result to a given hit information.</summary>
	/// <param name="o">Ray2D's Origin.</param>
	/// <param name="s">Box's Size.</param>
	/// <param name="a">Box's Angle.</param>
	/// <param name="d">Cast's Direction.</param>
	/// <param name="l">Ray2D's Length.</param>
	/// <param name="mask">LayerMask to selectively ignore certain Colliders.</param>
	/// <param name="minD">Only include objects with a Z coordinate (depth) greater than or equal to this value [Mathf.NegativeInfinity by default].</param>
	/// <param name="maxD">Only include objects with a Z coordinate (depth) less than or equal to this value [Mathf.Infinity by default].</param>
	/// <returns>RaycastHit2D Information.</returns>
	public static RaycastHit2D BoxCast(Vector2 o, Vector2 s, float a, Vector2 d, float l = Mathf.Infinity, int mask = Physics.DefaultRaycastLayers, float minD = Mathf.NegativeInfinity, float maxD = Mathf.Infinity)
	{
		Vector2 origin = o - (d * s.x * 0.5f);

		return Physics2D.BoxCast(origin, s, a, d, l, mask, minD, maxD);
	}

	/// <summary>Cast a Box along a direction to get all the RaycastHit2Ds intersected.</summary>
	/// <param name="o">Ray2D's Origin.</param>
	/// <param name="s">Box's Size.</param>
	/// <param name="a">Box's Angle.</param>
	/// <param name="d">Cast's Direction.</param>
	/// <param name="l">Ray2D's Length.</param>
	/// <param name="mask">LayerMask to selectively ignore certain Colliders.</param>
	/// <param name="minD">Only include objects with a Z coordinate (depth) greater than or equal to this value [Mathf.NegativeInfinity by default].</param>
	/// <param name="maxD">Only include objects with a Z coordinate (depth) less than or equal to this value [Mathf.Infinity by default].</param>
	/// <returns>RaycastHit2Ds intersected on the cast.</returns>
	public static RaycastHit2D[] BoxCastAll(Vector2 o, Vector2 s, float a, Vector2 d, float l = Mathf.Infinity, int mask = Physics.DefaultRaycastLayers, float minD = Mathf.NegativeInfinity, float maxD = Mathf.Infinity)
	{
		Vector2 origin = o - (d * s.x * 0.5f);

		return Physics2D.BoxCastAll(origin, s, a, d, l, mask, minD, maxD);
	}
#endregion

#region CapsuleCast:
	/// <summary>Cast a Capsule along a direction and stores the result to a given hit information.</summary>
	/// <param name="o">Capsule's Origin.</param>
	/// <param name="s">Capsule's Size.</param>
	/// <param name="capsuleDirection">Capsule's Direction.</param>
	/// <param name="a">Capsule's Angle.</param>
	/// <param name="d">Capsule's Direction.</param>
	/// <param name="l">Ray2D's Length.</param>
	/// <param name="mask">LayerMask to selectively ignore certain Colliders.</param>
	/// <param name="minD">Only include objects with a Z coordinate (depth) greater than or equal to this value [Mathf.NegativeInfinity by default].</param>
	/// <param name="maxD">Only include objects with a Z coordinate (depth) less than or equal to this value [Mathf.Infinity by default].</param>
	/// <returns>RaycastHit2D Information.</returns>
	public static RaycastHit2D CapsuleCast(Vector2 o, Vector2 s, CapsuleDirection2D capsuleDirection, float a, Vector2 d, float l = Mathf.Infinity, int mask = Physics.DefaultRaycastLayers, float minD = Mathf.NegativeInfinity, float maxD = Mathf.Infinity)
	{
		Vector2 origin = o - (d * (capsuleDirection == CapsuleDirection2D.Horizontal? s.x : s.y));

		return Physics2D.CapsuleCast(origin, s, capsuleDirection, a, d, l, mask, minD, maxD);
	}

	/// <summary>Cast a Capsule along a direction to get all the RaycastHit2Ds intersected.</summary>
	/// <param name="o">Capsule's Origin.</param>
	/// <param name="s">Capsule's Size.</param>
	/// <param name="capsuleDirection">Capsule's Direction.</param>
	/// <param name="a">Capsule's Angle.</param>
	/// <param name="d">Capsule's Direction.</param>
	/// <param name="l">Ray2D's Length.</param>
	/// <param name="mask">LayerMask to selectively ignore certain Colliders.</param>
	/// <param name="minD">Only include objects with a Z coordinate (depth) greater than or equal to this value [Mathf.NegativeInfinity by default].</param>
	/// <param name="maxD">Only include objects with a Z coordinate (depth) less than or equal to this value [Mathf.Infinity by default].</param>
	/// <returns>RaycastHit2Ds intersected on the cast.</returns>
	public static RaycastHit2D[] CapsuleCastAll(Vector2 o, Vector2 s, CapsuleDirection2D capsuleDirection, float a, Vector2 d, float l = Mathf.Infinity, int mask = Physics.DefaultRaycastLayers, float minD = Mathf.NegativeInfinity, float maxD = Mathf.Infinity)
	{
		Vector2 origin = o - (d * (capsuleDirection == CapsuleDirection2D.Horizontal? s.x : s.y));

		return Physics2D.CapsuleCastAll(origin, s, capsuleDirection, a, d, l, mask, minD, maxD);
	}
#endregion
}
}