using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Voidless
{
public class TimeConstrainedForceApplier
{
	private MonoBehaviour _monoBehaviour; 	/// <summary>MonoBehaviour's reference.</summary>
	private Rigidbody _body; 				/// <summary>Rigidbody's Reference.</summary>
	private ForceMode _forceMode; 			/// <summary>Force's Mode.</summary>
	private Vector3 _force; 				/// <summary>Desired Force.</summary>
	private Vector3 _velocity; 				/// <summary>Current Velocity.</summary>
	private float _duration; 				/// <summary>Force's Duration.</summary>
	private float _timeScale; 				/// <summary>Normalized Time's Scale.</summary>
	private float _progress; 				/// <summary>Force's Progress.</summary>
	private UnityEvent onForceEnds; 		/// <summary>Calback invoked whent he force ends.</summary>
	private Coroutine forceCoroutine; 		/// <summary>Force's Coroutine reference.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets monoBehaviour property.</summary>
	public MonoBehaviour monoBehaviour
	{
		get { return _monoBehaviour; }
		private set { _monoBehaviour = value; }
	}

	/// <summary>Gets and Sets body property.</summary>
	public Rigidbody body
	{
		get { return _body; }
		private set { _body = value; }
	}

	/// <summary>Gets and Sets forceMode property.</summary>
	public ForceMode forceMode
	{
		get { return _forceMode; }
		set { _forceMode = value; }
	}

	/// <summary>Gets and Sets force property.</summary>
	public Vector3 force
	{
		get { return _force; }
		private set { _force = value; }
	}

	/// <summary>Gets and Sets velocity property.</summary>
	public Vector3 velocity
	{
		get { return _velocity; }
		private set { _velocity = value; }
	}

	/// <summary>Gets and Sets duration property.</summary>
	public float duration
	{
		get { return _duration; }
		set { _duration = value; }
	}

	/// <summary>Gets and Sets timeScale property.</summary>
	public float timeScale
	{
		get { return _timeScale; }
		set { _timeScale = value; }
	}

	/// <summary>Gets and Sets progress property.</summary>
	public float progress
	{
		get { return _progress; }
		private set { _progress = value; }
	}

	/// <summary>Gets and Sets OnForceEnds property.</summary>
	public UnityEvent OnForceEnds
	{
		get { return onForceEnds; }
		set { onForceEnds = value; }
	}
#endregion

	/// <summary>TimeConstrainedForceApplier default constructor.</summary>
	/// <param name="_monoBehaviour">MonoBehaviour's reference.</param>
	/// <param name="_body">Rigidbody to displace.</param>
	/// <param name="_force">Force's Vector.</param>
	/// <param name="_duration">Force's Duration.</param>
	/// <param name="_forceMode">Force Mode [ForceMode.Force by default].</param>
	/// <param name="actions">Optional callbacks to invoke when the force ends.</param>
	public TimeConstrainedForceApplier(MonoBehaviour _monoBehaviour, Rigidbody _body, Vector3 _force, float _duration, ForceMode _forceMode = ForceMode.Force, params UnityAction[] actions)
	{
		body = _body;
		force = _force;
		duration = _duration;
		timeScale = 1.0f;

		if(OnForceEnds == null) OnForceEnds = new UnityEvent();
		foreach(UnityAction action in actions)
		{
			OnForceEnds.AddListener(action);
		}
	}

	/// <summary>Applies Force.</summary>
	public void ApplyForce()
	{
		monoBehaviour.StartCoroutine(ApplyForceRoutine(), ref forceCoroutine);
	}

	/// <summary>Cancels Force.</summary>
	public void CancelForce()
	{
		monoBehaviour.DispatchCoroutine(ref forceCoroutine);
		
		progress = 0.0f;
		velocity = Vector3.zero;
	}

	/// <summary>Coroutine that applies force.</summary>
	private IEnumerator ApplyForceRoutine()
	{
		progress = 0.0f;
		float inverseDuration = 1.0f / duration;
		float inverseMass = 1.0f / body.mass;
		float dt = 0.0f;
		float t = 0.0f;
		float previousMass = body.mass;
		
		while(progress < 1.0f)
		{
			dt = Time.fixedDeltaTime;
			t = dt * timeScale;

			switch(forceMode)
			{
				case ForceMode.Force:
				velocity += (force * inverseMass) * dt;
				break;

				case ForceMode.Acceleration:
				velocity += force * dt;
				break;

				case ForceMode.Impulse:
				velocity = force * inverseMass;
				break;

				default:
				velocity = force;
				break;
			}

			if(body.mass != previousMass) inverseMass = 1.0f / body.mass;
			previousMass = body.mass;

			body.MovePosition(body.position + (velocity * t));
			progress += t * inverseDuration;

			yield return VCoroutines.WAIT_PHYSICS_THREAD;
		}
	
		OnForceEnds.Invoke();
		CancelForce();
	}
}
}