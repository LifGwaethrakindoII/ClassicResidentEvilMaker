using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Flags]
public enum AbilityState 														/// <summary>Ability's States.</summary>
{
	Available = 1, 																/// <summary>Available State.</summary>
	Unavailable = 2, 															/// <summary>Unavailable State.</summary>
	Using = 4, 																	/// <summary>Using State.</summary>
	Fatigue = 8 																/// <summary>Fatigue State.</summary>
}

[Flags]
public enum Axes2D 																/// <summary>Axes on a 2D Space.</summary>
{
	None = 0, 																	/// <summary>No Axis.</summary>
	X = 1, 																		/// <summary>X Axis.</summary>
	Y = 2, 																		/// <summary>Y Axis.</summary>

	All = X | Y 																/// <summary>X and Y Axes.</summary>
}

[Flags]
public enum Axes3D 																/// <summary>Axes on a 3D space.</summary>
{
	None = 0, 																	/// <summary>No Axis.</summary>
	X = 1, 																		/// <summary>X Axis.</summary>
	Y = 2, 																		/// <summary>Y Axis.</summary>
	Z = 4, 																		/// <summary>Z Axis.</summary>

	/*NegativeX = 8, 																/// <summary>Negative X Axis.</summary>
	NegativeY = 16, 															/// <summary>Negative Y Axis.</summary>
	NegativeZ = 32, 															/// <summary>Negative Z Axis.</summary>*/

	XAndY = X | Y, 																/// <summary>X and Y Axes.</summary>
	XAndZ = X | Z, 																/// <summary>X and Z Axes.</summary>
	YAndZ = Y | Z, 																/// <summary>Y and Z Axes.</summary>
	All = X | Y | Z 															/// <summary>X, Y and Z Axes.</summary>
}

[Flags]
public enum DetectableControllers 												/// <summary>Detectable controllers on Play Mode.</summary>
{
	None = 0, 																	/// <summary>No detectable Controller.</summary>
	Pc = 1, 																	/// <summary>PC Controller.</summary>
	XBox = 2, 																	/// <summary>XBox Controller.</summary>

	PcAndXBox = Pc | XBox 														/// <summary>Both PC and XBox Controllers.</summary>
}

[Flags]
public enum FSMStates 															/// <summary>Basic Finite-State-Machine States.</summary>
{
	Unassigned = 0, 															/// <summary>Basic Finite-State-Machine State.</summary>
	Idle = 2, 																	/// <summary>Idle Finite-State-Machine State, for when the entity is passive.</summary>
	Aware = 4, 																	/// <summary>Aware Finite-State-Machine State, for when the enity knows an interest object is around.</summary>
	Intrigued = 8, 																/// <summary>Intrigued Finite-State-Machine State, for when the enity is aware something is up.</summary>
	Alert = 16, 																/// <summary>Alert Finite-State-Machine State, for when the entity has spoted an interest object.</summary>
	Aggressive = 32, 															/// <summary>Aggressive Finite-State-Machine State, for when the entity engages aggressively against the interest object.</summary>
	Fleeing = 64, 																/// <summary>Fleeing Finite-State-Machine State, for when the enity wants to escape from the interest object.</summary>
	Dead = 128 																	/// <summary>Dead Finite-State-Machine State, for when the entity is out of possibility of acting.</summary>
}

[Flags]
public enum HitColliderEventTypes 												/// <summary>Hit Collider Event's Types.</summary>
{
	Enter = 1, 																	/// <summary>Enter Hit Event Type.</summary>
	Stays = 2, 																	/// <summary>Stays Hit Event Type.</summary>
	Exit = 4, 																	/// <summary>Exit Hit Event Type.</summary>

	EnterAndExit = Enter | Exit, 												/// <summary>Enter & Exit Event Type.</summary>
	EnterAndStays = Enter | Stays, 												/// <summary>Enter & Stays Event Type.</summary>
	ExitAndStays = Exit | Stays, 												/// <summary>Exit & Stays Event Type.</summary>
	All = Enter | Stays | Exit 													/// <summary>All Event Types.</summary>
}

[Flags]
public enum InputEvaluations 													/// <summary>Input's Evaluations.</summary>
{
	Events = 1, 																/// <summary>Evaluation by Events.</summary>
	Polling = 2, 																/// <summary>Evaluations by polling an specific input.</summary>
	
	Both = Events | Polling 													/// <summary>Evaluations by both Events and Polling.</summary>
}

[Flags]
public enum OrientationSemantics 												/// <summary>Orientation Semantics.</summary>
{
	None = 0, 																	/// <summary>None Orientation.</summary>
	Middle = 1, 																/// <summary>Middle Orientation.</summary>
	Left = 2, 																	/// <summary>Left Orientation.</summary>
	Right = 4, 																	/// <summary>Right Orientation.</summary>
	Up = 8, 																	/// <summary>Up Orientation.</summary>
	Down = 16, 																	/// <summary>Down Orientation.</summary>
	Forward = 32, 																/// <summary>Forward Orientation.</summary>
	Back = 64, 																	/// <summary>Back Orientation.</summary>

	UpAndForward = Up | Forward, 												/// <summary>Up and Forward Orientation.</summary>
	UpAndBack = Up | Back, 														/// <summary>Up and Back Orientation.</summary>
	DownAndForward = Down | Forward, 											/// <summary>Down adn Forward Orientation.</summary>
	DownAndBack = Down | Back, 													/// <summary>Down And Back Orientation.</summary>
	LeftAndUp = Left | Up, 														/// <summary>Left and Up Orientation.</summary>
	LeftAndDown = Left | Down, 													/// <summary>Left and Down Orientation.</summary>
	LeftAndBack = Left | Back, 													/// <summary>Left and Back Orientation.</summary>
	LeftAndForward = Left | Forward, 											/// <summary>Left and Forward Orientation.</summary>
	RightAndUp = Right | Up, 													/// <summary>Right and Up Orientation.</summary>
	RightAndDown = Right | Down, 												/// <summary>Right and Down Orientation.</summary>
	RightAndBack = Right | Back, 												/// <summary>Right and Back Orientation.</summary>
	RightAndForward = Right | Forward, 											/// <summary>Right and Forward Orientation.</summary>
	LeftAndUpAndForward = LeftAndUp | Forward, 									/// <summary>Left and Up and Forward Orientation.</summary>
	LeftAndUpAndBack = LeftAndUp | Back, 										/// <summary>Left and Up and Back.</summary>
	LeftAndDownAndForward = LeftAndDown | Forward, 								/// <summary>Left and Down and Forward Orientation.</summary>
	LeftAndDownAndBack = LeftAndDown | Back, 									/// <summary>Left and Down and Back.</summary>
	RightAndUpAndForward = RightAndUp | Forward, 								/// <summary>Right and Up and Forward Orientation.</summary>
	RightAndUpAndBack = RightAndUp | Back, 										/// <summary>Right and Up and Back.</summary>
	RightAndDownAndForward = RightAndDown | Forward, 							/// <summary>Right and Down and Forward Orientation.</summary>
	RightAndDownAndBack = RightAndDown | Back 									/// <summary>Right and Down and Back.</summary>
}

[Flags]
public enum TransformProperties 												/// <summary>Transform's Components.</summary>
{
	None = 0, 																	/// <summary>No Component.</summary>
	Position = 1, 																/// <summary>Position's Component.</summary>
	Rotation = 2, 																/// <summary>Rotation's Component.</summary>
	Scale = 4, 																	/// <summary>Scale's Component.</summary>

	PositionAndRotation = Position | Rotation, 									/// <summary>Position and Rotation Components.</summary>
	PositionAndScale = Position | Scale, 										/// <summary>Position and Scale Components.</summary>
	RotationAndScale = Rotation | Scale, 										/// <summary>Rotation and Scale Components.</summary>
	All = Position | Rotation | Scale 											/// <summary>Position, Rotation and Scale Components.</summary>
}

[Flags]
public enum SceneChangeEvents 													/// <summary>List of all possible scene changes' events.</summary>
{
	None = 0, 																	/// <summary>No scene change event.</summary>
	SceneUnloaded = 1, 															/// <summary>Scene Unloaded event.</summary>
	SceneLoaded = 2, 															/// <summary>Scene Loaded event.</summary>
	ActiveSceneChanged = 4, 													/// <summary>Active Scene Changed event.</summary>

	SceneUnloadedAndSceneLoaded = SceneUnloaded | SceneLoaded, 					/// <summary>Scene Unloaded and Scene Loaded events.</summary>
	SceneUnloadedAndActiveSceneChanged = SceneUnloaded | ActiveSceneChanged, 	/// <summary>Scene Unloaded and Active Scene Changed events.</summary>
	SceneLoadedAndActiveSceneChanged = SceneLoaded | ActiveSceneChanged, 		/// <summary>Scene Loaded and Active Scene Changed events.</summary>
	All = SceneUnloaded | SceneLoaded | ActiveSceneChanged 						/// <summary>All Scene Change events.</summary>
}

public static class VEnums
{

#region AgnosticEnumOperations:
	/// <summary>Gets the number of active flags on a given int.</summary>
	/// <param name="_enumFlag">Enum Flag to count active [1] bits.</param>
	/// <returns>Number of active bits on enum flag.</returns>
	public static int GetActiveFlagsCount(int _enumFlag)
	{
		int count = 0;

		while(_enumFlag > 0)
		{
			_enumFlag &= (_enumFlag - 1);
			count++;
		}

		return count;
	}

	/// <summary>Checks if int enumerator contains flag.</summary>
	/// <param name='_enumFlag'>Enumerator Flag to make flag operation.</param>
	/// <param name='_flag'>Requested flag to check.</param>
	/// <returns>True if the int enumerator contains flag.</returns>
	public static bool HasFlag(int _enumFlag, int _flag){ return ((_enumFlag & _flag) == _flag); }
	
	/// <summary>Checks if int enumerator contains flags.</summary>
	/// <param name='_enumFlag'>Enumerator Flag to make flags operation.</param>
	/// <param name='_flags'>Requested flags to check.</param>
	/// <returns>True if the int enumerator contains all flags.</returns>
	public static bool HasFlags(int _enumFlag, params int[] _flags)
	{
		for(int i = 0; i < _flags.Length; i++)
		{
			if(!((_enumFlag & _flags[i]) == _flags[i])) return false;
		}
	
		return true;
	}
	
	/// <summary>Adds Flag [if there is not] to int enumerator.</summary>
	/// <param name='_enumFlag'>Enumerator Flag to make flag operation.</param>
	/// <param name='_flag'>Flag to add to the int enumerator.</param>
	public static void AddFlag(ref int _enumFlag, int _flag){ if(!HasFlag(_enumFlag, _flag)) _enumFlag |= _flag; }
	
	/// <summary>Adds Flags [if there are not] to int enumerator.</summary>
	/// <param name='_enumFlag'>Enumerator Flag to make flag operation.</param>
	/// <param name='_flag'>Flags to add to the int enumerator.</param>
	public static void AddFlags(ref int _enumFlag, params int[] _flags){ for(int i = 0; i < _flags.Length; i++) if(!HasFlag(_enumFlag, _flags[i])) _enumFlag |= _flags[i]; }
	
	/// <summary>Removes flag from int enumerator, if it has it.</summary>
	/// <param name='_enumFlag'>Enumerator Flag to make flag operation.</param>
	/// <param name='_flag'>Flag to remove from int enumerator.</param>
	public static void RemoveFlag(ref int _enumFlag, int _flag){ if(HasFlag(_enumFlag, _flag)) _enumFlag ^= _flag; }
	
	/// <summary>Removes flags from int enumerator, if it has it.</summary>
	/// <param name='_enumFlag'>Enumerator Flag to make flags operation.</param>
	/// <param name='_flags'>Flags to remove from int enumerator.</param>
	public static void RemoveFlags(ref int _enumFlag, params int[] _flags){ for(int i = 0; i < _flags.Length; i++) if(HasFlag(_enumFlag, _flags[i])) _enumFlag ^= _flags[i]; }
	
	/// <summary>Toggles flag from int enumerator, if it has it.</summary>
	/// <param name='_enumFlag'>Enumerator Flag to make flag operation.</param>
	/// <param name='_flag'>Flag to toggle from int enumerator.</param>
	public static void ToggleFlag(ref int _enumFlag, int _flag){ _enumFlag ^= _flag; }
	
	/// <summary>Toggles flags from int enumerator, if it has it.</summary>
	/// <param name='_enumFlag'>Enumerator Flag to make flags operation.</param>
	/// <param name='_flags'>Flags to toggle from int enumerator.</param>
	public static void ToggleFlags(ref int _enumFlag, params int[] _flags){ for(int i = 0; i < _flags.Length; i++) _enumFlag ^= _flags[i]; }
	
	/// <summary>Removes all int enumerator's flags, leaving all its bits to '0', and on its default value.</summary>
	/// <param name='_enumFlag'>Enumerator Flag to make flags operation.</param>
	public static void RemoveAllFlags(ref int _enumFlag){ _enumFlag = (int)0; }
#endregion

	/// <summary>Converts XBoxInputKey enumerator value to KeyCode value.</summary>
	/// <param name="_XBoxInputKey">XBoxInputKey value.</param>
	/// <returns>XBoxInputKey value to KeyCode, mapped relative to the platform.</returns>
	public static KeyCode ToKeyCode(this XBoxInputKey _XBoxInputKey)
	{
		switch(_XBoxInputKey)
		{
#if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || UNITY_XBOXONE || UNITY_WSA_10_0 || UNITY_WSA)
			case XBoxInputKey.A: 				return KeyCode.JoystickButton0;
			case XBoxInputKey.B: 				return KeyCode.JoystickButton1;
			case XBoxInputKey.X: 				return KeyCode.JoystickButton2;
			case XBoxInputKey.Y: 				return KeyCode.JoystickButton3;
			case XBoxInputKey.LB: 				return KeyCode.JoystickButton4;
			case XBoxInputKey.RB: 				return KeyCode.JoystickButton5;
			case XBoxInputKey.Back: 			return KeyCode.JoystickButton6;
			case XBoxInputKey.Start: 			return KeyCode.JoystickButton7;
			case XBoxInputKey.LeftStickClick: 	return KeyCode.JoystickButton8;
			case XBoxInputKey.RightStickClick: 	return KeyCode.JoystickButton9;
#elif (UNITY_STANDALONE_LINUX || UNITY_XBOXONE || UNITY_WSA_10_0 || UNITY_WSA)
			case XBoxInputKey.A: 				return KeyCode.JoystickButton0;
			case XBoxInputKey.B: 				return KeyCode.JoystickButton1;
			case XBoxInputKey.X: 				return KeyCode.JoystickButton2;
			case XBoxInputKey.Y: 				return KeyCode.JoystickButton3;
			case XBoxInputKey.LB: 				return KeyCode.JoystickButton4;
			case XBoxInputKey.RB: 				return KeyCode.JoystickButton5;
			case XBoxInputKey.Back: 			return KeyCode.JoystickButton6;
			case XBoxInputKey.Start: 			return KeyCode.JoystickButton7;
			case XBoxInputKey.LeftStickClick: 	return KeyCode.JoystickButton9;
			case XBoxInputKey.RightStickClick: 	return KeyCode.JoystickButton10;
			case XBoxInputKey.DPadUp: 			return KeyCode.JoystickButton13;
			case XBoxInputKey.DPadDown: 		return KeyCode.JoystickButton14;
			case XBoxInputKey.DPadLeft: 		return KeyCode.JoystickButton11;
			case XBoxInputKey.DPadRight: 		return KeyCode.JoystickButton12;
#elif (UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX || UNITY_XBOXONE || UNITY_WSA_10_0 || UNITY_WSA)
			case XBoxInputKey.A: 				return KeyCode.JoystickButton16;
			case XBoxInputKey.B: 				return KeyCode.JoystickButton17;
			case XBoxInputKey.X: 				return KeyCode.JoystickButton18;
			case XBoxInputKey.Y: 				return KeyCode.JoystickButton19;
			case XBoxInputKey.LB: 				return KeyCode.JoystickButton13;
			case XBoxInputKey.RB: 				return KeyCode.JoystickButton14;
			case XBoxInputKey.Back: 			return KeyCode.JoystickButton10;
			case XBoxInputKey.Start: 			return KeyCode.JoystickButton9;
			case XBoxInputKey.LeftStickClick: 	return KeyCode.JoystickButton11;
			case XBoxInputKey.RightStickClick: 	return KeyCode.JoystickButton12;
			case XBoxInputKey.DPadUp: 			return KeyCode.JoystickButton5;
			case XBoxInputKey.DPadDown: 		return KeyCode.JoystickButton6;
			case XBoxInputKey.DPadLeft: 		return KeyCode.JoystickButton7;
			case XBoxInputKey.DPadRight: 		return KeyCode.JoystickButton8;
			case XBoxInputKey.XBoxButton: 		return KeyCode.JoystickButton15;
#endif
            }

            return KeyCode.None;
	}

	/// <summary>Converts NintendoSwitchButton enumerator value to KeyCode value.</summary>
	/// <param name="_NintendoSwitchButton">NintendoSwitchButton value.</param>
	/// <returns>NintendoSwitchButton value to KeyCode, mapped relative to the platform.</returns>
	public static KeyCode ToKeyCode(NintendoSwitchButton _NintendoSwitchButton)
	{
		/*switch(_NintendoSwitchButton)
		{
			case NintendoSwitchButton.B: 				return KeyCode.JoystickButton0;
			case NintendoSwitchButton.A: 				return KeyCode.JoystickButton1;
			case NintendoSwitchButton.Y: 				return KeyCode.JoystickButton2;
			case NintendoSwitchButton.X: 				return KeyCode.JoystickButton3;
			case NintendoSwitchButton.L: 				return KeyCode.JoystickButton4;
			case NintendoSwitchButton.R: 				return KeyCode.JoystickButton5;
			case NintendoSwitchButton.Minus: 			return KeyCode.JoystickButton6;
			case NintendoSwitchButton.Plus: 			return KeyCode.JoystickButton7;
			case NintendoSwitchButton.LeftStick: 		return KeyCode.JoystickButton8;
			case NintendoSwitchButton.RightStick: 	return KeyCode.JoystickButton9;
			case NintendoSwitchButton.ZL: 			return KeyCode.JoystickButton10;
			case NintendoSwitchButton.ZR: 			return KeyCode.JoystickButton11;
			case NintendoSwitchButton.Down: 			return KeyCode.JoystickButton12;
			case NintendoSwitchButton.Right: 			return KeyCode.JoystickButton13;
			case NintendoSwitchButton.Left: 			return KeyCode.JoystickButton14;
			case NintendoSwitchButton.Up: 			return KeyCode.JoystickButton15;
		}*/

		return KeyCode.None;
	}

#region AbilityStatesFlagsOperations:
	/// <summary>Checks if AbilityState enumerator contains flag.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Requested flag to check.</param>
	/// <returns>True if the AbilityState enumerator contains flag.</returns>
	public static bool HasFlag(this AbilityState _enum, AbilityState _flag){ return ((_enum & _flag) == _flag); }
	
	/// <summary>Checks if AbilityState enumerator contains flags.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Requested flags to check.</param>
	/// <returns>True if the AbilityState enumerator contains all flags.</returns>
	public static bool HasFlags(this AbilityState _enum, params AbilityState[] _flags)
	{
		for(int i = 0; i < _flags.Length; i++)
		{
			if(!((_enum & _flags[i]) == _flags[i])) return false;
		}
	
		return true;
	}
	
	/// <summary>Adds Flag [if there is not] to AbilityState enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to add to the AbilityState enumerator.</param>
	public static void AddFlag(ref AbilityState _enum, AbilityState _flag){ if(!_enum.HasFlag(_flag)) _enum |= _flag; }
	
	/// <summary>Adds Flags [if there are not] to AbilityState enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flags to add to the AbilityState enumerator.</param>
	public static void AddFlags(ref AbilityState _enum, params AbilityState[] _flags){ for(int i = 0; i < _flags.Length; i++) if(!_enum.HasFlag(_flags[i])) _enum |= _flags[i]; }
	
	/// <summary>Removes flag from AbilityState enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to remove from AbilityState enumerator.</param>
	public static void RemoveFlag(ref AbilityState _enum, AbilityState _flag){ if(_enum.HasFlag(_flag)) _enum ^= _flag; }
	
	/// <summary>Removes flags from AbilityState enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to remove from AbilityState enumerator.</param>
	public static void RemoveFlags(ref AbilityState _enum, params AbilityState[] _flags){ for(int i = 0; i < _flags.Length; i++) if(_enum.HasFlag(_flags[i])) _enum ^= _flags[i]; }
	
	/// <summary>Toggles flag from AbilityState enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to toggle from AbilityState enumerator.</param>
	public static void ToggleFlag(ref AbilityState _enum, AbilityState _flag){ _enum ^= _flag; }
	
	/// <summary>Toggles flags from AbilityState enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to toggle from AbilityState enumerator.</param>
	public static void ToggleFlags(ref AbilityState _enum, params AbilityState[] _flags){ for(int i = 0; i < _flags.Length; i++) _enum ^= _flags[i]; }
	
	/// <summary>Removes all AbilityState enumerator's flags, leaving all its bits to '0', and on its default value.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	public static void RemoveAllFlags(ref AbilityState _enum){ _enum = (AbilityState)0; }
#endregion

#region Axes2DFlagsOperations:
	/// <summary>Checks if Axes2D enumerator contains flag.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Requested flag to check.</param>
	/// <returns>True if the Axes2D enumerator contains flag.</returns>
	public static bool HasFlag(this Axes2D _enum, Axes2D _flag){ return ((_enum & _flag) == _flag); }
	
	/// <summary>Checks if Axes2D enumerator contains flags.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Requested flags to check.</param>
	/// <returns>True if the Axes2D enumerator contains all flags.</returns>
	public static bool HasFlags(this Axes2D _enum, params Axes2D[] _flags)
	{
		for(int i = 0; i < _flags.Length; i++)
		{
			if(!((_enum & _flags[i]) == _flags[i])) return false;
		}
	
		return true;
	}
	
	/// <summary>Adds Flag [if there is not] to Axes2D enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to add to the Axes2D enumerator.</param>
	public static void AddFlag(ref Axes2D _enum, Axes2D _flag){ if(!_enum.HasFlag(_flag)) _enum |= _flag; }
	
	/// <summary>Adds Flags [if there are not] to Axes2D enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flags to add to the Axes2D enumerator.</param>
	public static void AddFlags(ref Axes2D _enum, params Axes2D[] _flags){ for(int i = 0; i < _flags.Length; i++) if(!_enum.HasFlag(_flags[i])) _enum |= _flags[i]; }
	
	/// <summary>Removes flag from Axes2D enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to remove from Axes2D enumerator.</param>
	public static void RemoveFlag(ref Axes2D _enum, Axes2D _flag){ if(_enum.HasFlag(_flag)) _enum ^= _flag; }
	
	/// <summary>Removes flags from Axes2D enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to remove from Axes2D enumerator.</param>
	public static void RemoveFlags(ref Axes2D _enum, params Axes2D[] _flags){ for(int i = 0; i < _flags.Length; i++) if(_enum.HasFlag(_flags[i])) _enum ^= _flags[i]; }
	
	/// <summary>Toggles flag from Axes2D enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to toggle from Axes2D enumerator.</param>
	public static void ToggleFlag(ref Axes2D _enum, Axes2D _flag){ _enum ^= _flag; }
	
	/// <summary>Toggles flags from Axes2D enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to toggle from Axes2D enumerator.</param>
	public static void ToggleFlags(ref Axes2D _enum, params Axes2D[] _flags){ for(int i = 0; i < _flags.Length; i++) _enum ^= _flags[i]; }
	
	/// <summary>Removes all Axes2D enumerator's flags, leaving all its bits to '0', and on its default value.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	public static void RemoveAllFlags(ref Axes2D _enum){ _enum = (Axes2D)0; }
#endregion

#region Axes3DFlagsOperations:
	/// <summary>Checks if Axes3D enumerator contains flag.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Requested flag to check.</param>
	/// <returns>True if the Axes3D enumerator contains flag.</returns>
	public static bool HasFlag(this Axes3D _enum, Axes3D _flag){ return ((_enum & _flag) == _flag); }
	
	/// <summary>Checks if Axes3D enumerator contains flags.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Requested flags to check.</param>
	/// <returns>True if the Axes3D enumerator contains all flags.</returns>
	public static bool HasFlags(this Axes3D _enum, params Axes3D[] _flags)
	{
		for(int i = 0; i < _flags.Length; i++)
		{
			if(!((_enum & _flags[i]) == _flags[i])) return false;
		}
	
		return true;
	}
	
	/// <summary>Adds Flag [if there is not] to Axes3D enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to add to the Axes3D enumerator.</param>
	public static void AddFlag(ref Axes3D _enum, Axes3D _flag){ if(!_enum.HasFlag(_flag)) _enum |= _flag; }
	
	/// <summary>Adds Flags [if there are not] to Axes3D enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flags to add to the Axes3D enumerator.</param>
	public static void AddFlags(ref Axes3D _enum, params Axes3D[] _flags){ for(int i = 0; i < _flags.Length; i++) if(!_enum.HasFlag(_flags[i])) _enum |= _flags[i]; }
	
	/// <summary>Removes flag from Axes3D enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to remove from Axes3D enumerator.</param>
	public static void RemoveFlag(ref Axes3D _enum, Axes3D _flag){ if(_enum.HasFlag(_flag)) _enum ^= _flag; }
	
	/// <summary>Removes flags from Axes3D enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to remove from Axes3D enumerator.</param>
	public static void RemoveFlags(ref Axes3D _enum, params Axes3D[] _flags){ for(int i = 0; i < _flags.Length; i++) if(_enum.HasFlag(_flags[i])) _enum ^= _flags[i]; }
	
	/// <summary>Toggles flag from Axes3D enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to toggle from Axes3D enumerator.</param>
	public static void ToggleFlag(ref Axes3D _enum, Axes3D _flag){ _enum ^= _flag; }
	
	/// <summary>Toggles flags from Axes3D enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to toggle from Axes3D enumerator.</param>
	public static void ToggleFlags(ref Axes3D _enum, params Axes3D[] _flags){ for(int i = 0; i < _flags.Length; i++) _enum ^= _flags[i]; }
	
	/// <summary>Removes all Axes3D enumerator's flags, leaving all its bits to '0', and on its default value.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	public static void RemoveAllFlags(ref Axes3D _enum){ _enum = (Axes3D)0; }
#endregion

#region DetectableControllersFlagsOperations:
	/// <summary>Checks if DetectableControllers enumerator contains flag.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Requested flag to check.</param>
	/// <returns>True if the DetectableControllers enumerator contains flag.</returns>
	public static bool HasFlag(this DetectableControllers _enum, DetectableControllers _flag){ return ((_enum & _flag) == _flag); }
	
	/// <summary>Checks if DetectableControllers enumerator contains flags.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Requested flags to check.</param>
	/// <returns>True if the DetectableControllers enumerator contains all flags.</returns>
	public static bool HasFlags(this DetectableControllers _enum, params DetectableControllers[] _flags)
	{
		for(int i = 0; i < _flags.Length; i++)
		{
			if(!((_enum & _flags[i]) == _flags[i])) return false;
		}
	
		return true;
	}
	
	/// <summary>Adds Flag [if there is not] to DetectableControllers enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to add to the DetectableControllers enumerator.</param>
	public static void AddFlag(ref DetectableControllers _enum, DetectableControllers _flag){ if(!_enum.HasFlag(_flag)) _enum |= _flag; }
	
	/// <summary>Adds Flags [if there are not] to DetectableControllers enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flags to add to the DetectableControllers enumerator.</param>
	public static void AddFlags(ref DetectableControllers _enum, params DetectableControllers[] _flags){ for(int i = 0; i < _flags.Length; i++) if(!_enum.HasFlag(_flags[i])) _enum |= _flags[i]; }
	
	/// <summary>Removes flag from DetectableControllers enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to remove from DetectableControllers enumerator.</param>
	public static void RemoveFlag(ref DetectableControllers _enum, DetectableControllers _flag){ if(_enum.HasFlag(_flag)) _enum ^= _flag; }
	
	/// <summary>Removes flags from DetectableControllers enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to remove from DetectableControllers enumerator.</param>
	public static void RemoveFlags(ref DetectableControllers _enum, params DetectableControllers[] _flags){ for(int i = 0; i < _flags.Length; i++) if(_enum.HasFlag(_flags[i])) _enum ^= _flags[i]; }
	
	/// <summary>Toggles flag from DetectableControllers enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to toggle from DetectableControllers enumerator.</param>
	public static void ToggleFlag(ref DetectableControllers _enum, DetectableControllers _flag){ _enum ^= _flag; }
	
	/// <summary>Toggles flags from DetectableControllers enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to toggle from DetectableControllers enumerator.</param>
	public static void ToggleFlags(ref DetectableControllers _enum, params DetectableControllers[] _flags){ for(int i = 0; i < _flags.Length; i++) _enum ^= _flags[i]; }
	
	/// <summary>Removes all DetectableControllers enumerator's flags, leaving all its bits to '0', and on its default value.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	public static void RemoveAllFlags(ref DetectableControllers _enum){ _enum = (DetectableControllers)0; }
#endregion

#region FSMStatesFlagsOperations:
	/// <summary>Checks if FSMStates enumerator contains flag.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Requested flag to check.</param>
	/// <returns>True if the FSMStates enumerator contains flag.</returns>
	public static bool HasFlag(this FSMStates _enum, FSMStates _flag){ return ((_enum & _flag) == _flag); }
	
	/// <summary>Checks if FSMStates enumerator contains flags.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Requested flags to check.</param>
	/// <returns>True if the FSMStates enumerator contains all flags.</returns>
	public static bool HasFlags(this FSMStates _enum, params FSMStates[] _flags)
	{
		for(int i = 0; i < _flags.Length; i++)
		{
			if(!((_enum & _flags[i]) == _flags[i])) return false;
		}
	
		return true;
	}
	
	/// <summary>Adds Flag [if there is not] to FSMStates enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to add to the FSMStates enumerator.</param>
	public static void AddFlag(ref FSMStates _enum, FSMStates _flag){ if(!_enum.HasFlag(_flag)) _enum |= _flag; }
	
	/// <summary>Adds Flags [if there are not] to FSMStates enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flags to add to the FSMStates enumerator.</param>
	public static void AddFlags(ref FSMStates _enum, params FSMStates[] _flags){ for(int i = 0; i < _flags.Length; i++) if(!_enum.HasFlag(_flags[i])) _enum |= _flags[i]; }
	
	/// <summary>Removes flag from FSMStates enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to remove from FSMStates enumerator.</param>
	public static void RemoveFlag(ref FSMStates _enum, FSMStates _flag){ if(_enum.HasFlag(_flag)) _enum ^= _flag; }
	
	/// <summary>Removes flags from FSMStates enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to remove from FSMStates enumerator.</param>
	public static void RemoveFlags(ref FSMStates _enum, params FSMStates[] _flags){ for(int i = 0; i < _flags.Length; i++) if(_enum.HasFlag(_flags[i])) _enum ^= _flags[i]; }
	
	/// <summary>Toggles flag from FSMStates enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to toggle from FSMStates enumerator.</param>
	public static void ToggleFlag(ref FSMStates _enum, FSMStates _flag){ _enum ^= _flag; }
	
	/// <summary>Toggles flags from FSMStates enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to toggle from FSMStates enumerator.</param>
	public static void ToggleFlags(ref FSMStates _enum, params FSMStates[] _flags){ for(int i = 0; i < _flags.Length; i++) _enum ^= _flags[i]; }
	
	/// <summary>Removes all FSMStates enumerator's flags, leaving all its bits to '0', and on its default value.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	public static void RemoveAllFlags(ref FSMStates _enum){ _enum = (FSMStates)0; }
#endregion

#region HitColliderEventTypesFlagsOperations:
	/// <summary>Checks if HitColliderEventTypes enumerator contains flag.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Requested flag to check.</param>
	/// <returns>True if the HitColliderEventTypes enumerator contains flag.</returns>
	public static bool HasFlag(this HitColliderEventTypes _enum, HitColliderEventTypes _flag){ return ((_enum & _flag) == _flag); }
	
	/// <summary>Checks if HitColliderEventTypes enumerator contains flags.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Requested flags to check.</param>
	/// <returns>True if the HitColliderEventTypes enumerator contains all flags.</returns>
	public static bool HasFlags(this HitColliderEventTypes _enum, params HitColliderEventTypes[] _flags)
	{
		for(int i = 0; i < _flags.Length; i++)
		{
			if(!((_enum & _flags[i]) == _flags[i])) return false;
		}
	
		return true;
	}
	
	/// <summary>Adds Flag [if there is not] to HitColliderEventTypes enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to add to the HitColliderEventTypes enumerator.</param>
	public static void AddFlag(ref HitColliderEventTypes _enum, HitColliderEventTypes _flag){ if(!_enum.HasFlag(_flag)) _enum |= _flag; }
	
	/// <summary>Adds Flags [if there are not] to HitColliderEventTypes enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flags to add to the HitColliderEventTypes enumerator.</param>
	public static void AddFlags(ref HitColliderEventTypes _enum, params HitColliderEventTypes[] _flags){ for(int i = 0; i < _flags.Length; i++) if(!_enum.HasFlag(_flags[i])) _enum |= _flags[i]; }
	
	/// <summary>Removes flag from HitColliderEventTypes enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to remove from HitColliderEventTypes enumerator.</param>
	public static void RemoveFlag(ref HitColliderEventTypes _enum, HitColliderEventTypes _flag){ if(_enum.HasFlag(_flag)) _enum ^= _flag; }
	
	/// <summary>Removes flags from HitColliderEventTypes enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to remove from HitColliderEventTypes enumerator.</param>
	public static void RemoveFlags(ref HitColliderEventTypes _enum, params HitColliderEventTypes[] _flags){ for(int i = 0; i < _flags.Length; i++) if(_enum.HasFlag(_flags[i])) _enum ^= _flags[i]; }
	
	/// <summary>Toggles flag from HitColliderEventTypes enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to toggle from HitColliderEventTypes enumerator.</param>
	public static void ToggleFlag(ref HitColliderEventTypes _enum, HitColliderEventTypes _flag){ _enum ^= _flag; }
	
	/// <summary>Toggles flags from HitColliderEventTypes enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to toggle from HitColliderEventTypes enumerator.</param>
	public static void ToggleFlags(ref HitColliderEventTypes _enum, params HitColliderEventTypes[] _flags){ for(int i = 0; i < _flags.Length; i++) _enum ^= _flags[i]; }
	
	/// <summary>Removes all HitColliderEventTypes enumerator's flags, leaving all its bits to '0', and on its default value.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	public static void RemoveAllFlags(ref HitColliderEventTypes _enum){ _enum = (HitColliderEventTypes)0; }
#endregion

#region InputEvaluationsFlagsOperations:
	/// <summary>Checks if InputEvaluations enumerator contains flag.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Requested flag to check.</param>
	/// <returns>True if the InputEvaluations enumerator contains flag.</returns>
	public static bool HasFlag(this InputEvaluations _enum, InputEvaluations _flag){ return ((_enum & _flag) == _flag); }
	
	/// <summary>Checks if InputEvaluations enumerator contains flags.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Requested flags to check.</param>
	/// <returns>True if the InputEvaluations enumerator contains all flags.</returns>
	public static bool HasFlags(this InputEvaluations _enum, params InputEvaluations[] _flags)
	{
		for(int i = 0; i < _flags.Length; i++)
		{
			if(!((_enum & _flags[i]) == _flags[i])) return false;
		}
	
		return true;
	}
	
	/// <summary>Adds Flag [if there is not] to InputEvaluations enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to add to the InputEvaluations enumerator.</param>
	public static void AddFlag(ref InputEvaluations _enum, InputEvaluations _flag){ if(!_enum.HasFlag(_flag)) _enum |= _flag; }
	
	/// <summary>Adds Flags [if there are not] to InputEvaluations enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flags to add to the InputEvaluations enumerator.</param>
	public static void AddFlags(ref InputEvaluations _enum, params InputEvaluations[] _flags){ for(int i = 0; i < _flags.Length; i++) if(!_enum.HasFlag(_flags[i])) _enum |= _flags[i]; }
	
	/// <summary>Removes flag from InputEvaluations enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to remove from InputEvaluations enumerator.</param>
	public static void RemoveFlag(ref InputEvaluations _enum, InputEvaluations _flag){ if(_enum.HasFlag(_flag)) _enum ^= _flag; }
	
	/// <summary>Removes flags from InputEvaluations enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to remove from InputEvaluations enumerator.</param>
	public static void RemoveFlags(ref InputEvaluations _enum, params InputEvaluations[] _flags){ for(int i = 0; i < _flags.Length; i++) if(_enum.HasFlag(_flags[i])) _enum ^= _flags[i]; }
	
	/// <summary>Toggles flag from InputEvaluations enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to toggle from InputEvaluations enumerator.</param>
	public static void ToggleFlag(ref InputEvaluations _enum, InputEvaluations _flag){ _enum ^= _flag; }
	
	/// <summary>Toggles flags from InputEvaluations enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to toggle from InputEvaluations enumerator.</param>
	public static void ToggleFlags(ref InputEvaluations _enum, params InputEvaluations[] _flags){ for(int i = 0; i < _flags.Length; i++) _enum ^= _flags[i]; }
	
	/// <summary>Removes all InputEvaluations enumerator's flags, leaving all its bits to '0', and on its default value.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	public static void RemoveAllFlags(ref InputEvaluations _enum){ _enum = (InputEvaluations)0; }
#endregion

#region OrientationSemanticsFlagsOperations:
	/// <summary>Checks if OrientationSemantics enumerator contains flag.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Requested flag to check.</param>
	/// <returns>True if the OrientationSemantics enumerator contains flag.</returns>
	public static bool HasFlag(this OrientationSemantics _enum, OrientationSemantics _flag){ return ((_enum & _flag) == _flag); }

	/// <summary>Checks if OrientationSemantics enumerator contains flags.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Requested flags to check.</param>
	/// <returns>True if the OrientationSemantics enumerator contains all flags.</returns>
	public static bool HasFlags(this OrientationSemantics _enum, params OrientationSemantics[] _flags)
	{
		for(int i = 0; i < _flags.Length; i++)
		{
			if(!((_enum & _flags[i]) == _flags[i])) return false;
		}

		return true;
	}

	/// <summary>Adds Flag [if there is not] to OrientationSemantics enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to add to the OrientationSemantics enumerator.</param>
	public static void AddFlag(ref OrientationSemantics _enum, OrientationSemantics _flag){ if(!_enum.HasFlag(_flag)) _enum |= _flag; }

	/// <summary>Adds Flags [if there are not] to OrientationSemantics enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flags to add to the OrientationSemantics enumerator.</param>
	public static void AddFlags(ref OrientationSemantics _enum, params OrientationSemantics[] _flags){ for(int i = 0; i < _flags.Length; i++) if(!_enum.HasFlag(_flags[i])) _enum |= _flags[i]; }

	/// <summary>Removes flag from OrientationSemantics enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to remove from OrientationSemantics enumerator.</param>
	public static void RemoveFlag(ref OrientationSemantics _enum, OrientationSemantics _flag){ if(_enum.HasFlag(_flag)) _enum ^= _flag; }

	/// <summary>Removes flags from OrientationSemantics enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to remove from OrientationSemantics enumerator.</param>
	public static void RemoveFlags(ref OrientationSemantics _enum, params OrientationSemantics[] _flags){ for(int i = 0; i < _flags.Length; i++) if(_enum.HasFlag(_flags[i])) _enum ^= _flags[i]; }

	/// <summary>Toggles flag from OrientationSemantics enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to toggle from OrientationSemantics enumerator.</param>
	public static void ToggleFlag(ref OrientationSemantics _enum, OrientationSemantics _flag){ _enum ^= _flag; }

	/// <summary>Toggles flags from OrientationSemantics enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to toggle from OrientationSemantics enumerator.</param>
	public static void ToggleFlags(ref OrientationSemantics _enum, params OrientationSemantics[] _flags){ for(int i = 0; i < _flags.Length; i++) _enum ^= _flags[i]; }

	/// <summary>Removes all OrientationSemantics enumerator's flags, leaving all its bits to '0', and on its default value.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	public static void RemoveAllFlags(ref OrientationSemantics _enum){ _enum = (OrientationSemantics)0; }
#endregion

#region TransformPropertiesFlagsOperations:
	/// <summary>Checks if TransformProperties enumerator contains flag.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Requested flag to check.</param>
	/// <returns>True if the TransformProperties enumerator contains flag.</returns>
	public static bool HasFlag(this TransformProperties _enum, TransformProperties _flag){ return ((_enum & _flag) == _flag); }
	
	/// <summary>Checks if TransformProperties enumerator contains flags.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Requested flags to check.</param>
	/// <returns>True if the TransformProperties enumerator contains all flags.</returns>
	public static bool HasFlags(this TransformProperties _enum, params TransformProperties[] _flags)
	{
		for(int i = 0; i < _flags.Length; i++)
		{
			if(!((_enum & _flags[i]) == _flags[i])) return false;
		}
	
		return true;
	}
	
	/// <summary>Adds Flag [if there is not] to TransformProperties enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to add to the TransformProperties enumerator.</param>
	public static void AddFlag(ref TransformProperties _enum, TransformProperties _flag){ if(!_enum.HasFlag(_flag)) _enum |= _flag; }
	
	/// <summary>Adds Flags [if there are not] to TransformProperties enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flags to add to the TransformProperties enumerator.</param>
	public static void AddFlags(ref TransformProperties _enum, params TransformProperties[] _flags){ for(int i = 0; i < _flags.Length; i++) if(!_enum.HasFlag(_flags[i])) _enum |= _flags[i]; }
	
	/// <summary>Removes flag from TransformProperties enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to remove from TransformProperties enumerator.</param>
	public static void RemoveFlag(ref TransformProperties _enum, TransformProperties _flag){ if(_enum.HasFlag(_flag)) _enum ^= _flag; }
	
	/// <summary>Removes flags from TransformProperties enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to remove from TransformProperties enumerator.</param>
	public static void RemoveFlags(ref TransformProperties _enum, params TransformProperties[] _flags){ for(int i = 0; i < _flags.Length; i++) if(_enum.HasFlag(_flags[i])) _enum ^= _flags[i]; }
	
	/// <summary>Toggles flag from TransformProperties enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to toggle from TransformProperties enumerator.</param>
	public static void ToggleFlag(ref TransformProperties _enum, TransformProperties _flag){ _enum ^= _flag; }
	
	/// <summary>Toggles flags from TransformProperties enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to toggle from TransformProperties enumerator.</param>
	public static void ToggleFlags(ref TransformProperties _enum, params TransformProperties[] _flags){ for(int i = 0; i < _flags.Length; i++) _enum ^= _flags[i]; }
	
	/// <summary>Removes all TransformProperties enumerator's flags, leaving all its bits to '0', and on its default value.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	public static void RemoveAllFlags(ref TransformProperties _enum){ _enum = (TransformProperties)0; }
#endregion

#region SceneChangeEventsFlagsOperations:
	/// <summary>Checks if SceneChangeEvents enumerator contains flag.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Requested flag to check.</param>
	/// <returns>True if the SceneChangeEvents enumerator contains flag.</returns>
	public static bool HasFlag(this SceneChangeEvents _enum, SceneChangeEvents _flag){ return ((_enum & _flag) == _flag); }
	
	/// <summary>Checks if SceneChangeEvents enumerator contains flags.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Requested flags to check.</param>
	/// <returns>True if the SceneChangeEvents enumerator contains all flags.</returns>
	public static bool HasFlags(this SceneChangeEvents _enum, params SceneChangeEvents[] _flags)
	{
		for(int i = 0; i < _flags.Length; i++)
		{
			if(!((_enum & _flags[i]) == _flags[i])) return false;
		}
	
		return true;
	}
	
	/// <summary>Adds Flag [if there is not] to SceneChangeEvents enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to add to the SceneChangeEvents enumerator.</param>
	public static void AddFlag(ref SceneChangeEvents _enum, SceneChangeEvents _flag){ if(!_enum.HasFlag(_flag)) _enum |= _flag; }
	
	/// <summary>Adds Flags [if there are not] to SceneChangeEvents enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flags to add to the SceneChangeEvents enumerator.</param>
	public static void AddFlags(ref SceneChangeEvents _enum, params SceneChangeEvents[] _flags){ for(int i = 0; i < _flags.Length; i++) if(!_enum.HasFlag(_flags[i])) _enum |= _flags[i]; }
	
	/// <summary>Removes flag from SceneChangeEvents enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to remove from SceneChangeEvents enumerator.</param>
	public static void RemoveFlag(ref SceneChangeEvents _enum, SceneChangeEvents _flag){ if(_enum.HasFlag(_flag)) _enum ^= _flag; }
	
	/// <summary>Removes flags from SceneChangeEvents enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to remove from SceneChangeEvents enumerator.</param>
	public static void RemoveFlags(ref SceneChangeEvents _enum, params SceneChangeEvents[] _flags){ for(int i = 0; i < _flags.Length; i++) if(_enum.HasFlag(_flags[i])) _enum ^= _flags[i]; }
	
	/// <summary>Toggles flag from SceneChangeEvents enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to toggle from SceneChangeEvents enumerator.</param>
	public static void ToggleFlag(ref SceneChangeEvents _enum, SceneChangeEvents _flag){ _enum ^= _flag; }
	
	/// <summary>Toggles flags from SceneChangeEvents enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to toggle from SceneChangeEvents enumerator.</param>
	public static void ToggleFlags(ref SceneChangeEvents _enum, params SceneChangeEvents[] _flags){ for(int i = 0; i < _flags.Length; i++) _enum ^= _flags[i]; }
	
	/// <summary>Removes all SceneChangeEvents enumerator's flags, leaving all its bits to '0', and on its default value.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	public static void RemoveAllFlags(ref SceneChangeEvents _enum){ _enum = (SceneChangeEvents)0; }
#endregion

}
}