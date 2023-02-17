using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VMaterial
{
	public static readonly MaterialTag MATERIAL_TAG_COLOR; 				/// <summary>Main Color Material Tag.</summary>
	public static readonly MaterialTag MATERIAL_TAG_COLOR_EMISSION; 	/// <summary>Emission Color Material Tag.</summary>
	public static readonly MaterialTag MATERIAL_TAG_ALBEDO; 			/// <summary>Albedo Material Tag.</summary>

	/// <summary>Static VMaterial's Constructor.</summary>
	static VMaterial()
	{
		MATERIAL_TAG_COLOR = "_Color";
		MATERIAL_TAG_COLOR_EMISSION = "_EmissionColor";
		MATERIAL_TAG_ALBEDO = "_Albedo";
	}
}
}