using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class ParticleEffectController : MonoBehaviour, IPoolObject
{
	public event OnPoolObjectDeactivation onPoolObjectDeactivation; 	/// <summary>Event invoked when this Pool Object is being deactivated.</summary>

	[SerializeField] private bool _dontDestroyOnLoad; 					/// <summary>Is this Pool Object going to be destroyed when changing scene? [By default it destroys it].</summary>
	[SerializeField] private List<ParticleEffect> _particleEffects; 	/// <summary>Particle Effects.</summary>
	[SerializeField] private List<ParticleSystem> _particleSystems; 	/// <summary>Particle Systems.</summary>
	private int _poolDictionaryID; 										/// <summary>Key's ID of this Pool Object on its respectrive pool dictionary.</summary>
	private bool _active; 												/// <summary>Is this Pool Object active [preferibaly unavailable to recycle]?.</summary>
	protected Coroutine fade;
	protected Coroutine scale;

	/// <summary>Gets and Sets particleEffects property.</summary>
	public List<ParticleEffect> particleEffects
	{
		get { return _particleEffects; }
		set { _particleEffects = value; }
	}

	/// <summary>Gets and Sets particleSystems property.</summary>
	public List<ParticleSystem> particleSystems
	{
		get { return _particleSystems; }
		set { _particleSystems = value; }
	}

	/// <summary>Gets and Sets dontDestroyOnLoad property.</summary>
	public bool dontDestroyOnLoad
	{
		get { return _dontDestroyOnLoad; }
		set { _dontDestroyOnLoad = value; }
	}

	/// <summary>Gets and Sets poolDictionaryID property.</summary>
	public int poolDictionaryID
	{
		get { return _poolDictionaryID; }
		set { _poolDictionaryID = value; }
	}

	/// <summary>Gets and Sets active property.</summary>
	public bool active
	{
		get { return _active; }
		set { _active = value; }
	}

#region UnityMethods:
	private void OnEnable()
	{
		active = true;
	}

	private void OnDisable()
	{
		PauseAll();
		active = false;
		if(onPoolObjectDeactivation != null) onPoolObjectDeactivation(this);
	}

	/// <summary>ParticleEffectController's instance initialization.</summary>
	private void Awake()
	{
		if(particleSystems == null || particleSystems.Count == 0) particleSystems = transform.GetComponentsFromChilds<ParticleSystem>();
	}
#endregion

#region IPoolObjectMethods:
	/// <summary>Independent Actions made when this Pool Object is being created.</summary>
	public virtual void OnObjectCreation()
	{
		gameObject.SetActive(false);
	}

	/// <summary>Actions made when this Pool Object is being reseted.</summary>
	public virtual void OnObjectReset()
	{
		if(gameObject.activeSelf) gameObject.SetActive(false);
		gameObject.SetActive(true);
	}

	/// <summary>Callback invoked when the object is deactivated.</summary>
	public virtual void OnObjectDeactivation()
	{
		gameObject.SetActive(false);
	}

	/// <summary>Actions made when this Pool Object is being destroyed.</summary>
	public virtual void OnObjectDestruction()
	{
		Destroy(gameObject);
	}
#endregion

	/// <summary>Plays all Particle Systems.</summary>
	public void PlayAll()
	{
		foreach(ParticleSystem system in particleSystems)
		{
			system.Play();
		}
	}

	/// <summary>Plays selected systems by given indices.</summary>
	/// <param name="_indices">Indices of all the systems to Play.</param>
	public void Play(params int[] _indices)
	{
		foreach(int index in _indices)
		{
			particleSystems[Mathf.Clamp(index, 0, particleSystems.Count - 1)].Play();
		}
	}

	/// <summary>Pauses all Particle Systems.</summary>
	public void PauseAll()
	{
		foreach(ParticleSystem system in particleSystems)
		{
			system.Pause();
		}
	}

	/// <summary>Pauses selected systems by given indices.</summary>
	/// <param name="_indices">Indices of all the systems to Pause.</param>
	public void Pause(params int[] _indices)
	{
		foreach(int index in _indices)
		{
			particleSystems[Mathf.Clamp(index, 0, particleSystems.Count - 1)].Pause();
		}
	}

	/// <summary>Clears all Particle Systems.</summary>
	public void ClearAll()
	{
		foreach(ParticleSystem system in particleSystems)
		{
			system.Clear();
		}
	}

	/// <summary>Clears selected systems by given indices.</summary>
	/// <param name="_indices">Indices of all the systems to Clear.</param>
	public void Clear(params int[] _indices)
	{
		foreach(int index in _indices)
		{
			particleSystems[Mathf.Clamp(index, 0, particleSystems.Count - 1)].Clear();
		}
	}

	public void FadeAll(float _duration, Fade _fade = Fade.Out, Func<float, float> function = null, Action onFadeEnds = null)
	{
		this.StartCoroutine(FadeSystem(_duration, _fade, function, onFadeEnds), ref fade);
	}

	private IEnumerator FadeSystem(float _duration, Fade _fade, Func<float, float> function, Action onFadeEnds)
	{
		float t = _fade == Fade.Out? 1.0f : 0.0f;

		while(_fade == Fade.Out? t > 0.0f : t < 1.0f)
		{
			for(int i = 0; i < particleSystems.Count; i++)
			{
				/*ParticleSystem.MainModule module = particleSystems[i].main;
				ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
				ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystems[i].particleCount];
				particleSystems[i].GetParticles(particles);
				
				for(int j = 0; j < particles.Length; j++)
				{
					byte alpha;
					VColor32.FloatToColor32Byte(out alpha, function != null ? function(t) : t);
					particles[j].startColor = particles[j].GetCurrentColor(particleSystems[i]).WithAlpha(alpha);
					module.startColor = emitParams.startColor.WithAlpha(alpha);
				}*/
			}

			if(_fade == Fade.Out) t -= (Time.deltaTime / _duration);
			else t += (Time.deltaTime / _duration);

			yield return null;
		}

		if(onFadeEnds != null) onFadeEnds();
		this.DispatchCoroutine(ref fade);
	}
}
}