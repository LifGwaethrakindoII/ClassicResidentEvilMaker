using System.Collections;
using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

using Object = UnityEngine.Object;

/*===========================================================================
**
** Class:  GameObjectPool
**
** Purpose: Object-Pool class that receives as a reference an object that
** implements both a MonoBehaviour class an the IPoolObject interface.
**
**
** Author: Lîf Gwaethrakindo
**
===========================================================================*/
namespace LGG
{
	[Serializable]
	public class GameObjectPool<T> : BaseObjectPool<T> where T : MonoBehaviour, IPoolObject
	{
		private const string TAG_GROUP = "Group_";
		private const string TAG_CLONE = "(Clone)";

		private Transform _poolGroup;

		/// <summary>Gets and Sets poolGroup property.</summary>
		public Transform poolGroup
		{
			get { return _poolGroup; }
			private set { _poolGroup = value; }
		}

		/// <summary>ObjectPool's Constructor.</summary>
		/// <param name="_referenceObject">Pool's Reference Prefab.</param>
		/// <param name="_size">Pool's starting size.</param>
		/// <param name="_limit">Pool's Limit.</param>
		/// <param name="_limitHandling">How to handle when Pool's size surpasses the limit.</param>
		public GameObjectPool(T _referenceObject, int _size = 0, int _limit = int.MaxValue, LimitHandling _limitHandling = LimitHandling.None) : base(_referenceObject, _size, _limit, _limitHandling)
		{ /*...*/ }

#region Methods:
/*===================================================================================================================================================
| 	Methods:																																		|
===================================================================================================================================================*/
		/// <summary>Creates Pool-Object.</summary>
		/// <returns>Created Pool-Object.</returns>
		protected override T CreatePoolObject()
		{
			return referenceObject != null ? Object.Instantiate(referenceObject) as T : null;
		}

		/// <summary>Recycles Pool Object from queue [dequeues], then it enqueues is again.</summary>
		/// <param name="_position">Pool Object's position.</param>
		/// <param name="_rotation">Pool Object's rotation.</param>
		/// <param name="onBeforeInvokingRecycleCallback">Callback to invoke before invoking OnObjectRecycle on this recycled Pool-Object [null by default].</param>
		/// <returns>Recycled Pool Object.</returns>
		public T Recycle(Vector3 _position = default(Vector3), Quaternion _rotation = default(Quaternion), Action<T> onBeforeInvokingRecycleCallback = null)
		{
			// Passing false so the recycle callback is not invoked inside the function.
			// Instead, it is called here but after setting the position and rotation.
			T recycledObject = InternalRecycle(false);

			if(recycledObject == null)
			{
				//Debug.LogError("[GameObjectPool] Unexpected error occurred while trying to recycle Pool-Object from reference " + referenceObject.name);
				return null;
			}

			if(_rotation == default(Quaternion)) _rotation = Quaternion.identity;

			recycledObject.transform.position = _position;
			recycledObject.transform.rotation = _rotation;
			if(onBeforeInvokingRecycleCallback != null) onBeforeInvokingRecycleCallback(recycledObject);
			recycledObject.OnObjectRecycled();

			return recycledObject;
		}

		/// <summary>Regroups Pool Object into Pool's Group Transform.</summary>
		/// <param name="_poolObject">Pool Object to reparent to Group's Transform.</param>
		public void ReparentToGroup(T _poolObject)
		{
			_poolObject.transform.parent = poolGroup;
		}

		/// <summary>Creates an array of GameObjectPools from an array of PoolGameObjects.</summary>
		/// <param name="_size">Default Pools' Size.</param>
		/// <param name="_objects">Array of PoolGameObjects.</param>
		/// <returns>Array of GameObjectPools from array of PoolGameObjects.</returns>
		public static GameObjectPool<T>[] PopulatedPools(int _size = 1, params T[] _objects)
		{
			if(_objects == null || _objects.Length == 0) return null;

			int length = _objects.Length;

			GameObjectPool<T>[] pools = new GameObjectPool<T>[length];

			for(int i = 0; i < length; i++)
			{
				pools[i] = new GameObjectPool<T>(_objects[i], _size);
			}

			return pools;
		}

		/// <summary>Creates a Dictionary of GameObjectPools from array of PoolGameObjects.</summary>
		/// <param name="_size">Default Pools' Size [1 by default].</param>
		/// <param name="_references">Array of PoolGameObjects.</param>
		/// <returns>Dictionary of GameObjectPools from array of PoolGameObjects.</returns>
		public static Dictionary<int, GameObjectPool<T>> PopulatedPoolsDictionary(int _size = 1, params T[] _references)
		{
			if(_references == null || _references.Length == 0) return null;

			Dictionary<int, GameObjectPool<T>> poolsDictionary = new Dictionary<int, GameObjectPool<T>>();

			foreach(T reference in _references)
			{
				poolsDictionary.Add(reference.GetInstanceID(), new GameObjectPool<T>(reference, _size));
			}

			return poolsDictionary;
		}
#endregion

#region Callbacks:
/*===================================================================================================================================================
| 	Callbacks:																																		|
===================================================================================================================================================*/
		/// <summary>Callback internally invoked after a Pool-Object has been successfully created.</summary>
		/// <param name="_poolObject">Pool-Object.</param>
		protected override void OnPoolObjectCreated(T _poolObject)
		{
			base.OnPoolObjectCreated(_poolObject);

			_poolObject.gameObject.name = _poolObject.gameObject.name.Replace(TAG_CLONE, string.Empty);
			if(poolGroup == null) poolGroup = new GameObject(TAG_GROUP + referenceObject.name).transform;
			ReparentToGroup(_poolObject);
		}

		/// <summary>Callback when evaluation for Pool-Objects' destruction should occur.</summary>
		public override void OnObjectsToDestroyEvaluation()
		{
			bool destroyGroup = true;

			foreach(T item in this)
			{
				if(item.dontDestroyOnLoad) destroyGroup = false;
				else
				{
					item.onPoolObjectEvent -= OnPoolObjectEvent;
					item.OnObjectDestruction();
					/// In case OnObjectDestruction didn't destroy it (?)
					if(item != null) Object.Destroy(item.gameObject);
				}
			}

			if(destroyGroup)
			{
				Object.Destroy(poolGroup.gameObject);
				occupiedMap.Clear();
				vacantMap.Clear();
				vacantQueue.Clear();
			}
		}

		/// <summary>Callback invoked when a IPoolObject event occurs.</summary>
		/// <param name="_poolObject">PoolObject Invoker.</param>
		/// <param name="_event">Type of Event that occured.</param>
		protected override void OnPoolObjectEvent(IPoolObject _poolObject, PoolObjectEvent _event)
		{
			base.OnPoolObjectEvent(_poolObject, _event);
			if(_event == PoolObjectEvent.Deactivated)
			{
				T element = _poolObject as T;
				if(element != null) ReparentToGroup(element);
			}
		}
#endregion
	}
}