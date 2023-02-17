using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class BillboardElement : MonoBehaviour
{
	[SerializeField] private Axes3D _axesConstrains; 	/// <summary>Axes constrains when turning on camera.</summary>
	private Vector3 _originalOrientation; 				/// <summary>Starting original orientation.</summary>
	private Vector3 _direction; 						/// <summary>Current direction towards camera.</summary>

	/// <summary>Gets and Sets axesConstrains property.</summary>
	public Axes3D axesConstrains
	{
		get { return _axesConstrains; }
		protected set { _axesConstrains = value; }
	}

	/// <summary>Gets and Sets originalOrientation property.</summary>
	public Vector3 originalOrientation
	{
		get { return _originalOrientation; }
		protected set { _originalOrientation = value; }
	}

	/// <summary>Gets and Sets direction property.</summary>
	public Vector3 direction
	{
		get { return _direction; }
		protected set { _direction = value; }
	}

#region UnityMethods:
	void Awake()
	{
		originalOrientation = new Vector3
		(
			transform.right.x,
			transform.up.y,
			transform.forward.z
		);
	}

	/// <summary>BillboardElement's tick at each frame.</summary>
	void Update ()
	{
		direction = new Vector3
		(
			axesConstrains.HasFlag(Axes3D.X) ? originalOrientation.x : Camera.main.transform.position.x - transform.position.x,
			axesConstrains.HasFlag(Axes3D.Y) ? originalOrientation.y : Camera.main.transform.position.y - transform.position.y,
			axesConstrains.HasFlag(Axes3D.Z) ? originalOrientation.z : Camera.main.transform.position.z - transform.position.z
		);

		transform.rotation = Quaternion.LookRotation(direction);
	}
#endregion
}
}