using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public interface IRay2DConvertible
{
	Vector2 origin { get; set; } 		/// <summary>Ray convertible's origin reference.</summary>
	Vector2 direction { get; set; }	 	/// <summary>Ray convertible's heading direction.</summary>
}
}