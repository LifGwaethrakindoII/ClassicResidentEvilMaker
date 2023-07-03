using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.REMaker
{
public class TEST_EquipWeaponToCharacter : MonoBehaviour
{
    [SerializeField] private HumanoidCharacter character;
    [SerializeField] private Weapon weapon;

    /// <summary>Callback invoked when scene loads, one frame before the first Update's tick.</summary>
    private void Start()
    {
        if(character == null || weapon == null) return;

        character.EquipWeapon(weapon);
    }
}
}