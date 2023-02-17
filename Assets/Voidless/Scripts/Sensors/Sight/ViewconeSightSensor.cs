using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Voidless
{
[RequireComponent(typeof(SphereCollider))]
public class ViewconeSightSensor : SightSensor
{
	[Space(5f)]
	[Header("Viewcone's Attributes:")]
	[SerializeField]
	[Range(0.0f, 360.0f)] private float _angleZ; 		/// <summary>Field of View's Angle in the Z's Axis.</summary>
	[SerializeField]
	[Range(0.0f, 360.0f)] private float _angleX; 		/// <summary>Field of View's Angle in the X's Axis.</summary>
	[SerializeField] private float _nearPlane; 			/// <summary>Near Plane.</summary>
	private SphereCollider _sphereCollider; 			/// <summary>SphereCollider's Component.</summary>

	/// <summary>Gets nearPlane property.</summary>
	public override float nearPlane { get { return _nearPlane; } }

	/// <summary>Gets and Sets angleZ property.</summary>
	public float angleZ
	{
		get { return _angleZ; }
		set { _angleZ = value; }
	}

	/// <summary>Gets and Sets angleX property.</summary>
	public float angleX
	{
		get { return _angleX; }
		set { _angleX = value; }
	}

	/// <summary>Gets sphereCollider Component.</summary>
	public SphereCollider sphereCollider
	{ 
		get
		{
			if(_sphereCollider == null) _sphereCollider = GetComponent<SphereCollider>();
			return _sphereCollider;
		}
	}

	/// <summary>Gets Far Plane property.</summary>
	public override float farPlane { get { return sphereCollider.radius; } }

	/// <summary>Updates Sensor.</summary>
	protected override void UpdateSensor()
	{
		/// \TODO Evaluate for both angleZ ad angleX.
		Collider collider = null;
		HashSet<int> evaluatedIDs = new HashSet<int>();
		Vector3 origin = transform.position + (transform.forward * nearPlane);
		Vector3 direction = Vector3.zero;
		float halfAngleZ = angleZ * 0.5f;
		float halfAngleX = angleX * 0.5f;
		float angle = 0.0f;
		int instanceID = 0;

		foreach(int key in inFOV.Keys.ToList())
		{
			if(!inFOV.ContainsKey(key)) continue;

			collider = inFOV[key];
			instanceID = collider.gameObject.GetInstanceID();
			direction = collider.transform.position - origin;
			angle = Vector3.Angle(transform.forward, direction);
			evaluatedIDs.Add(instanceID);

			if(angle <= Mathf.Max(halfAngleZ, halfAngleX) && IsVisible(collider)) ColliderSighted(collider);
		}

		foreach(int key in inSight.Keys.ToList())
		{
			if(!inSight.ContainsKey(key)) continue;

			collider = inSight[key];
			instanceID = collider.gameObject.GetInstanceID();
			direction = collider.transform.position - origin;
			angle = Vector3.Angle(transform.forward, direction);

			if(!evaluatedIDs.Contains(instanceID))
			{
				if(angle <= Mathf.Max(halfAngleZ, halfAngleX))
				{ /// The Collider is inside the Viewcone.
					if(!IsVisible(collider)) ColliderOccluded(collider);
				}else ColliderOutOfSight(collider, true); 	/// Collider not within viewcone, therefore out of sight.
			}
		}
	}
}
}