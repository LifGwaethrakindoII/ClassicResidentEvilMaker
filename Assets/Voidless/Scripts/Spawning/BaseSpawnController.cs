using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public abstract class BaseSpawnController<T> : MonoBehaviour where T : MonoBehaviour, IPoolObject
{
	[SerializeField] private T _spawnEntity; 					/// <summary>Spawn entity.</summary>
	[SerializeField] private WaypointGenerator _spawnPoints; 	/// <summary>Spawn points controller.</summary>
	[SerializeField] private int _startQuantity; 				/// <summary>Start quantity when the schedule starts.</summary>
	[SerializeField] private int _terminationScore; 			/// <summary>Spawn entities needed for the spawn controller to terminate.</summary>
	[SerializeField] private int _maxQuantity; 					/// <summary>Maximum spawn object's quantity.</summary>
	[SerializeField] private float _startDelay; 				/// <summary>Spawn start delay.</summary>
	[SerializeField] private float _waveWait; 					/// <summary>Spawn Wave Wait.</summary>
	[SerializeField] private bool _infiniteSpawning; 			/// <summary>Infinite Spawning ma dude?.</summary>
	private List<T> _activatedSpawnEntities; 					/// <summary>Spawn entities currently active.</summary>
	private int _score; 										/// <summary>Current spawns terminated score.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets spawnEntity property.</summary>
	public T spawnEntity
	{
		get { return _spawnEntity; }
		set { _spawnEntity = value; }
	}

	/// <summary>Gets and Sets spawnPoints property.</summary>
	public WaypointGenerator spawnPoints
	{
		get { return _spawnPoints; }
		set { _spawnPoints = value; }
	}

	/// <summary>Gets and Sets startQuantity property.</summary>
	public int startQuantity
	{
		get { return _startQuantity; }
		set { _startQuantity = value; }
	}

	/// <summary>Gets and Sets terminationScore property.</summary>
	public int terminationScore
	{
		get { return _terminationScore; }
		set { _terminationScore = value; }
	}

	/// <summary>Gets and Sets maxQuantity property.</summary>
	public int maxQuantity
	{
		get { return _maxQuantity; }
		set { _maxQuantity = value; }
	}

	/// <summary>Gets and Sets score property.</summary>
	public int score
	{
		get { return _score; }
		set { _score = value; }
	}

	/// <summary>Gets and Sets startDelay property.</summary>
	public float startDelay
	{
		get { return _startDelay; }
		set { _startDelay = value; }
	}

	/// <summary>Gets and Sets waveWait property.</summary>
	public float waveWait
	{
		get { return _waveWait; }
		set { _waveWait = value; }
	}

	/// <summary>Gets and Sets infiniteSpawning property.</summary>
	public bool infiniteSpawning
	{
		get { return _infiniteSpawning; }
		set { _infiniteSpawning = value; }
	}

	/// <summary>Gets and Sets activatedSpawnEntities property.</summary>
	public List<T> activatedSpawnEntities
	{
		get { return _activatedSpawnEntities; }
		set { _activatedSpawnEntities = value; }
	}
#endregion

#region UnityMethods:
	void OnDisable()
	{
		EndSpawnSchedule();
	}

	/// <summary>SpawnController's' instance initialization.</summary>
	void Awake()
	{
		activatedSpawnEntities = new List<T>(maxQuantity);
		score = 0;
	}

	/// <summary>SpawnController's starting actions before 1st Update frame.</summary>
	void Start ()
	{
		ObjectPoolManager.Instance.CreateGameObjectsPool<T>(spawnEntity, maxQuantity);	
	}
#endregion

	public void StartSpawnSchedule()
	{
		if(activatedSpawnEntities != null) activatedSpawnEntities = new List<T>(maxQuantity);
		score = 0;
		StartCoroutine(SpawnRoutine());
	}

	public void EndSpawnSchedule()
	{
		StopCoroutine(SpawnRoutine());
		if(activatedSpawnEntities.Count > 0)
		{
			for(int i = 0; i < activatedSpawnEntities.Count; i++)
			{
				activatedSpawnEntities[i].onPoolObjectDeactivation -= OnSpawnTermination;	
			}
		}
	}

	protected virtual void SpawnNewEntity()
	{
		Transform newSpawnPoint = spawnPoints.waypoints[UnityEngine.Random.Range(0, spawnPoints.waypoints.Count)].transform;
		T spawn = ObjectPoolManager.Instance.Recycle<T>(spawnEntity, newSpawnPoint.position, newSpawnPoint.rotation);
		OnEntityCreated(spawn);
		activatedSpawnEntities.Add(spawn);
		spawn.onPoolObjectDeactivation += OnSpawnTermination;
	}

	protected abstract void OnEntityCreated(T _entity);

	protected virtual void OnSpawnTermination(IPoolObject _entity)
	{
		activatedSpawnEntities.Remove(_entity as T);
		_entity.onPoolObjectDeactivation -= OnSpawnTermination;
		score++;
	}

	protected virtual IEnumerator SpawnRoutine()
	{
		for(int i = 0; i < startQuantity; i++)
		{
			SpawnNewEntity();
		}

		WaitForSeconds delayWait = new WaitForSeconds(startDelay);

		yield return delayWait;

		while(infiniteSpawning || (score < terminationScore))
		{
			WaitForSeconds waitForWave = new WaitForSeconds(waveWait);
			yield return waitForWave;
			
			if(activatedSpawnEntities.Count < maxQuantity)
			{
				SpawnNewEntity();
			}
			else
			{

			}
		}
		EndSpawnSchedule();
	}
}
}