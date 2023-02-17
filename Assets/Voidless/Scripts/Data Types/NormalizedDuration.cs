using UnityEngine;
using System;

namespace Voidless
{
[Serializable]
public struct NormalizedDuration
{
	[SerializeField] private float _duration; 	/// <summary>Un-normalized duration [in seconds].</summary>
	private float _normalizedTime; 				/// <summary>Normalized duration [from 0.0 to 1.0].</summary>

	/// <summary>Gets and Sets duration property.</summary>
	public float duration
	{
		get { return _duration; }
		set { _duration = value; }
	}

	/// <summary>Gets and Sets normalizedTime property.</summary>
	public float normalizedTime
	{
		get { return _normalizedTime; }
		set { _normalizedTime = value; }
	}

	/// <summary>Gets if the duration is still running.</summary>
	public bool running { get { return (normalizedTime < (1.0f + Mathf.Epsilon)); } }

	/// <summary>NormalizedDuration's constructor.</summary>
	/// <param name="_duration">Un-normalized duration.</param>
	public NormalizedDuration(float _duration) : this()
	{
		duration = _duration;
		normalizedTime = 0.0f;
	}

	/// <summary>Ticks the normalized time.</summary>
	/// <param name="_deltaTime">Optional Time Delta's reference.</param>
	public void Tick(float _deltaTime = -1.0f)
	{
		normalizedTime += (_deltaTime < 0.0f ? Time.deltaTime : _deltaTime / duration);
	}

	/// <summary>Resets the normalized duration.</summary>
	public void Reset()
	{
		normalizedTime = 0.0f;
	}
}	
}