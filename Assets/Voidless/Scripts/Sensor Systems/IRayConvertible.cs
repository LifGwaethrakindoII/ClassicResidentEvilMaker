using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public interface IRayConvertible
{
	Vector3 origin { get; set; } 		/// <summary>Ray convertible's origin reference.</summary>
	Vector3 direction { get; set; }	 	/// <summary>Ray convertible's heading direction.</summary>
}
}