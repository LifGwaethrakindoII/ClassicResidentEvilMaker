using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*===========================================================================
**
** Class:  HumanoidCharacterController
**
** Purpose: Class that does Input System reading for a referenced
** HumanoidCharacter.
**
** Action 1: Interact, Open Doors, etc.
** Action 2: Run
** Action 3: Prepare Weapon
** Action 4: Use Weapon
**
** Author: Lîf Gwaethrakindo
**
===========================================================================*/

namespace Voidless.REMaker
{
/// <summary>Event invoked when an action is performed.</summary>
/// <param name="ID">Action's ID.</param>
/// <param name="state">Input's State.</param>
public delegate void OnInputAction(int ID, InputState state);

public class HumanoidCharacterInputController : CharacterInputController<HumanoidCharacter>
{
	public const int FLAG_INPUT_ACTION_1 = 1 << 0;
	public const int FLAG_INPUT_ACTION_2 = 1 << 1;
	public const int FLAG_INPUT_ACTION_3 = 1 << 2;
	public const int FLAG_INPUT_ACTION_4 = 1 << 3;

	public static event OnInputAction onInputAction;

	[SerializeField] private ControllerInput _action1ControllerInput;
	[SerializeField] private ControllerInput _action2ControllerInput;
	[SerializeField] private ControllerInput _action3ControllerInput;
	[SerializeField] private ControllerInput _action4ControllerInput;

	/// <summary>Gets and Sets action1ControllerInput property.</summary>
	public ControllerInput action1ControllerInput
	{
		get { return _action1ControllerInput; }
		set { _action1ControllerInput = value; }
	}

	/// <summary>Gets and Sets action2ControllerInput property.</summary>
	public ControllerInput action2ControllerInput
	{
		get { return _action2ControllerInput; }
		set { _action2ControllerInput = value; }
	}

	/// <summary>Gets and Sets action3ControllerInput property.</summary>
	public ControllerInput action3ControllerInput
	{
		get { return _action3ControllerInput; }
		set { _action3ControllerInput = value; }
	}

	/// <summary>Gets and Sets action4ControllerInput property.</summary>
	public ControllerInput action4ControllerInput
	{
		get { return _action4ControllerInput; }
		set { _action4ControllerInput = value; }
	}

	/// <summary>Initializes.</summary>
	protected override void Initialize()
	{
		base.Initialize();

		VInputSystem.AddPerformedListener(GetInputActionReference(action1ControllerInput), OnAction1Performed);
		VInputSystem.AddPerformedListener(GetInputActionReference(action2ControllerInput), OnAction2Performed);
		VInputSystem.AddPerformedListener(GetInputActionReference(action3ControllerInput), OnAction3Performed);
		VInputSystem.AddPerformedListener(GetInputActionReference(action4ControllerInput), OnAction4Performed);
		VInputSystem.AddCanceledListener(GetInputActionReference(action1ControllerInput), OnAction1Canceled);
		VInputSystem.AddCanceledListener(GetInputActionReference(action2ControllerInput), OnAction2Canceled);
		VInputSystem.AddCanceledListener(GetInputActionReference(action3ControllerInput), OnAction3Canceled);
		VInputSystem.AddCanceledListener(GetInputActionReference(action4ControllerInput), OnAction4Canceled);
	}

	/// <summary>Callback internally invoked when the Axes are updated, but before the previous axes' values get updated.</summary>
	protected override void OnAxesUpdated()
	{
		if(character == null) return;

		character.SetLeftAxes(previousLeftAxes.sqrMagnitude >= (leftDeadZoneRadius * leftDeadZoneRadius) ? leftAxes : Vector2.zero);
		character.SetRightAxes(previousRightAxes.sqrMagnitude >= (rightDeadZoneRadius * rightDeadZoneRadius) ? rightAxes : Vector2.zero);
	}

	/// <summary>Callback invoked when the Action #1's InputAction is Performed.</summary>
	/// <param name="_context">Callback's Context.</param>
	private void OnAction1Performed(InputAction.CallbackContext _context)
	{
		inputFlags |= FLAG_INPUT_ACTION_1;
		InvokeInputActionEvent(1, InputState.Begins);
	}

	/// <summary>Callback invoked when the Action #1's InputAction is Canceled.</summary>
	/// <param name="_context">Callback's Context.</param>
	private void OnAction1Canceled(InputAction.CallbackContext _context)
	{
		inputFlags &= ~FLAG_INPUT_ACTION_1;
		InvokeInputActionEvent(1, InputState.Ended);		
	}

	/// <summary>Callback invoked when the Action #2's InputAction is Performed.</summary>
	/// <param name="_context">Callback's Context.</param>
	private void OnAction2Performed(InputAction.CallbackContext _context)
	{
		inputFlags |= FLAG_INPUT_ACTION_2;
		InvokeInputActionEvent(2, InputState.Begins);

		if(Vector3.Dot(leftAxes, Vector3.down) >= turnAroundDot) character.TurnAround();
		character.Run(true);
	}

	/// <summary>Callback invoked when the Action #2's InputAction is Canceled.</summary>
	/// <param name="_context">Callback's Context.</param>
	private void OnAction2Canceled(InputAction.CallbackContext _context)
	{
		inputFlags &= ~FLAG_INPUT_ACTION_2;
		InvokeInputActionEvent(2, InputState.Ended);
		character.Run(false);	
	}

	/// <summary>Callback invoked when the Action #3's InputAction is Performed.</summary>
	/// <param name="_context">Callback's Context.</param>
	private void OnAction3Performed(InputAction.CallbackContext _context)
	{
		inputFlags |= FLAG_INPUT_ACTION_3;
		InvokeInputActionEvent(3, InputState.Begins);
		character.Aim(true);
	}

	/// <summary>Callback invoked when the Action #3's InputAction is Canceled.</summary>
	/// <param name="_context">Callback's Context.</param>
	private void OnAction3Canceled(InputAction.CallbackContext _context)
	{
		inputFlags &= ~FLAG_INPUT_ACTION_3;
		InvokeInputActionEvent(3, InputState.Ended);
		character.Aim(false);		
	}

	/// <summary>Callback invoked when the Action #4's InputAction is Performed.</summary>
	/// <param name="_context">Callback's Context.</param>
	private void OnAction4Performed(InputAction.CallbackContext _context)
	{
		inputFlags |= FLAG_INPUT_ACTION_4;
		InvokeInputActionEvent(4, InputState.Begins);
		character.UseWeapon();
	}

	/// <summary>Callback invoked when the Action #4's InputAction is Canceled.</summary>
	/// <param name="_context">Callback's Context.</param>
	private void OnAction4Canceled(InputAction.CallbackContext _context)
	{
		inputFlags &= ~FLAG_INPUT_ACTION_4;
		InvokeInputActionEvent(4, InputState.Ended);		
	}

	/// <summary>Invokes Input Action's event.</summary>
	/// <param name="_ID">ID.</param>
	/// <param name="_inputState">Input's State.</param>
	public void InvokeInputActionEvent(int _ID, InputState _inputState)
	{
		//Debug.Log("[CharacterInputController] Action " + _ID + " " + _inputState.ToString() + ".");
		if(onInputAction != null) onInputAction(_ID, _inputState);	
	}
}
}