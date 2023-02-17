using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public abstract class SceneController : Singleton<SceneController>
{
	public delegate void OnObjectsOnSceneLoaded(); 					/// <summary>Delegate called when all objects on scene are loaded.</summary>	
	public static OnObjectsOnSceneLoaded onObjectsOnSceneLoaded; 	/// <summary>OnObjectsOnSceneLoaded subscription label.</summary>

	[Header("Loadable Objects:")]
	[SerializeField] private MonoBehaviour[] _loadableObjects; 		/// <summary>Scene's Loadable Objects.</summary>
	private Behavior _objectsLoader; 								/// <summary>TillObjectsLoad Coroutine controller.</summary>
	private bool _objectsLoaded; 									/// <summary>Have all Objects on Scene been loaded?.</summary>

	/// <summary>Gets and Sets loadableObjects property.</summary>
	public MonoBehaviour[] loadableObjects
	{
		get { return _loadableObjects; }
		set { _loadableObjects = value; }
	}

	/// <summary>Gets and Sets objectsLoader property.</summary>
	public Behavior objectsLoader
	{
		get { return _objectsLoader; }
		set { _objectsLoader = value; }
	}

	/// <summary>Gets and Sets objectsLoaded property.</summary>
	public bool objectsLoaded
	{
		get { return _objectsLoaded; }
		private set { _objectsLoaded = value; }
	}

	void Awake()
	{
		if(Instance != this) Destroy(gameObject);
		else WaitTillObjectsLoad(_loadableObjects);
	}

	/// <summary>Checks for loadableObjects for having ILoadable interface, then calls the WaiObjects Coroutine.</summary>
	/// <param name="_objects">Objects to check for ILoadable containment.</param>
	protected void WaitTillObjectsLoad(params MonoBehaviour[] _objects)
	{
		List<ILoadable> confirmedLoadables = new List<ILoadable>();

		for(int i = 0; i < _objects.Length; i++)
		{
			if(_objects[i].gameObject.GetComponent<ILoadable>() != null) confirmedLoadables.Add(_objects[i].GetComponent<ILoadable>());	
		}

		_objectsLoader = new Behavior(this, TillObjectsLoad(confirmedLoadables));
	}

	/// <summary>Checks the ILoadable collection until they are loaded to call OnObjectsLoaded Method.</summary>
	/// <param name="_loadables">Collection of confirmed ILoadables on the scene.</param>
	protected IEnumerator TillObjectsLoad(List<ILoadable> _loadables)
	{
		if(_loadables.Count > 0)
		{
			while(!_loadables.AllAccomplish<ILoadable>((l)=> { return l.Loaded; }))
			{
				for(int i = 0; i < loadableObjects.Length; i++)
				{
					
				}
				yield return null;
			}
		}
		
		OnObjectsLoaded();	
		if(_objectsLoader != null) _objectsLoader.EndBehavior();
	}

	/// <summary>Method Invoked when all of the ILoadable objects are loaded.</summary>
	protected abstract void OnObjectsLoaded();
}
}