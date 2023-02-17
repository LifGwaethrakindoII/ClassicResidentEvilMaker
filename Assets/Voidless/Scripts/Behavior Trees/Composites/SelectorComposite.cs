using System;
using System.Collections;
using System.Collections.Generic;

namespace Voidless
{
public class SelectorComposite<T> : BehaviorTreeComposite<T> where T : IComponentAgent<T, IEnumerator<TreeState>>
{
	/// <summary>SelectorComposite default constructor.</summary>
	/// <param name="_children">Children to add to this Composite.</param>
	public SelectorComposite(params IAgentComponent<T, IEnumerator<TreeState>>[] _children) : base(_children)
	{
		
	}

	/// <summary>Ticks the component.</summary>
	/// <param name="_agent">Component's Agent to tick.</param>
	/// <returns>Tick's result.</returns>
	public override IEnumerator<TreeState> Tick(T _agent)
	{
		IEnumerator<TreeState> currentChild = null;

		foreach(IAgentComponent<T, IEnumerator<TreeState>> child in children)
		{
			currentChild = child.Tick(_agent);
			currentChild.MoveNext();

			while(currentChild.Current == TreeState.Running)
			{
				yield return TreeState.Running;
				currentChild.MoveNext();
			}

			switch(currentChild.Current)
			{
				case TreeState.Success:
				case TreeState.Running:
				yield return TreeState.Success;
				yield break;

				case TreeState.Failure:
				case TreeState.Error:
				continue;
			}
		}

		yield return TreeState.Failure;
	}
}
}