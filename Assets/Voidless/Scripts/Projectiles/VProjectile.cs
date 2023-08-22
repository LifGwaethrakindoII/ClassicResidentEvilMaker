using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class VProjectile : PoolGameObject
{
    [SerializeField] private float _speed;
    [SerializeField] private float _lifespan;
    private float _lifeTime;

    /// <summary>Gets and Sets speed property.</summary>
    public float speed
    {
        get { return _speed; }
        set { _speed = value; }
    }

    /// <summary>Gets and Sets lifespan property.</summary>
    public float lifespan
    {
        get { return _lifespan; }
        set { _lifespan = value; }
    }

    /// <summary>Gets and Sets lifeTime property.</summary>
    public float lifeTime
    {
        get { return _lifeTime; }
        private set { _lifeTime = value; }
    }

    /// <summary>Updates VProjectile's instance at each frame.</summary>
    private void Update()
    {
        if(lifeTime >= lifespan) OnObjectDeactivation();
        else
        {
            transform.position += (transform.forward * speed * Time.deltaTime);
            lifeTime += Time.deltaTime;
        }
    }

    /// <summary>Callback invoked when this Pool Object is being recycled.</summary>
    public override void OnObjectRecycled()
    {
        base.OnObjectRecycled();
        lifeTime = 0.0f;
    }
}
}