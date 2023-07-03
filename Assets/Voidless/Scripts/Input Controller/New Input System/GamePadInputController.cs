using System.Collections;
using System.Text;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace Voidless
{
[Flags]
public enum ControllerInputs
{
    None = 0,
    LeftAction = 1,
    RightAction = 2,
    DownAction = 4,
    UpAction = 8,
    LeftShoulder1 = 16,
    LeftShoulder2 = 32,
    RightShoulder1 = 64,
    RightShoulder2 = 128,
    LeftStick = 256,
    RightStick = 512,
    DPadLeft = 1024,
    DPadRight = 2048,
    DPadDown = 4096,
    DPadUp = 8192
}

public enum ControllerInput
{
    None,
    LeftAction,
    RightAction,
    DownAction,
    UpAction,
    LeftShoulder1,
    LeftShoulder2,
    RightShoulder1,
    RightShoulder2,
    LeftStick,
    RightStick,
    DPadLeft,
    DPadRight,
    DPadDown,
    DPadUp
}

public class GamePadInputController : BaseInputController
{
    [Space(5f)]
    [Header("D-Pad's Axes:")]
    [TabGroup("Main", "Axes")][SerializeField] private InputActionReference _dPadLeftInputReference;
    [TabGroup("Main", "Axes")][SerializeField] private InputActionReference _dPadRightInputReference;
    [TabGroup("Main", "Axes")][SerializeField] private InputActionReference _dPadDownInputReference;
    [TabGroup("Main", "Axes")][SerializeField] private InputActionReference _dPadUpInputReference;
    [TabGroup("Main", "Axes")][SerializeField] private InputActionReference _leftStickClickInputReference;
    [TabGroup("Main", "Axes")][SerializeField] private InputActionReference _rightStickClickInputReference;
    [Space(5f)]
    [Header("Actions:")]
    [TabGroup("GamePad", "Actions")][SerializeField] private InputActionReference _leftActionInputReference;
    [TabGroup("GamePad", "Actions")][SerializeField] private InputActionReference _rightActionInputReference;
    [TabGroup("GamePad", "Actions")][SerializeField] private InputActionReference _downActionInputReference;
    [TabGroup("GamePad", "Actions")][SerializeField] private InputActionReference _upActionInputReference;
    [Space(5f)]
    [Header("Shoulders:")]
    [TabGroup("GamePad", "Shoulders")][SerializeField] private InputActionReference _leftShoulder1InputReference;
    [TabGroup("GamePad", "Shoulders")][SerializeField] private InputActionReference _leftShoulder2InputReference;
    [TabGroup("GamePad", "Shoulders")][SerializeField] private InputActionReference _rightShoulder1InputReference;
    [TabGroup("GamePad", "Shoulders")][SerializeField] private InputActionReference _rightShoulder2InputReference;
    [Space(5f)]
    [Header("Center Buttons:")]
    [TabGroup("GamePad", "Center Buttons")][SerializeField] private InputActionReference _leftCenterInputReference;
    [TabGroup("GamePad", "Center Buttons")][SerializeField] private InputActionReference _rightCenterInputReference;
    [TabGroup("GamePad", "Center Buttons")][SerializeField] private InputActionReference _mainCenterInputReference;
    private Vector2 _dPadAxes;
    private Vector2 _previousDPadAxes;
    private float _dPadAxesMagnitude;
    private ControllerInputs _inputStates;

#region Getters/Setters:
    /// <summary>Gets dPadLeftInputReference property.</summary>
    public InputActionReference dPadLeftInputReference { get { return _dPadLeftInputReference; } }

    /// <summary>Gets dPadRightInputReference property.</summary>
    public InputActionReference dPadRightInputReference { get { return _dPadRightInputReference; } }

    /// <summary>Gets dPadDownInputReference property.</summary>
    public InputActionReference dPadDownInputReference { get { return _dPadDownInputReference; } }

    /// <summary>Gets dPadUpInputReference property.</summary>
    public InputActionReference dPadUpInputReference { get { return _dPadUpInputReference; } }

    /// <summary>Gets leftStickClickInputReference property.</summary>
    public InputActionReference leftStickClickInputReference { get { return _leftStickClickInputReference; } }

    /// <summary>Gets rightStickClickInputReference property.</summary>
    public InputActionReference rightStickClickInputReference { get { return _rightStickClickInputReference; } }

    /// <summary>Gets leftActionInputReference property.</summary>
    public InputActionReference leftActionInputReference { get { return _leftActionInputReference; } }

    /// <summary>Gets rightActionInputReference property.</summary>
    public InputActionReference rightActionInputReference { get { return _rightActionInputReference; } }

    /// <summary>Gets downActionInputReference property.</summary>
    public InputActionReference downActionInputReference { get { return _downActionInputReference; } }

    /// <summary>Gets upActionInputReference property.</summary>
    public InputActionReference upActionInputReference { get { return _upActionInputReference; } }

    /// <summary>Gets leftShoulder1InputReference property.</summary>
    public InputActionReference leftShoulder1InputReference { get { return _leftShoulder1InputReference; } }

    /// <summary>Gets leftShoulder2InputReference property.</summary>
    public InputActionReference leftShoulder2InputReference { get { return _leftShoulder2InputReference; } }

    /// <summary>Gets rightShoulder1InputReference property.</summary>
    public InputActionReference rightShoulder1InputReference { get { return _rightShoulder1InputReference; } }

    /// <summary>Gets rightShoulder2InputReference property.</summary>
    public InputActionReference rightShoulder2InputReference { get { return _rightShoulder2InputReference; } }

    /// <summary>Gets leftCenterInputReference property.</summary>
    public InputActionReference leftCenterInputReference { get { return _leftCenterInputReference; } }

    /// <summary>Gets rightCenterInputReference property.</summary>
    public InputActionReference rightCenterInputReference { get { return _rightCenterInputReference; } }

    /// <summary>Gets mainCenterInputReference property.</summary>
    public InputActionReference mainCenterInputReference { get { return _mainCenterInputReference; } }

    /// <summary>Gets and Sets dPadAxes property.</summary>
    public Vector2 dPadAxes
    {
        get { return _dPadAxes; }
        protected set { _dPadAxes = value; }
    }

    /// <summary>Gets and Sets previousDPadAxes property.</summary>
    public Vector2 previousDPadAxes
    {
        get { return _previousDPadAxes; }
        protected set { _previousDPadAxes = value; }
    }

    /// <summary>Gets and Sets dPadAxesMagnitude property.</summary>
    public float dPadAxesMagnitude
    {
        get { return _dPadAxesMagnitude; }
        protected set { _dPadAxesMagnitude = value; }
    }

    /// <summary>Gets and Sets inputStates property.</summary>
    public ControllerInputs inputStates
    {
        get { return _inputStates; }
        protected set { _inputStates = value; }
    }
#endregion

    /// <summary>Initializes BaseCharacterInputController.</summary>
    protected override void Initialize()
    {
        base.Initialize();

        VInputSystem.Enable(
            dPadLeftInputReference,
            dPadRightInputReference,
            dPadDownInputReference,
            dPadUpInputReference,
            leftStickClickInputReference,
            rightStickClickInputReference,
            leftActionInputReference,
            rightActionInputReference,
            downActionInputReference,
            upActionInputReference,
            leftShoulder1InputReference,
            leftShoulder2InputReference,
            rightShoulder1InputReference,
            rightShoulder2InputReference,
            leftCenterInputReference,
            rightCenterInputReference,
            mainCenterInputReference
        );

        /// Started Subscriptions:
        dPadLeftInputReference.AddStartedListener(OnDPadLeftInputActionStarted);
        dPadRightInputReference.AddStartedListener(OnDPadRightInputActionStarted);
        dPadDownInputReference.AddStartedListener(OnDPadDownInputActionStarted);
        dPadUpInputReference.AddStartedListener(OnDPadUpInputActionStarted);
        leftStickClickInputReference.AddStartedListener(OnLeftStickClickInputActionStarted);
        rightStickClickInputReference.AddStartedListener(OnRightStickClickInputActionStarted);
        leftActionInputReference.AddStartedListener(OnLeftActionInputActionStarted);
        rightActionInputReference.AddStartedListener(OnRightActionInputActionStarted);
        downActionInputReference.AddStartedListener(OnDownActionInputActionStarted);
        upActionInputReference.AddStartedListener(OnUpActionInputActionStarted);
        leftShoulder1InputReference.AddStartedListener(OnLeftShoulder1InputActionStarted);
        leftShoulder2InputReference.AddStartedListener(OnLeftShoulder2InputActionStarted);
        rightShoulder1InputReference.AddStartedListener(OnRightShoulder1InputActionStarted);
        rightShoulder2InputReference.AddStartedListener(OnRightShoulder2InputActionStarted);
        leftCenterInputReference.AddStartedListener(OnLeftCenterInputActionStarted);
        rightCenterInputReference.AddStartedListener(OnRightCenterInputActionStarted);
        mainCenterInputReference.AddStartedListener(OnMainCenterInputActionStarted);

        /// Performed Subscriptions:
        dPadLeftInputReference.AddPerformedListener(OnDPadLeftInputActionPerformed);
        dPadRightInputReference.AddPerformedListener(OnDPadRightInputActionPerformed);
        dPadDownInputReference.AddPerformedListener(OnDPadDownInputActionPerformed);
        dPadUpInputReference.AddPerformedListener(OnDPadUpInputActionPerformed);
        leftStickClickInputReference.AddPerformedListener(OnLeftStickClickInputActionPerformed);
        rightStickClickInputReference.AddPerformedListener(OnRightStickClickInputActionPerformed);
        leftActionInputReference.AddPerformedListener(OnLeftActionInputActionPerformed);
        rightActionInputReference.AddPerformedListener(OnRightActionInputActionPerformed);
        downActionInputReference.AddPerformedListener(OnDownActionInputActionPerformed);
        upActionInputReference.AddPerformedListener(OnUpActionInputActionPerformed);
        leftShoulder1InputReference.AddPerformedListener(OnLeftShoulder1InputActionPerformed);
        leftShoulder2InputReference.AddPerformedListener(OnLeftShoulder2InputActionPerformed);
        rightShoulder1InputReference.AddPerformedListener(OnRightShoulder1InputActionPerformed);
        rightShoulder2InputReference.AddPerformedListener(OnRightShoulder2InputActionPerformed);
        leftCenterInputReference.AddPerformedListener(OnLeftCenterInputActionPerformed);
        rightCenterInputReference.AddPerformedListener(OnRightCenterInputActionPerformed);
        mainCenterInputReference.AddPerformedListener(OnMainCenterInputActionPerformed);

        /// Canceled Subscriptions:
        dPadLeftInputReference.AddCanceledListener(OnDPadLeftInputActionCanceled);
        dPadRightInputReference.AddCanceledListener(OnDPadRightInputActionCanceled);
        dPadDownInputReference.AddCanceledListener(OnDPadDownInputActionCanceled);
        dPadUpInputReference.AddCanceledListener(OnDPadUpInputActionCanceled);
        leftStickClickInputReference.AddCanceledListener(OnLeftStickClickInputActionCanceled);
        rightStickClickInputReference.AddCanceledListener(OnRightStickClickInputActionCanceled);
        leftActionInputReference.AddCanceledListener(OnLeftActionInputActionCanceled);
        rightActionInputReference.AddCanceledListener(OnRightActionInputActionCanceled);
        downActionInputReference.AddCanceledListener(OnDownActionInputActionCanceled);
        upActionInputReference.AddCanceledListener(OnUpActionInputActionCanceled);
        leftShoulder1InputReference.AddCanceledListener(OnLeftShoulder1InputActionCanceled);
        leftShoulder2InputReference.AddCanceledListener(OnLeftShoulder2InputActionCanceled);
        rightShoulder1InputReference.AddCanceledListener(OnRightShoulder1InputActionCanceled);
        rightShoulder2InputReference.AddCanceledListener(OnRightShoulder2InputActionCanceled);
        leftCenterInputReference.AddCanceledListener(OnLeftCenterInputActionCanceled);
        rightCenterInputReference.AddCanceledListener(OnRightCenterInputActionCanceled);
        mainCenterInputReference.AddCanceledListener(OnMainCenterInputActionCanceled);
    }

    /// <summary>Gets InputActionReference according to provided ControllerInput enum value.</summary>
    /// <param name="_controllerInput">CoontrollerInput's ID.</param>
    public InputActionReference GetInputActionReference(ControllerInput _controllerInput)
    {
        switch(_controllerInput)
        {
            case ControllerInput.LeftAction:        return leftActionInputReference;
            case ControllerInput.RightAction:       return rightActionInputReference;
            case ControllerInput.DownAction:        return downActionInputReference;
            case ControllerInput.UpAction:          return upActionInputReference;
            case ControllerInput.LeftShoulder1:     return leftShoulder1InputReference;
            case ControllerInput.LeftShoulder2:     return leftShoulder2InputReference;
            case ControllerInput.RightShoulder1:    return rightShoulder1InputReference;
            case ControllerInput.RightShoulder2:    return rightShoulder2InputReference;
            case ControllerInput.LeftStick:         return leftStickClickInputReference;
            case ControllerInput.RightStick:        return rightStickClickInputReference;
            case ControllerInput.DPadLeft:          return dPadLeftInputReference;
            case ControllerInput.DPadRight:         return dPadRightInputReference;
            case ControllerInput.DPadDown:          return dPadDownInputReference;
            case ControllerInput.DPadUp:            return dPadUpInputReference;
            case ControllerInput.None:
            default:                                return null;
        }
    }

#region StartedCallbacks:
/*======================================================================================================================
|   Started Callbacks:                                                                                                 |
======================================================================================================================*/

    /// <summary>Callback invoked when the D-Pad Left's InputAction has Started.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnDPadLeftInputActionStarted(CallbackContext _context)
    {
        inputStates |= ControllerInputs.DPadLeft;
    }

    /// <summary>Callback invoked when the D-Pad Right's InputAction has Started.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnDPadRightInputActionStarted(CallbackContext _context)
    {
        inputStates |= ControllerInputs.DPadRight;
    }

    /// <summary>Callback invoked when the D-Pad Down's InputAction has Started.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnDPadDownInputActionStarted(CallbackContext _context)
    {
        inputStates |= ControllerInputs.DPadDown;
    }

    /// <summary>Callback invoked when the D-Pad Up's InputAction has Started.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnDPadUpInputActionStarted(CallbackContext _context)
    {
        inputStates |= ControllerInputs.DPadUp;
    }

    /// <summary>Callback invoked when the Left Stick Click's InputAction has Started.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnLeftStickClickInputActionStarted(CallbackContext _context)
    {
        inputStates |= ControllerInputs.LeftStick;
    }

    /// <summary>Callback invoked when the Right Stick Click's InputAction has Started.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnRightStickClickInputActionStarted(CallbackContext _context)
    {
        inputStates |= ControllerInputs.RightStick;
    }

    /// <summary>Callback invoked when the Left-Action's InputAction has Started.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnLeftActionInputActionStarted(CallbackContext _context)
    {
        inputStates |= ControllerInputs.LeftAction;
    }

    /// <summary>Callback invoked when the Right-Action's InputAction has Started.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnRightActionInputActionStarted(CallbackContext _context)
    {
        inputStates |= ControllerInputs.RightAction;
    }

    /// <summary>Callback invoked when the Down-Action's InputAction has Started.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnDownActionInputActionStarted(CallbackContext _context)
    {
        inputStates |= ControllerInputs.DownAction;
    }

    /// <summary>Callback invoked when the Up-Action's InputAction has Started.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnUpActionInputActionStarted(CallbackContext _context)
    {
        inputStates |= ControllerInputs.UpAction;
    }

    /// <summary>Callback invoked when the Left-Shoulder 1's InputAction has Started.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnLeftShoulder1InputActionStarted(CallbackContext _context)
    {
        inputStates |= ControllerInputs.LeftShoulder1;
    }

    /// <summary>Callback invoked when the Left-Shoulder 2's InputAction has Started.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnLeftShoulder2InputActionStarted(CallbackContext _context)
    {
        inputStates |= ControllerInputs.LeftShoulder2;
    }

    /// <summary>Callback invoked when the Right-Shoulder 1's InputAction has Started.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnRightShoulder1InputActionStarted(CallbackContext _context)
    {
        inputStates |= ControllerInputs.RightShoulder1;
    }

    /// <summary>Callback invoked when the Right-Shoulder 2's InputAction has Started.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnRightShoulder2InputActionStarted(CallbackContext _context)
    {
        inputStates |= ControllerInputs.RightShoulder2;
    }

    /// <summary>Callback invoked when the Left-Center's InputAction has Started.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnLeftCenterInputActionStarted(CallbackContext _context) { /*...*/ }

    /// <summary>Callback invoked when the Right-Center's InputAction has Started.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnRightCenterInputActionStarted(CallbackContext _context) { /*...*/ }

    /// <summary>Callback invoked when the Main-Center's InputAction has Started.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnMainCenterInputActionStarted(CallbackContext _context) { /*...*/ }
#endregion

#region PerformedCallbacks:
/*======================================================================================================================
|   Performed Callbacks:                                                                                               |
======================================================================================================================*/

    /// <summary>Callback invoked when the D-Pad Left's InputAction has Performed.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnDPadLeftInputActionPerformed(CallbackContext _context)
    {
        inputStates |= ControllerInputs.DPadLeft;
    }

    /// <summary>Callback invoked when the D-Pad Right's InputAction has Performed.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnDPadRightInputActionPerformed(CallbackContext _context)
    {
        inputStates |= ControllerInputs.DPadRight;
    }

    /// <summary>Callback invoked when the D-Pad Down's InputAction has Performed.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnDPadDownInputActionPerformed(CallbackContext _context)
    {
        inputStates |= ControllerInputs.DPadDown;
    }

    /// <summary>Callback invoked when the D-Pad Up's InputAction has Performed.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnDPadUpInputActionPerformed(CallbackContext _context)
    {
        inputStates |= ControllerInputs.DPadUp;
    }

    /// <summary>Callback invoked when the Left Stick Click's InputAction has Performed.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnLeftStickClickInputActionPerformed(CallbackContext _context)
    {
        inputStates |= ControllerInputs.LeftStick;
    }

    /// <summary>Callback invoked when the Right Stick Click's InputAction has Performed.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnRightStickClickInputActionPerformed(CallbackContext _context)
    {
        inputStates |= ControllerInputs.RightStick;
    }

    /// <summary>Callback invoked when the Left-Action's InputAction has Performed.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnLeftActionInputActionPerformed(CallbackContext _context)
    {
        inputStates |= ControllerInputs.LeftAction;
    }

    /// <summary>Callback invoked when the Right-Action's InputAction has Performed.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnRightActionInputActionPerformed(CallbackContext _context)
    {
        inputStates |= ControllerInputs.RightAction;
    }

    /// <summary>Callback invoked when the Down-Action's InputAction has Performed.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnDownActionInputActionPerformed(CallbackContext _context)
    {
        inputStates |= ControllerInputs.DownAction;
    }

    /// <summary>Callback invoked when the Up-Action's InputAction has Performed.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnUpActionInputActionPerformed(CallbackContext _context)
    {
        inputStates |= ControllerInputs.UpAction;
    }

    /// <summary>Callback invoked when the Left-Shoulder 1's InputAction has Performed.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnLeftShoulder1InputActionPerformed(CallbackContext _context)
    {
        inputStates |= ControllerInputs.LeftShoulder1;
    }

    /// <summary>Callback invoked when the Left-Shoulder 2's InputAction has Performed.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnLeftShoulder2InputActionPerformed(CallbackContext _context)
    {
        inputStates |= ControllerInputs.LeftShoulder2;
    }

    /// <summary>Callback invoked when the Right-Shoulder 1's InputAction has Performed.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnRightShoulder1InputActionPerformed(CallbackContext _context)
    {
        inputStates |= ControllerInputs.RightShoulder1;
    }

    /// <summary>Callback invoked when the Right-Shoulder 2's InputAction has Performed.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnRightShoulder2InputActionPerformed(CallbackContext _context)
    {
        inputStates |= ControllerInputs.RightShoulder2;
    }

    /// <summary>Callback invoked when the Left-Center's InputAction has Performed.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnLeftCenterInputActionPerformed(CallbackContext _context) { /*...*/ }

    /// <summary>Callback invoked when the Right-Center's InputAction has Performed.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnRightCenterInputActionPerformed(CallbackContext _context) { /*...*/ }

    /// <summary>Callback invoked when the Main-Center's InputAction has Performed.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnMainCenterInputActionPerformed(CallbackContext _context) { /*...*/ }
#endregion

#region CanceledCallbacks:
/*======================================================================================================================
|   Canceled Callbacks:                                                                                                |
======================================================================================================================*/

    /// <summary>Callback invoked when the D-Pad Left's InputAction has Canceled.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnDPadLeftInputActionCanceled(CallbackContext _context)
    {
        inputStates &= ~ControllerInputs.DPadLeft;
    }

    /// <summary>Callback invoked when the D-Pad Right's InputAction has Canceled.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnDPadRightInputActionCanceled(CallbackContext _context)
    {
        inputStates &= ~ControllerInputs.DPadRight;
    }

    /// <summary>Callback invoked when the D-Pad Down's InputAction has Canceled.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnDPadDownInputActionCanceled(CallbackContext _context)
    {
        inputStates &= ~ControllerInputs.DPadDown;
    }

    /// <summary>Callback invoked when the D-Pad Up's InputAction has Canceled.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnDPadUpInputActionCanceled(CallbackContext _context)
    {
        inputStates &= ~ControllerInputs.DPadUp;
    }

    /// <summary>Callback invoked when the Left Stick Click's InputAction has Canceled.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnLeftStickClickInputActionCanceled(CallbackContext _context)
    {
        inputStates &= ~ControllerInputs.LeftStick;
    }

    /// <summary>Callback invoked when the Right Stick Click's InputAction has Canceled.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnRightStickClickInputActionCanceled(CallbackContext _context)
    {
        inputStates &= ~ControllerInputs.RightStick;
    }

    /// <summary>Callback invoked when the Left-Action's InputAction has Canceled.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnLeftActionInputActionCanceled(CallbackContext _context)
    {
        inputStates &= ~ControllerInputs.LeftAction;
    }

    /// <summary>Callback invoked when the Right-Action's InputAction has Canceled.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnRightActionInputActionCanceled(CallbackContext _context)
    {
        inputStates &= ~ControllerInputs.RightAction;
    }

    /// <summary>Callback invoked when the Down-Action's InputAction has Canceled.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnDownActionInputActionCanceled(CallbackContext _context)
    {
        inputStates &= ~ControllerInputs.DownAction;
    }

    /// <summary>Callback invoked when the Up-Action's InputAction has Canceled.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnUpActionInputActionCanceled(CallbackContext _context)
    {
        inputStates &= ~ControllerInputs.UpAction;
    }

    /// <summary>Callback invoked when the Left-Shoulder 1's InputAction has Canceled.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnLeftShoulder1InputActionCanceled(CallbackContext _context)
    {
        inputStates &= ~ControllerInputs.LeftShoulder1;
    }

    /// <summary>Callback invoked when the Left-Shoulder 2's InputAction has Canceled.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnLeftShoulder2InputActionCanceled(CallbackContext _context)
    {
        inputStates &= ~ControllerInputs.LeftShoulder2;
    }

    /// <summary>Callback invoked when the Right-Shoulder 1's InputAction has Canceled.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnRightShoulder1InputActionCanceled(CallbackContext _context)
    {
        inputStates &= ~ControllerInputs.RightShoulder1;
    }

    /// <summary>Callback invoked when the Right-Shoulder 2's InputAction has Canceled.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnRightShoulder2InputActionCanceled(CallbackContext _context)
    {
        inputStates &= ~ControllerInputs.RightShoulder2;
    }

    /// <summary>Callback invoked when the Left-Center's InputAction has Canceled.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnLeftCenterInputActionCanceled(CallbackContext _context) { /*...*/ }

    /// <summary>Callback invoked when the Right-Center's InputAction has Canceled.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnRightCenterInputActionCanceled(CallbackContext _context) { /*...*/ }

    /// <summary>Callback invoked when the Main-Center's InputAction has Canceled.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected virtual void OnMainCenterInputActionCanceled(CallbackContext _context) { /*...*/ }
#endregion

    /// <returns>String representing this GamePadInputController.</returns>
    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();

        builder.AppendLine(base.ToString());
        builder.Append("Input  Flags: ");
        builder.AppendLine(inputStates.ToString());

        return builder.ToString();
    }
}
}