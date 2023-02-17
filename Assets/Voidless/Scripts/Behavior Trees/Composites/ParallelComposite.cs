using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class ParallelComposite<T> : BehaviorTreeComposite<T> where T : IComponentAgent<T, IEnumerator<TreeState>>
{
	/// <summary>ParallelComposite default constructor.</summary>
	/// <param name="_children">Children to add to this Composite.</param>
	public ParallelComposite(params IAgentComponent<T, IEnumerator<TreeState>>[] _children) : base(_children)
	{
		
	}

	/// <summary>Ticks the component.</summary>
	/// <param name="_agent">Component's Agent to tick.</param>
	/// <returns>Tick's result.</returns>
	public override IEnumerator<TreeState> Tick(T _agent)
	{
		yield return TreeState.Failure;
	}
}
}