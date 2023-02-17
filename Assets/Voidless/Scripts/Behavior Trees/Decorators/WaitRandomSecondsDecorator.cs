using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class WaitRandomSecondsDecorator<T> : WaitSecondsDecorator<T> where T : IComponentAgent<T, IEnumerator<TreeState>>
{
	private FloatRange _randomWaitRange; 	/// <summary>Random seconds' wait range.</summary>

	/// <summary>Gets and Sets randomWaitRange property.</summary>
	public FloatRange randomWaitRange
	{
		get { return _randomWaitRange; }
		set { _randomWaitRange = value; }
	}

	/// <summary>WaitRandomSecondsDecorator default constructor.</summary>
	/// <param name="_randomWaitRange">Random Wait's range duration.</param>
	/// <param name="_child">Child to decorate.</param>
	public WaitRandomSecondsDecorator(FloatRange _randomWaitRange, IAgentLeaf<T, IEnumerator<TreeState>> _child) : base(0.0f, _child)
	{
		randomWaitRange = _randomWaitRange;
	}

	/// <summary>Ticks the component.</summary>
	/// <param name="_agent">Component's Agent to tick.</param>
	/// <returns>Tick's result.</returns>
	public override IEnumerator<TreeState> Tick(T _agent)
	{
		secondsWait = UnityEngine.Random.Range(randomWaitRange.min, randomWaitRange.max);
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