using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class GameObjectPool<T> : BaseObjectPool<T> where T : MonoBehaviour, IPoolObject
{
	private const string TAG_GROUP = "Group_"; 		/// <summary>Group's prefix.</summary>
	private const string TAG_CLONE = "(Clone)"; 	/// <summary>Clone's Sufix.</summary>

	private Transform _poolGroup; 					/// <summary>Pool's Group.</summary>

	/// <summary>Gets and Sets poolGroup property.</summary>
	public Transform poolGroup
	{
		get { return _poolGroup; }
		private set { _poolGroup = value; }
	}

	/// <summary>GameObjectPool's Constructor.</summary>
	/// <param name="_referenceObject">Pool's Reference Prefab.</param>
	/// <param name="_size">Pool's starting size.</param>
	/// <param name="_limit">Pool's Limit.</param>
	public GameObjectPool(T _referenceObject, int _size = 1, int _limit = int.MaxValue) : base(_referenceObject, _size, _limit)
	{
		/// \TODO Not very sure yet:
		/*if(_referenceObject != null)
		{
			T poolObject = Recycle(Vector3.zero, Quaternion.identity);
			poolObject.OnObjectDeactivation();
		}*/
	}

	/// <summary>Adds Pool Object.</summary>
	/// <returns>Added Pool Object.</returns>
	public override T Add()
	{
		T newObject = null;

		if(Count < limit)
		{
			newObject = Object.Instantiate(referenceObject) as T;
			newObject.gameObject.name = newObject.gameObject.name.Replace(TAG_CLONE, string.Empty);
			//Object.DontDestroyOnLoad(newObject.gameObject);

			//Debug.Log("[GameObjectPool] Do I have? " + newObject.ToString());

			if(Count == 0)
			{
				poolGroup = new GameObject(TAG_GROUP + referenceObject.name).transform;
				//Object.DontDestroyOnLoad(poolGroup.gameObject);
			}

			newObject.transform.SetParent(poolGroup);
			poolStackQueue.Enqueue(newObject);
		} else if(poolStackQueue.Count > 0)
		{
			Debug.LogWarning("[GameObjectPool] Pool's Size has reaches its limit, recycling instead.");
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
		return Recycle(Vector3.zero, Quaternion.identity);
	}

	/// <summary>Recycles Pool Object from queue [dequeues], then it enqueues is again.</summary>
	/// <param name="_position">Pool Object's position.</param>
	/// <param name="_rotation">Pool Object's rotation.</param>
	/// <returns>Recycled Pool Object.</returns>
	public T Recycle(Vector3 _position = default(Vector3), Quaternion _rotation = default(Quaternion))
	{
		if(_rotation == default(Quaternion)) _rotation = Quaternion.identity;
		
		T recycledObject = poolStackQueue.Count > 0 ? poolStackQueue.PeekQueue() : null;

		if(recycledObject != null && !recycledObject.active)
		{
			poolStackQueue.Dequeue();
			poolStackQueue.Enqueue(recycledObject);
		}
		else recycledObject = Add();

		recycledObject.transform.position = _position;
		recycledObject.transform.rotation = _rotation;
		recycledObject.OnObjectReset();
		return recycledObject;
	}

	/// <summary>Evaluates which objects to destroy.</summary>
	public override void EvaluateObjectsToDestroy()
	{
		bool destroyGroup = true;

		foreach(T item in poolStackQueue)
		{
			if(item.dontDestroyOnLoad) destroyGroup = false;
			else Object.Destroy(item.gameObject);
		}

		if(destroyGroup)
		{
			Object.Destroy(poolGroup.gameObject);
			poolStackQueue.Clear();
		}
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
}
}