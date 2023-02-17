using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Voidless
{
//public enum InputState { Performed, Canceled }

/// <summary>Event invoked when an action is performed.</summary>
/// <param name="ID">Action's ID.</param>
/// <param name="state">Input's State.</param>
public delegate void OnInputAction(int ID, InputState state);

public class CharacterInputController : BaseCharacterInputController<Character>
{
	public static event OnInputAction onInputAction; 	/// <summary>OnInputAction's delegate.</summary>

	[Space(5f)]
	[SerializeField] private string runID; 		/// <summary>Run's ID.</summary>
	[SerializeField] private string action1ID; 	/// <summary>Action #1's ID.</summary>
	private InputAction runAction; 				/// <summary>Run's Action.</summary>
	private InputAction action1; 				/// <summary>Input Action #1.</summary>

	/// <summary>Sets Input's Actions.</summary>
	protected override void SetInputActions()
	{
		base.SetInputActions();

		if(!string.IsNullOrEmpty(runID)) runAction = actionMap.FindAction(runID, true);
		if(!string.IsNullOrEmpty(action1ID)) action1 = actionMap.FindAction(action1ID, true);

		if(action1 != null)
		{
			action1.performed += OnAction1Performed;
			action1.canceled += OnAction1Canceled;
		}

		if(runAction == null) return;

		runAction.performed += OnRunActionPerformed;
		runAction.canceled += OnRunActionCanceled;
	}

	/// <summary>Callback internally invoked when the Axes are updated, but before the previous axes' values get updated.</summary>
	protected override void OnAxesUpdated()
	{
		if(character == null) return;

		if(previousLeftAxes.sqrMagnitude >= (leftDeadZoneRadius * leftDeadZoneRadius))
		{
			character.Move(leftAxes);
			
			/*if(Mathf.Abs(leftAxes.x) >= (leftDeadZoneRadius * leftDeadZoneRadius))
			character.RotateSelf(leftAxes.x);*/
		}
		else character.GoIdle();
	}

	/// <summary>Callback invoked when the Run's InputAction is Performed.</summary>
	/// <param name="_context">Callback's Context.</param>
	private void OnRunActionPerformed(InputAction.CallbackContext _context)
	{
		character.Run(true);		
	}

	/// <summary>Callback invoked when the Run's InputAction is Canceled.</summary>
	/// <param name="_context">Callback's Context.</param>
	private void OnRunActionCanceled(InputAction.CallbackContext _context)
	{
		character.Run(false);		
	}

	/// <summary>Callback invoked when the Action #1's InputAction is Performed.</summary>
	/// <param name="_context">Callback's Context.</param>
	private void OnAction1Performed(InputAction.CallbackContext _context)
	{
		Debug.Log("[CharacterInputController] Action 1 Performed...");
		if(onInputAction != null) onInputAction(1, InputState.Begins);
	}

	/// <summary>Callback invoked when the Action #1's InputAction is Canceled.</summary>
	/// <param name="_context">Callback's Context.</param>
	private void OnAction1Canceled(InputAction.CallbackContext _context)
	{
		if(onInputAction != null) onInputAction(1, InputState.Ended);		
	}
}
}