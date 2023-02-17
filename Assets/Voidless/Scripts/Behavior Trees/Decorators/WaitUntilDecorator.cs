using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class WaitUntilDecorator<T> : BehaviorTreeDecorator<T> where T : IComponentAgent<T, IEnumerator<TreeState>>
{
	private Predicate<T> _WaitCondition; 	/// <summary>Wait's Condition.</summary>

	/// <summary>Gets and Sets WaitCondition property.</summary>
	public Predicate<T> WaitCondition
	{
		get { return _WaitCondition; }
		set { _WaitCondition = value; }
	}

	/// <summary>WaitUntilDecorator default constructor.</summary>
	/// <param name="waitCondition">Wait's Condition.</param>
	/// <param name="_child">Child to decorate.</param>
	public WaitUntilDecorator(Predicate<T> waitCondition, IAgentLeaf<T, IEnumerator<TreeState>> _child) : base(_child)
	{
		WaitCondition = waitCondition;
	}

	/// <summary>Ticks the component.</summary>
	/// <param name="_agent">Component's Agent to tick.</param>
	/// <returns>Tick's result.</returns>
	public override IEnumerator<TreeState> Tick(T _agent)
	{
		while(!WaitCondition(_agent))
		{
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