using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Voidless.REMaker
{
[Flags]
public enum Interactions
{
    None = 0,
    Pickable = 1,
    Removable = 2,
    Equippable = 4,
    Consumable = 8,
    Combinable = 16,

    WeaponDefaultInteractions = Pickable | Removable | Equippable,
    RecoveryDefaultInteractions = Pickable | Removable | Consumable,
    AmmoDefaultInteractions = Pickable | Removable | Combinable,
    KeyDefaultInteractions = Pickable
}

public abstract class Item : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private Interactions _interactions;
    [SerializeField] private Sprite _icon;
    [SerializeField] private TransformData[] _grabPoints;
    [SerializeField] private AudioSource _audioSource;

    /// <summary>Gets and Sets name property.</summary>
    public string name
    {
        get { return _name; }
        set { _name = value; }
    }

    /// <summary>Gets and Sets description property.</summary>
    public string description
    {
        get { return _description; }
        set { _description = value; }
    }

    /// <summary>Gets and Sets interactions property.</summary>
    public Interactions interactions
    {
        get { return _interactions; }
        set { _interactions = value; }
    }

    /// <summary>Gets and Sets icon property.</summary>
    public Sprite icon
    {
        get { return _icon; }
        set { _icon = value; }
    }

    /// <summary>Gets and Sets grabPoints property.</summary>
    public TransformData[] grabPoints
    {
        get { return _grabPoints; }
        set { _grabPoints = value; }
    }

    /// <summary>Gets audioSource property.</summary>
    public AudioSource audioSource { get { return _audioSource; } }

    /// <summary>Draws Gizmos on Editor mode.</summary>
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.white.WithAlpha(0.5f);
        float r = 0.02f;

        if(grabPoints != null) for(int i = 0; i < grabPoints.Length; i++)
        {
            TransformData data = GetGrabPointData(i);
            Gizmos.DrawSphere(data.localPosition, r);
#if UNITY_EDITOR
            if(!data.showHandles)VGizmos.DrawTransformData(data, r);
#endif
        }
    }

    /// <summary>Resets Weapon's instance to its default values.</summary>
    protected virtual void Reset() { /*...*/ }

    /// <summary>Item's instance initialization when loaded [Before scene loads].</summary>
    protected virtual void Awake() { /*...*/ }

    /// <summary>Gets Grab Point Offset [local position].</summary>
    /// <param name="index">Grab Point's Index [0 by default].</param>
    public Vector3 GetGrabPointOffset(int index = 0)
    {
        bool hasGrabPoints = grabPoints != null;
        
        return hasGrabPoints ? grabPoints[index].localPosition : Vector3.zero;   
    }

    /// <summary>Gets Grab Point.</summary>
    /// <param name="index">Grab Point's Index.</param>
    public TransformData GetGrabPointData(int index = 0)
    {
        bool hasGrabPoints = grabPoints != null;
        TransformData data = hasGrabPoints ? grabPoints[index] : transform;
        
        if(hasGrabPoints) data.parent = transform;

        return  data; 
    }

    /// <summary>Callback invoked when this Item is picked up.</summary>
    public virtual void OnPickup() { /*...*/ }

    /// <summary>Callback invoked when this Item is used.</summary>
    public virtual void OnUse() { /*...*/ }
}
}