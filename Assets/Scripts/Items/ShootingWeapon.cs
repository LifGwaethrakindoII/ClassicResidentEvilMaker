using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Voidless.REMaker
{
public enum ShootingWeaponType
{
    Automatic,
    SemiAutomatic,
    BoltAction,
    LeverAction,
    PumpAction,
    Revolver
}

public class ShootingWeapon : Weapon
{
    [Space(5f)]
    [Header("Shooting Weapon's Attributes:")]
    [SerializeField] private ShootingWeaponType _shootingWeaponType;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private Transform _ejectionPort;
    [SerializeField] private float _range;
    [SerializeField][Range(0.0f, 180.0f)] private float _horizontalAngle;
    [SerializeField][Range(0.0f, 180.0f)] private float _verticalAngle;
    [SerializeField] private float _rechargeSpeed;
    [SerializeField] private int _ammoCapacity;
    private int _ammoCount;

    /// <summary>Gets and Sets shootingWeaponType property.</summary>
    public ShootingWeaponType shootingWeaponType
    {
        get { return _shootingWeaponType; }
        set { _shootingWeaponType = value; }
    }

    /// <summary>Gets and Sets muzzle property.</summary>
    public Transform muzzle
    {
        get { return _muzzle; }
        set { _muzzle = value; }
    }

    /// <summary>Gets and Sets ejectionPort property.</summary>
    public Transform ejectionPort
    {
        get { return _ejectionPort; }
        set { _ejectionPort = value; }
    }

    /// <summary>Gets and Sets range property.</summary>
    public float range
    {
        get { return _range; }
        set { _range = value; }
    }

    /// <summary>Gets and Sets horizontalAngle property.</summary>
    public float horizontalAngle
    {
        get { return _horizontalAngle; }
        set { _horizontalAngle = Mathf.Clamp(value, 0.0f, 180.0f); }
    }

    /// <summary>Gets and Sets verticalAngle property.</summary>
    public float verticalAngle
    {
        get { return _verticalAngle; }
        set { _verticalAngle = Mathf.Clamp(value, 0.0f, 180.0f); }
    }

    /// <summary>Gets and Sets rechargeSpeed property.</summary>
    public float rechargeSpeed
    {
        get { return _rechargeSpeed; }
        set { _rechargeSpeed = value; }
    }

    /// <summary>Gets and Sets ammoCapacity property.</summary>
    public int ammoCapacity
    {
        get { return _ammoCapacity; }
        set { _ammoCapacity = value; }
    }

    /// <summary>Gets and Sets ammoCount property.</summary>
    public int ammoCount
    {
        get { return _ammoCount; }
        set { _ammoCount = value; }
    }

    /// <summary>Draws Gizmos on Editor mode.</summary>
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.blue;
        
        if(muzzle != null)
        {
            float h = horizontalAngle * 0.5f;
            float v = verticalAngle * 0.5f;

            Gizmos.DrawRay(muzzle.position, muzzle.forward * range);
#if UNITY_EDITOR
            Handles.color = Color.blue.WithAlpha(0.25f);
            Handles.DrawSolidArc(muzzle.position, muzzle.up, muzzle.forward, h, range);
            Handles.DrawSolidArc(muzzle.position, -muzzle.up, muzzle.forward, h, range);
            Handles.DrawSolidArc(muzzle.position, muzzle.right, muzzle.forward, v, range);
            Handles.DrawSolidArc(muzzle.position, -muzzle.right, muzzle.forward, v, range);
#endif
        }
    }
}
}