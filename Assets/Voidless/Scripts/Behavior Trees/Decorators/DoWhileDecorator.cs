using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class DoWhileDecorator<T> : WaitUntilDecorator<T> where T : IComponentAgent<T, IEnumerator<TreeState>>
{
	/// <summary>DoWhileDecorator default constructor.</summary>
	/// <param name="waitCondition">Wait's Condition.</param>
	/// <param name="_child">Child to decorate.</param>
	public DoWhileDecorator(Predicate<T> waitCondition, IAgentLeaf<T, IEnumerator<TreeState>> _child) : base(waitCondition, _child)
	{

	}

	/// <summary>Ticks the component.</summary>
	/// <param name="_agent">Component's Agent to tick.</param>
	/// <returns>Tick's result.</returns>
	public override IEnumerator<TreeState> Tick(T _agent)
	{
		IEnumerator<TreeState> currentChild = child.Tick(_agent);

		while(WaitCondition(_agent))
		{
			if(!currentChild.MoveNext()) currentChild = child.Tick(_agent);
			yield return currentChild.Current;
		}
	}
}
}