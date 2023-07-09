using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/*===========================================================================
**
** Class:  TransformRigidbody
**
** Purpose: Class that emulates some Rigidbody functions for the Transform,
** and therefore for the main threads (Update, LateUpdate, Coroutines, etc.).
**
** Very useful to displace and rotate Kinematic Rigidbodies.
**
** NOTE: It (still) lacks many important Rigidbody features, such as
** collision resolutions, inertia, etc.
**
** Author: Lîf Gwaethrakindo
**
===========================================================================*/
namespace Voidless
{
public enum LocomotionComponent { Transform, Rigidbody }

public enum UpdateType { Normal, Late, Fixed }

public class TransformRigidbody : MonoBehaviour
{
    [InfoBox("@ToString()")]
    [SerializeField] private LocomotionComponent _componentType;
    [SerializeField] private UpdateType _updateType;
    [SerializeField] private bool _useGravity;
    [SerializeField] private float _mass;
    [SerializeField]/*[Range(0.0f, 1.0f)]*/ private float _drag;
    [SerializeField]/*[Range(0.0f, 1.0f)]*/ private float _angularDrag;
    [SerializeField] private Vector3 _size;
    [Space(5f)]
    [SerializeField] private Rigidbody _rigidbody;
    private Vector3 _velocity;
    private Vector3 _angularVelocity;
    private Vector3 _localVelocity;
    private Vector3 _localAngularVelocity;
    private float _velocitySqrMagnitude;
    private float _velocityMagnitude;
    private float _angularVelocitySqrMagnitude;
    private float _angularVelocityMagnitude;
    private float _momentOfInertia;

#region Getters/Setters:
    /// <summary>Gets and Sets componentType property.</summary>
    public LocomotionComponent componentType
    {
        get { return _componentType; }
        set { _componentType = value; }
    }

    /// <summary>Gets and Sets updateType property.</summary>
    public UpdateType updateType
    {
        get { return _updateType; }
        set { _updateType = value; }
    }

    /// <summary>Gets and Sets mass property.</summary>
    public float mass
    {
        get { return nonKinematicRigidbodyCondition ? rigidbody.mass : _mass; }
        set
        {
            _mass = value;
            if(nonKinematicRigidbodyCondition) rigidbody.mass = _mass;
        }
    }

    /// <summary>Gets and Sets drag property.</summary>
    public float drag
    {
        get { return nonKinematicRigidbodyCondition ? rigidbody.drag : _drag; }
        set
        {
            _drag = value;
            if(nonKinematicRigidbodyCondition) rigidbody.drag = _drag;
        }
    }

    /// <summary>Gets and Sets angularDrag property.</summary>
    public float angularDrag
    {
        get { return nonKinematicRigidbodyCondition ? rigidbody.angularDrag : _angularDrag; }
        set
        {
            _angularDrag = value;
            if(nonKinematicRigidbodyCondition) rigidbody.angularDrag = _angularDrag;
        }
    }

    /// <summary>Gets and Sets velocitySqrMagnitude property.</summary>
    public float velocitySqrMagnitude
    {
        get { return _velocitySqrMagnitude; }
        set { _velocitySqrMagnitude = value; }
    }

    /// <summary>Gets and Sets velocityMagnitude property.</summary>
    public float velocityMagnitude
    {
        get { return _velocityMagnitude; }
        set { _velocityMagnitude = value; }
    }

    /// <summary>Gets and Sets angularVelocitySqrMagnitude property.</summary>
    public float angularVelocitySqrMagnitude
    {
        get { return _angularVelocitySqrMagnitude; }
        set { _angularVelocitySqrMagnitude = value; }
    }

    /// <summary>Gets and Sets angularVelocityMagnitude property.</summary>
    public float angularVelocityMagnitude
    {
        get { return _angularVelocityMagnitude; }
        set { _angularVelocityMagnitude = value; }
    }

    /// <summary>Gets and Sets momentOfInertia property.</summary>
    public float momentOfInertia
    {
        get { return _momentOfInertia; }
        private set { _momentOfInertia = value; }
    }

    /// <summary>Gets and Sets useGravity property.</summary>
    public bool useGravity
    {
        get { return rigidbodyCondition ? rigidbody.useGravity : _useGravity; }
        set
        {
            _useGravity = value;
            if(rigidbodyCondition) rigidbody.useGravity = _useGravity;
        }
    }

    /// <summary>Evaluates if this should be treated as a Rigidbody.</summary>
    public bool rigidbodyCondition { get { return componentType == LocomotionComponent.Rigidbody && rigidbody != null; } }

    /// <summary>Evaluates if this should be treated as a Non-Kinematic Rigidbody.</summary>
    public bool nonKinematicRigidbodyCondition { get { return componentType == LocomotionComponent.Rigidbody && rigidbody != null && !rigidbody.isKinematic; } }

    /// <summary>Gets and Sets velocity property.</summary>
    public Vector3 velocity
    {
        get { return nonKinematicRigidbodyCondition ? rigidbody.velocity : _velocity; }
        set
        {
            _velocity = value;
            if(nonKinematicRigidbodyCondition) rigidbody.velocity = _velocity;
        }
    }

    /// <summary>Gets and Sets angularVelocity property.</summary>
    public Vector3 angularVelocity
    {
        get { return nonKinematicRigidbodyCondition ? rigidbody.angularVelocity : _angularVelocity; }
        set
        {
            _angularVelocity = value;
            if(nonKinematicRigidbodyCondition) rigidbody.angularVelocity = _angularVelocity;
        }
    }

    /// <summary>Gets and Sets localVelocity property.</summary>
    public Vector3 localVelocity
    {
        get
        {
            _localVelocity = Quaternion.Inverse(rotation) * velocity;
            return _localVelocity;
        }
        private set
        {
            _localVelocity = value;
            _velocity = rotation * _localVelocity;
        }
    }

    /// <summary>Gets and Sets localAngularVelocity property.</summary>
    public Vector3 localAngularVelocity
    {
        get
        {
            _localAngularVelocity = Quaternion.Inverse(rotation) * angularVelocity;
            return _localAngularVelocity;
        }
        private set
        {
            _localAngularVelocity = value;
            _angularVelocity = rotation * _localAngularVelocity;
        }
    }

    /// <summary>Gets and Sets rigidbody property.</summary>
    public Rigidbody rigidbody
    {
        get { return _rigidbody; }
        set { _rigidbody = value; }
    }

    /// <summary>Gets and Sets position property.</summary>
    public Vector3 position
    {
        get { return rigidbodyCondition ? rigidbody.position : transform.position; }
        set
        {
            transform.position = value;
            if(rigidbodyCondition) rigidbody.position = value;
        }
    }

    /// <summary>Gets and Sets size property.</summary>
    public Vector3 size
    {
        get { return _size; }
        set { _size = value; }
    }

    /// <summary>Gets and Sets rotation property.</summary>
    public Quaternion rotation
    {
        get { return nonKinematicRigidbodyCondition ? rigidbody.rotation : transform.rotation; }
        set
        {
            if(nonKinematicRigidbodyCondition) rigidbody.rotation = value;
            else transform.rotation = value;
        }
    }
#endregion

    /// <summary>Draws Gizmos on Editor mode when TransformRigidbody's instance is selected.</summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(position, rotation * size);
    }

    /// <summary>Resets TransformRigidbody's instance to its default values.</summary>
    private void Reset()
    {
        mass = 1.0f;
        drag = 0.1f;
        angularDrag = 0.1f;
        useGravity = true;
    }

    /// <summary>Updates TransformRigidbody's instance at each frame.</summary>
    private void Update()
    {
        if(nonKinematicRigidbodyCondition || updateType != UpdateType.Normal) return;
        TransformRigidbodyUpdate();
    }

    /// <summary>Updates TransformRigidbody's instance at the end of each frame.</summary>
    private void LateUpdate()
    {
        if(nonKinematicRigidbodyCondition || updateType != UpdateType.Late) return;
        TransformRigidbodyUpdate();
    }

    /// <summary>Updates TransformRigidbody's instance at each Physics Thread's frame.</summary>
    private void FixedUpdate()
    {
        if(nonKinematicRigidbodyCondition || updateType != UpdateType.Fixed) return;
        TransformRigidbodyUpdate();
    }

    /// <summary>Updates TransformRigidbody.</summary>
    private void TransformRigidbodyUpdate()
    {
        momentOfInertia = Mathf.Abs(VMath.BoxMomentOfInertia(size.x, size.y, size.z, mass));
        UpdateDisplacement();
        UpdateRotation();
    }

    /// <summary>Updates Velolcity's Displacement.</summary>
    private void UpdateDisplacement()
    {
        velocity = velocity.NaNFilter();
        if(velocity.sqrMagnitude == 0.0f) return;

        if(useGravity) AddForce(Physics.gravity);

        // Apply Drag:
        if(drag > 0.0f)
        {
            AddForce(-velocity * drag);
        
        }/* else if(drag >= 1.0f && velocitySqrMagnitude > 0.0f)
        {
            velocity *= 0.0f;
        }*/

        transform.Translate(velocity * GetDeltaTime(), Space.World);

        velocitySqrMagnitude = velocity.sqrMagnitude;
        velocityMagnitude = Mathf.Sqrt(velocitySqrMagnitude);
    }

    /// <summary>Updates Rotation.</summary>
    private void UpdateRotation()
    {
        angularVelocity = angularVelocity.NaNFilter();
        if(angularVelocity.sqrMagnitude == 0.0f) return;

        // Apply Drag:
        if(angularDrag > 0.0f)
        {
            AddTorque(-angularVelocity * angularDrag, ForceMode.Impulse);
        
        }/* else if(angularDrag >= 1.0f && angularVelocitySqrMagnitude > 0.0f)
        {
            angularVelocity *= 0.0f;
        }*/

        Quaternion r = Quaternion.Euler(localAngularVelocity);
        rotation *= r;
        //transform.Rotate(r, Space.World);

        angularVelocitySqrMagnitude = angularVelocity.sqrMagnitude;
        angularVelocityMagnitude = Mathf.Sqrt(angularVelocitySqrMagnitude);
    }

    /// <summary>Moves to Rotation.</summary>
    /// <param name="r">Target Rotation as Quaternion.</param>
    public void MoveRotation(Quaternion r)
    {
        switch(componentType)
        {
            case LocomotionComponent.Transform:
                TransformMoveRotation(r);
            break;

            case LocomotionComponent.Rigidbody:
                if(rigidbody == null || (rigidbody != null && rigidbody.isKinematic))
                    TransformMoveRotation(r);
                if(rigidbody != null)
                    rigidbody.MoveRotation(r);
            break;
        }
    }

    /// <summary>Limits Local Velocity.</summary>
    /// <param name="l">Limits.</param>
    public void LimitLocalVelocity(Vector3 l)
    {
        l = l.Abs();

        Vector3 lv = localVelocity;

        lv = new Vector3(
            Mathf.Clamp(lv.x, -l.x, l.x),
            Mathf.Clamp(lv.y, -l.y, l.y),
            Mathf.Clamp(lv.z, -l.z, l.z)
        );

        localVelocity = lv;
    }

    /// <summary>Limits Local Angular-Velocity.</summary>
    /// <param name="l">Limits.</param>
    public void LimitLocalAngularVelocity(Vector3 l)
    {
        l = l.Abs();

        Vector3 lav = localAngularVelocity;

        lav = new Vector3(
            Mathf.Clamp(lav.x, -l.x, l.x),
            Mathf.Clamp(lav.y, -l.y, l.y),
            Mathf.Clamp(lav.z, -l.z, l.z)
        );

        localAngularVelocity = lav;
    }

    /// <summary>Moves Rotation as a Rigidbody [via Transform's API].</summary>
    /// <param name="rotation">Target rotation.</param>
    private void TransformMoveRotation(Quaternion rotation)
    {
        // Calculate the rotation delta and normalize it
        Quaternion deltaRotation = rotation * Quaternion.Inverse(rotation);
        deltaRotation.ToAngleAxis(out float angle, out Vector3 axis);
        axis.Normalize();

        // Apply drag to the angular velocity
        if(angularDrag > 0.0f && angularVelocitySqrMagnitude > 0.0f)
        {
            AddTorque(-angularVelocity * angularDrag);
        
        } else if(angularDrag > 1.0f & angularVelocitySqrMagnitude > 0.0f)
        {
            angularVelocity *= 0.0f;
        }

        // Apply the rotation and angular velocity
        transform.rotation = rotation;
        angularVelocity += axis * (angle * Mathf.Deg2Rad) * GetDeltaTime();
    }

    /// <summary>Moves to Position as a Rigidbody [via Transform's API].</summary>
    /// <param name="p">Target Position.</param>
    public void MovePosition(Vector3 p)
    {
        switch(componentType)
        {
            case LocomotionComponent.Transform:
                TransformMovePosition(p);
            break;

            case LocomotionComponent.Rigidbody:
                if(rigidbody == null || (rigidbody != null && rigidbody.isKinematic))
                    TransformMovePosition(p);
                if(rigidbody != null)
                    rigidbody.MovePosition(p);
            break;
        }
    }    

    /// <summary>Moves to Position as a Rigidbody [via Transform's API].</summary>
    /// <param name="p">Target Position.</param>
    private void TransformMovePosition(Vector3 p)
    {
        position = p;
    }

    [Button("Sleep")]
    /// <summary>Cancels Velocity and Angular-Velocity.</summary>
    public void Sleep()
    {
        SleepDisplacement();
        SleepRotation();
    }

    [Button("Sleep Displacement")]
    /// <summary>Cancels Velocity.</summary>
    public void SleepDisplacement()
    {
        velocity *= 0.0f;
    }

    [Button("Sleep Rotation")]
    /// <summary>Cancels Angular-Velocity.</summary>
    public void SleepRotation()
    {
        angularVelocity *= 0.0f;
    }    

    /// <param name="_rigidbody">Rigidbody's Reference.</param>
    /// <returns>Local Velocity from Rigidbody.</returns>
    public Vector3 GetLocalVelocity()
    {
        return rigidbodyCondition ? rigidbody.GetLocalVelocity() : transform.InverseTransformDirection(velocity);
    }

    /// <param name="_rigidbody">Rigidbody's Reference.</param>
    /// <returns>Local Angular Velocity from Rigidbody.</returns>
    public Vector3 GetLocalAngularVelocity()
    {
        return rigidbodyCondition ? rigidbody.GetLocalAngularVelocity() : transform.InverseTransformDirection(angularVelocity);
    }

    /// <summary>Gets proper force given the ForceMode.</summary>
    /// <param name="f">Force.</param>
    /// <param name="mode">Force Mode [ForceMode.Force by default].</param>
    public Vector3 GetForceModeForce(Vector3 f, ForceMode mode = ForceMode.Force)
    {
        switch(mode)
        {
            case ForceMode.Force:           return f = (f / mass) * GetDeltaTime();
            case ForceMode.Acceleration:    return f = f * GetDeltaTime();
            case ForceMode.Impulse:         return f = (f / mass);
            case ForceMode.VelocityChange:
            default:                        return f;
        }
    }

    [Button("Add Force")]
    /// <summary>Adds Force.</summary>
    /// <param name="force">Force.</param>
    /// <param name="mode">Force Mode [ForceMode.Force by default].</param>
    public void AddForce(Vector3 force, ForceMode mode = ForceMode.Force)
    {
        switch(nonKinematicRigidbodyCondition)
        {
            case true:
                rigidbody.AddForce(force, mode);
            break;

            case false:
                velocity += GetForceModeForce(force, mode);
            break;
        }
    }

    [Button("Add Relative Force")]
    /// <summary>Adds Force.</summary>
    /// <param name="force">Force.</param>
    /// <param name="mode">Force Mode [ForceMode.Force by default].</param>
    public void AddRelativeForce(Vector3 force, ForceMode mode = ForceMode.Force)
    {
        switch(nonKinematicRigidbodyCondition)
        {
            case true:
                rigidbody.AddRelativeForce(force, mode);
            break;

            case false:
                velocity += rotation * GetForceModeForce(force, mode);
            break;
        }
    }

    [Button("Add Torque")]
    /// <summary>Adds Torque.</summary>
    /// <param name="torque">Torque.</param>
    /// <param name="mode">Force Mode [ForceMode.Force by default].</param>
    public void AddTorque(Vector3 torque, ForceMode mode = ForceMode.Force)
    {
        switch(nonKinematicRigidbodyCondition)
        {
            case true:
                rigidbody.AddTorque(torque, mode);
            break;

            case false:
                Vector3 angularAcceleration = torque / momentOfInertia;
                angularVelocity += GetForceModeForce(angularAcceleration, mode);
            break;
        }

        //Debug.Log("[TransformRigidbody] Adding Torque...");
    }

    [Button("Add Relative Torque")]
    /// <summary>Adds relative Torque.</summary>
    /// <param name="torque">Torque.</param>
    /// <param name="mode">Force Mode [ForceMode.Force by default].</param>
    public void AddRelativeTorque(Vector3 torque, ForceMode mode = ForceMode.Force)
    {
        switch(nonKinematicRigidbodyCondition)
        {
            case true:
                rigidbody.AddRelativeTorque(torque, mode);
            break;

            case false:
                Vector3 angularAcceleration = torque / momentOfInertia;
                angularVelocity += rotation * GetForceModeForce(angularAcceleration, mode);
            break;
        }

        //Debug.Log("[TransformRigidbody] Adding Relative Torque...");
    }

    /// <returns>Proper delta-time considering the Update Type.</returns>
    public float GetDeltaTime()
    {
        switch(updateType)
        {
            case UpdateType.Normal:
            case UpdateType.Late:
            default:                    return Time.deltaTime;

            case UpdateType.Fixed:      return Time.fixedDeltaTime;
        }
    }

    /// <summary>Projects force given a Force Mode.</summary>
    public Vector3 Project(Vector3 f, float t, ForceMode mode = ForceMode.Force)
    {
        Vector3 p = position;

        switch(mode)
        {
            case ForceMode.Force:
                Vector3 forceProjection = f * (t / mass);
                return p + velocity * t + 0.5f * forceProjection * t;

            case ForceMode.Acceleration:
                Vector3 accelerationProjection = f * t;
                return p + velocity * t + 0.5f * accelerationProjection * t;

            case ForceMode.Impulse:
                Vector3 impulseProjection = f / mass;
                return p + velocity * t + impulseProjection;

            case ForceMode.VelocityChange:
                return p + f * t;

            default:
                return p;
        }
    }

    /// <returns>String representing this TransformRigidbody.</returns>
    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("Treated as a Non-Kinematic: ");
        builder.AppendLine(nonKinematicRigidbodyCondition.ToString());
        builder.Append("Velocity: ");
        builder.AppendLine(velocity.ToString());
        builder.Append("Velocity's Magnitude: ");
        builder.AppendLine(velocityMagnitude.ToString());
        builder.Append("Angular Velocity: ");
        builder.AppendLine(angularVelocity.ToString());
        builder.Append("Angular Velocity's Magnitude: ");
        builder.AppendLine(angularVelocityMagnitude.ToString());
        builder.Append("Local Velocity: ");
        builder.AppendLine(localVelocity.ToString());
        builder.Append("Local Angular Velocity: ");
        builder.AppendLine(localAngularVelocity.ToString());
        builder.Append("Moment of Inertia: ");
        builder.AppendLine(momentOfInertia.ToString());

        return builder.ToString();
    }
}
}