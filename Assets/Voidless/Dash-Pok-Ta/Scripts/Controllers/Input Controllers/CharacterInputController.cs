using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace Voidless.DashPokTa
{
public class CharacterInputController : GamePadInputController
{
    [SerializeField] private Character _character;

    /// <summary>Gets and Sets character property.</summary>
    public Character character
    {
        get { return _character; }
        set { _character = value; }
    }

    /// <summary>BaseCharacterInputController's tick at each frame.</summary>
    protected override void Update ()
    {
        base.Update();
        
        if(character == null) return;

        if(leftAxes.sqrMagnitude > 0.0f)
        character.Move(leftAxes);   
    }

    /// <summary>Callback invoked when the Down-Action's InputAction has Started.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected override void OnDownActionInputActionStarted(CallbackContext _context)
    {
        Debug.Log("[CharacterInputController] Jumping input pressed.");
        base.OnDownActionInputActionStarted(_context);
        if(character == null) return;
        character.Jump();
    }

    /// <summary>Callback invoked when the Left-Action's InputAction has Started.</summary>
    /// <param name="_context">Callback's Context.</param>
    protected override void OnLeftActionInputActionStarted(CallbackContext _context)
    {
        Debug.Log("[CharacterInputController] Dashing input pressed.");
        base.OnLeftActionInputActionStarted(_context);
        if(character == null) return;
        character.Dash();
    }
}
}