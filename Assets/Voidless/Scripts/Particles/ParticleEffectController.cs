using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class ParticleEffectController : PoolGameObject
{
	[SerializeField] private List<ParticleEffect> _particleEffects;
	[SerializeField] private List<ParticleSystem> _particleSystems;
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

	/// <summary>ParticleEffectController's instance initialization.</summary>
	private void Awake()
	{
		if(particleSystems == null || particleSystems.Count == 0) particleSystems = transform.GetComponentsFromChilds<ParticleSystem>();
	}

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