using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[RequireComponent(typeof(MeshCollider))]
public class FOVFrustumSightSensor : SightSensor
{
	[Space(5f)]
	[Header("FOV's Attributes:")]
	[SerializeField] private FOVFrustumColliderData _FOVFrustumData; 	/// <summary>FOV's Frustum Data.</summary>
	private MeshCollider _meshCollider; 								/// <summary>MeshCollider's Component.</summary>

	/// <summary>Gets and Sets FOVFrustumData property.</summary>
	public FOVFrustumColliderData FOVFrustumData
	{
		get { return _FOVFrustumData; }
		set
		{
			_FOVFrustumData = value;
			UpdateFOVFrustum();
		}
	}

	/// <summary>Gets and Sets meshCollider Component.</summary>
	public MeshCollider meshCollider
	{ 
		get
		{
			if(_meshCollider == null) _meshCollider = GetComponent<MeshCollider>();
			return _meshCollider;
		}
	}

	/// <summary>Gets Far Plane property.</summary>
	public override float farPlane { get { return FOVFrustumData.FOVData.farPlane; } }

	/// <summary>Updates FOV's Frustum.</summary>
	public void UpdateFOVFrustum()
	{
		meshCollider.sharedMesh = FOVFrustumData.mesh;
		meshCollider.convex = true;
		meshCollider.isTrigger = true;
	}
}
}