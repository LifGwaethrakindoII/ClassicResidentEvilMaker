using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[RequireComponent(typeof(AudioSource))]
public class SoundEffectLooper : PoolGameObject
{
	[SerializeField] private AudioClip _clip; 							/// <summary>Sound-Effect to loop.</summary>
	[SerializeField][Range(0.0f, 1.0f)] private float _volumeScale; 	/// <summary>Volume-Scale.</summary>
	private AudioSource _source; 										/// <summary>AudioSource's Component.</summary>

	/// <summary>Gets and Sets clip property.</summary>
	public AudioClip clip
	{
		get { return _clip; }
		set { _clip = value; }
	}

	/// <summary>Gets and Sets volumeScale property.</summary>
	public float volumeScale
	{
		get { return _volumeScale; }
		set { _volumeScale = value; }
	}

	/// <summary>Gets source Component.</summary>
	public AudioSource source
	{ 
		get
		{
			if(_source == null) _source = GetComponent<AudioSource>();
			return _source;
		}
	}

	/// <summary>Resets SoundEffectLooper's instance to its default values.</summary>
	private void Reset()
	{
		volumeScale = 1.0f;
	}

	/// <summary>Plays and loops sound-effect.</summary>
	public void Play()
	{
		if(clip == null) return;

		source.clip = clip;
		source.loop = true;
		source.time = 0.0f;
		source.volume = volumeScale;

		source.Play();
	}

	/// <summary>Resets Loop.</summary>
	public void ResetLoop()
	{
		source.time = 0.0f;
		source.volume = volumeScale;
	}

	/// <summary>Stops Looper.</summary>
	public void Stop()
	{
		source.Stop();
		source.volume = 0.0f;
		OnObjectDeactivation();
	}
}
}