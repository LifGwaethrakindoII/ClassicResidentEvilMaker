using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class DoWhileWaitRandomSecondsDecorator<T> : WaitRandomSecondsDecorator<T> where T : IComponentAgent<T, IEnumerator<TreeState>>
{
	/// <summary>DoWhileWaitRandomSecondsDecorator default constructor.</summary>
	/// <param name="_randomWaitRange">Random Wait's range duration.</param>
	/// <param name="_child">Child to decorate.</param>
	public DoWhileWaitRandomSecondsDecorator(FloatRange _randomWaitRange, IAgentLeaf<T, IEnumerator<TreeState>> _child) : base(_randomWaitRange, _child)
	{

	}

	/// <summary>Ticks the component.</summary>
	/// <param name="_agent">Component's Agent to tick.</param>
	/// <returns>Tick's result.</returns>
	public override IEnumerator<TreeState> Tick(T _agent)
	{
		IEnumerator<TreeState> currentChild = child.Tick(_agent);
		secondsWait = UnityEngine.Random.Range(randomWaitRange.min, randomWaitRange.max);
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