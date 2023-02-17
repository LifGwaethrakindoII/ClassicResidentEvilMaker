using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Voidless
{
[Serializable]
public class Cooldown
{
	[SerializeField] private MonoBehaviour _monoBehaviour; 	/// <summary>MonoBehaviour's Reference that will wall this Coroutine.</summary>
	[SerializeField] private float _duration; 				/// <summary>Cooldown's Duration.</summary>
	[SerializeField] private UnityEvent onCooldownEnds; 	/// <summary>Event invoked when the cooldown ends.</summary>
	private float _progress; 								/// <summary>Cooldown's Progress.</summary>
	private Coroutine coroutine; 							/// <summary>Cooldown's Coroutine Reference.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets monoBehaviour property.</summary>
	public MonoBehaviour monoBehaviour
	{
		get { return _monoBehaviour; }
		set { _monoBehaviour = value; }
	}

	/// <summary>Gets and Sets OnCooldownEnds property.</summary>
	public UnityEvent OnCooldownEnds
	{
		get { return onCooldownEnds; }
		set { onCooldownEnds = value; }
	}

	/// <summary>Gets and Sets duration property.</summary>
	public float duration
	{
		get { return _duration; }
		set { _duration = value; }
	}

	/// <summary>Gets and Sets progress property.</summary>
	public float progress
	{
		get { return _progress; }
		private set { _progress = value; }
	}

	/// <summary>Gets onCooldown property.</summary>
	public bool onCooldown { get { return coroutine != null; } }

	/// <summary>Gets active property.</summary>
	public bool active { get { return coroutine != null; } }
#endregion

	/// <summary>Cooldown constructor.</summary>
	/// <param name="_duration">Cooldown's Duration.</param>
	public Cooldown(float _duration)
	{
		duration = _duration;
		progress = 1.0f;
		coroutine = null;
		OnCooldownEnds = new UnityEvent();
	}

	/// <summary>Cooldown constructor.</summary>
	/// <param name="_monoBehaviour">MonoBehaviour's Reference.</param>
	/// <param name="_duration">Cooldown's Duration.</param>
	public Cooldown(MonoBehaviour _monoBehaviour, float _duration) : this(_duration)
	{
		monoBehaviour = _monoBehaviour;
	}

	/// <summary>Cooldown constructor.</summary>
	/// <param name="_monoBehaviour">MonoBehaviour's Reference.</param>
	/// <param name="_duration">Cooldown's Duration.</param>
	/// <param name="actions">Set of actions to add to this UnityEvent.</param>
	public Cooldown(MonoBehaviour _monoBehaviour, float _duration, params UnityAction[] actions) : this(_monoBehaviour, _duration)
	{
		if(actions != null) foreach(UnityAction action in actions)
		{
			OnCooldownEnds.AddListener(action);
		}
	}

	/// <summary>Initializes Cooldown.</summary>
	/// <param name="_monoBehaviour">MonoBehaviour's Reference.</param>
	/// <param name="actions">Set of actions to add to this UnityEvent.</param>
	public void Initialize(MonoBehaviour _monoBehaviour, params UnityAction[] actions)
	{
		monoBehaviour = _monoBehaviour;
		OnCooldownEnds = new UnityEvent();
		
		if(actions != null) foreach(UnityAction action in actions)
		{
			OnCooldownEnds.AddListener(action);
		}
	}

	/// <summary>Begins Cooldown.</summary>
	public void Begin()
	{
		if(monoBehaviour == null || !monoBehaviour.gameObject.activeSelf) return;
		monoBehaviour.StartCoroutine(CooldownCoroutine(), ref coroutine);
	}

	/// <summary>Ends Cooldown.</summary>
	public void End()
	{
		monoBehaviour.DispatchCoroutine(ref coroutine);
		progress = 1.0f;
		OnCooldownEnds.Invoke();
	}

	/// <summary>Cooldown's Coroutine.</summary>
	private IEnumerator CooldownCoroutine()
	{
		float durationInverse = 1.0f / duration;
		progress = 0.0f;

		while(progress < 1.0f)
		{
			progress += (Time.deltaTime * durationInverse);
			yield return null;
		}

		End();
	}
}
}