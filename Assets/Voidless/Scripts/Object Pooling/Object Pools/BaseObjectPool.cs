using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public abstract class BaseObjectPool<T> : IObjectPool<T>, IEnumerable<T> where T : IPoolObject
{
	private StackQueue<T> _poolStackQueue; 	/// <summary>Under the hood's dictionary pool StackQueue.</summary>
	private T _referenceObject; 			/// <summary>Pool's Reference Prefab.</summary>
	private int _limit; 					/// <summary>Pool's Limit.</summary>
	private int _occupiedSlotsCount; 		/// <summary>Occupied Slots' Count.</summary>
	private int _vacantSlotsCount; 			/// <summary>Vacant Slots' Count.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets poolStackQueue property.</summary>
	public StackQueue<T> poolStackQueue
	{
		get { return _poolStackQueue; }
		private set { _poolStackQueue = value; }
	}

	/// <summary>Gets and Sets referenceObject property.</summary>
	public T referenceObject
	{
		get { return _referenceObject; }
		set { _referenceObject = value; }
	}

	/// <summary>Gets and Sets limit property.</summary>
	public int limit
	{
		get { return _limit; }
		set { _limit = value == int.MinValue ? int.MaxValue : value; }
	}

	/// <summary>Gets and Sets occupiedSlotsCount property.</summary>
	public int occupiedSlotsCount
	{
		get { return _occupiedSlotsCount; }
		set
		{
			_occupiedSlotsCount = value;
			_vacantSlotsCount = Count - occupiedSlotsCount;
		}
	}

	/// <summary>Gets and Sets vacantSlotsCount property.</summary>
	public int vacantSlotsCount
	{
		get { return _vacantSlotsCount; }
		set { _vacantSlotsCount = value; }
	}

	/// <summary>Gets Count property.</summary>
	public int Count { get { return poolStackQueue.Count; } }

	/// <summary>Gets IsReadOnly property.</summary>
	public bool IsReadOnly { get { return false; } }
#endregion

	/// <summary>BaseObjectPool's Constructor.</summary>
	/// <param name="_referenceObject">Pool's Reference Prefab.</param>
	/// <param name="_size">Pool's starting size.</param>
	/// <param name="_limit">Pool's Limit.</param>
	public BaseObjectPool(T _referenceObject, int _size = 1, int _limit = int.MaxValue)
	{
		if(_referenceObject == null)
		{
			//Debug.LogError("[BaseObjectPool] Reference Object is null...");
			return;
		}

		poolStackQueue = new StackQueue<T>();
		referenceObject = _referenceObject;
		limit = _limit;

		for(int i = 0; i < _size; i++)
		{
			//Debug.Log("[BaseObjectPool] Adding Reference Object: " + _referenceObject.ToString());
			Add();	
		}
	}

	/// <summary>Adds Pool Object.</summary>
	/// <returns>Added Pool Object.</returns>
	public abstract T Add();

	/// <summary>Adds created Object into Pool.</summary>
	/// <param name="_poolObject">Object to add to Pool.</param>
	public virtual void Add(T _poolObject)
	{
		poolStackQueue.Enqueue(_poolObject);
		_poolObject.OnObjectCreation();
	}

	/// <summary>Recycles Pool Object from queue [dequeues], then it enqueues is again.</summary>
	/// <returns>Recycled Pool Object.</returns>
	public abstract T Recycle();

	/// <summary>Peeks from the Pool's StackQueue<T>. If there wasn't any object, the pool will try to add one.</summary>
	/// <returns>Peeked Object.</returns>
	public virtual T Peek()
	{
		T item = poolStackQueue.PeekQueue();
		if(item == null) item = Add();

		return item;
	}

	/// <summary>Deactivates provided Pool's Object, liberating it for a possible recycling.</summary>
	/// <param name="_poolObject">Pool Object to deactivate.</param>
	public virtual void Deactivate(T _poolObject)
	{
		poolStackQueue.Enqueue(_poolObject);
		_poolObject.OnObjectDeactivation();
		_poolObject.active = false;
	}

	/// <summary>Deactivates provided Pool's Object, liberating it for a possible recycling.</summary>
	/// <param name="_poolObject">Pool Object to deactivate.</param>
	public virtual void Deactivate(IPoolObject _poolObject)
	{
		if(_poolObject.GetType() == typeof(T))
		{
			poolStackQueue.Enqueue((T)_poolObject);
			_poolObject.OnObjectDeactivation();
			_poolObject.active = false;
		} else Debug.LogError("[BaseObjectPool] " + _poolObject.GetType().Name + " not type " + typeof(T).Name);
	}

	/// <summary>Dispatched Pool Object and returns it.</summary>
	/// <returns>Dispatched Pool Object.</returns>
	public virtual T Dispatch()
	{
		T dispatchedObject = poolStackQueue.Dequeue();

		if(dispatchedObject != null)
		dispatchedObject.OnObjectDestruction();

		return dispatchedObject;
	}

	/// <summary>Evaluates which objects to destroy.</summary>
	public abstract void EvaluateObjectsToDestroy();

	/// <returns>Returns an enumerator T that iterates through the collection.</returns>
	public IEnumerator<T> GetEnumerator()
	{
		return poolStackQueue.GetEnumerator();
	}

	/// <summary>Returns an enumerator that iterates through the collection.</summary>
	IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <returns>String Representing this Object's Pool.</returns>
    public override string ToString()
    {
    	StringBuilder builder = new StringBuilder();

    	builder.Append("Pool of Type: ");
    	builder.AppendLine(typeof(T).Name);
    	builder.Append("Limit: ");
    	builder.AppendLine(limit.ToString());
    	builder.Append("Occupied Slots': ");
    	builder.AppendLine(occupiedSlotsCount.ToString());
    	builder.Append("Vacant Slots': ");
    	builder.AppendLine(vacantSlotsCount.ToString());
    	builder.Append("Size: ");
    	builder.AppendLine(Count.ToString());
    	if(Count > 0)
    	{
    		int index = 0;

    		builder.AppendLine("Pool's Objects: ");
	    	builder.AppendLine("{");

	    	foreach(T item in this)
	    	{
	    		builder.Append("\tElement ");
	    		builder.Append(index.ToString());
	    		builder.Append(": ");
	    		builder.AppendLine(item.ToString());
	    		index++;
	    	}

	    	builder.Append("}");
    	}
	    else builder.Append("No Pool Objects yet registered.");

    	return builder.ToString();
    }
}
}