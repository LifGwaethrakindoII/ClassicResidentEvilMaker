using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VColor32
{
	/// <summary>Converts normalized float value to byte.</summary>
	/// <param name="_b">Byte value to convert.</param>
	/// <param name="_f">Float value that converts the Byte.</param>
	/// <returns>Converted Byte.</returns>
	public static byte FloatToColor32Byte(out byte _b, float _f)
	{
		return _b = (byte)(_f >= 1.0f ? 255 : (_f <= 0.0f ? 0 : (byte)Mathf.Floor(_f * 256.0f)));
	}

	/// <summary>Sets Color32 Alpha.</summary>
	/// <param name="_color">The Color32 that will have its Alpha modified.</param>
	/// <param name="_alpha">Updated Color32 Alpha Component.</param>
	/// <returns>New modified Color32.</returns>
	public static Color32 WithAlpha(this Color32 _color, byte _alpha)
	{
		return _color = new Color32(_color.r, _color.g, _color.b, _alpha);
	}
}
}