using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public struct Trigger2DInformation
{
	private Collider2D _collider; 	/// <summary>Collider that has been intersected with.</summary>
	private Vector3 _contactPoint;	/// <summary>The point of intersection.</summary>
	private Vector3 _direction; 	/// <summary>Direction of the Collider2D A towards Collider2D B.</summary>
	private Vector3 _velocity; 		/// <summary>Object's Velocity.</summary>

	/// <summary>Gets and Sets collider property.</summary>
	public Collider2D collider
	{
		get { return _collider; }
		private set { _collider = value; }
	}

	/// <summary>Gets and Sets contactPoint property.</summary>
	public Vector3 contactPoint
	{
		get { return _contactPoint; }
		private set { _contactPoint = value; }
	}

	/// <summary>Gets and Sets direction property.</summary>
	public Vector3 direction
	{
		get { return _direction; }
		set { _direction = value; }
	}

	/// <summary>Gets and Sets velocity property.</summary>
	public Vector3 velocity
	{
		get { return _velocity; }
		private set { _velocity = value; }
	}

	/// <summary>Trigger2DInformation's Constructor.</summary>
	/// <param name="_collider">Collider2D's reference.</param>
	/// <param name="_contactPoint">Point of contact.</param>
	public Trigger2DInformation(Collider2D a, Collider2D b, Vector3 _velocity = default(Vector3)) : this()
	{
		if(a == null || b == null) return;

		Vector3 point = a.bounds.ClosestPoint(b.transform.position);

		collider = b;
		contactPoint = point;
		direction = point - a.transform.position;
		velocity = _velocity;
	}

	/// <summary>Trigger2DInformation's Constructor.</summary>
	/// <param name="_collider">Collider2D's reference.</param>
	/// <param name="_contactPoint">Point of contact.</param>
	public Trigger2DInformation(Vector3 _origin, RaycastHit2D _hitInfo, Vector3 _velocity = default(Vector3)) : this()
	{
		if(_hitInfo.collider == null) return;

		Vector3 point = _hitInfo.point;

		collider = _hitInfo.collider;
		contactPoint = point;
		direction = point - _origin;
		velocity = _velocity;
	}

	/// <summary>Creates a Trigger2DInformation structore from 2 Collider2Ds.</summary>
	/// <param name="a">Collider2D A.</param>
	/// <param name="b">Collider2D B.</param>
	/// <returns>Trigger2DInformation given two Collider2Ds.</returns>
	public static Trigger2DInformation Create(Collider2D a, Collider2D b, Vector3 _velocity = default(Vector3))
	{
		return new Trigger2DInformation(a, b, _velocity);
	}

	/// <returns>String representing this Trigger2D's Information.</returns>
	public override string ToString()
	{
		if(collider == null)
		return "No Trigger 2D information could be retreived since there was no Collider2D data...";

		StringBuilder builder = new StringBuilder();

		builder.AppendLine("Trigger2D's Information: ");
		builder.Append("Collider2D of type ");
		builder.Append(collider.name);
		builder.Append(" from GameObject ");
		builder.AppendLine(collider.gameObject.name);
		builder.Append("Contact Point: ");
		builder.AppendLine(contactPoint.ToString());
		builder.Append("Direction: ");
		builder.Append(direction.ToString());
		builder.Append("Velocity: ");
		builder.Append(velocity.ToString());

		return builder.ToString();
	}
}
}