using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public enum AnimatorParameterType
{
	Bool,
	Int,
	Float,
	BlendTreeFloat
}

[Serializable]
public struct AnimatorParameter
{
	public AnimatorCredential key; 		/// <summary>Animator's Parameter Key.</summary>
	public AnimatorParameterType type; 	/// <summary>Parameter's Type.</summary>
	public bool boolValue; 				/// <summary>Boolean's Value.</summary>
	public int intValue; 				/// <summary>Integer's Value.</summary>
	public float floatValue; 			/// <summary>Float's Value.</summary>
}
}