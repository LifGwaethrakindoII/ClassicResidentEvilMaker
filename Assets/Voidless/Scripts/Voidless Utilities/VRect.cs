using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VRect
{
	/// <summary>Returns rect with modified X.</summary>
	/// <param name="_rect">Rect to extend.</param>
	/// <param name="_x">X to modify from extended Rect.</param>
	/// <returns>Modified Rect.</returns>
	public static Rect WithX(this Rect _rect, float _x)
	{
		Rect result = _rect;
		result.x = _x;

		return result;
	}

	/// <summary>Returns rect with modified Y.</summary>
	/// <param name="_rect">Rect to extend.</param>
	/// <param name="_y">Y to modify from extended Rect.</param>
	/// <returns>Modified Rect.</returns>
	public static Rect WithY(this Rect _rect, float _y)
	{
		Rect result = _rect;
		result.y = _y;
		
		return result;
	}

	/// <summary>Returns rect with modified Width.</summary>
	/// <param name="_rect">Rect to extend.</param>
	/// <param name="_width">Width to modify from extended Rect.</param>
	/// <returns>Modified Rect.</returns>
	public static Rect WithWidth(this Rect _rect, float _width)
	{
		Rect result = _rect;
		result.width = _width;
		
		return result;
	}

	/// <summary>Returns rect with modified Height.</summary>
	/// <param name="_rect">Rect to extend.</param>
	/// <param name="_height">Height to modify from extended Rect.</param>
	/// <returns>Modified Rect.</returns>
	public static Rect WithHeight(this Rect _rect, float _height)
	{
		Rect result = _rect;
		result.height = _height;
		
		return result;
	}

	/// <summary>Returns rect with added Y.</summary>
	/// <param name="_rect">Rect to extend.</param>
	/// <param name="_x">X to add from extended Rect.</param>
	/// <returns>Modified Rect.</returns>
	public static Rect WithAddedX(this Rect _rect, float _x)
	{
		Rect result = _rect;
		result.x += _x;

		return result;
	}

	/// <summary>Returns rect with added Y.</summary>
	/// <param name="_rect">Rect to extend.</param>
	/// <param name="_y">Y to add from extended Rect.</param>
	/// <returns>Modified Rect.</returns>
	public static Rect WithAddedY(this Rect _rect, float _y)
	{
		Rect result = _rect;
		result.y += _y;
		
		return result;
	}

	/// <summary>Returns rect with modified Width.</summary>
	/// <param name="_rect">Rect to extend.</param>
	/// <param name="_width">Width to modify from extended Rect.</param>
	/// <returns>Modified Rect.</returns>
	public static Rect WithAddedWidth(this Rect _rect, float _width)
	{
		Rect result = _rect;
		result.width += _width;
		
		return result;
	}

	/// <summary>Returns rect with modified Height.</summary>
	/// <param name="_rect">Rect to extend.</param>
	/// <param name="_height">Height to modify from extended Rect.</param>
	/// <returns>Modified Rect.</returns>
	public static Rect WithAddedHeight(this Rect _rect, float _height)
	{
		Rect result = _rect;
		result.height += _height;
		
		return result;
	}
}
}