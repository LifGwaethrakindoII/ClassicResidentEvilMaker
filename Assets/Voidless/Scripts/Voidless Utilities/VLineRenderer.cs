using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VLineRenderer
{
	public static void DrawCone(this LineRenderer _lineRenderer, float a, float r)
	{
		int points = 3;
	    float radian = a * Mathf.Deg2Rad;
	    float radianFract = radian / (float)points;
	    Vector3 center = _lineRenderer.transform.position;    //Where unit stands
	     
	    _lineRenderer.SetVertexCount(points + 3);    //Add start/finish points
	    Vector3 vect = center;    //Start point is center point.
	    _lineRenderer.SetPosition(0, vect);

	    for(int x = 0; x < points + 1; x++)
	    {
	        vect = center;
	        vect.x += (float)(Math.Cos(radianFract * x) * r);
	        vect.z += (float)(Math.Sin(radianFract * x) * r);
	        _lineRenderer.SetPosition(x + 1, vect);    //Skip first/last points.
	    }
	     
	    vect = center;
	    _lineRenderer.SetPosition(points + 2, vect);    //Last point is center point.											
	}

	/// <summary>Sets Start and End Width of LineRenderer equal to the same value.</summary>
	/// <param name="_lineRenderer">LineRenderer's reference.</param>
	/// <param name="_width">Width for Start and End points.</param>
	public static void SetWidth(this LineRenderer _lineRenderer, float _width)
	{
		_lineRenderer.startWidth = _width;
		_lineRenderer.endWidth = _width;
	}

	/// <summary>Gets interpolation of both Start and End points' width.</summary>
	/// <param name="_lineRenderer">LineRenderer's reference.</param>
	/// <param name="t">Normalized Time t.</param>
	/// <returns>Interpolation of  Start and Ends' width.</returns>
	public static float GetWidth(this LineRenderer _lineRenderer, float t = 1.0f)
	{
		return  Mathf.Lerp(_lineRenderer.startWidth, _lineRenderer.endWidth, t);
	}
}
}