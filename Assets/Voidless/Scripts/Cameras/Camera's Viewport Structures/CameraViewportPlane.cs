using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public struct CameraViewportPlane
{
	public Vector3 centerPoint; 		/// <summary>Viewport's Center point.</summary>
	public Vector3 topLeftPoint;		/// <summary>Viewport's Top-Left point.</summary>
	public Vector3 topRightPoint;		/// <summary>Viewport's Top-Right point.</summary>
	public Vector3 bottomLeftPoint;		/// <summary>Viewport's Bottom-Left point.</summary>
	public Vector3 bottomRightPoint;	/// <summary>Viewport's Bottom-Right point.</summary>

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

	/// <returns>Viewport Plane's Size.</returns>
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