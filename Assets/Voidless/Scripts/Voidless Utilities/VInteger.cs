using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// https://graphics.stanford.edu/~seander/bithacks.html#DetermineIfPowerOf2

namespace Voidless
{
public static class VInteger
{
	/// <summary>Gets the number of active flags on a given int.</summary>
	/// <param name="_enumFlag">Enum Flag to count active [1] bits.</param>
	/// <returns>Number of active bits on enum flag.</returns>
	public static int GetActiveFlagsCount(this int _enumFlag)
	{
		int count = 0;

		while(_enumFlag > 0)
		{
			_enumFlag &= (_enumFlag - 1);
			count++;
		}

		return count;
	}

	/// <summary>Gets flags that were added to a number.</summary>
	/// <param name="a">Main number.</param>
	/// <param name="b">Flags that were added.</param>
	/// <returns>Flags that were newly added to number.</returns>
	public static int GetAddeedFlags(int a, int b)
	{
		return ~a & b;
	}

	/// <summary>Evaluates if number contains all given flags.</summary>
	/// <param name="x">Number to evaluate.</param>
	/// <param name="flags">Flags to evaluate.</param>
	/// <returns>True if the given number contains all flags.</returns>
	public static bool HasFlags(this int x, int flags)
	{
		return (x | flags) == x;
	}

	/// <summary>Evaluates if number has any of the bit flags of number b.</summary>
	/// <param name="a">Number to evaluate.</param>
	/// <param name="b">Bitchain.</param>
	/// <returns>True if number a has any of b's bits.</returns>
	public static bool HasAnyFlag(this int a, int b)
	{
		return (a | (a & b)) == a;
	}

	/// <summary>Removes flags from number.</summary>
	/// <param name="x">Number to modify.</param>
	/// <param name="flags">Flags to remove.</param>
	/// <returns>Number with removed flags.</returns>
	public static int WithRemovedFlags(this int x, int flags)
	{
		return x & ~flags;
	}

	/// <summary>Gets removed flags from given number [the operation is commutative].</summary>
	/// <param name="a">Main number.</param>
	/// <param name="b">Flags to remove.</param>
	/// <returns>Bit chain with flags that were removed.</returns>
	public static int GetRemovedFlags(this int a, int b)
	{
		return a & b;
	}

	/// <summary>Checks if given number has just one bit on.</summary>
	/// <param name="x">Number to evaluate.</param>
	/// <returns>True if number has just one bit on, false otherwise.</returns>
	public static bool HasOneBit(int x)
	{
		return x == 0 ? (x & (x - 1)) == 0 : false;
	}

	/// <summary>Turns Nth bit on a given number.</summary>
	/// <param name="x">Number to modify.</param>
	/// <param name="n">Bit position to turn on.</param>
	/// <returns>Number with Nth bit turned on.</returns>
	public static int TurnBitAtPosition(this int x, int n)
	{
		return (n <= 0) ? x : x | (1 << (n - 1));
	}

	/// <summary>Converts LayerMask's value to integer.</summary>
	/// <param name="_layer">LayerMask's reference.</param>
	/// <returns>Integer representing value's power.</returns>
	public static int GetLayerMaskInt(this LayerMask _layer)
	{
		return _layer.value.Log();
	}

	/// <summary>Calculates Logarithm in base b.</summary>
	/// <param name="x">Number to calculate logarithm.</param>
	/// <param name="b">Logarithm's base [2 as default].</param>
	/// <returns>X's logarithm in base b.</returns>
	public static int Log(this int x, int b = 2)
	{
		int count = 0;

		while(x > 1)
		{
			x /= b;
			count++;
		}

		return count;
	}
}
}