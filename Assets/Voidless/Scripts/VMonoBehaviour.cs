using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Object = UnityEngine.Object;

namespace Voidless
{
public class VMonoBehaviour : MonoBehaviour, IEqualityComparer<VMonoBehaviour>
{
    /// <summary>Reset to default values.</summary>
    public virtual void Reset() { /*...*/ }

    /// <summary>This function is called when the object becomes enabled and active.</summary>
    protected virtual void OnEnable() { /*...*/ }

    /// <summary>This function is called when the behaviour becomes disabled.</summary>
    protected virtual void OnDisable() { /*...*/ }

    /// <summary>Awake is called when the script instance is being loaded.</summary>
    protected virtual void Awake() { /*...*/ }

    /// <summary>Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.</summary>
    protected virtual void Start() { /*...*/ }

    /// <summary>Destroying the attached Behaviour will result in the game or Scene receiving OnDestroy.</summary>
    protected virtual void OnDestroy() { /*...*/ }

    /// <summary>Update is called every frame, if the MonoBehaviour is enabled.</summary>
    protected virtual void Update() { /*...*/ }

    /// <summary>LateUpdate is called every frame, if the Behaviour is enabled.</summary>
    protected virtual void LateUpdate() { /*...*/ }

    /// <summary>Frame-rate independent MonoBehaviour.FixedUpdate message for physics calculations.</summary>
    protected virtual void FixedUpdate() { /*...*/ }

#region CollisionCallbacks:
    /// <summary>OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider.</summary>
    /// <param name="_collision">The Collision data associated with this collision.</param>
    protected virtual void OnCollisionEnter(Collision _collision) { /*...*/ }

    /// <summary>OnCollisionStay is called once per frame for every collider/rigidbody that is touching rigidbody/collider.</summary>
    /// <param name="_collision">The Collision data associated with this collision.</param>
    protected virtual void OnCollisionStay(Collision _collision) { /*...*/ }

    /// <summary>OnCollisionExit is called when this collider/rigidbody has stopped touching another rigidbody/collider.</summary>
    /// <param name="_collision">The Collision data associated with this collision.</param>
    protected virtual void OnCollisionExit(Collision _collision) { /*...*/ }
#endregion

#region Collision2DCallbacks:
    /// <summary>Sent when an incoming collider makes contact with this object's collider (2D physics only).</summary>
    /// <param name="_collision">The Collision2D data associated with this collision.</param>
    protected virtual void OnCollisionEnter2D(Collision2D _collision) { /*...*/ }

    /// <summary>Sent each frame where a collider on another object is touching this object's collider (2D physics only).</summary>
    /// <param name="_collision">The Collision2D data associated with this collision.</param>
    protected virtual void OnCollisionStay2D(Collision2D _collision) { /*...*/ }

    /// <summary>Sent when a collider on another object stops touching this object's collider (2D physics only).</summary>
    /// <param name="_collision">The Collision2D data associated with this collision.</param>
    protected virtual void OnCollisionExit2D(Collision2D _collision) { /*...*/ }
#endregion

#region TriggerCallbacks:
    /// <summary>When a GameObject intersects with another GameObject, Unity calls OnTriggerEnter.</summary>
    /// <param name="_collider">Collider involved in the Trigger intersection.</param>
    protected virtual void OnTriggerEnter(Collider _collider) { /*...*/ }

    /// <summary>OnTriggerExit is called when the other Collider has stopped touching the trigger.</summary>
    /// <param name="_collider">Collider involved in the Trigger intersection.</param>
    protected virtual void OnTriggerStay(Collider _collider) { /*...*/ }

    /// <summary>OnTriggerExit is called when the other Collider has stopped touching the trigger.</summary>
    /// <param name="_collider">Collider involved in the Trigger intersection.</param>
    protected virtual void OnTriggerExit(Collider _collider) { /*...*/ }
#endregion

#region Trigger2DCallbacks:
    /// <summary>This message is sent to the trigger Collider2D and the Rigidbody2D (if any) that the trigger Collider2D belongs to, and to the Rigidbody2D (or the Collider2D if there is no Rigidbody2D) that touches the trigger.</summary>
    /// <param name="_collider">Collider2D involved in the Trigger intersection.</param>
    protected virtual void OnTriggerEnter2D(Collider2D _collider) { /*...*/ }

    /// <summary>Sent each frame where another object is within a trigger collider attached to this object (2D physics only).</summary>
    /// <param name="_collider">Collider involved in the Trigger intersection.</param>
    protected virtual void OnTriggerStay2D(Collider2D _collider) { /*...*/ }

    /// <summary>Sent when another object leaves a trigger collider attached to this object (2D physics only).</summary>
    /// <param name="_collider">Collider involved in the Trigger intersection.</param>
    protected virtual void OnTriggerExit2D(Collider2D _collider) { /*...*/ }
#endregion

#region IEqualityComparer:
    /// <summary>Determines whether the specified objects are equal.</summary>
    /// <param name="a">MonoBehaviour A.</param>
    /// <param name="b">MonoBehaviour B.</param>
    public bool Equals(VMonoBehaviour a, VMonoBehaviour b)
    {
        return a.GetInstanceID() == b.GetInstanceID();
    }

    /// <returns>Returns a hash code for the specified object.</returns>
    public int GetHashCode(VMonoBehaviour reference)
    {
        return reference.GetInstanceID().GetHashCode();
    }

    /// <summary>Checks if another object is equal to this.</summary>
    /// <param name="obj">Object to compare against.</param>
    public override bool Equals(object obj)
    {
        Object o = obj as Object;

        return (o == null) ? false : this.GetInstanceID() == o.GetInstanceID();
    }

    /// <returns>Generates a number corresponding to the value of the object to support the use of a hash table.</returns>
    public override int GetHashCode()
    {
        return this.GetInstanceID().GetHashCode();
    }
#endregion
}
}