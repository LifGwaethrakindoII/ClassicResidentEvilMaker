using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
/// <summary>Event invoked when the AsynchronousBehavior changes state.</summary>
/// <param name="_behavior">AsynchronousBehavior that changed state.</param>
/// <param name="_state">New AsynchronousBehavior's state.</param>
public delegate void OnAsynchronousBehaviorEvent<T>(AsynchronousBehavior<T> _behavior, BehaviorState _state);

#pragma warning disable 108
public class AsynchronousBehavior<T> : Behavior, IEnumerator<T>, IFiniteStateMachine<BehaviorState>
{
	public event OnAsynchronousBehaviorEvent<T> asynchronousBehaviorEvent; 	/// <summary>OnAsynchronousBehaviorEvent subscription's event.</summary>

	private IEnumerator<T> _enumeratorT; 									/// <summary>AsynchronousBehavior's T's IEnumerator.</summary>

	/// <summary>Gets and Sets enumerator property.</summary>
	public override IEnumerator enumerator
	{
		get { return enumeratorT; }
		protected set { enumerator = value; }
	}

	/// <summary>Gets data property.</summary>
	public T data { get { return enumeratorT.Current; } }

	/// <summary>Gets and Sets enumeratorT property.</summary>
	public IEnumerator<T> enumeratorT
	{
		get { return _enumeratorT; }
		set
		{
			_enumeratorT = value;
			enumerator = enumeratorT;
		}
	}

#region IEnumeratorTProperties:
	/// <summary>Gets Current property.</summary>
	public T Current { get { return enumeratorT.Current; } }
#endregion

#region FiniteStateMachine:
	/// <summary>Enters BehaviorState State.</summary>
	/// <param name="_state">BehaviorState State that will be entered.</param>
	public override void OnEnterState(BehaviorState _state)
	{
		base.OnEnterState(_state);
		if(asynchronousBehaviorEvent != null) asynchronousBehaviorEvent(this, _state);
	}
#endregion

	/// <summary>AsynchronousBehavior's constructor.</summary>
	/// <param name="_enumeratorT">T's IEnumerator.</param>
	/// <param name="_startAutomagically">Start the Asynchronous Behavior automagically?.</param>
	public AsynchronousBehavior(IEnumerator<T> _enumeratorT, bool _startAutomagically = true)
	{
		enumeratorT = _enumeratorT;
		enumerator = enumeratorT;
		if(_startAutomagically) StartBehavior();
	}

	/// <summary>Overload AsynchronousBehavior's constructor.</summary>
	/// <param name="_monoBehaviour">MonoBehaviour from where the coroutine belongs.</param>
	/// <param name="_enumeratorT">T's IEnumerator.</param>
	/// <param name="_startAutomagically">Start the Asynchronous Behavior automagically?.</param>
	public AsynchronousBehavior(MonoBehaviour _monoBehaviour, IEnumerator<T> _enumeratorT, bool _startAutomagically = true) : base(_monoBehaviour, _enumeratorT, _startAutomagically)
	{
		enumeratorT = _enumeratorT;
	}

	/// <summary>Starts the Behavior's Coroutine.</summary>
	public override void StartBehavior()
	{
		if(state == BehaviorState.Ready)
		{
			if(monoBehaviourDependency) coroutine = monoBehaviour.StartCoroutine(enumerator);
			this.ChangeState(BehaviorState.Running);
		}
	}

	/// <summary>Pauses the Behavior's Coroutine.</summary>
	public override void PauseBehavior()
	{
		if(state == BehaviorState.Running || state == BehaviorState.Ready || state == BehaviorState.Restarted) this.ChangeState(BehaviorState.Paused);
	}

	/// <summary>Resumes the Behavior [if it was paused].</summary>
	public override void ResumeBehavior()
	{
		if(state == BehaviorState.Paused) this.ChangeState(BehaviorState.Running);
	}

	/// <summary>Stops the current Coroutine, then it starts it again.</summary>
	public override void ResetBehavior()
	{
		if(state == BehaviorState.Running || state == BehaviorState.Paused || state == BehaviorState.Ready)
		{
			this.ChangeState(BehaviorState.Restarted);

			if(monoBehaviourDependency)
			{
				monoBehaviour.StopCoroutine(coroutine);
				coroutine = monoBehaviour.StartCoroutine(enumerator);
			}

			this.ChangeState(BehaviorState.Ready);
			StartBehavior();
		}
	}

	/// <summary>Ends the Behavior.</summary>
	public override void EndBehavior()
	{
		if(state != BehaviorState.Running) this.ChangeState(BehaviorState.Finished);
	}

#region IEnumeratorTMethods:
	/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
	public void Dispose()
	{
		enumeratorT.Dispose();
	}

	/// <summary>Advances the enumerator to the next element of the collection.</summary>
	public override bool MoveNext()
	{
		switch(state)
		{
			case BehaviorState.Running:
			return enumeratorT.MoveNext();

			case BehaviorState.Paused:
			return true;

			case BehaviorState.Finished:
			return false;
		}

		return enumeratorT.MoveNext();
	}

	/// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
	public override void Reset()
	{
		enumeratorT.Reset();
	}
#endregion

}
}