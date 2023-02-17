using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public enum LoopType
{
	Update,
	LateUpdate,
	FixedUpdate
}

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(VCameraViewportHandler))]
[RequireComponent(typeof(OrientationNormalAdjuster))]
public abstract class VCamera : MonoBehaviour
{
	[SerializeField] private LoopType _updateCameraAt; 		/// <summary>Loop to update the camera at.</summary>
	[SerializeField] protected Axes3D _ignoreFocusAxes; 	/// <summary>Center's Focus Axes to ignore when following.</summary>
	private Dictionary<int, VCameraDelimiter> _delimiters; 	/// <summary>Camera's Delimiters.</summary>
	private VCameraTargetRetriever _targetRetriever; 		/// <summary>VCameraTargetRetriever's Component.</summary>
	private VCameraViewportHandler _viewportHandler; 		/// <summary>VCameraViewportHandler's Component.</summary>
	private VCameraDistanceAdjuster _distanceAdjuster; 		/// <summary>VCameraDistanceAdjuster's Component.</summary>
	private OrientationNormalAdjuster _normalAdjuster; 		/// <summary>OrientationNormalAdjuster's Component.</summary>
	private Camera _camera; 								/// <summary>Camera's Component.</summary>
	private Vector3 _centerFocusDirection; 					/// <summary>Center Focus' Direction.</summary>
	protected CameraViewportPlane boundariesPlane; 			/// <summary>Camera Boundaries' Plane.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[SerializeField] protected Color gizmosColor; 			/// <summary>Gizmos' Color.</summary>
#endif

#region Getters/Setters:
	/// <summary>Gets and Sets updateCameraAt property.</summary>
	public LoopType updateCameraAt
	{
		get { return _updateCameraAt; }
		protected set { _updateCameraAt = value; }
	}

	/// <summary>Gets and Sets ignoreFocusAxes property.</summary>
	public Axes3D ignoreFocusAxes
	{
		get { return _ignoreFocusAxes; }
		set { _ignoreFocusAxes = value; }
	}

	/// <summary>Gets and Sets delimiters property.</summary>
	public Dictionary<int, VCameraDelimiter> delimiters
	{
		get { return _delimiters; }
		protected set { _delimiters = value; }
	}
	
	/// <summary>Gets and Sets centerFocusDirection property.</summary>
	public Vector3 centerFocusDirection
	{
		get { return _centerFocusDirection; }
		set { _centerFocusDirection = value; }
	}

	/// <summary>Gets targetRetriever Component.</summary>
	public VCameraTargetRetriever targetRetriever
	{ 
		get
		{
			if(_targetRetriever == null) _targetRetriever = GetComponent<VCameraTargetRetriever>();
			return _targetRetriever;
		}
	}

	/// <summary>Gets and Sets camera Component.</summary>
	public new Camera camera
	{ 
		get
		{
			if(_camera == null) _camera = GetComponent<Camera>();
			return _camera;
		}
	}

	/// <summary>Gets and Sets viewportHandler Component.</summary>
	public VCameraViewportHandler viewportHandler
	{ 
		get
		{
			if(_viewportHandler == null) _viewportHandler = GetComponent<VCameraViewportHandler>();
			return _viewportHandler;
		}
	}

	/// <summary>Gets distanceAdjuster Component.</summary>
	public VCameraDistanceAdjuster distanceAdjuster
	{ 
		get
		{
			if(_distanceAdjuster == null) _distanceAdjuster = GetComponent<VCameraDistanceAdjuster>();
			return _distanceAdjuster;
		}
	}

	/// <summary>Gets and Sets normalAdjuster Component.</summary>
	public OrientationNormalAdjuster normalAdjuster
	{ 
		get
		{
			if(_normalAdjuster == null) _normalAdjuster = GetComponent<OrientationNormalAdjuster>();
			return _normalAdjuster;
		}
	}

	/// <summary>Implicit VCamera to Camera operator.</summary>
	public static implicit operator Camera(VCamera _baseCamera) { return _baseCamera.camera; }
#endregion

	/// <summary>Draws Gizmos on Editor mode.</summary>
	protected virtual void OnDrawGizmos()
	{
#if UNITY_EDITOR
		Gizmos.color = gizmosColor;

		Gizmos.DrawRay(transform.position, centerFocusDirection);
		Gizmos.DrawRay(transform.position, centerFocusDirection);

		if(!Application.isPlaying) return;

		boundariesPlane.DrawGizmos();
#endif
	}

	/// <summary>Resets Component.</summary>
	protected virtual void Reset()
	{
		ignoreFocusAxes = Axes3D.None;
#if UNITY_EDITOR
		gizmosColor = Color.white;
#endif
	}
	
	/// <summary>VCamera's tick at the end of each frame.</summary>
	private void LateUpdate()
	{
		switch(updateCameraAt)
		{
			case LoopType.Update:
			case LoopType.LateUpdate:
			OnUpdate();
			CameraUpdate();
			break;
		}
	}

	/// <summary>Updates VCamera's instance at each Physics Thread's frame.</summary>
	private void FixedUpdate()
	{
		switch(updateCameraAt)
		{
			case LoopType.FixedUpdate:
			OnUpdate();
			CameraFixedUpdate();
			break;
		}
	}

	/// <summary>Callback called on either LateUpdate or FixedUpdate.</summary>
	protected virtual void OnUpdate()
	{
		distanceAdjuster.UpdateDistance();
		UpdateCenterFocusDirection();
		VCameraViewportHandler.UpdateViewportPlane(camera, distanceAdjuster.distance, ref boundariesPlane);
	}

	/// <summary>Updates Camera.</summary>
	protected abstract void CameraUpdate();

	/// <summary>Updates Camera on Physics' Thread.</summary>
	protected abstract void CameraFixedUpdate();

	/// <summary>Calculates an adjusted direction given direction's axes.</summary>
	/// <param name="_x">Axis X.</param>
	/// <param name="_y">Axis Y.</param>
	/// <returns>Adjusted Direction.</returns>
	public virtual Vector3 GetAdjustedDirection(float _x, float _y)
	{
		return (transform.right * _x) + (normalAdjuster.forward * _y);
	}

	/// <summery>Sets Direction between Focus Center and Target's Position.</summery>
	protected virtual void UpdateCenterFocusDirection()
	{
		Ray viewportRay = camera.ViewportPointToRay(new Vector3(viewportHandler.gridAttributes.centerX, viewportHandler.gridAttributes.centerY, 0.0f));
		Ray centerRay = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
		Vector3 scaledViewportDirection = viewportRay.direction * distanceAdjuster.distance;
		Vector3 centerProjection = VVector3.VectorProjection(scaledViewportDirection, centerRay.direction);
		centerFocusDirection = (centerProjection - (scaledViewportDirection));
		
		if(camera.orthographic)
		{
			Vector3 shift = transform.InverseTransformDirection(viewportRay.direction);
			shift.z = 0.0f;
			centerFocusDirection = transform.TransformVector(shift);
		}

#if UNITY_EDITOR
		Debug.DrawRay(viewportRay.origin, scaledViewportDirection, gizmosColor);
		Debug.DrawRay(centerRay.origin, centerProjection, gizmosColor);
#endif
	}

	/// <returns>Boundaries' Viewport Plane.</returns>
	public CameraViewportPlane GetBoundariesPlane()
	{
		return boundariesPlane;
	}

	/// <returns>Axes where the target is currently within.</returns>
	/// <param name="_point">Point to evaluate.</param>
	public Axes3D GetAxesWhereTargetIsWithin(Vector3 _point)
	{
		return viewportHandler.Axes3DWithinGridFocusArea(_point);
	}

	/// <returns>Focus' Direction, ignoring the flagged axes.</returns>
	public Vector3 GetCenterFocusDirection()
	{
		return transform.IgnoreVectorAxes(centerFocusDirection, ignoreFocusAxes, true);
	}

	/// <returns>Gets Delta Time according to the Loop Type.</returns>
	public float GetDeltaTime()
	{
		switch(updateCameraAt)
		{
			case LoopType.Update:
			case LoopType.LateUpdate: 	return Time.deltaTime;
			case LoopType.FixedUpdate: 	return Time.fixedDeltaTime;
			default: 					return Time.smoothDeltaTime;
		}
	}

	/// <summary>Adds VCameraDelimiter's reference.</summary>
	/// <param name="_delimiter">Calculator to add to dictionary.</param>
	public void AddDelimiter(VCameraDelimiter _delimiter)
	{
		if(_delimiter == null) return;

		int ID = _delimiter.GetInstanceID();

		if(delimiters == null) delimiters = new Dictionary<int, VCameraDelimiter>();
		if(!delimiters.ContainsKey(ID)) delimiters.Add(ID, _delimiter);
	}

	/// <summary>Removes VCameraDelimiter's reference.</summary>
	/// <param name="_delimiter">Calculator to add to dictionary.</param>
	public void RemoveDelimiter(VCameraDelimiter _delimiter)
	{
		if(_delimiter == null) return;

		int ID = _delimiter.GetInstanceID();

		if(delimiters == null) return;
		
		if(delimiters.ContainsKey(ID)) delimiters.Remove(ID);
	}
}
}