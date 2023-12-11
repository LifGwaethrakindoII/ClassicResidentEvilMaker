using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[CreateAssetMenu(menuName = PATH_OCCLUSIONEVALUATOR + " Default")]
public class VCameraOcclusionEvaluatorModule : VCameraModule
{
	public const string PATH_OCCLUSIONEVALUATOR = PATH_ROOT + " Occlusion Evaluators / ";

	[SerializeField] private LayerMask _hitMask;
	[Space(5f)]
	[Header("Collision's Attributes:")]
	[SerializeField] private float _radius;
	[Space(5f)]
	[Header("Raycast's Attributes:")]
	[SerializeField]
	[Range(0.0f, 5.0f)]private float _pointRatio;
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Gizmos' Attributes:")]
	[SerializeField] private Color rayOriginColor;
	[SerializeField] private float gizmosSphereRadius;
	[SerializeField] private float rayDuration;
	[SerializeField] private bool drawGizmos;
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

	/// <summary>Draws Gizmos taking the VCamera's argument into account.</summary>
	/// <param name="_camera">VCamera's requesting to Draw Gizmos.</param>
	public override void DrawGizmos(VCamera _camera)
	{
#if UNITY_EDITOR
		if(drawGizmos)
		{
			Gizmos.color = rayOriginColor;
			CameraViewportPlane nearPlane = _camera.viewportHandler.NearPlane;

			Gizmos.DrawWireSphere(_camera.transform.position, radius);

			foreach(Vector3 point in _camera.viewportHandler)
			{
				Gizmos.DrawSphere(Vector3.LerpUnclamped(_camera.viewportHandler.gridAttributes.center, point, pointRatio), gizmosSphereRadius);
			}
		}	
#endif
	}

	/// <summary>Resets Module.</summary>
	public override void Reset()
	{
		pointRatio = 1.0f;
#if UNITY_EDITOR
		rayOriginColor = Color.white;
		gizmosSphereRadius = 0.05f;
		rayDuration = 1.5f;
		drawGizmos = true;
#endif
	}

	/// <summary>Is there a hit between target point and the viewport's collision points?.</summary>
	/// <param name="_point">Point to evaluate viewport's points against.</param>
	/// <returns>True if any of the viewport's points has an intersection, false if none does.</returns>
	public virtual bool HitBetweenViewportPoints(VCamera _camera, Vector3 _point)
	{
		CameraViewportPlane nearPlane = _camera.viewportHandler.NearPlane;
		Ray ray = default(Ray);
		Vector3 rayOrigin;
		Vector3 direction = Vector3.zero;
		float distance = 0.0f;

		foreach(Vector3 point in _camera.viewportHandler)
		{
			rayOrigin = Vector3.LerpUnclamped(_camera.viewportHandler.gridAttributes.center, point, pointRatio);
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
	public virtual bool HitBetweenViewportPoints(VCamera _camera, Vector3 _point, out RaycastHit _hit)
	{
		CameraViewportPlane nearPlane = _camera.viewportHandler.NearPlane;
		Ray ray = default(Ray);
		Vector3 rayOrigin;
		Vector3 direction = Vector3.zero;
		float distance = 0.0f;
		_hit = default(RaycastHit);

		foreach(Vector3 point in _camera.viewportHandler)
		{
			rayOrigin = Vector3.LerpUnclamped(_camera.viewportHandler.gridAttributes.center, point, pointRatio);
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
	public virtual float GetCalculatedDistance(VCamera _camera, Vector3 _target)
	{
		CameraViewportPlane nearPlane = _camera.viewportHandler.NearPlane;
		Ray ray = default(Ray);
		RaycastHit hit;
		Vector3 direction = Vector3.zero;
		float distance = Mathf.Infinity;
		float hitDistance = 0.0f;

		foreach(Vector3 point in _camera.viewportHandler)
		{
			direction = (Vector3.LerpUnclamped(_camera.viewportHandler.gridAttributes.center, point, pointRatio) - _target);
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

		if(Physics.SphereCast(_camera.transform.position, radius, _camera.transform.forward, out hit, radius, hitMask)) distance -= radius;

		return distance;
	}
}
}