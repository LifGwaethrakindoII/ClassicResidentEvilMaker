using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class VCameraViewportHandler : VCameraComponent, IEnumerable<Vector3>
{
	private const float PLANE_NEAR_EPSILON = 0.0001f; 		/// <summary>Additional Epsilon for the Plane's Z.</summary>

	[SerializeField] private CameraGrid _gridAttributes; 	/// <summary>Camera's Grid Attributes.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Gizmos' Attributes:")]
	[SerializeField] private Color centerColor; 			/// <summary>Center's Color.</summary>
	[SerializeField] private Color onRangeColor; 			/// <summary>On-Range's Color.</summary>
	[SerializeField] private Color offRangeColor; 			/// <summary>Off-Range's Color.</summary>
	[SerializeField] private bool drawGrid; 				/// <summary>Draw Camera's Grid?.</summary>
#endif
	private CameraViewportPlane nearPlane; 					/// <summary>Camera's Near Viewport Plane.</summary>

	/// <summary>Gets and Sets gridAttributes property.</summary>
	public CameraGrid gridAttributes
	{
		get { return _gridAttributes; }
		set { _gridAttributes = value; }
	}

	/// <summary>Gets NearPlane property.</summary>
	public CameraViewportPlane NearPlane { get { return nearPlane; } }

#region UnityMethods:
	/// <summary>Draws Gizmos.</summary>
	private void OnDrawGizmos()
	{
#if UNITY_EDITOR
		if(!Application.isPlaying)
		{
			UpdateViewportPlane(vCamera.camera.nearClipPlane, ref nearPlane);
			UpdateViewportGridPoints(ref nearPlane);
		}
		if(drawGrid) DrawCameraGrid();
#endif
	}

	/// <summary>Tick at the end of each frame.</summary>
	private void Update()
	{
		UpdateViewportPlane(vCamera.camera.nearClipPlane, ref nearPlane);
		UpdateViewportGridPoints(ref nearPlane);
	}

	/// <summary>Resets component.</summary>
	private void Reset()
	{
#if UNITY_EDITOR
		centerColor = Color.white;
		onRangeColor = Color.green;
		offRangeColor = Color.red;
#endif
		gridAttributes = new CameraGrid(0.5f);
	}
#endregion

	/// <summary>Updates viewport plane's reference.</summary>
	/// <param name="z">Z's value.</param>
	/// <param name="_plane">Viewport's plane reference to modify.</param>
	public void UpdateViewportPlane(float z, ref CameraViewportPlane _plane)
	{
		z += PLANE_NEAR_EPSILON;
		float y = (Mathf.Tan(vCamera.camera.fieldOfView * Mathf.Deg2Rad * 0.5f) * z);
		float x = y * vCamera.camera.aspect;

		_plane.centerPoint = transform.TransformPoint(new Vector3(0.0f, 0.0f, z));
		_plane.topLeftPoint = transform.TransformPoint(new Vector3(-x, y, z));
		_plane.topRightPoint = transform.TransformPoint(new Vector3(x, y, z));
		_plane.bottomLeftPoint = transform.TransformPoint(new Vector3(-x, -y, z));
		_plane.bottomRightPoint = transform.TransformPoint(new Vector3(x, -y, z));
	}

	/// <summary>Updates viewport plane's reference.</summary>
	/// <param name="_camera">Camera's Reference.</param>
	/// <param name="z">Z's value.</param>
	/// <param name="_plane">Viewport's plane reference to modify.</param>
	public static void UpdateViewportPlane(Camera _camera, float z, ref CameraViewportPlane _plane)
	{
		Transform transform = _camera.transform;
		z += PLANE_NEAR_EPSILON;
		float y = (Mathf.Tan(_camera.fieldOfView * Mathf.Deg2Rad * 0.5f) * z);
		float x = y * _camera.aspect;

		_plane.centerPoint = transform.TransformPoint(new Vector3(0.0f, 0.0f, z));
		_plane.topLeftPoint = transform.TransformPoint(new Vector3(-x, y, z));
		_plane.topRightPoint = transform.TransformPoint(new Vector3(x, y, z));
		_plane.bottomLeftPoint = transform.TransformPoint(new Vector3(-x, -y, z));
		_plane.bottomRightPoint = transform.TransformPoint(new Vector3(x, -y, z));
	}

	/// <summary>Updates Viewport Grid's Points.</summary>
	/// <param name="_plane">Viewport's plane reference to modify.</param>
	public void UpdateViewportGridPoints(ref CameraViewportPlane _plane)
	{
		_gridAttributes.SetCenter(new Vector3
		(
			Mathf.Lerp(_plane.topLeftPoint.x, _plane.topRightPoint.x, gridAttributes.centerX),
			Mathf.Lerp(_plane.bottomRightPoint.y, _plane.topRightPoint.y, gridAttributes.centerY),
			_plane.centerPoint.z
		));
	}

	/// <summary>Updates Boundaries' Size so it matches with Camera's Viewport.</summary>
	/// <param name="_camera">Camera's reference.</param>
	/// <param name="z">Z's Value [distance between boundaries container and the camera...].</param>
	/// <param name="boundariesContainer">BoundariesContainer's reference.</param>
	public static void UpdateBoundaries(Camera _camera, float z, Boundaries2DContainer boundariesContainer)
	{
		CameraViewportPlane plane = default(CameraViewportPlane);
		UpdateViewportPlane(_camera, z, ref plane);

		float x = (plane.bottomRightPoint - plane.bottomLeftPoint).magnitude;
		float y = (plane.bottomRightPoint - plane.topRightPoint).magnitude;
		Vector3 size = boundariesContainer.size;

		size.x = x;
		size.y = y;
		boundariesContainer.size = size;
	}

	/// <summary>Evaluates if given point is within focus area.</summary>
	/// <param name="_point">Point to evaluate.</param>
	/// <param name="_axes">Axes to evaluate.</param>
	/// <param name="_calculateViewportPoint">Calculate point's position relative to viewport?.</param>
	/// <returns>True if point within focus point, false otherwise.</returns>
	public bool PointWithinGridFocusArea(Vector3 _point, Axes2D _axes = Axes2D.All, bool _calculateViewportPoint = true)
	{
		if(_calculateViewportPoint) _point = vCamera.camera.WorldToViewportPoint(_point);

		return (!_axes.HasFlag(Axes2D.X)
				|| (_axes.HasFlag(Axes2D.X) && _point.x > gridAttributes.HorizontalMin()
				&& 	_point.x < gridAttributes.HorizontalMax())
				&& !_axes.HasFlag(Axes2D.Y)	
				|| (_axes.HasFlag(Axes2D.Y) && _point.y > gridAttributes.VerticalMin()
				&& 	_point.y < gridAttributes.VerticalMax()));
	}

	/// <summary>Evalues which point's axes are within focus area.</summary>
	/// <param name="_point">Point to evaluate.</param>
	/// <param name="_calculateViewportPoint">Calculate point's position relative to viewport?.</param>
	/// <returns>Point's Axes within focus area [as Axes2D].</returns>
	public Axes2D Axes2DWithinGridFocusArea(Vector3 _point, bool _calculateViewportPoint = true)
	{
		Axes2D axes = Axes2D.None;

		if(_calculateViewportPoint) _point = vCamera.camera.WorldToViewportPoint(_point);

		if(_point.x > gridAttributes.HorizontalMin()
			&& 	_point.x < gridAttributes.HorizontalMax()) axes |= Axes2D.X;

		if(_point.y > gridAttributes.VerticalMin()
			&& 	_point.y < gridAttributes.VerticalMax()) axes|= Axes2D.Y;

		return axes;
	}

	/// <summary>Evalues which point's axes are within focus area.</summary>
	/// <param name="_point">Point to evaluate.</param>
	/// <param name="_calculateViewportPoint">Calculate point's position relative to viewport?.</param>
	/// <returns>Point's Axes within focus area [as Axes3D].</returns>
	public Axes3D Axes3DWithinGridFocusArea(Vector3 _point, bool _calculateViewportPoint = true)
	{
		Axes3D axes = Axes3D.None;

		if(_calculateViewportPoint) _point = vCamera.camera.WorldToViewportPoint(_point);

		if(_point.x > gridAttributes.HorizontalMin()
			&& 	_point.x < gridAttributes.HorizontalMax()) axes |= Axes3D.X;

		if(_point.y > gridAttributes.VerticalMin()
			&& 	_point.y < gridAttributes.VerticalMax()) axes|= Axes3D.Y;

		return axes;
	}

	/// <returns>Returns an enumerator that iterates through the Near Plane's points.</returns>
	public IEnumerator<Vector3> GetEnumerator()
	{
		//yield return nearPlane.centerPoint;
		yield return gridAttributes.center;
		yield return nearPlane.bottomRightPoint;
		yield return nearPlane.bottomLeftPoint;
		yield return nearPlane.topLeftPoint;
		yield return nearPlane.topRightPoint;
	}

	/// <returns>Returns an enumerator that iterates through the Near Plane's points.</returns>
	IEnumerator IEnumerable.GetEnumerator()
	{
		yield return GetEnumerator();
	}

#if UNITY_EDITOR
	/// <summary>Draws Camera's Grid.</summary>
	private void DrawCameraGrid()
	{
		Gizmos.color = centerColor;
		/// Draw Center Lines:
		Gizmos.DrawLine(
			Vector3.Lerp(nearPlane.bottomRightPoint, nearPlane.topRightPoint, gridAttributes.centerY),
			Vector3.Lerp(nearPlane.bottomLeftPoint, nearPlane.topLeftPoint, gridAttributes.centerY));
		Gizmos.DrawLine(
			Vector3.Lerp(nearPlane.topLeftPoint, nearPlane.topRightPoint, gridAttributes.centerX),
			Vector3.Lerp(nearPlane.bottomLeftPoint, nearPlane.bottomRightPoint, gridAttributes.centerX));

		Axes2D axesInside = Axes2DWithinGridFocusArea(vCamera.targetRetriever.GetTargetPosition());

		Gizmos.color = axesInside.HasFlag(Axes2D.X) ? onRangeColor : offRangeColor;
		/// Horizontal Left Line:
		Gizmos.DrawLine(
			Vector3.Lerp(nearPlane.topLeftPoint, nearPlane.topRightPoint, gridAttributes.HorizontalMin()),
			Vector3.Lerp(nearPlane.bottomLeftPoint, nearPlane.bottomRightPoint, gridAttributes.HorizontalMin()));
		/// Horizontal Right Line:
		Gizmos.DrawLine(
			Vector3.Lerp(nearPlane.topLeftPoint, nearPlane.topRightPoint, gridAttributes.HorizontalMax()),
			Vector3.Lerp(nearPlane.bottomLeftPoint, nearPlane.bottomRightPoint, gridAttributes.HorizontalMax()));
		
		Gizmos.color = axesInside.HasFlag(Axes2D.Y) ? onRangeColor : offRangeColor;
		/// Vertical Up Line:
		Gizmos.DrawLine(
			Vector3.Lerp(nearPlane.bottomRightPoint, nearPlane.topRightPoint, gridAttributes.VerticalMax()),
			Vector3.Lerp(nearPlane.bottomLeftPoint, nearPlane.topLeftPoint, gridAttributes.VerticalMax()));
		/// Vertical Down Line:
		Gizmos.DrawLine(
			Vector3.Lerp(nearPlane.bottomRightPoint, nearPlane.topRightPoint, gridAttributes.VerticalMin()),
			Vector3.Lerp(nearPlane.bottomLeftPoint, nearPlane.topLeftPoint, gridAttributes.VerticalMin()));
	}
#endif
}
}