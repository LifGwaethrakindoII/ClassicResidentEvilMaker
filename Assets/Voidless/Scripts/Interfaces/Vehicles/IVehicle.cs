using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public interface IVehicle : IBaseVehicle
{
	Rigidbody body { get; } 			/// <summary>Vehicle's Rigidbody Component.</summary>
	Vector3 dimensions { get; set; } 	/// <summary>Vehicle's Dimensions.</summary>
}
}