using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public struct TriggerInformation
{
	private Collider _collider; 	/// <summary>Collider that has been intersected with.</summary>
	private Vector3 _contactPoint;	/// <summary>The point of intersection.</summary>
	private Vector3 _direction; 	/// <summary>Direction of the Collider A towards Collider B.</summary>

	/// <summary>Gets and Sets collider property.</summary>
	public Collider collider
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

	/// <summary>TriggerInformation's Constructor.</summary>
	/// <param name="_collider">Collider's reference.</param>
	/// <param name="_contactPoint">Point of contact.</param>
	public TriggerInformation(Collider a, Collider b) : this()
	{
		Vector3 point = a.bounds.ClosestPoint(b.transform.position);

		collider = b;
		contactPoint = point;
		direction = point - a.transform.position;
	}

	/// <summary>Creates a TriggerInformation structore from 2 Colliders.</summary>
	/// <param name="a">Collider A.</param>
	/// <param name="b">Collider B.</param>
	/// <returns>TriggerInformation given two Colliders.</returns>
	public static TriggerInformation Create(Collider a, Collider b)
	{
		return new TriggerInformation(a, b);
	}

	/// <returns>String representing this Trigger's Information.</returns>
	public override string ToString()
	{
		if(collider == null)
		return "No Trigger information could be retreived since there was no Collider data...";

		StringBuilder builder = new StringBuilder();

		builder.AppendLine("Trigger's Information: ");
		builder.Append("Collider of type ");
		builder.Append(collider.name);
		builder.Append(" from GameObject ");
		builder.AppendLine(collider.gameObject.name);
		builder.Append("Contact Point: ");
		builder.AppendLine(contactPoint.ToString());
		builder.Append("Direction: ");
		builder.Append(direction.ToString());

		return builder.ToString();
	}
}
}