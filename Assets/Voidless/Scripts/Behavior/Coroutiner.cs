using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class Coroutiner : Singleton<Coroutiner>
{
#region BehaviorRequests:
	/// <summary>Requests a Behavior that will wait for some seconds, and then invoke a callback.</summary>
	/// <param name="_monoBehaviour">Requester MonoBehaviour.</param>
	/// <param name="_seconds">Wait duration.</param>
	/// <param name="onBehaviorEnds">Callback invoked when Behavior ends.</param>
	/// <returns>Reference to the Behavior created.</returns>
	public Behavior WaitSecondsBehavior(MonoBehaviour _monoBehaviour, float _seconds, Action onBehaviorEnds = null)
	{
		return new Behavior(_monoBehaviour, WaitSecondsAndDo(_monoBehaviour, _seconds, onBehaviorEnds));
	}

	/// <summary>Requests a Behavior that will do an action while waiting for some seconds, and then invokes a callback.</summary>
	/// <param name="_monoBehaviour">Requester MonoBehaviour.</param>
	/// <param name="onDoWhile">Action invoked while the wait is given.</param>
	/// <param name="_seconds">Wait duration.</param>
	/// <param name="onBehaviorEnds">Callback invoked when Behavior ends.</param>
	/// <returns>Reference to the Behavior created.</returns>
	public Behavior DoWhileWaitingSecondsBehavior(MonoBehaviour _monoBehaviour, Action doWhileAction, float _seconds, Action onBehaviorEnds = null)
	{
		return (doWhileAction != null) ? new Behavior(_monoBehaviour, DoWhileWaitingSeconds(_monoBehaviour, doWhileAction, _seconds, onBehaviorEnds)) : null;
	}
#endregion

#region CoroutineRequests:
	/// <summary>Requests a Coroutine that will wait for some seconds, and then invoke a callback.</summary>
	/// <param name="_monoBehaviour">Requester MonoBehaviour.</param>
	/// <param name="_seconds">Wait duration.</param>
	/// <param name="onCoroutineEnds">Callback invoked when Coroutine ends.</param>
	/// <returns>Reference to the Coroutine created.</returns>
	public Coroutine WaitSecondsCoroutine(MonoBehaviour _monoBehaviour, float _seconds, Action onCoroutineEnds = null)
	{
		return StartCoroutine(WaitSecondsAndDo(_monoBehaviour, _seconds, onCoroutineEnds));
	}

	/// <summary>Requests a Coroutine that will do an action while waiting for some seconds, and then invokes a callback.</summary>
	/// <param name="_monoBehaviour">Requester MonoBehaviour.</param>
	/// <param name="onDoWhile">Action invoked while the wait is given.</param>
	/// <param name="_seconds">Wait duration.</param>
	/// <param name="onCoroutineEnds">Callback invoked when Coroutine ends.</param>
	/// <returns>Reference to the Coroutine created.</returns>
	public Coroutine DoWhileWaitingSecondsCoroutine(MonoBehaviour _monoBehaviour, Action doWhileAction, float _seconds, Action onCoroutineEnds = null)
	{
		return (doWhileAction != null) ? StartCoroutine(DoWhileWaitingSeconds(_monoBehaviour, doWhileAction, _seconds, onCoroutineEnds)) : null;
	}
#endregion

#region IEnumerators:
	/// <summary>Wait for some seconds, and then invoke a callback.</summary>
	/// <param name="_monoBehaviour">Requester MonoBehaviour.</param>
	/// <param name="_seconds">Wait duration.</param>
	/// <param name="onIEnumeratorEnds">Callback invoked when IEnumerator ends.</param>
	private IEnumerator WaitSecondsAndDo(MonoBehaviour _monoBehaviour, float _seconds, Action onIEnumeratorEnds = null)
	{
		CountdownTimer currentWait = new CountdownTimer(_seconds);

		while(_monoBehaviour.enabled && currentWait.running)
		{
			currentWait.Tick();
			yield return null;
		}

		if(onIEnumeratorEnds != null) onIEnumeratorEnds();
	}

	/// <summary>Do an action while waiting for some seconds, and then invokes a callback.</summary>
	/// <param name="_monoBehaviour">Requester MonoBehaviour.</param>
	/// <param name="onDoWhile">Action invoked while the wait is given.</param>
	/// <param name="_seconds">Wait duration.</param>
	/// <param name="onIEnumeratorEnds">Callback invoked when IEnumerator ends.</param>
	private IEnumerator DoWhileWaitingSeconds(MonoBehaviour _monoBehaviour, Action doWhileAction, float _seconds, Action onIEnumeratorEnds = null)
	{
		CountdownTimer currentWait = new CountdownTimer(_seconds);

		while(_monoBehaviour.enabled && currentWait.running)
		{
			doWhileAction();
			currentWait.Tick();
			yield return null;
		}

		if(onIEnumeratorEnds != null) onIEnumeratorEnds();
	}
#endregion
}
}