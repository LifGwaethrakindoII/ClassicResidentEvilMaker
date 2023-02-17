using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class VCameraOcclusionHandler : VCameraDistanceCalculator
{
	[SerializeField] private LayerMask _hitMask; 		/// <summary>Hit's Layer Mask.</summary>
	[Space(5f)]
	[Header("Collision's Attributes:")]
	[SerializeField] private float _radius; 			/// <summary>Collision Sphere's Radius.</summary>
	[Space(5f)]
	[Header("Raycast's Attributes:")]
	[SerializeField]
	[Range(0.0f, 5.0f)]private float _pointRatio; 		/// <summary>Point's Ratio.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Gizmos' Attributes:")]
	[SerializeField] private Color rayOriginColor; 		/// <summary>Ray Origin's Color.</summary>
	[SerializeField] private float gizmosSphereRadius; 	/// <summary>Gizmos Sphere's Radius.</summary>
	[SerializeField] private float rayDuration; 		/// <summary>Ray's Duration.</summary>
	[SerializeField] private bool drawGizmos; 			/// <summary>Draw Gizmos?.</summary>
#endif

	/// <summary>Gets and Sets hitMask property.</summary>
	public LayerMask hitMask
	{
		get { return _hitMask; }
		set { _hitMask = value; }
	}

	/// <summary>Gets and Sets radius property.</summary>
	public float radius
	{
		get { return _radius; }
		set { _radius = value; }
	}

	/// <summary>Gets and Sets pointRatio property.</summary>
	public float pointRatio
	{
		get { return _pointRatio; }
		set { _pointRatio = value; }
	}

#region UnityMethods:
	/// <summary>Draws Gizmos.</summary>
	private void OnDrawGizmos()
	{
#if UNITY_EDITOR
		if(drawGizmos)
		{
			Gizmos.color = rayOriginColor;
			CameraViewportPlane nearPlane = vCamera.viewportHandler.NearPlane;

			Gizmos.DrawWireSphere(vCamera.transform.position, radius);

			foreach(Vector3 point in vCamera.viewportHandler)
			{
				Gizmos.DrawSphere(Vector3.LerpUnclamped(vCamera.viewportHandler.gridAttributes.center, point, pointRatio), gizmosSphereRadius);
			}
		}	
#endif
	}

	/// <summary>Resets component.</summary>
	private void Reset()
	{
		pointRatio = 1.0f;
#if UNITY_EDITOR
		rayOriginColor = Color.white;
		gizmosSphereRadius = 0.05f;
		rayDuration = 1.5f;
		drawGizmos = true;
#endif
	}
#endregion

	/// <summary>Is there a hit between target point and the viewport's collision points?.</summary>
	/// <param name="_point">Point to evaluate viewport's points against.</param>
	/// <returns>True if any of the viewport's points has an intersection, false if none does.</returns>
	public bool HitBetweenViewportPoints(Vector3 _point)
	{
		CameraViewportPlane nearPlane = vCamera.viewportHandler.NearPlane;
		Ray ray = default(Ray);
		Vector3 rayOrigin;
		Vector3 direction = Vector3.zero;
		float distance = 0.0f;

		foreach(Vector3 point in vCamera.viewportHandler)
		{
			rayOrigin = Vector3.LerpUnclamped(vCamera.viewportHandler.gridAttributes.center, point, pointRatio);
			direction = _point - rayOrigin;
			ray = new Ray(rayOrigin, direction);
			distance = direction.magnitude;

#if UNITY_EDITOR
			if(drawGizmos) Debug.DrawRay(rayOrigin, direction, rayOriginColor, rayDuration);
#endif

			if(Physics.Raycast(ray, distance, hitMask)) return true;
		}

		return false;
	}

	/// <summary>Is there a hit between target point and the viewport's collision points?.</summary>
	/// <param name="_point">Point to evaluate viewport's points against.</param>
	/// <param name="_hit">Hit information.</param>
	/// <returns>True if any of the viewport's points has an intersection, false if none does.</returns>
	public bool HitBetweenViewportPoints(Vector3 _point, out RaycastHit _hit)
	{
		CameraViewportPlane nearPlane = vCamera.viewportHandler.NearPlane;
		Ray ray = default(Ray);
		Vector3 rayOrigin;
		Vector3 direction = Vector3.zero;
		float distance = 0.0f;
		_hit = default(RaycastHit);

		foreach(Vector3 point in vCamera.viewportHandler)
		{
			rayOrigin = Vector3.LerpUnclamped(vCamera.viewportHandler.gridAttributes.center, point, pointRatio);
			direction = _point - rayOrigin;
			ray = new Ray(rayOrigin, direction);
			distance = direction.magnitude;

#if UNITY_EDITOR
			if(drawGizmos) Debug.DrawRay(rayOrigin, direction, rayOriginColor, rayDuration);
#endif

			if(Physics.Raycast(ray, out _hit, distance, hitMask)) return true;
		}

		return false;
	}

	/// <summary>Gets Calculated distance towards given target.</summary>
	/// <param name="_target">Target.</param>
	/// <returns>Calculated distance towards given target.</returns>
	public override float GetCalculatedDistance(Vector3 _target)
	{
		CameraViewportPlane nearPlane = vCamera.viewportHandler.NearPlane;
		Ray ray = default(Ray);
		RaycastHit hit;
		Vector3 direction = Vector3.zero;
		float distance = Mathf.Infinity;
		float hitDistance = 0.0f;

		foreach(Vector3 point in vCamera.viewportHandler)
		{
			direction = (Vector3.LerpUnclamped(vCamera.viewportHandler.gridAttributes.center, point, pointRatio) - _target);
			ray = new Ray(_target, direction);

			if(Physics.Raycast(ray, out hit, hitMask))
			{
				hitDistance = hit.distance;
				if(hitDistance < distance)
				{
					distance = hitDistance;
#if UNITY_EDITOR
					if(drawGizmos) Debug.DrawRay(ray.origin, direction, rayOriginColor, rayDuration);
#endif
				}
			}
		}

		if(Physics.SphereCast(vCamera.transform.position, radius, transform.forward, out hit, radius, hitMask)) distance -= radius;

		return distance;
	}
}
}