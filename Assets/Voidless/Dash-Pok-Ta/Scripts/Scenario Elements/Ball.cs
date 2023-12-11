using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.DashPokTa
{
[RequireComponent(typeof(Rigidbody))]
public class Ball : DashPokTaObject
{
    [SerializeField] private SphereCollider _sphereCollider;
    [Space(5f)]
    [SerializeField] private float _minimumHeight;
    [SerializeField] private float _maxVerticalVelocity;
    [SerializeField] private float _maxHorizontalVelocity;
    private Rigidbody _rigidbody;

    /// <summary>Gets sphereCollider property.</summary>
    public SphereCollider sphereCollider { get { return _sphereCollider; } }

    /// <summary>Gets and Sets minimumHeight property.</summary>
    public float minimumHeight
    {
        get { return _minimumHeight; }
        set { _minimumHeight = value; }
    }

    /// <summary>Gets and Sets maxVerticalVelocity property.</summary>
    public float maxVerticalVelocity
    {
        get { return _maxVerticalVelocity; }
        set { _maxVerticalVelocity = value; }
    }

    /// <summary>Gets and Sets maxHorizontalVelocity property.</summary>
    public float maxHorizontalVelocity
    {
        get { return _maxHorizontalVelocity; }
        set { _maxHorizontalVelocity = value; }
    }

    /// <summary>Gets rigidbody Component.</summary>
    public Rigidbody rigidbody
    { 
        get
        {
            if(_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
            return _rigidbody;
        }
    }

    /// <summary>Updates Ball's instance at each Physics Thread's frame.</summary>
    private void FixedUpdate()
    {
        ClampVelocity();
    }

    /// <summary>Clamps Rigidbody's velocity.</summary>
    private void ClampVelocity()
    {
        Vector3 velocity = rigidbody.velocity;
        Vector2 horizontalVelocity = new Vector2(velocity.x, velocity.z);
        float verticalVelocity = velocity.y;

        horizontalVelocity = Vector2.ClampMagnitude(horizontalVelocity, maxHorizontalVelocity);
        verticalVelocity = Mathf.Clamp(verticalVelocity, -maxVerticalVelocity, maxVerticalVelocity);
        velocity = new Vector3(horizontalVelocity.x, verticalVelocity, horizontalVelocity.y);
        rigidbody.velocity = velocity;
    }

    /// <summary>Adds Force to Rigidbody, then clamps its velocity.</summary>
    /// <param name="f">Force to add.</param>
    /// <param name="m">Force Mode [ForceMode.Force by default].</param>
    public void AddForce(Vector3 f, ForceMode m = ForceMode.Force)
    {
        rigidbody.AddForce(f, m);
        ClampVelocity();
    }

    /// <summary>Event triggered when this Collider/Rigidbody begun having contact with another Collider/Rigidbody.</summary>
    /// <param name="col">The Collision data associated with this collision Event.</param>
    protected override void OnCollisionEnter(Collision col)
    {
        GameObject obj = col.gameObject;
    
        if(obj.CompareTag("Player"))
        {
            if(obj.TryGetComponent<Character>(out Character character))
            {
                AddForce(character.GetOverallForce(), ForceMode.Impulse);
            }
        }

        //Debug.Log("[Ball] Collided with: " + obj.name);
    }

    /// <summary>Event triggered when this Collider/Rigidbody begun having contact with another Collider/Rigidbody.</summary>
    /// <param name="col">The Collision data associated with this collision Event.</param>
    protected override void OnCollisionStay(Collision col)
    {
        OnCollisionExit(col);
    }

    /// <summary>Event triggered when this Collider/Rigidbody began having contact with another Collider/Rigidbody.</summary>
    /// <param name="col">The Collision data associated with this collision Event.</param>
    protected override void OnCollisionExit(Collision col)
    {
        GameObject obj = col.gameObject;
        float y = rigidbody.velocity.y;

        if(y > 0.0f && y < minimumHeight)
        {
            float dy = minimumHeight - y;
            rigidbody.AddForce(Vector3.up * dy, ForceMode.VelocityChange);
        }
    }
}
}