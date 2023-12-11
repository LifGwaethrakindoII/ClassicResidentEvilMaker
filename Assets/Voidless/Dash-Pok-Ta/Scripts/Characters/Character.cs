using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.DashPokTa
{
[Flags]
public enum CharacterActionFlags
{
    None = 0,
    Jumping = 1,
    Dashing = 2,
    OnDashCooldown = 4,
}

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CapsuleCollider))]
public class Character : DashPokTaObject
{
    [Space(5f)]
    [Header("Displacement's Attributes:")]
    [SerializeField] private float _acceleration;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _turningSpeed;
    [Space(5f)]
    [Header("Dash's Attributes:")]
    [SerializeField] private float _dashForce;
    [SerializeField] private float _dashDuration;
    [SerializeField] private float _dashCooldown;
    [SerializeField] private float _dashGravityScalar;
    [Space(5f)]
    [Header("Jump's Attributes:")]
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _jumpGravity;
    [SerializeField] private float _jumpTime;
    [SerializeField] private float _gravityScalar;
    [Space(5f)]
    [SerializeField] private TrailRenderer _dashTrail;
    private CharacterController _characterController;
    private CapsuleCollider _capsuleCollider;
    private Vector3 force;
    private Vector3 externalForce;
    private float horizontalSpeed;
    private CharacterActionFlags _actionFlags;
    protected Coroutine dashCoroutine;

    /// <summary>Gets and Sets acceleration property.</summary>
    public float acceleration
    {
        get { return _acceleration; }
        set { _acceleration = value; }
    }

    /// <summary>Gets and Sets maxSpeed property.</summary>
    public float maxSpeed
    {
        get { return _maxSpeed; }
        set { _maxSpeed = value; }
    }

    /// <summary>Gets and Sets dashForce property.</summary>
    public float dashForce
    {
        get { return _dashForce; }
        set { _dashForce = value; }
    }

    /// <summary>Gets and Sets dashDuration property.</summary>
    public float dashDuration
    {
        get { return _dashDuration; }
        set { _dashDuration = value; }
    }

    /// <summary>Gets and Sets dashCooldown property.</summary>
    public float dashCooldown
    {
        get { return _dashCooldown; }
        set { _dashCooldown = value; }
    }

    /// <summary>Gets and Sets dashGravityScalar property.</summary>
    public float dashGravityScalar
    {
        get { return _dashGravityScalar; }
        set { _dashGravityScalar = value; }
    }

    /// <summary>Gets and Sets jumpHeight property.</summary>
    public float jumpHeight
    {
        get { return _jumpHeight; }
        set { _jumpHeight = value; }
    }

    /// <summary>Gets and Sets jumpGravity property.</summary>
    public float jumpGravity
    {
        get { return _jumpGravity; }
        set { _jumpGravity = value; }
    }

    /// <summary>Gets and Sets jumpTime property.</summary>
    public float jumpTime
    {
        get { return _jumpTime; }
        set { _jumpTime = value; }
    }

    /// <summary>Gets and Sets gravityScalar property.</summary>
    public float gravityScalar
    {
        get { return _gravityScalar; }
        set { _gravityScalar = value; }
    }

    /// <summary>Gets and Sets turningSpeed property.</summary>
    public float turningSpeed
    {
        get { return _turningSpeed; }
        set { _turningSpeed = value; }
    }

    /// <summary>Gets and Sets actionFlags property.</summary>
    public CharacterActionFlags actionFlags
    {
        get { return _actionFlags; }
        set { _actionFlags = value; }
    }

    /// <summary>Gets dashTrail property.</summary>
    public TrailRenderer dashTrail { get { return _dashTrail; } }

    /// <summary>Gets characterController Component.</summary>
    public CharacterController characterController
    { 
        get
        {
            if(_characterController == null) _characterController = GetComponent<CharacterController>();
            return _characterController;
        }
    }

    /// <summary>Gets capsuleCollider Component.</summary>
    public CapsuleCollider capsuleCollider
    { 
        get
        {
            if(_capsuleCollider == null) _capsuleCollider = GetComponent<CapsuleCollider>();
            return _capsuleCollider;
        }
    }

    /// <summary>Draws Gizmos on Editor mode when Character's instance is selected.</summary>
    private void OnDrawGizmosSelected()
    {
        characterController.SetAttributesToCapsuleCollider(capsuleCollider);
    }

    /// <summary>Character's instance initialization when loaded [Before scene loads].</summary>
    protected override void Awake()
    {
        base.Awake();
        dashTrail.Activate(false);
    }

    /// <summary>Updates Character's instance at each frame.</summary>
    protected override void Update()
    {
        force += Physics.gravity * GetGravityScalar() * Time.deltaTime;
        characterController.Move(force * Time.deltaTime);
        
        if(characterController.isGrounded)
        {
            actionFlags &= ~CharacterActionFlags.Jumping;
            force.y = 0.0f;
        }
    }

    /// <summary>Displaces Character.</summary>
    /// <param name="axes">Movement Axes [World Space].</param>
    public void Move(Vector2 axes)
    {
        Vector3 movement = new Vector3(axes.x, 0.0f, axes.y);
        Quaternion lookRotation = Quaternion.LookRotation(movement);
        float dot = Vector3.Dot(axes, transform.forward);
        float scalar = VMath.RemapValue(dot, -1.0f, 1.0f, 0.5f, 1.0f);
        float dt = Time.deltaTime;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, turningSpeed);
        horizontalSpeed += acceleration * Time.deltaTime;
        horizontalSpeed = Mathf.Min(horizontalSpeed, maxSpeed);
        characterController.Move(horizontalSpeed * dt * movement);
    }

    /// <summary>Makes the Character jump [if it is grounded].</summary>
    public void Jump()
    {
        /// Do not jump if the character is either grounded or jumping
        if(!characterController.isGrounded || (actionFlags | CharacterActionFlags.Jumping) == actionFlags) return;

        actionFlags |= CharacterActionFlags.Jumping;
        force += Vector3.up * CalculateJumpForce();
        Debug.DrawRay(transform.position, Vector3.up * jumpHeight, Color.cyan, 3.0f);
    }

    /// <summary>Makes the Character Dash.</summary>
    public void Dash()
    {
        /// Unable to flash if you are either dashing or on dash cooldown.
        if((actionFlags & (CharacterActionFlags.Dashing | CharacterActionFlags.OnDashCooldown)) != 0) return;

        Vector3 dashImpulse = transform.forward * dashForce;

        actionFlags |= CharacterActionFlags.Dashing;
        force += dashImpulse;
        dashTrail.Activate();

        Debug.DrawRay(transform.position, dashImpulse, Color.red, 3.0f);

        SetCooldown(dashDuration, ref dashCoroutine, ()=>
        {
            actionFlags &= ~CharacterActionFlags.Dashing;
            actionFlags |= CharacterActionFlags.OnDashCooldown;
            force -= dashImpulse;
            dashTrail.Activate(false);

            SetCooldown(dashCooldown, ref dashCoroutine, ()=>
            {
                actionFlags &= ~CharacterActionFlags.OnDashCooldown;
                this.DispatchCoroutine(ref dashCoroutine);
            });
        });
    }

    /// <summary>Gets the most appropiate gravity scalar given the current state of the Character.</summary>
    public float GetGravityScalar()
    {
        if((actionFlags | CharacterActionFlags.Jumping) == actionFlags) return gravityScalar;
        else if((actionFlags | CharacterActionFlags.Dashing) == actionFlags) return dashGravityScalar;
        else return gravityScalar;
    }

    /// <returns>Overall Force (force plus external force).</returns>
    public Vector3 GetOverallForce() { return force + externalForce; }

    /// <returns>Necessary Jump Force.</returns>
    public float CalculateJumpForce()
    {
        float g = Mathf.Abs(Physics.gravity.y) * gravityScalar;
        return Mathf.Sqrt(2.0f * g * jumpHeight);
    }

    /// <summary>Sets cooldown and invokes a callback after the cooldown ends.</summary>
    /// <param name="time">Cooldown's duration.</param>
    /// <param name="cooldownCoroutine">Cooldown Coroutine's reference.</param>
    /// <param name="onCooldownEnds">Callback invoked when the cooldown ends, null by default.</param>
    public void SetCooldown(float time, ref Coroutine cooldownCoroutine, Action onCooldownEnds = null)
    {
        this.StartCoroutine(this.WaitSeconds(time, onCooldownEnds), ref cooldownCoroutine);
    }

    /// <summary>Event triggered when this Collider/Rigidbody begun having contact with another Collider/Rigidbody.</summary>
    /// <param name="col">The Collision data associated with this collision Event.</param>
    private void OnCollisionEnter(Collision col)
    {
        GameObject obj = col.gameObject;
    
        //Debug.Log("[Character] Collided with " + obj.name);
    }
}
}