using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class DoWhileWaitSecondsDecorator<T> : WaitSecondsDecorator<T> where T : IComponentAgent<T, IEnumerator<TreeState>>
{
	/// <summary>DoWhileWaitSecondsDecorator default constructor.</summary>
	/// <param name="_secondsWait">Wait duration.</param>
	/// <param name="_child">Child to decorate.</param>
	public DoWhileWaitSecondsDecorator(float _secondsWait, IAgentLeaf<T, IEnumerator<TreeState>> _child) : base(_secondsWait, _child)
	{

	}

	/// <summary>Ticks the component.</summary>
	/// <param name="_agent">Component's Agent to tick.</param>
	/// <returns>Tick's result.</returns>
	public override IEnumerator<TreeState> Tick(T _agent)
	{
		IEnumerator<TreeState> currentChild = child.Tick(_agent);
		float currentWait = 0.0f;

		while(currentWait < (secondsWait + Mathf.Epsilon))
		{
			if(!currentChild.MoveNext()) currentChild = child.Tick(_agent);
			currentWait += Time.deltaTime;
			yield return currentChild.Current;
		}

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