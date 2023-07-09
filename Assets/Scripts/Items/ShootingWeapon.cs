using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
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
    [Space(5f)]
    [Header("Effects:")]
    [SerializeField] private ParticleEffect _ejectionEffect;
    [SerializeField] private ParticleEffect _shootingEffect;
    [SerializeField] private VisualEffect _ejectionVFX;
    [SerializeField] private VisualEffect _shootingVFX;
    [SerializeField] private AudioClip _shootingSFX;
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

    /// <summary>Gets and Sets ejectionEffect property.</summary>
    public ParticleEffect ejectionEffect
    {
        get { return _ejectionEffect; }
        set { _ejectionEffect = value; }
    }

    /// <summary>Gets and Sets shootingEffect property.</summary>
    public ParticleEffect shootingEffect
    {
        get { return _shootingEffect; }
        set { _shootingEffect = value; }
    }

    /// <summary>Gets and Sets ejectionVFX property.</summary>
    public VisualEffect ejectionVFX
    {
        get { return _ejectionVFX; }
        set { _ejectionVFX = value; }
    }

    /// <summary>Gets and Sets shootingVFX property.</summary>
    public VisualEffect shootingVFX
    {
        get { return _shootingVFX; }
        set { _shootingVFX = value; }
    }

    /// <summary>Gets and Sets shootingSFX property.</summary>
    public AudioClip shootingSFX
    {
        get { return _shootingSFX; }
        set { _shootingSFX = value; }
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

    /// <summary>Item's instance initialization when loaded [Before scene loads].</summary>
    protected override void Awake()
    {
        base.Awake();

        if(ejectionVFX != null)
        {
            ejectionVFX.gameObject.SetActive(false);
            ejectionVFX.Stop();
        }
        if(shootingVFX != null)
        {
            shootingVFX.gameObject.SetActive(false);
            shootingVFX.Stop();
        }
    }

    /// <summary>Gets Colliders inside Weapon's attack range.</summary>
    protected override Collider[] GetCollidersInsideAttackRange()
    {

        return null;
    }

    /// <summary>Callback invoked when this Weapon should be used.</summary>
    public override void OnUse()
    {
        /*if(ejectionVFX != null)
        {
            ejectionVFX.gameObject.SetActive(true);
            ejectionVFX.transform.position = ejectionPort.position;
            ejectionVFX.transform.rotation = ejectionPort.rotation;
            ejectionVFX.Play();
        }
        if(shootingVFX != null)
        {
            shootingVFX.gameObject.SetActive(true);
            shootingVFX.transform.position = muzzle.position;
            shootingVFX.transform.rotation = muzzle.rotation;
            shootingVFX.Play();   
        }*/
        if(audioSource != null && shootingSFX != null)
        {
            audioSource.PlaySound(shootingSFX, false);
        }

        Collider[] colliders = VPhysics.OverlapViewCone(transform.position, muzzle.forward, horizontalAngle, verticalAngle, mask);

        if(colliders == null || colliders.Length == 0)
        {
            Debug.Log("[ShootingWeapon] No potential targets detected.");
            return;
        }

        float maxDot = Mathf.NegativeInfinity;
        Collider fittestCollider = null;

        foreach(Collider collider in colliders)
        {
            Vector3 direction = collider.transform.position - transform.position;
            direction.Normalize();
            float dot = Vector3.Dot(transform.forward, direction);
            if(dot > maxDot)
            {
                fittestCollider = collider;
                maxDot = dot;
            }
        }

        if(fittestCollider != null)
        {
            Debug.Log("[ShootingWeapon] Found fittest one: " + fittestCollider.gameObject.name);
            HitInteractable hitInteractable = fittestCollider.GetComponent<HitInteractable>();
            if(hitInteractable != null)
            {
                Vector3 d = fittestCollider.transform.position - transform.position;
                Debug.Log("[ShootingWeapon] Invoking Hit Event...");
                hitInteractable.InvokeHitEvent(gameObject, d, damage);
            }
        }
        else Debug.Log("[ShootingWeapon] Nope...");
    }
}
}