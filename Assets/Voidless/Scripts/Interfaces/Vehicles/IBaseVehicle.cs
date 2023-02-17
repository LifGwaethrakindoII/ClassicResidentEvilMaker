using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public interface IBaseVehicle
{
	float maxSpeed { get; set; } 	/// <summary>Vehicle's Max Speed.</summary>
	float maxForce { get; set; } 	/// <summary>Vehicle's Max Steering Force.</summary>
}
}