using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[RequireComponent(typeof(Boundaries2DContainer))]
public class Boundaries2DDelimiter : VCameraDelimiter
{
#if UNITY_EDITOR
	[SerializeField] private Color gizmosColor; 				/// <summary>Gizmos' Color.</summary>
#endif
	private Boundaries2DContainer _boundariesContainer; 	/// <summary>Boundaries2DContainer's Component.</summary>
	private CameraViewportPlane projectedPlane; 				/// <summary>Camera's Projected Plane.</summary>

	/// <summary>Gets boundariesContainer Component.</summary>
	public Boundaries2DContainer boundariesContainer
	{ 
		get
		{
			if(_boundariesContainer == null) _boundariesContainer = GetComponent<Boundaries2DContainer>();
			return _boundariesContainer;
		}
	}

#if UNITY_EDITOR
	/// <summary>Resets Boundaries2DDelimiter's instance to its default values.</summary>
	private void Reset()
	{
		gizmosColor = Color.white;
	}

	/// <summary>Draws Gizmos on Editor mode.</summary>
	private void OnDrawGizmos()
	{
		if(!Application.isPlaying) return;
		
		Gizmos.color = gizmosColor;
		projectedPlane.DrawGizmos();
	}
#endif

	/// <summary>Gives target delimited by this component's implementation.</summary>
	/// <param name="_target">Target to delimit.</param>
	/// <returns>Target delimited by this component's implementation.</returns>
	public override Vector3 Delimited(Vector3 _target)
	{
		projectedPlane = vCamera.GetBoundariesPlane();

		Vector3 direction = (_target - transform.position).WithZ(0.0f);
		Vector3 offset = Vector3.zero;
		Vector3 minLimits = boundariesContainer.min;
		Vector3 maxLimits = boundariesContainer.max;
		Vector3 oMin = projectedPlane.Min();
		Vector3 oMax = projectedPlane.Max();

		projectedPlane.Displace(direction);

		float pMinX = projectedPlane.Min().x;
		float pMaxX = projectedPlane.Max().x;
		float pMinY = projectedPlane.Min().y;
		float pMaxY = projectedPlane.Max().y;
		float minX = minLimits.x;
		float maxX = maxLimits.x;
		float minY = minLimits.y;
		float maxY = maxLimits.y;

		/// X's Evaluation:
		if(oMin.x < minX && oMax.x > minX)
		{
			_target.x = Mathf.Lerp(minX, maxX, 0.5f);

		} else if(pMaxX > maxX)
		{
			offset.x = maxX - pMaxX;
		
		} else if(pMinX < minX)
		{
			offset.x = minX - pMinX;
		}

		/// Y's Evaluation:
		if(oMin.y < minY && oMax.y > maxY)
		{
			_target.y = Mathf.Lerp(minY, maxY, 0.5f);

		} else if(pMaxY > maxY)
		{
			offset.y = maxY - pMaxY;
		
		} else if(pMinY < minY)
		{
			offset.y = minY - pMinY;
		}

		return _target + offset;
	}
}
}