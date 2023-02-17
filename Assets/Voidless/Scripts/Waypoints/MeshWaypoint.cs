using UnityEngine;

namespace Voidless
{
public class MeshWaypoint : Waypoint
{
#if UNITY_EDITOR
	[SerializeField] private Mesh _mesh; 			/// <summary>Waypoint's Mesh.</summary>
#endif
	private MeshCollider _meshCollider; 			/// <summary>MeshCollider's Component.</summary>

#if UNITY_EDITOR
	/// <summary>Gets and Sets mesh property.</summary>
	public Mesh mesh
	{
		get { return _mesh; }
		set { _mesh = value; }
	}
#endif

	/// <summary>Gets and Sets meshCollider Component.</summary>
	public MeshCollider meshCollider
	{ 
		get
		{
			if(_meshCollider == null) _meshCollider = GetComponent<MeshCollider>();
			return _meshCollider;
		}
	}

#if UNITY_EDITOR
	/// <summary>Draws Waypoint's Gizmos.</summary>
	protected override void DrawWaypoint()
	{
		Gizmos.color = color;
		switch(drawType)
		{
			case DrawTypes.Wired:
			Gizmos.DrawWireMesh(mesh, transform.position, transform.rotation, transform.localScale);
			break;

			case DrawTypes.Solid:
			Gizmos.DrawMesh(mesh, transform.position, transform.rotation, transform.localScale);
			break;
		}
	}
#endif

	/// <summary>Resizes Waypoint's collider.</summary>
	public override void ResizeCollider()
	{

	}
}
}