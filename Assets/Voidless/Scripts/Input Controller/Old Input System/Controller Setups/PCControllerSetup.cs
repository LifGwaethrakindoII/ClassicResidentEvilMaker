using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public class PCControllerSetup : BaseControllerSetup<KeyCode>
{
	public const int MOUSE_INPUTS_LENGTH = 3; 		/// <summary>Length of Mouse's Inputs.</summary>
	public const int INPUT_INDEX_MOUSE_LEFT = 0; 	/// <summary>Mouse's Left Button Index.</summary>
	public const int INPUT_INDEX_MOUSE_RIGHT = 1; 	/// <summary>Mouse's Right Button Index.</summary>
	public const int INPUT_INDEX_MOUSE_MIDDLE = 3; 	/// <summary>Mouse's Middle Button Index.</summary>

	/// <summary>Gets leftAxisX property.</summary>
	public override float leftAxisX { get { return Input.GetAxis("Horizontal"); } }

	/// <summary>Gets leftAxisY property.</summary>
	public override float leftAxisY { get { return Input.GetAxis("Vertical"); } }

	/// <summary>Gets rightAxisX property.</summary>
	public override float rightAxisX { get { return Input.GetAxis("Horizontal"); } }

	/// <summary>Gets rightAxisY property.</summary>
	public override float rightAxisY { get { return Input.GetAxis("Vertical"); } }

	/// <summary>Gets leftTrigger property.</summary>
	public override float leftTrigger { get { return 0.0f; } }

	/// <summary>Gets rightTrigger property.</summary>
	public override float rightTrigger { get { return 0.0f; } }

	/// <summary>Gets dPadAxisX property.</summary>
	public override float dPadAxisX { get { return 0.0f; } }

	/// <summary>Gets dPadAxisY property.</summary>
	public override float dPadAxisY { get { return 0.0f; } }

	/// <summary>Gets mouseAxisX property.</summary>
	public float mouseAxisX { get { return Input.GetAxis("Mouse X"); } }

	/// <summary>Gets mouseAxisY property.</summary>
	public float mouseAxisY { get { return Input.GetAxis("Mouse Y"); } }

	/// <summary>PCControllerSetup's Constructor.</summary>
	public PCControllerSetup() : base() {}
}
}