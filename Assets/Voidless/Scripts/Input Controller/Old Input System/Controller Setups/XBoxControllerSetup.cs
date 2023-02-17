using System.Collections;
using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public enum XBoxInputKey 	/// <summary>A more comfortable Input Mapping enumerator for XBox 360 / One controller.</summary>
{
	A, 						/// <summary>A Button XBox Controller Input.</summary>	
	B, 						/// <summary>B Button XBox Controller Input.</summary>	
	X, 						/// <summary>X Button XBox Controller Input.</summary>
	Y, 						/// <summary>Y Button XBox Controller Input.</summary>
	LB, 					/// <summary>Left Bumper XBox Controller Input.</summary>
	RB, 					/// <summary>Right Bumper XBox Controller Input.</summary>
	LeftTrigger, 			/// <summary>Left Trigger XBox Controller Input.</summary>
	RightTrigger, 			/// <summary>Right Trigger XBox Controller Input.</summary>
	Back, 					/// <summary>Back Button XBox Controller Input.</summary>
	Start, 					/// <summary>Start Button XBox Controller Input.</summary>
	LeftStickClick, 		/// <summary>Left Stick Click XBox Controller Input.</summary>
	RightStickClick, 		/// <summary>Right Stick Click XBox Controller Input.</summary>
//#if (UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX)
	DPadUp, 				/// <summary>Directional-Pad Up Button XBox Controller Input. Just for both Mac and Linux platforms</summary>
	DPadDown, 				/// <summary>Directional-Pad Down Button XBox Controller Input. Just for both Mac and Linux platforms</summary>
	DPadLeft, 				/// <summary>Directional-Pad Left Button XBox Controller Input. Just for both Mac and Linux platforms</summary>
	DPadRight, 				/// <summary>Directional-Pad Right Button XBox Controller Input. Just for both Mac and Linux platforms</summary>
//#endif
#if (UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX)
	XBoxButton 				/// <summary>XBox Button XBox Controller Input. Just for Mac platform only</summary>
#endif
}

[Serializable]
public class XBoxControllerSetup : BaseControllerSetup<XBoxInputKey>
{
	public const string DEFAULT_AXIS_LEFT_X = "XBox_Horizontal_Left";
	public const string DEFAULT_AXIS_LEFT_Y = "XBox_Vertical_Left";
	public const string DEFAULT_AXIS_RIGHT_X = "XBox_Horizontal_Right";
	public const string DEFAULT_AXIS_RIGHT_Y = "XBox_Vertical_Right";
	public const string DEFAULT_AXIS_TRIGGER_LEFT = "XBox_Left_Trigger";
	public const string DEFAULT_AXIS_TRIGGER_RIGHT = "XBox_Right_Trigger";
	public const string DEFAULT_AXIS_DPAD_X = "XBox_Horizontal_DPad";
	public const string DEFAULT_AXIS_DPAD_Y = "XBox_Vertical_DPad";

	[SerializeField] private string _leftAxisXKey;
	[SerializeField] private string _leftAxisYKey;
	[SerializeField] private string _rightAxisXKey;
	[SerializeField] private string _rightAxisYKey;
	[SerializeField] private string _rightTriggerKey;
	[SerializeField] private string _leftTriggerKey;
	[SerializeField] private string _dPadAxisXKey;
	[SerializeField] private string _dPadAxisYKey;
	public InputState leftTriggerState;
	public InputState rightTriggerState;
	public InputState dPadUpState;
	public InputState dPadDownState;
	public InputState dPadLeftState;
	public InputState dPadRightState;

#region Getters/Setters:
	/// <summary>Gets and Sets leftAxisXKey property.</summary>
	public string leftAxisXKey
	{
		get { return _leftAxisXKey; }
		set { _leftAxisXKey = value; }
	}

	/// <summary>Gets and Sets leftAxisYKey property.</summary>
	public string leftAxisYKey
	{
		get { return _leftAxisYKey; }
		set { _leftAxisYKey = value; }
	}

	/// <summary>Gets and Sets rightAxisXKey property.</summary>
	public string rightAxisXKey
	{
		get { return _rightAxisXKey; }
		set { _rightAxisXKey = value; }
	}

	/// <summary>Gets and Sets rightAxisYKey property.</summary>
	public string rightAxisYKey
	{
		get { return _rightAxisYKey; }
		set { _rightAxisYKey = value; }
	}

	/// <summary>Gets and Sets rightTriggerKey property.</summary>
	public string rightTriggerKey
	{
		get { return _rightTriggerKey; }
		set { _rightTriggerKey = value; }
	}

	/// <summary>Gets and Sets leftTriggerKey property.</summary>
	public string leftTriggerKey
	{
		get { return _leftTriggerKey; }
		set { _leftTriggerKey = value; }
	}

	/// <summary>Gets and Sets dPadAxisXKey property.</summary>
	public string dPadAxisXKey
	{
		get { return _dPadAxisXKey; }
		set { _dPadAxisXKey = value; }
	}

	/// <summary>Gets and Sets dPadAxisYKey property.</summary>
	public string dPadAxisYKey
	{
		get { return _dPadAxisYKey; }
		set { _dPadAxisYKey = value; }
	}

	/*/// <summary>Gets and Sets dPadUpState property.</summary>
	public InputState dPadUpState
	{
		get { return _dPadUpState; }
		set { _dPadUpState = value; }
	}

	/// <summary>Gets and Sets dPadDownState property.</summary>
	public InputState dPadDownState
	{
		get { return _dPadDownState; }
		set { _dPadDownState = value; }
	}

	/// <summary>Gets and Sets dPadLeftState property.</summary>
	public InputState dPadLeftState
	{
		get { return _dPadLeftState; }
		set { _dPadLeftState = value; }
	}

	/// <summary>Gets and Sets dPadRightState property.</summary>
	public InputState dPadRightState
	{
		get { return _dPadRightState; }
		set { _dPadRightState = value; }
	}*/
#endregion

	/// <summary>Gets leftAxisX property.</summary>
	public override float leftAxisX { get { return Input.GetAxis(leftAxisXKey); } }

	/// <summary>Gets leftAxisY property.</summary>
	public override float leftAxisY { get { return Input.GetAxis(leftAxisYKey); } }

	/// <summary>Gets rightAxisX property.</summary>
	public override float rightAxisX { get { return Input.GetAxis(rightAxisXKey); } }

	/// <summary>Gets rightAxisY property.</summary>
	public override float rightAxisY { get { return Input.GetAxis(rightAxisYKey); } }

	/// <summary>Gets leftTrigger property.</summary>
	public override float leftTrigger { get { return Input.GetAxis(leftTriggerKey); } }

	/// <summary>Gets rightTrigger property.</summary>
	public override float rightTrigger { get { return Input.GetAxis(rightTriggerKey); } }

	/// <summary>Gets dPadAxisX property.</summary>
	public override float dPadAxisX { get { return Input.GetAxis(dPadAxisXKey); } }

	/// <summary>Gets dPadAxisY property.</summary>
	public override float dPadAxisY { get { return Input.GetAxis(dPadAxisYKey); } }

	/// <summary>XBoxControllerSetup's Constructor.</summary>
	public XBoxControllerSetup() : base()
	{
		leftAxisXKey = DEFAULT_AXIS_LEFT_X;
		leftAxisYKey = DEFAULT_AXIS_LEFT_Y;
		rightAxisXKey = DEFAULT_AXIS_RIGHT_X;
		rightAxisYKey = DEFAULT_AXIS_RIGHT_Y;
		rightTriggerKey = DEFAULT_AXIS_TRIGGER_RIGHT;
		leftTriggerKey = DEFAULT_AXIS_TRIGGER_LEFT;
		dPadAxisXKey = DEFAULT_AXIS_DPAD_X;
		dPadAxisYKey = DEFAULT_AXIS_DPAD_Y;
		dPadUpState = InputState.None;
		dPadDownState = InputState.None;
		dPadLeftState = InputState.None;
		dPadRightState = InputState.None;
	}

	/// <returns>String representing this Controller's Setup.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.AppendLine(base.ToString());
		builder.AppendLine("Axes' Keys: \n");
		builder.Append("Left's Axis X: ");
		builder.AppendLine(leftAxisXKey);
		builder.Append("Left's Axis Y: ");
		builder.AppendLine(leftAxisYKey);
		builder.Append("Right's Axis X: ");
		builder.AppendLine(rightAxisXKey);
		builder.Append("Right's Axis Y: ");
		builder.AppendLine(rightAxisYKey);
		builder.Append("Right Trigger's Axis: ");
		builder.AppendLine(rightTriggerKey);
		builder.Append("Left Trigger's Axis: ");
		builder.AppendLine(leftTriggerKey);
		builder.Append("D-Pad's Axis X: ");
		builder.AppendLine(dPadAxisXKey);
		builder.Append("D-Pad's Axis Y: ");
		builder.AppendLine(dPadAxisYKey);
		builder.AppendLine();
		builder.AppendLine("Axes Keys' States: \n");
		builder.Append("Left Trigger's State: ");
		builder.AppendLine(leftTriggerState.ToString());
		builder.Append("Right Trigger's State: ");
		builder.AppendLine(rightTriggerState.ToString());
		builder.Append("D-Pad Up's State: ");
		builder.AppendLine(dPadUpState.ToString());
		builder.Append("D-Pad Down's State: ");
		builder.AppendLine(dPadDownState.ToString());
		builder.Append("D-Pad Left's State: ");
		builder.AppendLine(dPadLeftState.ToString());
		builder.Append("D-Pad Right's State: ");
		builder.AppendLine(dPadRightState.ToString());

		return builder.ToString();
	}
}
}