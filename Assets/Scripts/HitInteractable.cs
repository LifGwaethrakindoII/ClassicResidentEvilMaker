using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.REMaker
{
/// <summary>Event invoked when a hit occurs.</summary>
/// <param name="_source">GameObject that provoked the hit.</param>
/// <param name="_direction">Hit's direction in global space.</param>
/// <param name="_force">Force applied on the hit.</param>
public delegate void OnHitEvent(GameObject _source, Vector3 _direction, float _force);

public class HitInteractable : MonoBehaviour
{
    public event OnHitEvent onHitEvent;

    /// <summary>Invokes Hit's Event.</summary>
    /// <param name="_source">GameObject that provoked the hit.</param>
    /// <param name="_direction">Hit's direction in global space.</param>
    /// <param name="_force">Force applied on the hit.</param>
    public void InvokeHitEvent(GameObject _source, Vector3 _direction, float _force)
    {
        if(onHitEvent != null) onHitEvent(_source, _direction, _force);
    }
}
}