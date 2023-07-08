using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.REMaker
{
public class Weapon : Item
{
    [Space(5f)]
    [Header("Weapon's Attributes:")]
    [SerializeField] private LayerMask _mask;
    [SerializeField] private GameObjectTag[] _tags;
    [SerializeField] private float _damage;
    [SerializeField] private float _rate;
    [SerializeField][Range(0.0f, 1.0f)] private float _criticalRatio;
    [Space(5f)]
    [SerializeField] private UnityHash _performHash;

    /// <summary>Gets and Sets mask property.</summary>
    public LayerMask mask
    {
        get { return _mask; }
        set { _mask = value; }
    }

    /// <summary>Gets and Sets tags property.</summary>
    public GameObjectTag[] tags
    {
        get { return _tags; }
        set { _tags = value; }
    }

    /// <summary>Gets and Sets damage property.</summary>
    public float damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    /// <summary>Gets and Sets rate property.</summary>
    public float rate
    {
        get { return _rate; }
        set { _rate = value; }
    }

    /// <summary>Gets and Sets criticalRatio property.</summary>
    public float criticalRatio
    {
        get { return _criticalRatio; }
        set { _criticalRatio = Mathf.Clamp(value, 0.0f, 1.0f); }
    }

    /// <summary>Gets performHash property.</summary>
    public UnityHash performHash { get { return _performHash; } }

    /// <summary>Resets Weapon's instance to its default values.</summary>
    protected override void Reset()
    {
        base.Reset();
        interactions = Interactions.WeaponDefaultInteractions; // (Pickable, Removable, Equippable)
    }

    /// <summary>Gets Colliders inside Weapon's attack range.</summary>
    protected virtual Collider[] GetCollidersInsideAttackRange() { return null; }

    /// <summary>Callback invoked when this Weapon should be used.</summary>
    public override void OnUse() { Debug.Log("[Weapon] Calling Weapon.OnUse()"); }
}
}