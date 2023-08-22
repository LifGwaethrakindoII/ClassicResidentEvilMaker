using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.REMaker
{
public class TestingRoomSceneController : MonoBehaviour
{
    private DialogueNode _weaponChoicePrompt;

    /// <summary>Gets and Sets weaponChoicePrompt property.</summary>
    public DialogueNode weaponChoicePrompt
    {
        get { return _weaponChoicePrompt; }
        set { _weaponChoicePrompt = value; }
    }

    /// <summary>TestingRoomSceneController's instance initialization when loaded [Before scene loads].</summary>
    private void Awake()
    {
        weaponChoicePrompt = new DialogueNode(
            new DialogueText("WeaponPrompt", "Choose your weapon..."),
            new DialogueText("AK7Choice", "AK-47")
        );

        Debug.Log("[TestingRoomSceneController] Weapon Choice Prompt: " + weaponChoicePrompt.ToString());
    }
}
}