using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public interface ICloneable<T>
{
	/// <summary>Makes a shallow copy of an object's instance.</summary>
	/// <returns>Shallow copy, on new instance.</returns>
	T Clone();
}
}