using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public interface IRange<T> where T : IComparable<T>
{
	T min { get; set; } 	/// <summary>Minimum's Value.</summary>
	T max { get; set; } 	/// <summary>Maximum's Value.</summary>

	/// <returns>Range's Median.</returns>
	T GetMedian();

	/// <returns>Range's Length.</returns>
	T GetLength();

	/// <returns>Maximum Value.</returns>
	T Max();

	/// <returns>Minimum Value.</returns>
	T Min();
}
}