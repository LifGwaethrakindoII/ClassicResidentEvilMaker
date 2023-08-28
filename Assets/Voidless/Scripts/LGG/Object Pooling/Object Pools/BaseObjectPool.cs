using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Sirenix.OdinInspector;

/*===========================================================================
**
** Class:  BaseObjectPool<T>
**
** Purpose: Base generic structure that acts as an Object Pool. The generic
** type T expects an object that implements the IPoolObject interface.
**
**
** Author: Lîf Gwaethrakindo
**
===========================================================================*/

namespace LGG
{
	/// \TODO Return additional result info when requesting Pool-Objects.
	/// Both internally and from requester classes.
	/// Also, a better nomenclature (will leave this as placeholder of the  states I want to express).
	[Flags]
	public enum PoolObjectResult
	{
		Success = 1,
		Failure = 2,
		LimitReached = 4,
		DequeuedOldedstAvailable = 8,
		Error = 16,

		SuccessAndLimitReached = Success | LimitReached,
		LimitReachedAndDequeuedOldedstAvailable = LimitReached | DequeuedOldedstAvailable
	}

	[Serializable]
	public abstract class BaseObjectPool<T> : IObjectPool<T>, IEnumerable<T> where T : class, IPoolObject
	{
		//[InfoBox("@ToString()")]
		[SerializeField] private T _referenceObject;
		[SerializeField] private int _limit = 1000;
		[SerializeField] private LimitHandling _limitHandling;
		private HashSet<T> _occupiedMap;
		private HashSet<T> _vacantMap;
		private Queue<T> _vacantQueue;

#region Getters/Setters:
		/// <summary>Gets and Sets referenceObject property.</summary>
		public T referenceObject
		{
			get { return _referenceObject; }
			set { _referenceObject = value; }
		}

		/// <summary>Gets and Sets limitHandling property.</summary>
		public LimitHandling limitHandling
		{
			get { return _limitHandling; }
			set { _limitHandling = value; }
		}

		/// <summary>Gets and Sets occupiedMap property.</summary>
		public HashSet<T> occupiedMap
		{
			get { return _occupiedMap; }
			private set { _occupiedMap = value; }
		}

		/// <summary>Gets and Sets vacantMap property.</summary>
		public HashSet<T> vacantMap
		{
			get { return _vacantMap; }
			private set { _vacantMap = value; }
		}

		/// <summary>Gets and Sets vacantQueue property.</summary>
		public Queue<T> vacantQueue
		{
			get { return _vacantQueue; }
			private set { _vacantQueue = value; }
		}

		/// <summary>Gets and Sets limit property.</summary>
		public int limit
		{
			get { return _limit; }
			set { _limit = Mathf.Max(value, 0); }
		}

		/// <summary>Gets occupiedSlotsCount property.</summary>
		public int occupiedSlotsCount { get { return occupiedMap != null ? occupiedMap.Count : 0; } }

		/// <summary>Gets vacantSlotsCount property.</summary>
		public int vacantSlotsCount { get { return vacantQueue != null ? vacantQueue.Count : 0; } }

		/// <summary>Gets Count property.</summary>
		public int Count { get { return occupiedSlotsCount + vacantSlotsCount; } }

		/// <summary>Gets IsReadOnly property.</summary>
		public bool IsReadOnly { get { return false; } }
#endregion

		/// <summary>ObjectPool's Constructor.</summary>
		/// <param name="_referenceObject">Pool's Reference Prefab.</param>
		/// <param name="_size">Pool's starting size.</param>
		/// <param name="_limit">Pool's Limit.</param>
		/// <param name="_limitHandling">How to handle when Pool's size surpasses the limit.</param>
		public BaseObjectPool(T _referenceObject, int _size = 1, int _limit = int.MaxValue, LimitHandling _limitHandling = LimitHandling.None)
		{
			Initialize(_referenceObject, _size, _limit, _limitHandling);
		}

#region Methods&Functions:
/*===================================================================================================================================================
| 	Methods & Functions:																															|
===================================================================================================================================================*/
		/// <summary>Initializes Object-Pool [Use this if you are serializing your pool in Inspector]. If you won't inject values in the inspector, use the constructor instead.</summary>
		/// <param name="_referenceObject">Pool's Reference Prefab.</param>
		/// <param name="_size">Pool's starting size.</param>
		/// <param name="_limit">Pool's Limit.</param>
		/// <param name="_limitHandling">How to handle when Pool's size surpasses the limit.</param>
		public void Initialize(T _referenceObject, int _size = 1, int _limit = int.MaxValue, LimitHandling _limitHandling = LimitHandling.None)
		{
			referenceObject = _referenceObject;
			limit = _limit;
			limitHandling = _limitHandling;
			Initialize(_size);
		}

		/// <summary>Initializes Object-Pool [Use this if you are serializing your pool in Inspector]. If you won't inject values in the inspector, use the constructor instead.</summary>
		/// <param name="_size">Pool's Starting Size [1 by default].</param>
		public void Initialize(int _size = 1)
		{
			occupiedMap = new HashSet<T>();
			vacantMap = new HashSet<T>();
			vacantQueue = new Queue<T>();

			for(int i = 0; i < _size; i++)
			{
				Add();	
			}
		}

		/// <summary>Creates Pool-Object.</summary>
		/// <returns>Created Pool-Object.</returns>
		protected abstract T CreatePoolObject();

		/// <summary>Internal Add's routine. Allows for setting whether to enqueue the created Pool-Object. Adds Pool Object into the vacant slots.</summary>
		/// <param name="_enqueue">Enqueue to Vacant's queue? true by default.</param>
		/// <returns>Added Pool Object.</returns>
		protected virtual T InternalAdd(bool _enqueue = true)
		{
			/*
			- If the size of the occupied slots does not surpass the limit:
				- Create a reproduction of the reference object [with CreatePoolObject()]
				- Enqueue the new created reproduction into the queue.
			- If, however, the occupied slots the limit:
				- Fetch for a Pool-Object inside the queue that is currently inactive.
				- If there was an available Pool-Object, use that as a fake new Pool-Object.
			*/
			bool slotsAvailable = Count < limit;
			T newObject = slotsAvailable ? CreatePoolObject() : HandleLimit();
			
			if(newObject != null && slotsAvailable)
			{
				/* 
				- If slots were available when obtaining a Pool-Object, it means this object was not recycled.
				- In that case, register for the first time this Pool-Object with OnPoolObjectCreated's callback.
				- Otherwise, this Pool-Object was recycled, so it is not necessary to register it with the callback.
				*/
				OnPoolObjectCreated(newObject);
				if(_enqueue) Enqueue(newObject);
			}

			return newObject;
		}

		/// \TODO TEMPORAL?
		/// <summary>Adds existing instance into this pool.</summary>
		/// <param name="_poolObject">Instance to register to this pool.</param>
		/// <param name="_enqueue">Enqueue? This will deactivate it and register it to the vacant slots, otherwise it will be registered into the occupied slots. True by default.</param>
		public virtual void Add(T _poolObject, bool _enqueue = true)
		{
			if(occupiedMap.Contains(_poolObject) || vacantMap.Contains(_poolObject)) return;
			OnPoolObjectCreated(_poolObject);

			switch(_enqueue)
			{
				case true:
					Enqueue(_poolObject);
				break;

				case false:
					occupiedMap.Add(_poolObject);
				break;
			}
		}

		/// <summary>Adds Pool Object into the vacant slots.</summary>
		/// <returns>Added Pool Object.</returns>
		public virtual T Add()
		{
			return InternalAdd();
		}

		/// <summary>Recycles Pool Object from queue [dequeues], then it enqueues is again.</summary>
		/// <returns>Recycled Pool Object.</returns>
		public virtual T Recycle()
		{
			return InternalRecycle();
		}

		/// <summary>Internal Recycle's routine. Allows for setting whether to invoke recycled Pool-Object's OnObjectRecycled.</summary>
		/// <param name="_invokeRecycleCallback">Invoke Pool-Object's OnObjectRecycled? true by default.</param>
		protected virtual T InternalRecycle(bool _invokeRecycleCallback = true)
		{
			/*
			- If all slots in the Queue are occupied, create a new reproduction and use that as a recycled Pool-Object.
			- Else, it is asumed that there are vacant slots, and an element from there will be dequeued to be recycled.
			*/
			bool noVacants = vacantSlotsCount == 0;
			T recycledObject = noVacants ? InternalAdd(false) : Dequeue();

			if(recycledObject == null) return null;

			occupiedMap.Add(recycledObject);
			if(_invokeRecycleCallback) recycledObject.OnObjectRecycled();

			return recycledObject;
		}

		/// <summary>Deactivates provided Pool's Object, liberating it for a possible recycling.</summary>
		/// <param name="_poolObject">Pool Object to deactivate.</param>
		public virtual void Deactivate(T _poolObject)
		{
			Enqueue(_poolObject);
			_poolObject.OnObjectDeactivation();
			_poolObject.active = false;
		}

		/// <summary>Deactivates provided Pool's Object, liberating it for a possible recycling.</summary>
		/// <param name="_poolObject">Pool Object to deactivate.</param>
		public virtual void Deactivate(IPoolObject _poolObject)
		{
			if(_poolObject.GetType() == typeof(T))
			{
				Enqueue((T)_poolObject);
				_poolObject.OnObjectDeactivation();
				_poolObject.active = false;
			}
			else Debug.LogError("[BaseObjectPool] " + _poolObject.GetType().Name + " not type " + typeof(T).Name);
		}

		/// <summary>Dispatched Pool Object and returns it.</summary>
		/// <returns>Dispatched Pool Object.</returns>
		public virtual T Dispatch()
		{
			if(Count == 0) return null;

			T dispatchedObject = vacantSlotsCount > 0 ? Dequeue() : FetchForRandomActivePoolObject();

			if(dispatchedObject != null)
			dispatchedObject.OnObjectDestruction();

			return dispatchedObject;
		}

		/// <summary>Handles Pool's Limit.</summary>
		/// <returns>Pool-Object given the set way to handle limit.</returns>
		public virtual T HandleLimit()
		{
			switch(limitHandling)
			{
				/// \TODO Make also an occupiedQueue and instead of fetching for a random HashSet element, dequeue from that queue.
				/// (under the assumption that the first element is the eldest of the requested Pool-Objects).
				case LimitHandling.RecycleRandom: 	return FetchForRandomActivePoolObject();
				case LimitHandling.None:
				default: 							return null;
			}
		}

		/// <summary>Enqueues Pool-Object.</summary>
		/// <param name="_poolObject">Pool-Object to Enqueue.</param>
		protected virtual void Enqueue(T _poolObject)
		{
			occupiedMap.Remove(_poolObject);
			if(vacantMap.Contains(_poolObject)) return;
			vacantMap.Add(_poolObject);
			vacantQueue.Enqueue(_poolObject);
		}

		/// <summary>Dequeues first Pool-Object inside the Queue, if it was active it will also decrease the occupied slots' count.</summary>
		/// <returns>Dequeued Pool-Object.</returns>
		protected virtual T Dequeue()
		{
			T poolObject = vacantSlotsCount > 0 ? vacantQueue.Dequeue() : null;
			if(poolObject != null) vacantMap.Remove(poolObject);
			return poolObject;
		}

		/// <summary>Fetches for a random active Pool-Object.</summary>
		/// <returns>First Pool-Object retreived from Active's mapping.</returns>
		protected virtual T FetchForRandomActivePoolObject()
		{
			if(occupiedSlotsCount == 0) return null;

			HashSet<T> temp = new HashSet<T>(occupiedMap);

			foreach(T item in temp)
			{
				occupiedMap.Remove(item);

				if(item == null) continue;
				else return item;
			}

			return null;
		}
#endregion

#region Callbacks:
/*===================================================================================================================================================
| 	Callbacks:																																		|
===================================================================================================================================================*/
		/// <summary>Callback internally invoked after a Pool-Object has been successfully created.</summary>
		/// <param name="_poolObject">Pool-Object Created.</param>
		protected virtual void OnPoolObjectCreated(T _poolObject)
		{
			_poolObject.OnObjectCreation();
			_poolObject.onPoolObjectEvent -= OnPoolObjectEvent;
			_poolObject.onPoolObjectEvent += OnPoolObjectEvent;
		}

		/// <summary>Callback invoked when a IPoolObject event occurs.</summary>
		/// <param name="_poolObject">PoolObject Invoker.</param>
		/// <param name="_event">Type of Event that occured.</param>
		protected virtual void OnPoolObjectEvent(IPoolObject _poolObject, PoolObjectEvent _event)
		{
			T element = _poolObject as T;

			if(element == null) return;

			switch(_event)
			{
				case PoolObjectEvent.Deactivated:
					Enqueue(element);
				break;

				case PoolObjectEvent.Destroyed:
					element.onPoolObjectEvent -= OnPoolObjectEvent;
					occupiedMap.Remove(element);
				break;
			}
		}

		/// <summary>Callback when evaluation for Pool-Objects' destruction should occur.</summary>
		public virtual void OnObjectsToDestroyEvaluation() { /*...*/ }
#endregion

		/// <returns>Returns an enumerator T that iterates through the collection.</returns>
		public IEnumerator<T> GetEnumerator()
		{
			foreach(T poolObject in vacantQueue)
			{
				yield return poolObject;
			}

			foreach(T poolObject in occupiedMap)
			{
				yield return poolObject;
			}
		}

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		IEnumerator IEnumerable.GetEnumerator()
	    {
	        return GetEnumerator();
	    }

	    /// <param name="_queue">Pool Queue to represent as string.</param>
	    /// <returns>String representing provided Pool Queue.</returns>
	    private string PoolQueueToString(Queue<T> _queue)
	    {
	    	if(_queue == null || _queue.Count == 0) return "Empty";

	    	StringBuilder builder = new StringBuilder();

	    	int index = 0;

	    	builder.AppendLine("{");

	    	foreach(T item in _queue)
	    	{
	    		if(item == null) continue;

	    		builder.Append("\tElement ");
	    		builder.Append(index.ToString());
	    		builder.Append(": ");
	    		builder.AppendLine(item.ToString());
	    		index++;
	    	}

	    	builder.Append("}");

	    	return builder.ToString();
	    }

	    /// <returns>String Representing this Object's Pool.</returns>
	    public override string ToString()
	    {
	    	StringBuilder builder = new StringBuilder();

	    	builder.Append("Pool of Type: ");
	    	builder.AppendLine(typeof(T).Name);
	    	builder.Append("Current Pool's Size: ");
	    	builder.AppendLine(Count.ToString());
	    	builder.Append("Occupied Slots: ");
	    	builder.AppendLine(occupiedSlotsCount.ToString());
	    	builder.Append("Vacant Slots: ");
	    	builder.Append(vacantSlotsCount.ToString());

	    	return builder.ToString();
	    }
	}
}