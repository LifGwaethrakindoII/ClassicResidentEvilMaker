using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Voidless
{
//// Add GameObjectPool's Shit to this class.
#pragma warning disable 642
public class ObjectPoolManager : Singleton<ObjectPoolManager>, IEnumerable<KeyValuePair<int, IObjectPool<IPoolObject>>>
{
	[Space(5f)]
	[SerializeField] private SceneChangeEvents evaluatePoolsClearanceAt; 	/// <summary>Events to evaluate pools' clearance at.</summary>
	private Dictionary<int, IObjectPool<IPoolObject>> _poolDictionary; 		/// <summary>Pool's Dictionary.</summary>
	private Queue<IEnumerator> _addRequests; 								/// <summary>Pool Object Additions' Requests.</summary>
	private Queue<IEnumerator> _recycleRequests; 							/// <summary>Pool Object Recycling's Requests.</summary>
	private Queue<IEnumerator> _deactivationRequests; 						/// <summary>Pool Objects Deactivation's Requests.</summary>
	private Queue<IEnumerator> _dispatchRequests; 							/// <summary>Pool Objects Dispatch's Requests.</summary>
	private Coroutine requestIterator; 										/// <summary>Request's iterator coroutine.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets EvaluatePoolsClearanceAt property.</summary>
	public SceneChangeEvents EvaluatePoolsClearanceAt
	{
		get { return evaluatePoolsClearanceAt; }
		set { evaluatePoolsClearanceAt = value; }
	}

	/// <summary>Gets and Sets poolDictionary property.</summary>
	public Dictionary<int, IObjectPool<IPoolObject>> poolDictionary
	{
		get { return _poolDictionary; }
		private set { _poolDictionary = value; }
	}

	/// <summary>Gets and Sets addRequests property.</summary>
	public Queue<IEnumerator> addRequests
	{
		get { return _addRequests; }
		private set { _addRequests = value; }
	}

	/// <summary>Gets and Sets recycleRequests property.</summary>
	public Queue<IEnumerator> recycleRequests
	{
		get { return _recycleRequests; }
		private set { _recycleRequests = value; }
	}

	/// <summary>Gets and Sets deactivationRequests property.</summary>
	public Queue<IEnumerator> deactivationRequests
	{
		get { return _deactivationRequests; }
		set { _deactivationRequests = value; }
	}

	/// <summary>Gets and Sets dispatchRequests property.</summary>
	public Queue<IEnumerator> dispatchRequests
	{
		get { return _dispatchRequests; }
		set { _dispatchRequests = value; }
	}
#endregion

#region UnityMethods:
	/// <summary>Subscribes to SceneManager's events.</summary>
	private void OnEnable()
	{
		SceneManager.sceneUnloaded += OnSceneUnloaded;
		SceneManager.sceneLoaded += OnSceneLoaded;
		SceneManager.activeSceneChanged += OnActiveSceneChanged;
	}

	/// <summary>Unsubscribes to SceneManager's events.</summary>
	private void OnDisable()
	{
		SceneManager.sceneUnloaded -= OnSceneUnloaded;
		SceneManager.sceneLoaded -= OnSceneLoaded;
		SceneManager.activeSceneChanged -= OnActiveSceneChanged;
	}

	/// <summary>Callback called on Awake if this Object is the Singleton's Instance.</summary>
   	protected override void OnAwake()
	{
		poolDictionary = new Dictionary<int, IObjectPool<IPoolObject>>();
		addRequests = new Queue<IEnumerator>();
		recycleRequests = new Queue<IEnumerator>();
		deactivationRequests = new Queue<IEnumerator>();
		dispatchRequests = new Queue<IEnumerator>();
		this.StartCoroutine(UpdateRequests(), ref requestIterator);
	}
#endregion

#region MainCalls:
	/// <summary>Creates GameObject's Pool.</summary>
	/// <param name="_poolObject">Object's Prefab reference.</param>
	/// <param name="_size">Pool's size [0 as default].</param>
	/// <param name="_limit">Pool's Limit [Maximum's Value as default].</param>
	public void CreateGameObjectsPool<T>(T _poolObject, int _size = 0, int _limit = int.MaxValue) where T : MonoBehaviour, IPoolObject
	{
		int instanceID = _poolObject.gameObject.GetInstanceID();

#if UNITY_5

#elif UNITY_2017_1

#endif

		if(!poolDictionary.ContainsKey(instanceID)) poolDictionary.Add(instanceID, new GameObjectPool<T>(_poolObject, _size, _limit));
		else
		DebugMessage(instanceID, LogType.Warning, "No Object Pool will be created.", true);
	}

	/// <summary>Creates GameObject's Pool if it does not exist.</summary>
	/// <param name="_poolObject">Object's Prefab reference.</param>
	public void CreateGameObjectsPoolIfNotCreated<T>(T _poolObject) where T : MonoBehaviour, IPoolObject
	{
		int instanceID = _poolObject.gameObject.GetInstanceID();

#if UNITY_5

#elif UNITY_2017_1

#endif

		if(!poolDictionary.ContainsKey(instanceID)) poolDictionary.Add(instanceID, new GameObjectPool<T>(_poolObject));
		else
		DebugMessage(instanceID, LogType.Warning, "No Object Pool will be created.", true);
	}

	/// <summary>Adds Pool's Object.</summary>
	/// <param name="_poolObject">Object's Prefab reference.</param>
	/// <returns>Added Pool's Object.</returns>
	public T Add<T>(T _poolObject) where T : MonoBehaviour, IPoolObject
	{
		CreateGameObjectsPoolIfNotCreated<T>(_poolObject);
		return _poolObject != null ? Add<T>(_poolObject.gameObject.GetInstanceID()) : null;
	}

	/// <summary>Adds Pool's Object.</summary>
	/// <param name="_instanceID">Instance ID of the Pool's Object reference.</param>
	/// <returns>Added Pool's Object.</returns>
	public T Add<T>(int _instanceID) where T : MonoBehaviour, IPoolObject
	{
		if(poolDictionary.ContainsKey(_instanceID)) return poolDictionary[_instanceID].Add() as T;
		else return null;
	}

	/// <summary>Recycles Pool's Object.</summary>
	/// <param name="_poolObject">Object's Prefab reference.</param>
	/// <param name="_position">Object's Position upon recycling [by default Vector3.zero].</param>
	/// <param name="_rotation">Object's Rotation upon recycling [by default Quaternion.identity].</param>
	/// <returns>Recycled Pool's Object [if Pool was registered on Dictionary].</returns>
	public T Recycle<T>(T _poolObject, Vector3 _position = default(Vector3), Quaternion _rotation = default(Quaternion)) where T : MonoBehaviour, IPoolObject
	{
		CreateGameObjectsPoolIfNotCreated<T>(_poolObject);
		if(_rotation == default(Quaternion)) _rotation = Quaternion.identity;
		return _poolObject != null ? Recycle<T>(_poolObject.gameObject.GetInstanceID(), _position, _rotation) : null;
	}

	/// <summary>Recycles Pool's Object.</summary>
	/// <param name="_instanceID">Instance ID of the Pool's Object reference.</param>
	/// <param name="_position">Object's Position upon recycling [by default Vector3.zero].</param>
	/// <param name="_rotation">Object's Rotation upon recycling [by default Quaternion.identity].</param>
	/// <returns>Recycled Pool's Object [if Pool was registered on Dictionary].</returns>
	public T Recycle<T>(int _instanceID, Vector3 _position = default(Vector3), Quaternion _rotation = default(Quaternion)) where T : MonoBehaviour, IPoolObject
	{
		if(_rotation == default(Quaternion)) _rotation = Quaternion.identity;
		if(poolDictionary.ContainsKey(_instanceID))
		{
			T recycledObject = poolDictionary[_instanceID].Recycle() as T;
			recycledObject.transform.position = _position;
			recycledObject.transform.rotation = _rotation;
			return recycledObject;
		}
		else
		{

			DebugMessage(_instanceID, LogType.Error, "Returning null.");
			return null;
		}
	}

	/// <summary>Deactivates Pool's Object.</summary>
	/// <param name="_poolObject">Object's Prefab reference.</param>
	public void Deactivate<T>(T _poolObject) where T : MonoBehaviour, IPoolObject
	{
		int instanceID = _poolObject.gameObject.GetInstanceID();

		if(poolDictionary.ContainsKey(instanceID)) poolDictionary[instanceID].Deactivate(_poolObject);
		else
		DebugMessage(instanceID, LogType.Warning, "No deactivation will be made.");
	}

	/// <summary>Dispatches Pool's Object.</summary>
	/// <param name="_poolObject">Object's Prefab reference.</param>
	public void Dispatch<T>(T _poolObject) where T : MonoBehaviour, IPoolObject
	{
		Dispatch<T>(_poolObject.gameObject.GetInstanceID());
	}

	/// <summary>Dispatches Pool's Object.</summary>
	/// <param name="_instanceID">Instance ID of the Pool's Object reference.</param>
	public void Dispatch<T>(int _instanceID) where T : MonoBehaviour, IPoolObject
	{
		if(poolDictionary.ContainsKey(_instanceID)) poolDictionary[_instanceID].Dispatch();
		else
		DebugMessage(_instanceID, LogType.Warning,"No dispatchment will be made.");
	}
#endregion

	/// <summary>Evaluates all pools' clearance.</summary>
	private void EvaluatePoolsClearance()
	{
		foreach(KeyValuePair<int, IObjectPool<IPoolObject>> pair in this)
		{
			pair.Value.EvaluateObjectsToDestroy();
		}
	}

#region PrivateMethods:
	/// <summary>Updates provided request's queue.</summary>
	/// <param name="_requestQueue">Requests' queue to update.</param>
	private void UpdateRequestQueue(Queue<IEnumerator> _requestQueue)
	{
		if(_requestQueue.Count > 0)
		while(_requestQueue.Count > 0)
		{
			IEnumerator requestIterator = _requestQueue.Dequeue();
			while(requestIterator.MoveNext());
		}
	}

	/// <summary>Debugs message at console.</summary>
	/// <param name="_instanceID">Instance's ID.</param>
	/// <param name="_type">Type of message to debug.</param>
	/// <param name="_additionalMessage">Additional message to provide.</param>
	/// <param name="_contains">Is the instance ID already registered on Pool's Dictionary?.</param>
	private void DebugMessage(int _instanceID, LogType _type, string _additionalMessage = null, bool _contains = false)
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("[ObjectPoolManager] ");
		if(_contains)
		{
			builder.Append("Pool Dictionary Already Contains an Entry of ID: ");
			builder.Append(_instanceID.ToString());
			builder.Append(". ");
		}
		else
		{
			builder.Append("Key with ID: ");
			builder.Append(_instanceID.ToString());
			builder.Append(" not registered on dictionary. ");
		}
		if(!string.IsNullOrEmpty(_additionalMessage)) builder.Append(_additionalMessage);

		switch(_type)
		{
			case LogType.Log:
			break;

			case LogType.Warning:
			break;

			case LogType.Error:
			break;
		}
	}
#endregion

	/*/// <summary>Gets count of pool containing given instance.</summary>
	/// <param name="_poolObject">Pool Object.</param>
	/// <returns>Count of number of elements recycled from given instance, 0 if the pool object is not registered on dictionary.</returns>
	public int GetCount<T>(T _poolObject) where T : MonoBehaviour, IPoolObject
	{
		int instanceID = _poolObject.gameObject.GetInstanceID();

		return poolDictionary.ContainsKey(instanceID) ? poolDictionary[instanceID].Count : 0;
	}*/

	/// <returns>Returns an enumerator T that iterates through the collection.</returns>
	public IEnumerator<KeyValuePair<int, IObjectPool<IPoolObject>>> GetEnumerator()
	{
		return poolDictionary.GetEnumerator();
	}

	/// <summary>Returns an enumerator that iterates through the collection.</summary>
	IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

	/// <returns>String representing this Object Pool's Manager.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.AppendLine("Object Pool's Manager Data:");
		builder.AppendLine();
		builder.Append("Current Addition Requests' Count: ");
		builder.AppendLine(addRequests.Count.ToString());
		builder.Append("Current Recycle Requests' Count: ");
		builder.AppendLine(recycleRequests.Count.ToString());
		builder.Append("Current Dispatch Requests' Count: ");
		builder.AppendLine(dispatchRequests.Count.ToString());
		if(poolDictionary.Count > 0)
		{
			int index = 0;

			builder.AppendLine("Pool's Registered: ");
			builder.AppendLine("\n");

			foreach(KeyValuePair<int, IObjectPool<IPoolObject>> pair in this)
			{
				builder.Append("Pool #");
				builder.Append(index.ToString());
				builder.Append(": ");
				builder.AppendLine(pair.Value.ToString());
				builder.AppendLine();

				index++;
			}
		}
		else builder.Append("No Pools yet registered.");

		return builder.ToString();
	}

#region Requests:
	/// <summary>Requests addition.</summary>
	/// <param name="_poolObject">Object's prefab reference.</param>
	/// <param name="onAdd">Callbakc to invoke when the OBject is added.</param>
	public void RequestAdd<T>(T _poolObject, Action<T> onAdd) where T : MonoBehaviour, IPoolObject
	{
		CreateGameObjectsPoolIfNotCreated<T>(_poolObject);
		int instanceID = _poolObject.gameObject.GetInstanceID();

		if(poolDictionary.ContainsKey(instanceID)) addRequests.Enqueue(AddAtEndOfFrame<T>(_poolObject, onAdd));
		else
		DebugMessage(instanceID, LogType.Error, "No addition will be made.");
	}

	/// <summary>Requests recycle.</summary>
	/// <param name="_poolObject">Object's Prefab reference.</param>
	/// <param name="onRecycle">Callback to invoke when the Object is recycled.</param>
	/// <param name="_position">Object's Position upon recycling [by default Vector3.zero].</param>
	/// <param name="_rotation">Object's Rotation upon recycling [by default Quaternion.identity].</param>
	public void RequestRecycle<T>(T _poolObject, Action<T> onRecycle, Vector3 _position = default(Vector3), Quaternion _rotation = default(Quaternion)) where T : MonoBehaviour, IPoolObject
	{
		CreateGameObjectsPoolIfNotCreated<T>(_poolObject);
		int instanceID = _poolObject.gameObject.GetInstanceID();

		if(poolDictionary.ContainsKey(instanceID))
		{
			if(_rotation == default(Quaternion)) _rotation = Quaternion.identity;
			recycleRequests.Enqueue(RecycleAtEndOfFrame<T>(_poolObject, onRecycle, _position, _rotation));
		}
		else
		DebugMessage(instanceID, LogType.Error, "No recycle will be made.");
	}

	/// <summary>Requests deactivation.</summary>
	/// <param name="_poolObject">Object's prefab reference.</param>
	public void RequestDeactivation<T>(T _poolObject) where T : MonoBehaviour, IPoolObject
	{
		CreateGameObjectsPoolIfNotCreated<T>(_poolObject);
		int instanceID = _poolObject.gameObject.GetInstanceID();

		if(poolDictionary.ContainsKey(instanceID)) deactivationRequests.Enqueue(DeactivateAtEndOfFrame(_poolObject));
		else
		DebugMessage(instanceID, LogType.Error, "No deactivation will be made.");
	}

	/// <summary>Requests recycle.</summary>
	/// <param name="_poolObject">Object's Prefab reference.</param>
	public void RequestDispatch<T>(T _poolObject) where T : MonoBehaviour, IPoolObject
	{
		CreateGameObjectsPoolIfNotCreated<T>(_poolObject);
		int instanceID = _poolObject.gameObject.GetInstanceID();

		if(poolDictionary.ContainsKey(instanceID)) dispatchRequests.Enqueue(DispatchAtEndOfFrame<T>(_poolObject));
		else
		DebugMessage(instanceID, LogType.Error, "No dispatchment will be made.");
	}
#endregion

#region SceneCallbacks:
	/// <summary>Callback invoked when a scene is unloaded.</summary>
	/// <param name="_currentScene">Current Scene.</param>
	private void OnSceneUnloaded(Scene _currentScene)
	{
		if(Application.isPlaying && evaluatePoolsClearanceAt.HasFlag(SceneChangeEvents.SceneUnloaded))
		EvaluatePoolsClearance();
	}

	/// <summary>Callbakc invoked when a scene is loaded.</summary>
	/// <param name="_scene">Scene Loaded.</param>
	/// <param name="_mode">Loading's mode.</param>
	private void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
    	if(Application.isPlaying && evaluatePoolsClearanceAt.HasFlag(SceneChangeEvents.SceneLoaded))
    	EvaluatePoolsClearance();
    }

    /// <summary>Callback invoked when the active scene changed.</summary>
    /// <param name="_currentScene">Current Scene.</param>
    /// <param name="_nextScene">Next Scene.</param>
    private void OnActiveSceneChanged(Scene _currentScene, Scene _nextScene)
    {
    	if(Application.isPlaying && evaluatePoolsClearanceAt.HasFlag(SceneChangeEvents.ActiveSceneChanged))
    	EvaluatePoolsClearance();
    }
#endregion

#region Iterators:
	/// <summary>Updates and attends all requests at end of frame.</summary>
	private IEnumerator UpdateRequests()
	{
		while(true)
		{
			WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

			yield return waitForEndOfFrame;

			UpdateRequestQueue(addRequests);
			UpdateRequestQueue(recycleRequests);
			UpdateRequestQueue(deactivationRequests);
			UpdateRequestQueue(dispatchRequests);
		}
	}

	/// <summary>Adds Pool's Object at end of frame.</summary>
	/// <param name="_poolObject">Object's Prefab reference.</param>
	/// <param name="onAdd">Callback to invoke when the Object is added.</param>
	private IEnumerator AddAtEndOfFrame<T>(T _poolObject, Action<T> onAdd) where T : MonoBehaviour, IPoolObject
	{
		int instanceID = _poolObject.gameObject.GetInstanceID();

		if(poolDictionary.ContainsKey(instanceID))
		{
			T addedObject = Add<T>(_poolObject);
			if(onAdd != null) onAdd(addedObject);
		}
		else
		DebugMessage(instanceID, LogType.Error, "No addition will be made");

		yield return null;
	}

	/// <summary>Recycles Pool's Object at the end of frame.</summary>
	/// <param name="_poolObject">Object's Prefab reference.</param>
	/// <param name="onRecycle">Callback to invoke when the Object is recycled.</param>
	/// <param name="_position">Object's Position upon recycling [by default Vector3.zero].</param>
	/// <param name="_rotation">Object's Rotation upon recycling [by default Quaternion.identity].</param>
	private IEnumerator RecycleAtEndOfFrame<T>(T _poolObject, Action<T> onRecycle, Vector3 _position, Quaternion _rotation) where T : MonoBehaviour, IPoolObject
	{
		int instanceID = _poolObject.gameObject.GetInstanceID();

		if(poolDictionary.ContainsKey(instanceID))
		{
			T recycledObject = Recycle<T>(instanceID, _position, _rotation);
			if(onRecycle != null) onRecycle(recycledObject);
		}
		else
		DebugMessage(instanceID, LogType.Error, "No recycle will be made");

		yield return null;
	}

	/// <summary>Deactivates Pool's Object at end of frame.</summary>
	/// <param name="_poolObject">Object's Prefab reference.</param>
	private IEnumerator DeactivateAtEndOfFrame<T>(T _poolObject) where T : MonoBehaviour, IPoolObject
	{
		int instanceID = _poolObject.gameObject.GetInstanceID();

		if(poolDictionary.ContainsKey(instanceID)) Deactivate<T>(_poolObject);
		else
		DebugMessage(instanceID, LogType.Error, "No deactivation will be made");

		yield return null;
	}

	/// <summary>Dispatches Pool's Object at the end of frame.</summary>
	/// <param name="_poolObject">Object's Prefab reference.</param>
	private IEnumerator DispatchAtEndOfFrame<T>(T _poolObject) where T : MonoBehaviour, IPoolObject
	{
		int instanceID = _poolObject.gameObject.GetInstanceID();

		if(poolDictionary.ContainsKey(instanceID)) Dispatch<T>(instanceID);
		else
		DebugMessage(instanceID, LogType.Error, "No dispatchment will be made");

		yield return null;
	}
#endregion
}
}