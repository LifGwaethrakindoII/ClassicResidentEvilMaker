using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public interface ISteeringVehicle
{
	float maxSpeed { get; set; }			/// <summary>Vehicle's Maximum Speed.</summary>
	float maxSteeringForce { get; set; } 	/// <summary>Vehicle's Maximum Speed.</summary>
	float mass { get; set; } 				/// <summary>Vehicle's Mass.</summary>
	Vector3 velocity { get; set; } 			/// <summary>Vehicle's Current Velocity Vector.</summary>
}
}