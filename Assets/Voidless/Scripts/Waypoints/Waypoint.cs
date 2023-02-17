using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public enum DrawTypes 									/// <summary>Waypoint's Draw Types.</summary>
{
	Wired, 												/// <summary>Wired Draw Type.</summary>
	Solid 												/// <summary>Solid Draw Type.</summary>
}

public abstract class Waypoint : MonoBehaviour
{
	[SerializeField] private Vector3 _dimensions; 		/// <summary>Waypoint's Dimension.</summary>
#if UNITY_EDITOR
	[SerializeField] private bool _drawWhenSelected; 	/// <summary>Just draw this Gizmo when selected? Otherwise, it will draw in any case.</summary>
	[SerializeField] private Color _color; 				/// <summary>Waypoint's color.</summary>
	[SerializeField] private float _normalProjection; 	/// <summary>Waypoint's normal projection length.</summary>
	[SerializeField] private DrawTypes _drawType; 		/// <summary>Waypoint's Draw Type [whether solid or wired].</summary>
	private Waypoint _waypoint; 						/// <summary>Waypoint's Component.</summary>
	private Matrix4x4 _priorMatrix; 					/// <summary>Prior Matrix's on calculations.</summary>
#endif

#region Getters/Setters:
	/// <summary>Gets and Sets dimensions property.</summary>
	public Vector3 dimensions
	{
		get { return _dimensions; }
		set
		{
			_dimensions = value;
			ResizeCollider();
		}
	}

	/// <summary>Gets and Sets position property.</summary>
	public Vector3 position
	{
		get { return transform.position; }
		set { transform.position = value; }
	}

	/// <summary>Gets and Sets eulerAngles property.</summary>
	public Vector3 eulerAngles
	{
		get { return transform.eulerAngles; }
		set { transform.eulerAngles = value; }
	}

	/// <summary>Gets and Sets rotation property.</summary>
	public Quaternion rotation
	{
		get { return transform.rotation; }
		set { transform.rotation = value; }
	}
#endregion

#if UNITY_EDITOR
	/// <summary>Gets and Sets drawWhenSelected property.</summary>
	public bool drawWhenSelected
	{
		get { return _drawWhenSelected; }
		set { _drawWhenSelected = value; }
	}

	/// <summary>Gets and Sets color property.</summary>
	public Color color
	{
		get { return _color; }
		set { _color = value; }
	}

	/// <summary>Gets and Sets normalProjection property.</summary>
	public float normalProjection
	{
		get { return _normalProjection; }
		set { _normalProjection = value; }
	}

	/// <summary>Gets and Sets drawType property.</summary>
	public DrawTypes drawType
	{
		get { return _drawType; }
		set { _drawType = value; }
	}

	/// <summary>Gets and Sets priorMatrix property.</summary>
	public Matrix4x4 priorMatrix
	{
		get { return _priorMatrix; }
		protected set { _priorMatrix = value; }
	}
#endif

	void Awake()
	{
		ResizeCollider();
	}

	/// <summary>Resizes Waypoint's collider.</summary>
	public abstract void ResizeCollider();

#if UNITY_EDITOR
	/// <summary>Draws Waypoint, with its respective attributes and normals.</summary>
	void OnDrawGizmos()
	{
		CalculateGizmosMatrix();
		if(!drawWhenSelected) DrawWaypoint();
		Gizmos.matrix = priorMatrix;
		transform.DrawNormals(normalProjection);
	}

	/// <summary>Draws Waypoint only when selected, with its respective attributes and normals.</summary>
	void OnDrawGizmosSelected()
	{
		if(drawWhenSelected) DrawWaypoint();
	}

	/// \TODO Learn more about Quaternion's math.
	/// <summary>Calculates Gizmo's Matrix.</summary>
	protected virtual void CalculateGizmosMatrix()
	{
		Matrix4x4 gizmoTransform = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
		priorMatrix = Gizmos.matrix;
		Gizmos.matrix *= gizmoTransform;
	}

	/// <summary>Draws Waypoint's Gizmos.</summary>
	protected abstract void DrawWaypoint();
#endif

}
}