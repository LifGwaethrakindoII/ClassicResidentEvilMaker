using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public abstract class PoolObjectRequester<T> : MonoBehaviour, ICameraVisibleHandler where T : MonoBehaviour, IPoolObject
{
	[SerializeField] private T _requestedPoolObject; 		/// <summary>Pool Object to Request.</summary>
	[SerializeField] private float _invisibleTolerance; 	/// <summary>Description.</summary>
	[SerializeField] private bool _unableToRequest; 		/// <summary>Is this Requester unable to request a Pool Object? by default false.</summary>
	private T _poolObjectReference; 						/// <summary>Pool Object retrieved's reference.</summary>
	private bool _seenByCamera; 							/// <summary>Has this GameObject been seen by the camera?.</summary>
	protected Coroutine tolerance; 							/// <summary>Tolerance's Coroutine reference.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets requestedPoolObject property.</summary>
	public T requestedPoolObject
	{
		get { return _requestedPoolObject; }
		protected set { _requestedPoolObject = value; }
	}

	/// <summary>Gets and Sets poolObjectReference property.</summary>
	public T poolObjectReference
	{
		get { return _poolObjectReference; }
		protected set { _poolObjectReference = value; }
	}

	/// <summary>Gets and Sets invisibleTolerance property.</summary>
	public float invisibleTolerance
	{
		get { return _invisibleTolerance; }
		set { _invisibleTolerance = value; }
	}

	/// <summary>Gets and Sets unableToRequest property.</summary>
	public bool unableToRequest
	{
		get { return _unableToRequest; }
		set { _unableToRequest = value; }
	}

	/// <summary>Gets and Sets seenByCamera property.</summary>
	public bool seenByCamera
	{
		get { return _seenByCamera; }
		set { _seenByCamera = value; }
	}
#endregion

#region UnityMethods:
	/// <summary>Callback invoked when this component is enabled.</summary>
	private void OnEnable()
	{
		if(poolObjectReference != null) poolObjectReference.onPoolObjectDeactivation += OnPoolObjectDeactivated;
	}

	/// <summary>Callbak invoked when this component is disabled.</summary>
	private void OnDisable()
	{
		if(poolObjectReference != null) poolObjectReference.onPoolObjectDeactivation -= OnPoolObjectDeactivated;
	}

	/// <summary>Callback invoked when this GameObject is rendered by the camara.</summary>
	private void OnBecameVisible()
	{
		if(Application.isPlaying) RequestRecycle();
	}

	/// <summary>Callback invoked when this GameObject is not rendered by the camara.</summary>
	private void OnBecameInvisible()
	{
		if(Application.isPlaying)
		{
			seenByCamera = false;
			if(invisibleTolerance > 0.0f) this.StartCoroutine(this.WaitSeconds(invisibleTolerance,
			()=>
			{
				RequestDeactivation();
			}), ref tolerance);
			else
			RequestDeactivation();
		}
	}
#endregion

#region Callbacks:
	/// <summary>Callback invoked when this requester receives a recycled Pool Object.</summary>
	/// <param name="_poolObject">Pool Object retrieved.</param>
	protected virtual void OnRecycle(T _poolObject)
	{
		seenByCamera = true;
		unableToRequest = true;
		poolObjectReference = _poolObject;
		poolObjectReference.onPoolObjectDeactivation += OnPoolObjectDeactivated;
	}

	/// <summary>Callback invoked when this reference's Pool Object is deactivated.</summary>
	/// <param name="_poolObject">Pool Object deactivated.</param>
	protected virtual void OnPoolObjectDeactivated(IPoolObject _poolObject)
	{
		unableToRequest = false;
		poolObjectReference.onPoolObjectDeactivation -= OnPoolObjectDeactivated;
		poolObjectReference = null;

		if(DeactivationPredicate()) gameObject.SetActive(false);
	}
#endregion

	/// <summary>Deactivation Condition.</summary>
	/// <returns>Condition's Result.</returns>
	protected virtual bool DeactivationPredicate() { return false; }

	/// <summary>Requests Recycle.</summary>
	protected virtual void RequestRecycle()
	{
		if(!seenByCamera && !unableToRequest)
		{
			this.DispatchCoroutine(ref tolerance);
			if(poolObjectReference == null || poolObjectReference != null && !poolObjectReference.active)
			ObjectPoolManager.Instance.RequestRecycle(requestedPoolObject, OnRecycle, transform.position, transform.rotation);
		}
	}

	/// <summary>Requests Deactivation.</summary>
	protected virtual void RequestDeactivation()
	{
		if(poolObjectReference != null && poolObjectReference.active)
		ObjectPoolManager.Instance.RequestDeactivation(poolObjectReference);
	}
}
}