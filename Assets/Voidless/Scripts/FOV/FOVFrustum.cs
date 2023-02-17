using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public enum ProjectionType 												/// <summary>Projection Types.</summary>
{
	Perspective, 														/// <summary>Perspective Projection.</summary>
	Orthographic 														/// <summary>Orthographic Projection.</summary>
}

[RequireComponent(typeof(MeshCollider))]
public class FOVFrustum : MonoBehaviour
{
	[SerializeField] private ProjectionType _projectionType; 			/// <summary>Projection's Type.</summary>
	[SerializeField] private LayerMask _visibleMask; 					/// <summary>LayerMask of Colliders that are visible.</summary>
	[SerializeField] private FOVFrustumColliderData _FOVFrustumData; 	/// <summary>FOV's Frustum Data.</summary>
	[SerializeField] private List<Collider> _collidersOnFOV; 			/// <summary>Set of Colliders on FOV's Area.</summary>
	private MeshCollider _meshCollider; 								/// <summary>MeshCollider's Component.</summary>
	private Vector3 _perspective; 										/// <summary>Projection's Vector.</summary>
	[SerializeField] private Collider TESTCollider; 	/// <summary>TEST Collider.</summary>

	/// <summary>Gets and Sets projectionType property.</summary>
	public ProjectionType projectionType
	{
		get { return _projectionType; }
		set { _projectionType = value; }
	}

	/// <summary>Gets and Sets visibleMask property.</summary>
	public LayerMask visibleMask
	{
		get { return _visibleMask; }
		set { _visibleMask = value; }
	}

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

	/// <summary>Gets and Sets collidersOnFOV property.</summary>
	public List<Collider> collidersOnFOV
	{
		get { return _collidersOnFOV; }
		set { _collidersOnFOV = value; }
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

	/// <summary>Gets and Sets perspective property.</summary>
	public Vector3 perspective
	{
		get { return _perspective; }
		set { _perspective = value; }
	}

	private void OnDrawGizmos()
	{
		if(TESTCollider != null)
		{
			float radius = 0.05f;
			Gizmos.color = Color.cyan;

			Gizmos.DrawWireCube(TESTCollider.transform.position, TESTCollider.bounds.size);
			TESTCollider.GetBoundsCornerVertices().ForEach((Vector3 point) =>
			{
				Gizmos.color = Color.cyan;

				Vector3 c = transform.TransformPoint(new Vector3(0.0f, 0.0f, FOVFrustumData.FOVData.nearPlane));
				Vector3 direction = point - c;
				Vector3 localPoint = transform.InverseTransformPoint(direction);
				float d = Vector3.Distance(c, localPoint);
				//float scale = 1.0f - (d / FOVFrustumData.FOVData.planeDelta);
				Vector3 scaleVector = new Vector3(1.0f/d, 1.0f/d, 1.0f/d);

				Gizmos.DrawSphere(c, radius);
				Gizmos.DrawSphere(point, radius);
				Vector3 perspectivePoint = Vector3.Scale(point, scaleVector);

				Gizmos.color = Color.red;
				Gizmos.DrawSphere(perspectivePoint, radius);
				Gizmos.color = Color.yellow;
				Gizmos.DrawSphere(localPoint, radius);
			});
		}
		if(collidersOnFOV != null)
		foreach(Collider collider in collidersOnFOV)
		{

		}
		meshCollider.sharedMesh.RecalculateNormals();
		foreach(Vector3 normal in meshCollider.sharedMesh.normals)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawRay(transform.position, normal * 2.5f);
		}
	}

	private void Awake()
	{
		collidersOnFOV = new List<Collider>();
	}

	/// <summary>Updates FOV's Frustum.</summary>
	public void UpdateFOVFrustum()
	{
		meshCollider.sharedMesh = FOVFrustumData.mesh;
		meshCollider.isTrigger = false;
		perspective = new Vector3(FOVFrustumData.FOVData.inverseZ, FOVFrustumData.FOVData.inverseZ, 1.0f);
	}

	/// <summary>Event triggered when this Collider enters another Collider trigger.</summary>
	/// <param name="col">The other Collider involved in this Event.</param>
	private void OnTriggerEnter(Collider col)
	{
		GameObject obj = col.gameObject;
		if(obj.IsOnLayerMask(visibleMask)) collidersOnFOV.Add(col);
	}

	/// <summary>Event triggered when this Collider exits another Collider trigger.</summary>
	/// <param name="col">The other Collider involved in this Event.</param>
	private void OnTriggerExit(Collider col)
	{
		GameObject obj = col.gameObject;
		if(obj.IsOnLayerMask(visibleMask)) collidersOnFOV.Remove(col);
	}
}
}