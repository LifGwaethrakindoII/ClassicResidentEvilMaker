using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
/// <summary>Event invoked when this FOV Sight leaves another collider.</summary>
/// <param name="_collider">Collider sighted.</param>
/// <param name="_eventType">Type of interaction.</param>
public delegate void OnSightEvent2D(Collider2D _collider, HitColliderEventTypes _eventType);

[RequireComponent(typeof(PolygonCollider2D))]
public class FOVSight2D : MonoBehaviour
{
	public event OnSightEvent2D onSightEvent; 					/// <summary>OnSightEnter2D subscription delegate.</summary>		

	[Header("Events' Attributes:")]
	[SerializeField] private HitColliderEventTypes _eventType; 	/// <summary>Trigger Event Types.</summary>
	[SerializeField] private LayerMask _visibleLayers; 			/// <summary>Visible Layer Mask's Objects.</summary>
	[Space(5f)]
	[Header("FOV's Attributes:")]
	[SerializeField]
	[Range(0.0f, Mathf.Infinity)] private float _nearPlane; 	/// <summary>Near Plane's Dimension.</summary>
	[SerializeField]
	[Range(0.0f, 180.0f)] private float _FOVAngle; 				/// <summary>Field of View's Angle.</summary>
	[SerializeField] private float _length; 					/// <summary>Sight's Length.</summary>
	private PolygonCollider2D _polygonCollider; 				/// <summary>PolygonCollider2D's Component.</summary>

	/// <summary>Gets and Sets eventType property.</summary>
	public HitColliderEventTypes eventType
	{
		get { return _eventType; }
		set { _eventType = value; }
	}

	/// <summary>Gets and Sets visibleLayers property.</summary>
	public LayerMask visibleLayers
	{
		get { return _visibleLayers; }
		set { _visibleLayers = value; }
	}

	/// <summary>Gets and Sets nearPlane property.</summary>
	public float nearPlane
	{
		get { return _nearPlane; }
		set
		{
			_nearPlane = value;
			UpdateFOVSight();
		}
	}

	/// <summary>Gets and Sets FOVAngle property.</summary>
	public float FOVAngle
	{
		get { return _FOVAngle; }
		set
		{
			_FOVAngle = value;
			UpdateFOVSight();
		}
	}

	/// <summary>Gets and Sets length property.</summary>
	public float length
	{
		get { return _length; }
		set
		{
			_length = value;
			UpdateFOVSight();
		}
	}

	/// <summary>Gets and Sets polygonCollider Component.</summary>
	public PolygonCollider2D polygonCollider
	{ 
		get
		{
			if(_polygonCollider == null) _polygonCollider = GetComponent<PolygonCollider2D>();
			return _polygonCollider;
		}
	}

#region UnityMethods:
	private void OnDrawGizmos()
	{
		UpdateFOVSight();
	}

	/// <summary>FOVSight2D's instance initialization.</summary>
	private void Awake()
	{
		UpdateFOVSight();
	}

	private void OnTriggerEnter2D(Collider2D _collider)
	{
		if(!eventType.HasFlag(HitColliderEventTypes.Enter)) return;
		if((visibleLayers | _collider.gameObject.layer) == visibleLayers  && onSightEvent != null) onSightEvent(_collider, HitColliderEventTypes.Enter);
	}

	private void OnTriggerStay2D(Collider2D _collider)
	{
		if(!eventType.HasFlag(HitColliderEventTypes.Stays)) return;
		if((visibleLayers | _collider.gameObject.layer) == visibleLayers  && onSightEvent != null) onSightEvent(_collider, HitColliderEventTypes.Stays);
	}

	private void OnTriggerExit2D(Collider2D _collider)
	{
		if(!eventType.HasFlag(HitColliderEventTypes.Exit)) return;
		if((visibleLayers | _collider.gameObject.layer) == visibleLayers  && onSightEvent != null) onSightEvent(_collider, HitColliderEventTypes.Exit);
	}
#endregion

	/// <summary>Updates FOV's Sight.</summary>
	private void UpdateFOVSight()
	{
		EulerRotation rotation = new EulerRotation(0.0f, 0.0f, 90.0f);
		float halfAngle = (FOVAngle * 0.5f  * Mathf.Deg2Rad);
		float nearX = Mathf.Tan(halfAngle);
		float x = Mathf.Cos(halfAngle);
		float y = Mathf.Sqrt((length * length) + (x * x));
		float farX = (nearX * length);

		nearX *= nearPlane;

		Vector2[] newPoints = new Vector2[]
		{
			rotation * new Vector3(-nearX, -nearPlane),
			rotation * new Vector3(nearX, -nearPlane),
			rotation * new Vector3(farX, -y),
			rotation * new Vector3(-farX, -y)
		};

		polygonCollider.isTrigger = true;
		polygonCollider.points = newPoints;
	}
}
}