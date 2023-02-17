using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_SWITCH
using nn.hid;
#endif

namespace Voidless
{
[Flags]
public enum NintendoSwitchButton
{
	None,
    A = 0x1 << 0,
    B = 0x1 << 1,
    X = 0x1 << 2,
    Y = 0x1 << 3,
    StickL = 0x1 << 4,
    StickR = 0x1 << 5,
    L = 0x1 << 6,
    R = 0x1 << 7,
    ZL = 0x1 << 8,
    ZR = 0x1 << 9,
    Plus = 0x1 << 10,
    Minus = 0x1 << 11,
    Left = 0x1 << 12,
    Up = 0x1 << 13,
    Right = 0x1 << 14,
    Down = 0x1 << 15,
    StickLLeft = 0x1 << 16,
    StickLUp = 0x1 << 17,
    StickLRight = 0x1 << 18,
    StickLDown = 0x1 << 19,
    StickRLeft = 0x1 << 20,
    StickRUp = 0x1 << 21,
    StickRRight = 0x1 << 22,
    StickRDown = 0x1 << 23,
    LeftSL = 0x1 << 24,
    LeftSR = 0x1 << 25,
    RightSL = 0x1 << 26,
    RightSR = 0x1 << 27,
}

public static class VNintendoSwitch
{
#if UNITY_SWITCH
	public static readonly NpadId[] NPAD_IDS_ALL; 					/// <summary>All Possible NpadIds [With the exception of Invalid].</summary>
	public static readonly NpadId[] NPAD_IDS_HANDHELD; 				/// <summary>Short-hand for Handheld mode only.</summary>
	public static readonly NpadId[] NPAD_IDS_HANDHELD_AND_JOYCON; 	/// <summary>NpadIds for single player on Hanheld or JoyCon.</summary>

	/// <summary>VNintendoSwitch's Static Constructor.</summary>
	static VNintendoSwitch()
	{
		NPAD_IDS_ALL = new NpadId[]
		{
			NpadId.No1,
			NpadId.No2,
			NpadId.No3,
			NpadId.No4,
			NpadId.No5,
			NpadId.No6,
			NpadId.No7,
			NpadId.No8,
			NpadId.Handheld
		};

		NPAD_IDS_HANDHELD = new NpadId[] { NpadId.Handheld };
		NPAD_IDS_HANDHELD_AND_JOYCON = new NpadId[] { NpadId.Handheld, NpadId.No1 };
	}

	/// <summary>Converts given enum from NintendoSwitchButton to NpadButton.</summary>
	/// <param name="_button">Enum to convert into NpadButton.</param>
	/// <returns>NintendoSwitchButton to NpadButton.</returns>
	public static NpadButton ToNpadButton(this NintendoSwitchButton _button)
	{
		return (NpadButton)(_button);
	}

	/// \TODO Decide a better
	/// <summary>Takes the NpadStyle from the given ID and return the most according mapping of given NintendoSwitchButton.</summary>
	/// <param name="_ID">Npad's ID.</param>
	/// <param name="_button">Button to re-map [if internally deemed necessary].</param>
	/// <returns>Re-mapped NintendoSwitchButton to proper NpadButton.</returns>
	public static NpadButton IDStyleToNpadButton(this NpadId _ID, NintendoSwitchButton _button)
	{
		NpadStyle style = Npad.GetStyleSet(_ID);
		NpadJoyHoldType holdType = NpadJoy.GetHoldType();

		switch(style)
		{
			case NpadStyle.None: 							return NpadButton.None;
	        case NpadStyle.FullKey: 
	        case NpadStyle.Handheld: 						return _button.ToNpadButton(); 
	        case NpadStyle.JoyDual: 						return _button.ToNpadButton();
	        case NpadStyle.JoyLeft:
	        switch(holdType)
	        {
	        	case NpadJoyHoldType.Horizontal:
	        	switch(_button)
	        	{
	        		case NintendoSwitchButton.None: 		return NpadButton.None;
				    case NintendoSwitchButton.A: 			return NpadButton.Down;
				    case NintendoSwitchButton.B: 			return NpadButton.Left;
				    case NintendoSwitchButton.X: 			return NpadButton.Right;
				    case NintendoSwitchButton.Y: 			return NpadButton.Up;
				    case NintendoSwitchButton.StickL: 		return NpadButton.StickL;
				    case NintendoSwitchButton.StickR: 		return NpadButton.None;
				    case NintendoSwitchButton.L: 			return NpadButton.LeftSL;
				    case NintendoSwitchButton.R: 			return NpadButton.LeftSR;
				    case NintendoSwitchButton.ZL: 			return NpadButton.ZL;
				    case NintendoSwitchButton.ZR: 			return NpadButton.None;
				    case NintendoSwitchButton.Plus: 		return NpadButton.Minus;
				    case NintendoSwitchButton.Minus: 		return NpadButton.None;
				    case NintendoSwitchButton.Left: 		return NpadButton.None;
				    case NintendoSwitchButton.Up: 			return NpadButton.None;
				    case NintendoSwitchButton.Right: 		return NpadButton.None;
				    case NintendoSwitchButton.Down: 		return NpadButton.None;
				    case NintendoSwitchButton.StickLLeft: 	return NpadButton.StickLUp;
				    case NintendoSwitchButton.StickLUp: 	return NpadButton.StickLRight;
				    case NintendoSwitchButton.StickLRight: 	return NpadButton.StickLDown;
				    case NintendoSwitchButton.StickLDown: 	return NpadButton.StickLLeft;
				    case NintendoSwitchButton.StickRLeft: 	return NpadButton.None;
				    case NintendoSwitchButton.StickRUp: 	return NpadButton.None;
				    case NintendoSwitchButton.StickRRight: 	return NpadButton.None;
				    case NintendoSwitchButton.StickRDown: 	return NpadButton.None;
				    case NintendoSwitchButton.LeftSL: 		return NpadButton.LeftSL;
				    case NintendoSwitchButton.LeftSR: 		return NpadButton.LeftSR;
				    case NintendoSwitchButton.RightSL: 		return NpadButton.RightSL;
				    case NintendoSwitchButton.RightSR: 		return NpadButton.RightSR;
	        	}
	        	break;

	        	case NpadJoyHoldType.Vertical:
	        	switch(_button)
	        	{
	        		case NintendoSwitchButton.None: 		return NpadButton.None;
				    case NintendoSwitchButton.A: 			return NpadButton.Right;
				    case NintendoSwitchButton.B: 			return NpadButton.Down;
				    case NintendoSwitchButton.X: 			return NpadButton.Up;
				    case NintendoSwitchButton.Y: 			return NpadButton.Left;
				    case NintendoSwitchButton.StickL: 		return NpadButton.StickL;
				    case NintendoSwitchButton.StickR: 		return NpadButton.None;
				    case NintendoSwitchButton.L: 			return NpadButton.L;
				    case NintendoSwitchButton.R: 			return NpadButton.LeftSR;
				    case NintendoSwitchButton.ZL: 			return NpadButton.ZL;
				    case NintendoSwitchButton.ZR: 			return NpadButton.None;
				    case NintendoSwitchButton.Plus: 		return NpadButton.Minus;
				    case NintendoSwitchButton.Minus: 		return NpadButton.None;
				    case NintendoSwitchButton.Left: 		return NpadButton.None;
				    case NintendoSwitchButton.Up: 			return NpadButton.None;
				    case NintendoSwitchButton.Right: 		return NpadButton.None;
				    case NintendoSwitchButton.Down: 		return NpadButton.None;
				    case NintendoSwitchButton.StickLLeft: 	return NpadButton.StickLLeft;
				    case NintendoSwitchButton.StickLUp: 	return NpadButton.StickLUp;
				    case NintendoSwitchButton.StickLRight: 	return NpadButton.StickLRight;
				    case NintendoSwitchButton.StickLDown: 	return NpadButton.StickLDown;
				    case NintendoSwitchButton.StickRLeft: 	return NpadButton.None;
				    case NintendoSwitchButton.StickRUp: 	return NpadButton.None;
				    case NintendoSwitchButton.StickRRight: 	return NpadButton.None;
				    case NintendoSwitchButton.StickRDown: 	return NpadButton.None;
				    case NintendoSwitchButton.LeftSL: 		return NpadButton.LeftSL;
				    case NintendoSwitchButton.LeftSR: 		return NpadButton.LeftSR;
				    case NintendoSwitchButton.RightSL: 		return NpadButton.RightSL;
				    case NintendoSwitchButton.RightSR: 		return NpadButton.RightSR;
	        	}
	        	break;
	        }
	        break;

	        case NpadStyle.JoyRight:
	        switch(holdType)
	        {
	        	case NpadJoyHoldType.Horizontal:
	        	switch(_button)
	        	{
	        		case NintendoSwitchButton.None: 		return NpadButton.None;
				    case NintendoSwitchButton.A: 			return NpadButton.X;
				    case NintendoSwitchButton.B: 			return NpadButton.A;
				    case NintendoSwitchButton.X: 			return NpadButton.Y;
				    case NintendoSwitchButton.Y: 			return NpadButton.B;
				    case NintendoSwitchButton.StickL: 		return NpadButton.StickR;
				    case NintendoSwitchButton.StickR: 		return NpadButton.None;
				    case NintendoSwitchButton.L: 			return NpadButton.LeftSL;
				    case NintendoSwitchButton.R: 			return NpadButton.LeftSR;
				    case NintendoSwitchButton.ZL: 			return NpadButton.ZL;
				    case NintendoSwitchButton.ZR: 			return NpadButton.None;
				    case NintendoSwitchButton.Plus: 		return NpadButton.Plus;
				    case NintendoSwitchButton.Minus: 		return NpadButton.None;
				    case NintendoSwitchButton.Left: 		return NpadButton.None;
				    case NintendoSwitchButton.Up: 			return NpadButton.None;
				    case NintendoSwitchButton.Right: 		return NpadButton.None;
				    case NintendoSwitchButton.Down: 		return NpadButton.None;
				    case NintendoSwitchButton.StickLLeft: 	return NpadButton.StickRDown;
				    case NintendoSwitchButton.StickLUp: 	return NpadButton.StickRLeft;
				    case NintendoSwitchButton.StickLRight: 	return NpadButton.StickRUp;
				    case NintendoSwitchButton.StickLDown: 	return NpadButton.StickRRight;
				    case NintendoSwitchButton.StickRLeft: 	return NpadButton.None;
				    case NintendoSwitchButton.StickRUp: 	return NpadButton.None;
				    case NintendoSwitchButton.StickRRight: 	return NpadButton.None;
				    case NintendoSwitchButton.StickRDown: 	return NpadButton.None;
				    case NintendoSwitchButton.LeftSL: 		return NpadButton.LeftSL;
				    case NintendoSwitchButton.LeftSR: 		return NpadButton.LeftSR;
				    case NintendoSwitchButton.RightSL: 		return NpadButton.RightSL;
				    case NintendoSwitchButton.RightSR: 		return NpadButton.RightSR;
	        	}
	        	break;

	        	case NpadJoyHoldType.Vertical:
	        	switch(_button)
	        	{
	        		case NintendoSwitchButton.None: 		return NpadButton.None;
				    case NintendoSwitchButton.A: 			return NpadButton.A;
				    case NintendoSwitchButton.B: 			return NpadButton.B;
				    case NintendoSwitchButton.X: 			return NpadButton.X;
				    case NintendoSwitchButton.Y: 			return NpadButton.Y;
				    case NintendoSwitchButton.StickL: 		return NpadButton.None;
				    case NintendoSwitchButton.StickR: 		return NpadButton.StickR;
				    case NintendoSwitchButton.L: 			return NpadButton.L;
				    case NintendoSwitchButton.R: 			return NpadButton.LeftSR;
				    case NintendoSwitchButton.ZL: 			return NpadButton.None;
				    case NintendoSwitchButton.ZR: 			return NpadButton.ZR;
				    case NintendoSwitchButton.Plus: 		return NpadButton.Plus;
				    case NintendoSwitchButton.Minus: 		return NpadButton.None;
				    case NintendoSwitchButton.Left: 		return NpadButton.None;
				    case NintendoSwitchButton.Up: 			return NpadButton.None;
				    case NintendoSwitchButton.Right: 		return NpadButton.None;
				    case NintendoSwitchButton.Down: 		return NpadButton.None;
				    case NintendoSwitchButton.StickLLeft: 	return NpadButton.None;
				    case NintendoSwitchButton.StickLUp: 	return NpadButton.None;
				    case NintendoSwitchButton.StickLRight: 	return NpadButton.None;
				    case NintendoSwitchButton.StickLDown: 	return NpadButton.None;
				    case NintendoSwitchButton.StickRLeft: 	return NpadButton.StickRLeft;
				    case NintendoSwitchButton.StickRUp: 	return NpadButton.StickRUp;
				    case NintendoSwitchButton.StickRRight: 	return NpadButton.StickRRight;
				    case NintendoSwitchButton.StickRDown: 	return NpadButton.StickRDown;
				    case NintendoSwitchButton.LeftSL: 		return NpadButton.LeftSL;
				    case NintendoSwitchButton.LeftSR: 		return NpadButton.LeftSR;
				    case NintendoSwitchButton.RightSL: 		return NpadButton.RightSL;
				    case NintendoSwitchButton.RightSR: 		return NpadButton.RightSR;
	        	}
	        	break;
	        }
	        break;

	        case NpadStyle.Invalid: 						return NpadButton.None;	
		}

		return NpadButton.None;
	}
	
	/// <summary>Remaps Axis depending of the NpadStyle contained on the given NpadID.</summary>
	/// <param name="_ID">Npad's ID.</param>
	/// <param name="_axis">Axis retrieved from ID's NpadState.</param>
	/// <returns>Remapped Axis.</returns>
	public static Vector2 IDStyleToLeftAxis(this NpadId _ID, Vector2 _axis)
	{
		NpadStyle style = Npad.GetStyleSet(_ID);
		NpadJoyHoldType holdType = NpadJoy.GetHoldType();

		switch(style)
		{
			case NpadStyle.None: 					return Vector2.zero;
	        case NpadStyle.FullKey: 				return _axis;
	        case NpadStyle.Handheld: 				return _axis; 
	        case NpadStyle.JoyDual: 				return _axis;
	        case NpadStyle.JoyLeft:
	        switch(holdType)
	        {
	        	case NpadJoyHoldType.Horizontal: 	return new Vector2(-_axis.y, _axis.x);
	        	case NpadJoyHoldType.Vertical: 		return _axis;
	        }
	        break;

	        case NpadStyle.JoyRight:
	        switch(holdType)
	        {
	        	case NpadJoyHoldType.Horizontal: 	return new Vector2(_axis.y, -_axis.x);
	        	case NpadJoyHoldType.Vertical: 		return _axis;
	        }
	        break;

	        case NpadStyle.Invalid: 				return Vector2.zero;	
		}

		return _axis;
	}

	/// <summary>Converts Player's ID to NpadId.</summary>
	/// <param name="_playerID">Player's ID.</param>
	/// <returns>Npad interpreted from Player's ID.</returns>
	public static NpadId ToNpadID(this int _playerID)
	{
		switch(_playerID)
		{
			case 0: return NpadId.Handheld;
			case 1: return NpadId.No1;
			case 2: return NpadId.No2;
			case 3: return NpadId.No3;
			case 4: return NpadId.No4;
			case 5: return NpadId.No5;
			case 6: return NpadId.No6;
			case 7: return NpadId.No7;
			case 8: return NpadId.No8;
		}

		return NpadId.Invalid;
	}
#endif 
}
}