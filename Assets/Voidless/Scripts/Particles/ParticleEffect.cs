using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
/// \TODO Do Interfacing to modify the following ParticleSystem's Modules:
/*
	- collision: ParticleSystem.CollisionModule
	- colorBySpeed: ParticleSystem.ColorBySpeedModule
	- colorOverLifeTime: ParticleSystem.ColorOverLifeTimeModule
	- customData: ParticleSystem.CustomDataModule
	- emission: ParticleSystem.EmissionModule
	- externalForces: ParticleSystem.ExternalForcesModule
	- forceOverLifetime: ParticleSystem.ForceOverLifetimeModule
	- inheritVelocity: ParticleSystem.InheritVelocityModule
	- lights: ParticleSystem.LightsModule
	- limitVelocityOverLifetime: ParticleSystem.LimitVelocityOverLifetimeModule
	- main: ParticleSystem.MainModule
	- noise: ParticleSystem.NoiseModule
	- rotationBySpeed
	- rotationOverLifetime: ParticleSystem.RotationOverLifetimeModule
	- shape: ParticleSystem.ShapeModule
	- sizeBySpeed: ParticleSystem.SizeModule
	- sizeOverLifetime: ParticleSystem.SizeOverLifetimeModule
	- textureSheetAnimation: ParticleSystem.TextureSheetAnimationModule
	- trails: ParticleSystem.TrailsModule	
	- trigger: ParticleSystem.TriggerModule	
	- velocityOverLifetime: ParticleSystem.VelocityOverLifetimeModule
*/

/// \TODO Instead of Coroutine, use Behavior so you can pause and resume the coroutines.

/*
How states are set on the following situations:

|---------------------------------------------------------------------|
|                | Is Alive: | Is Playing: | Is Paused: | Is Stopped: |
|----------------|----------------------------------------------------|
| On Enabled():  | False     | False       | False      | True        |
|----------------|----------------------------------------------------|
| On Disabled(): | False     | False       | False      | True        |
|----------------|----------------------------------------------------|
| On Simulate(): | True      | True        | False      | False       |
|----------------|----------------------------------------------------|
| On Play():     | True      | True        | False      | False       |
|----------------|----------------------------------------------------|
| On Pause():    | True      | False       | True       | False       |
|----------------|----------------------------------------------------|
| On Stop():     | False     | False       | False      | True        |
|----------------|----------------------------------------------------|
| On Clear():    | False     | False       | False      | True        |
|----------------|----------------------------------------------------|

* If the ParticleSystem has loop = false, and the last particle has emmited, the ParticleSystem stops. Marking isStopped = true and everything else
= false.
*/
[ExecuteInEditMode]
public class ParticleEffect : MonoBehaviour, IPoolObject
{
	private const byte FLAG_IS_ALIVE = 1 << 0; 							/// <summary>Is Alive's Flag.</summary>
	private const byte FLAG_IS_PLAYING = 1 << 1; 						/// <summary>Is Playing's Flag.</summary>
	private const byte FLAG_IS_PAUSED = 1 << 2; 						/// <summary>Is Paused's Flag.</summary>
	private const byte FLAG_IS_STOPPED = 1 << 3; 						/// <summary>Is Stopped's Flag.</summary>

	public event OnPoolObjectDeactivation onPoolObjectDeactivation; 	/// <summary>Event invoked when this Pool Object is being deactivated.</summary>

	[SerializeField] private ParticleSystem[] _systems; 				/// <summary>ParticleSystems' Components controlled by this Effect.</summary>
	private Dictionary<int, Behavior> _cooldowns; 						/// <summary>Cooldowns associated with each looping system.</summary>
	private int _poolDictionaryID; 										/// <summary>Key's ID of this Pool Object on its respectrive pool dictionary.</summary>
	private bool _dontDestroyOnLoad; 									/// <summary>Is this Pool Object going to be destroyed when changing scene? [By default it destroys it].</summary>
	private bool _active; 												/// <summary>Is this Pool Object active [preferibaly unavailable to recycle]?.</summary>
	private int _cooldownsCount; 										/// <summary>Cooldowns' Count.</summary>
	private byte statesMask; 											/// <summary>Bit flags that store the states of the Particle Effect.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets systems property.</summary>
	public ParticleSystem[] systems
	{
		get { return _systems; }
		set { _systems = value; }
	}

	/// <summary>Gets and Sets cooldowns property.</summary>
	public Dictionary<int, Behavior> cooldowns
	{
		get { return _cooldowns; }
		set { _cooldowns = value; }
	}

	/// <summary>Gets and Sets poolDictionaryID property.</summary>
	public int poolDictionaryID
	{
		get { return _poolDictionaryID; }
		set { _poolDictionaryID = value; }
	}

	/// <summary>Gets and Sets cooldownsCount property.</summary>
	public int cooldownsCount
	{
		get { return _cooldownsCount; }
		set { _cooldownsCount = value; }
	}

	/// <summary>Gets and Sets dontDestroyOnLoad property.</summary>
	public bool dontDestroyOnLoad
	{
		get { return _dontDestroyOnLoad; }
		set { _dontDestroyOnLoad = value; }
	}

	/// <summary>Gets and Sets active property.</summary>
	public bool active
	{
		get { return _active; }
		set { _active = value; }
	}

	/// <summary>Gets isAlive property.</summary>
	public bool isAlive { get { return (statesMask | FLAG_IS_ALIVE) == statesMask; } }

	/// <summary>Gets isPlaying property.</summary>
	public bool isPlaying { get { return (statesMask | FLAG_IS_PLAYING) == statesMask; } }

	/// <summary>Gets isPaused property.</summary>
	public bool isPaused { get { return (statesMask | FLAG_IS_PAUSED) == statesMask; } }

	/// <summary>Gets isStopped property.</summary>
	public bool isStopped { get { return (statesMask | FLAG_IS_STOPPED) == statesMask; } }
#endregion

	/// <summary>Callback invoked when ParticleEffect's instance is enabled.</summary>
	private void OnEnable()
	{
		if(cooldowns == null) cooldowns = new Dictionary<int, Behavior>();
		Play();
	}

	/// <summary>Callback invoked when ParticleEffect's instance is disabled.</summary>
	private void OnDisable()
	{
		if(cooldowns == null) cooldowns = new Dictionary<int, Behavior>();
		Stop();
		Clear();
		if(onPoolObjectDeactivation != null) onPoolObjectDeactivation(this);
	}

	/// <summary>ParticleEffect's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		statesMask = 0;
		cooldownsCount = 0;
		OnAwake();
	}

	/// <summary>Callback internally invoked after Awake.</summary>
	protected virtual void OnAwake() {/*...*/}

	/// <summary>Updates ParticleEffect's instance at each frame.</summary>
	private void Update()
	{
		
	}

#region ParticleSystemMethods:
	/// <summary>Plays all the PArticle Systems.</summary>
	/// <param name="_withChildren">Pause also the children contained? [true as default].</param>
	public void Play(bool _withChildren = true)
	{
		if(systems == null) return;
		if(!gameObject.activeSelf) gameObject.SetActive(true);

		cooldownsCount = 0;

		foreach(ParticleSystem system in systems)
		{
			try
			{
				system.Play(_withChildren);
				StartCooldown(system);
			}
			catch(Exception e)
			{
				Debug.LogError(gameObject.name + ": " + e.Message, gameObject);
			}
		}

		statesMask = FLAG_IS_ALIVE | FLAG_IS_PLAYING;
	}

	/// <summary>Simulates the Particle Systems.</summary>
	/// <param name="_withChildren">Pause also the children contained? [true as default].</param>
	/// <param name="t">Time period to advance this simulation by [0.0f by default].</param>
	public void Simulate(bool _withChildren = true, float t = 0.0f)
	{
		if(systems == null) return;
		if(!gameObject.activeSelf) gameObject.SetActive(true);
		
		foreach(ParticleSystem system in systems)
		{
			system.Simulate(0.0f, _withChildren);
			StartCooldown(system);
		}

		statesMask = FLAG_IS_ALIVE | FLAG_IS_PLAYING;
	}

	/// <summary>Pauses the Particle Systems.</summary>
	/// <param name="_withChildren">Pause also the children contained? [true as default].</param>
	public void Pause(bool _withChildren = true)
	{
		if(!gameObject.activeSelf) gameObject.SetActive(true);
		if(systems == null) return;
		
		foreach(ParticleSystem system in systems)
		{
			system.Pause(_withChildren);
			PauseCooldown(system);
		}

		statesMask = FLAG_IS_ALIVE | FLAG_IS_PAUSED;
	}

	/// <summary>Stops the Particle Systems.</summary>
	/// <param name="_withChildren">Pause also the children contained? [true as default].</param>
	public void Stop(bool _withChildren = true)
	{
		if(systems == null) return;
		
		foreach(ParticleSystem system in systems)
		{
			try
			{
				system.Stop(_withChildren);
			}
			catch(Exception e)
			{
				Debug.LogError(gameObject.name + ": " + e.Message, gameObject);
			}
		}

		StopCooldowns();
		statesMask = FLAG_IS_STOPPED;
	}

	/// <summary>Clears the Particle Systems.</summary>
	/// <param name="_withChildren">Clear also the children contained? [true as default].</param>
	public void Clear(bool _withChildren = true)
	{
		if(systems == null) return;
		
		foreach(ParticleSystem system in systems)
		{
			if(system != null) system.Clear(_withChildren);
		}

		StopCooldowns();
		statesMask = FLAG_IS_STOPPED;
	}
#endregion

#region CooldownsMethods:
	/// <param name="_duration">Cooldown's Duration.</param>
	/// <returns>Cooldown iterator for ParticleSystem.</returns>
	protected virtual IEnumerator GetParticleSystemCooldown(float _duration)
	{
		return this.WaitSeconds(_duration, OnCooldownEnds);
	}

	/// <summary>Evaluates if given system is looping to start a cooldown.</summary>
	/// <param name="_system">ParticleSystem to evaluate.</param>
	private void StartCooldown(ParticleSystem _system)
	{
		if(!Application.isPlaying || _system.main.loop) return;
		
		int instanceID = _system.gameObject.GetInstanceID();
		
		if(cooldowns.ContainsKey(instanceID))
		{
			Behavior cooldown = cooldowns[instanceID];

			if(cooldown != null)
			{
				StopCoroutine(cooldown.coroutine);
				cooldowns[instanceID] = null;
			}
		}
		else cooldowns.Add(instanceID, null);

		cooldowns[instanceID] = this.StartBehaviorCoroutine(GetParticleSystemCooldown(_system.main.duration + _system.main.startLifetime.constantMax));
		cooldownsCount++;
	}

	/// <summary>Resumes ParticleSystem's Cooldown.</summary>
	/// <param name="_system">Particle System's cooldown to resume.</param>
	private void ResumeCooldown(ParticleSystem _system)
	{
		if(!Application.isPlaying || _system.main.loop) return;

		int instanceID = _system.gameObject.GetInstanceID();

		if(cooldowns.ContainsKey(instanceID))
		{
			Behavior cooldown = cooldowns[instanceID];
			if(cooldown != null && cooldown.state == BehaviorState.Paused) cooldown.ChangeState(BehaviorState.Running);
		}
	}

	/// <summary>Pauses PartycleSystem's Cooldown.</summary>
	/// <param name="_system">ParticeSystem's Cooldown to Pause.</param>
	private void PauseCooldown(ParticleSystem _system)
	{
		if(!Application.isPlaying || _system.main.loop) return;

		int instanceID = _system.gameObject.GetInstanceID();
		
		if(cooldowns.ContainsKey(instanceID))
		{
			Behavior cooldown = cooldowns[instanceID];
			if(cooldown != null) cooldown.ChangeState(BehaviorState.Paused);
		}
	}

	/// <summary>Stops all registered cooldowns.</summary>
	private void StopCooldowns()
	{
		foreach(Behavior cooldown in cooldowns.Values)
		{
			if(cooldown != null) StopCoroutine(cooldown.coroutine);
		}

		cooldowns.Clear();
	}
#endregion

#region IPoolObjectCallbacks:
	/// <summary>Independent Actions made when this Pool Object is being created.</summary>
	public void OnObjectCreation()
	{
		this.DefaultOnCreation();
	}

	/// <summary>Actions made when this Pool Object is being reseted.</summary>
	public void OnObjectReset()
	{
		this.DefaultOnRecycle();
	}

	/// <summary>Callback invoked when the object is deactivated.</summary>
	public void OnObjectDeactivation()
	{
		this.DefaultOnDeactivation();
	}

	/// <summary>Actions made when this Pool Object is being destroyed.</summary>
	public void OnObjectDestruction()
	{
		this.DefaultOnDestruction();
	}
#endregion

	/// <summary>Gets Particle Systems on self-contained children.</summary>
	public void GetParticleSystems()
	{
		ParticleSystem[] particleSystems = transform.GetComponentsInChildren<ParticleSystem>();

		if(particleSystems.Length > 0) systems = particleSystems;
	}

	/// <summary>Sets Simulation Time for all main modules.</summary>
	/// <param name="t">New Time.</param>
	public void SetSimulationTime(float t)
	{
		if(systems == null) return;
		
		ParticleSystem.MainModule module = default(ParticleSystem.MainModule);

		foreach(ParticleSystem system in systems)
		{
			module = system.main;
			module.simulationSpeed = t;
		}	
	}

	/// <summary>Callback internally invoked when the cooldown ends.</summary>
	protected virtual void OnCooldownEnds()
	{
		cooldownsCount--;
		if(cooldownsCount <= 0 && cooldowns.Count == systems.Length) OnObjectDeactivation();
	}
}
}