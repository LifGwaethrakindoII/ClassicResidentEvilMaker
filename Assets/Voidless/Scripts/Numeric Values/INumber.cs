using System;
using System.Collections.Generic;

namespace Voidless
{
public interface INumber<T> : IComparable<T>
{
	T value { get; set; } 	/// <summary>Number's value.</summary>
}
}