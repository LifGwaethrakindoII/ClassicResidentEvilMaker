using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public enum InterpolateOrigin
{
	PathWaypointGenerator,
	FirstWaypoint
}

[RequireComponent(typeof(VCameraOrbitedDisplacementFollow))]
[RequireComponent(typeof(VCameraRotationFollow))]
public class ThirdPersonCamera : VCamera
{
	private VCameraOrbitedDisplacementFollow _orbitFollow; 	/// <summary>VCameraOrbitedDisplacementFollow's Component.</summary>
	private VCameraRotationFollow _rotationFollow; 			/// <summary>VCameraRotationFollow's Component.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets orbitFollow Component.</summary>
	public VCameraOrbitedDisplacementFollow orbitFollow
	{ 
		get
		{
			if(_orbitFollow == null) _orbitFollow = GetComponent<VCameraOrbitedDisplacementFollow>();
			return _orbitFollow;
		}
	}

	/// <summary>Gets and Sets rotationFollow Component.</summary>
	public VCameraRotationFollow rotationFollow
	{ 
		get
		{
			if(_rotationFollow == null) _rotationFollow = GetComponent<VCameraRotationFollow>();
			return _rotationFollow;
		}
	}
#endregion

	/// <summary>Updates Camera.</summary>
	protected override void CameraUpdate()
	{
		/// \TODO Update this
		/*orbitFollow.OnLateUpdate();
		rotationFollow.OnLateUpdate();*/
	}

	/// <summary>Updates Camera on Physics' Thread.</summary>
	protected override void CameraFixedUpdate()
	{
		CameraUpdate();
	}
}
}