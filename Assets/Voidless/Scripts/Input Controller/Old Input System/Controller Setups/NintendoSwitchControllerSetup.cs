using System.Collections;
using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public class NintendoSwitchControllerSetup : BaseControllerSetup<NintendoSwitchButton>
{
//	Switch is in handheld mode or tabletop and Joy-Con attached to the console:
//		- Joystick1...: Joy-Con attached to the Switch console.
//		- Joystick1-9 Wireless Controllers 1-8
//		- Joystick10: SDEV Debug Pad

//	Any other state:
//		- Joystick1-8: Wireless Controllers 1-8
//		- Joystick10: SDEV Debug Pad

//	NOTE: With Unity versions prior to 2020, joystick * button takes a value from 1 to 9. Specify 9 to receive the input of the SDEV debug pad.
//	
//	Axis:

//		X Axis: Left-Stick X-Axis
//		Y Axis: Left-Stick Y-Axis
//		3rd Axis: ZL, ZR Buttons
//		4th Axis: Right-Stick X-Axis
//		5th Axis: Right-Stick Y-Axis
//		6th Axis: Right/Left Buttons
//		7th Axis: Up/Down Buttons

	/// \TODO THIS IS NOT CORRECT:
	/// <summary>Gets leftAxisX property.</summary>
	public override float leftAxisX { get { return Input.GetAxis("Horizontal"); } }

	/// <summary>Gets leftAxisY property.</summary>
	public override float leftAxisY { get { return Input.GetAxis("Horizontal"); } }

	/// <summary>Gets rightAxisX property.</summary>
	public override float rightAxisX { get { return Input.GetAxis("Horizontal"); } }

	/// <summary>Gets rightAxisY property.</summary>
	public override float rightAxisY { get { return Input.GetAxis("Horizontal"); } }

	/// <summary>Gets leftTrigger property.</summary>
	public override float leftTrigger { get { return Input.GetAxis("Horizontal"); } }

	/// <summary>Gets rightTrigger property.</summary>
	public override float rightTrigger { get { return Input.GetAxis("Horizontal"); } }

	/// <summary>Gets dPadAxisX property.</summary>
	public override float dPadAxisX { get { return Input.GetAxis("Horizontal"); } }

	/// <summary>Gets dPadAxisY property.</summary>
	public override float dPadAxisY { get { return Input.GetAxis("Horizontal"); } }
}
}