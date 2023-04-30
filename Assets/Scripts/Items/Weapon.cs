using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.REMaker
{
public class Weapon : Item
{
    [Space(5f)]
    [Header("Weapon's Attributes:")]
    [SerializeField] private float _damage;
    [SerializeField] private float _rate;
    [SerializeField][Range(0.0f, 1.0f)] private float _criticalRatio;

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
}
}