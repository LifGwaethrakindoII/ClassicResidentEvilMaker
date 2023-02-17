using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
// Camera's Frustum -Maths: https://docs.unity3d.com/es/2018.4/Manual/FrustumSizeAtDistance.html
[RequireComponent(typeof(Boundaries2DContainer))]
public class FitTargetBoundsDistanceCalculator : VCameraDistanceCalculator
{
	private Boundaries2DContainer _boundariesContainer; 	/// <summary>Boundaries2DContainer's Component.</summary>

	/// <summary>Gets boundariesContainer Component.</summary>
	public Boundaries2DContainer boundariesContainer
	{ 
		get
		{
			if(_boundariesContainer == null) _boundariesContainer = GetComponent<Boundaries2DContainer>();
			return _boundariesContainer;
		}
	}

	/// <summary>Gets Calculated distance towards given target.</summary>
	/// <param name="_target">Target.</param>
	/// <returns>Calculated distance towards given target.</returns>
	public override float GetCalculatedDistance(Vector3 _target)
	{
		Camera camera = vCamera.camera;
		CameraViewportPlane plane = vCamera.GetBoundariesPlane();
		Vector2 minLimits = boundariesContainer.min;
		Vector2 maxLimits = boundariesContainer.max;
		Vector2 viewportSize = plane.Size();
		Bounds bounds = vCamera.targetRetriever.GetTargetBounds();
		float f = camera.fieldOfView;
		float r = camera.aspect;
		float x = 0.0f;
		float y = 0.0f;
		float z = 0.0f;

		/// Evaluate if Target Bounds are within the Camera's Viewport:
		bool insideViewport = ((bounds.min.x >= plane.Min().x && bounds.max.x <= plane.Max().x)
		&& (bounds.min.y >= plane.Min().y && bounds.max.y <= plane.Max().y));

		if(!insideViewport)
		{
			float longestPointX = Mathf.Max(Mathf.Abs(bounds.center.x - minLimits.x), Mathf.Abs(bounds.center.x - maxLimits.x));
			float longestPointY = Mathf.Max(Mathf.Abs(bounds.center.y - minLimits.y), Mathf.Abs(bounds.center.y - maxLimits.y));

			if(longestPointX > longestPointY)
			{
				x = longestPointX;
				y = x / r;
			
			} else if(longestPointY > longestPointX)
			{
				y = longestPointY;
				x = y * r;
			}
		}
		else
		{
			if(bounds.extents.x > bounds.extents.y)
			{
				x = bounds.extents.x;
				y = x / r;
			
			} else if(bounds.extents.y > bounds.extents.x)
			{
				y = bounds.extents.y;
				x = y * r;
			}
		}

		/* Solve for Z:
			y = tan([fov * Deg2Rad] / 2) * z;
			x = y * r;
			y = x / r;
			z = y / tan([fov * Deg2Rad] / 2);
		*/
		z = y / Mathf.Tan(f * Mathf.Deg2Rad * 0.5f);

		/// Update the plane to it gets projected with the new Z's length:
		VCameraViewportHandler.UpdateViewportPlane(camera, z, ref plane);

		/// Evaluate if (updated) Viewport Plane is inside the Limits:
		bool insideLimits = ((plane.Min().x >= minLimits.x && plane.Max().x <= maxLimits.x)
		&& (plane.Min().y >= minLimits.y && plane.Max().y <= maxLimits.y));

		if(!insideLimits)
		{
			/*
				1.- Get the axes (X or Y) where the limits are past.
				2.- if both axes are past the limits, compare which one has a bigger difference between its respective limit.
				3.- If the X axis' difference is bigger than Y's, calculate Y from X. Otherwise don't do anything extra.
				4.- Solve for Z using z = y / tan(f/2)
			*/

			float? minX = null;
			float? minY = null;
			float? maxX = null;
			float? maxY = null;
			float? dX = null;
			float? dY = null;

			if(plane.Min().x < minLimits.x)	minX = Mathf.Abs(minLimits.x - plane.Min().x);
			if(plane.Min().y < minLimits.y)	minY = Mathf.Abs(minLimits.y - plane.Min().y);
			if(plane.Max().x > maxLimits.x)	maxX = Mathf.Abs(maxLimits.x - plane.Max().x);
			if(plane.Max().x > maxLimits.x)	maxY = Mathf.Abs(maxLimits.y - plane.Max().y);

			if(minX.HasValue || maxX.HasValue) dX = Mathf.Max(minX.GetValueOrDefault(), maxX.GetValueOrDefault()); 
			if(minX.HasValue || maxX.HasValue) dY = Mathf.Max(minY.GetValueOrDefault(), maxY.GetValueOrDefault());

			if(dX.HasValue)
			{
				if(dX == minX) x = Mathf.Abs(minLimits.x - plane.centerPoint.x);
				else if(dX == maxX) x = Mathf.Abs(maxLimits.x - plane.centerPoint.x);
			}
			if(dY.HasValue && dY > dX)
			{
				if(dY == minY) y = Mathf.Abs(minLimits.y - plane.centerPoint.y);
				else if(dY == maxY) y = Mathf.Abs(maxLimits.y - plane.centerPoint.y);	
			}
			else y = x / r;

			z = y / Mathf.Tan(f * Mathf.Deg2Rad * 0.5f);
		}

		return z;
	}
}
}