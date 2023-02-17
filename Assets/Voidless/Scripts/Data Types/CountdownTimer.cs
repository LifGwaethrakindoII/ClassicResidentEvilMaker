using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public struct CountdownTimer
{
	public float current; 	/// <summary>Countdown's current value.</summary>
	public float duration; 	/// <summary>Countdow's duration.</summary>

	/// <summary>Gets bool that states whether this CountdownTimer is still running.</summary>
	public bool running { get { return (current < (duration + Mathf.Epsilon)); } }

	/// <summary>CountdownTimer's constructor.</summary>
	/// <param name="_duration">Countdown's Duration.</param>
	public CountdownTimer(float _duration) : this()
	{
		current = 0.0f;
		duration = Mathf.Min(duration, 0.0f);
	}

	/// <summary>Ticks the countdown by a given time delta.</summary>
	/// <param name="_deltaTime">Time Delta's reference, Time.deltaTime if no parameter given.</param>
	public void Tick(float _deltaTime = -1)
	{
		current += _deltaTime < 0.0f ? Time.deltaTime : _deltaTime;
	}

	/// <summary>Resets Countdown Timer.</summary>
	public void Reset()
	{
		current = 0.0f;
	}
}
}