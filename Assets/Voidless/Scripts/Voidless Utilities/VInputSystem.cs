using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Voidless
{
public static class VInputSystem
{
    /// <summary>Subscribes/Unsubscribes to InputActionReference InputAction's Started Callback.</summary>
    /// <param name="_reference">InputActionReference that ought to contain the InputAction.</param>
    /// <param name="_callback">Callback to invoke when InputActionReference's InputAction invokes the Started event.</param>
    public static void AddStartedListener(this InputActionReference _reference, Action<InputAction.CallbackContext> _callback, bool _add = true)
    {
        if(_reference == null || _reference.action == null) return;

        switch(_add)
        {
            case true:
                _reference.action.started += _callback;
            break;

            case false:
                _reference.action.started -= _callback;
            break;
        }
    }

    /// <summary>Subscribes/Unsubscribes to InputActionReference InputAction's Performed Callback.</summary>
    /// <param name="_reference">InputActionReference that ought to contain the InputAction.</param>
    /// <param name="_callback">Callback to invoke when InputActionReference's InputAction invokes the Performed event.</param>
    public static void AddPerformedListener(this InputActionReference _reference, Action<InputAction.CallbackContext> _callback, bool _add = true)
    {
        if(_reference == null || _reference.action == null) return;

        switch(_add)
        {
            case true:
                _reference.action.performed += _callback;
            break;

            case false:
                _reference.action.performed -= _callback;
            break;
        }
    }

    /// <summary>Subscribes/Unsubscribes to InputActionReference InputAction's Canceled Callback.</summary>
    /// <param name="_reference">InputActionReference that ought to contain the InputAction.</param>
    /// <param name="_callback">Callback to invoke when InputActionReference's InputAction invokes the Canceled event.</param>
    public static void AddCanceledListener(this InputActionReference _reference, Action<InputAction.CallbackContext> _callback, bool _add = true)
    {
        if(_reference == null || _reference.action == null) return;

        switch(_add)
        {
            case true:
                _reference.action.canceled += _callback;
            break;

            case false:
                _reference.action.canceled -= _callback;
            break;
        }
    }

    /// <summary>Reads value from InputAction contained inside the InputActionReference.</summary>
    /// <param name="_reference">InputActionReference's reference.</param>
    /// <param name="_default">Default value to return if there is no InputAction inside the InputActionReference [default(T) by default].</param>
    /// <returns>Value from InputAction inside InputActionReference.</returns>
    public static T ReadValue<T>(this InputActionReference _reference, T _default = default(T)) where T : struct
    {
        return _reference.action != null ? _reference.action.ReadValue<T>() : _default;
    }

    /// <summary>Enables Action contained within each InputActionReference of the array.</summary>
    /// <param name="_references">Set of InputActionReferences.</param>
    public static void Enable(params InputActionReference[] _references)
    {
        foreach(InputActionReference reference in _references)
        {
            if(reference == null || reference.action == null) continue;

            reference.action.Enable();
        }
    }
}
}