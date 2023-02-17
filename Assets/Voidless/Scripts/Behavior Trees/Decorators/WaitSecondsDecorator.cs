using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class WaitSecondsDecorator<T> : BehaviorTreeDecorator<T> where T : IComponentAgent<T, IEnumerator<TreeState>>
{
	private float _secondsWait; 	/// <summary>Wait duration [in seconds].</summary>

	/// <summary>Gets and Sets secondsWait property.</summary>
	public float secondsWait
	{
		get { return _secondsWait; }
		set { _secondsWait = value; }
	}

	/// <summary>WaitSecondsDecorator default constructor.</summary>
	/// <param name="_secondsWait">Wait duration.</param>
	/// <param name="_child">Child to decorate.</param>
	public WaitSecondsDecorator(float _secondsWait, IAgentLeaf<T, IEnumerator<TreeState>> _child) : base(_child)
	{
		secondsWait = _secondsWait;
	}

	/// <summary>Ticks the component.</summary>
	/// <param name="_agent">Component's Agent to tick.</param>
	/// <returns>Tick's result.</returns>
	public override IEnumerator<TreeState> Tick(T _agent)
	{
		float currentWait = 0.0f;

		while(currentWait < (secondsWait + Mathf.Epsilon))
		{
			currentWait += Time.deltaTime;
			yield return TreeState.Running;
		}

		IEnumerator<TreeState> currentChild = child.Tick(_agent);
		currentChild.MoveNext();

		while(currentChild.Current == TreeState.Running)
		{
			yield return TreeState.Running;
			currentChild.MoveNext();
		}

		yield return currentChild.Current;
	}
}
}