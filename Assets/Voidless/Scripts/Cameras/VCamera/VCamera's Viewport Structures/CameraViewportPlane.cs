using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*===========================================================================
**
** Class:  CameraViewportPlane
**
** Purpose: This structure contains coordinates for a camera's viewport in
** World-space. This struct is mostly used inside VCameraViewportHandler.
**
** NOTE: Not marked as Serializable since the data is internally set by
** external methods.
**
** Author: Lîf Gwaethrakindo
**
===========================================================================*/
namespace Voidless
{
public struct CameraViewportPlane
{
	public Vector3 centerPoint;
	public Vector3 topLeftPoint;
	public Vector3 topRightPoint;
	public Vector3 bottomLeftPoint;
	public Vector3 bottomRightPoint;

	/// <summary>CameraViewportPlane contructor.</summary>
	/// <param name="_centerPoint">Viewport's Center point.</param>
	/// <param name="_topLeftPoint">Viewport's Top-Left point.</param>
	/// <param name="_topRightPoint">Viewport's Top-Right point.</param>
	/// <param name="_bottomLeftPoint">Viewport's Bottom-Left point.</param>
	/// <param name="_bottomRightPoint">Viewport's Bottom-Right point.</param>
	public CameraViewportPlane(Vector3 _centerPoint, Vector3 _topLeftPoint, Vector3 _topRightPoint, Vector3 _bottomLeftPoint, Vector3 _bottomRightPoint)
	{
		centerPoint = _centerPoint;
		topLeftPoint = _topLeftPoint;
		topRightPoint = _topRightPoint;
		bottomLeftPoint = _bottomLeftPoint;
		bottomRightPoint = _bottomRightPoint;
	}

	/// <returns>Minimum Point [Bottom-Left].</returns>
	public Vector3 Min() { return bottomLeftPoint; }

	/// <summary>Maximum Point [Top-Right].</summary>
	public Vector3 Max() { return topRightPoint; }

	/// <returns>Viewport Plane's Size as a Vector2. Where X is Width and Y is Height.</returns>
	public Vector2 Size()
	{
		return new Vector2(
			Mathf.Abs(topRightPoint.x - topLeftPoint.x),
			Mathf.Abs(topRightPoint.y - bottomRightPoint.y)
		); 
	}

	/// <summary>Displaces ViewportPlane.</summary>
	/// <param name="_displacement">Displacement.</param>
	public void Displace(Vector3 _displacement)
	{
		centerPoint += _displacement;
		topLeftPoint += _displacement;
		topRightPoint += _displacement;
		bottomLeftPoint += _displacement;
		bottomRightPoint += _displacement;
	}

	/// <summary>Interpolates CameraViewportPlane A to CameraViewportPlane B at a normalized time t.</summary>
	/// <param name="a">CameraViewportPlane A.</param>
	/// <param name="b">CameraViewportPlane B.</param>
	/// <param name="t">Normalized Time t, internally clamped.</param>
	public static CameraViewportPlane Lerp(CameraViewportPlane a, CameraViewportPlane b, float t)
	{
		return new CameraViewportPlane(
			Vector3.Lerp(a.centerPoint, b.centerPoint, t),
			Vector3.Lerp(a.topLeftPoint, b.topLeftPoint, t),
			Vector3.Lerp(a.topRightPoint, b.topRightPoint, t),
			Vector3.Lerp(a.bottomLeftPoint, b.bottomLeftPoint, t),
			Vector3.Lerp(a.bottomRightPoint, b.bottomRightPoint, t)
		);
	}

	/// <summary>Draws Viewport Plane on Gizmos' Mode.</summary>
	public void DrawGizmos()
	{
#if UNITY_EDITOR
		Gizmos.DrawLine(bottomLeftPoint, bottomRightPoint);
		Gizmos.DrawLine(bottomLeftPoint, topLeftPoint);
		Gizmos.DrawLine(topLeftPoint, topRightPoint);
		Gizmos.DrawLine(bottomRightPoint, topRightPoint);
#endif
	}
}
}