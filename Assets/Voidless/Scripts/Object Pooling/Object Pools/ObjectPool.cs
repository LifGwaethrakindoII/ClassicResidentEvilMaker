using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class ObjectPool<T> : BaseObjectPool<T> where T : class, IPoolObject, new()
{
	/// <summary>ObjectPool's Constructor.</summary>
	/// <param name="_referenceObject">Pool's Reference Prefab.</param>
	/// <param name="_size">Pool's starting size.</param>
	/// <param name="_limit">Pool's Limit.</param>
	public ObjectPool(int _size = 0, int _limit = int.MaxValue) : base(null, _size, _limit)
	{
		/// ...
	}

	/// <summary>Adds Pool Object.</summary>
	/// <returns>Added Pool Object.</returns>
	public override T Add()
	{
		T newObject = null;

		if(Count < limit)
		{
			newObject = new T();
			poolStackQueue.Enqueue(newObject);
		} else if(poolStackQueue.Count > 0)
		{
			newObject = poolStackQueue.PeekQueue();
			if(newObject != null && !newObject.active) newObject = Recycle();
		}
		newObject.OnObjectCreation();

		return newObject;
	}
	
	/// <summary>Recycles Pool Object from queue [dequeues], then it enqueues is again.</summary>
	/// <returns>Recycled Pool Object.</returns>
	public override T Recycle()
	{
		T recycledObject = poolStackQueue.Count > 0 ? poolStackQueue.PeekQueue() : null;

		if(recycledObject != null && !recycledObject.active)
		{
			poolStackQueue.Dequeue();
			poolStackQueue.Enqueue(recycledObject);
		}
		else recycledObject = Add();

		recycledObject.OnObjectReset();

		return recycledObject;
	}
	
	/// <summary>Evaluates which objects to destroy.</summary>
	public override void EvaluateObjectsToDestroy()
	{
		/// ...
	}
}
}