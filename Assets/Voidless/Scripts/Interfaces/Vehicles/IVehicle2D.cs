using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public interface IVehicle2D : IBaseVehicle
{
	Rigidbody2D body { get; } 			/// <summary>Vehicle's Rigidbody Component.</summary>
	Vector2 dimensions { get; set; } 	/// <summary>Vehicle's Dimensions.</summary>
}
}