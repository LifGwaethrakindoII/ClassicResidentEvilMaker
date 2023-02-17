using System;
using System.Collections;
using System.Collections.Generic;

namespace Voidless
{
public class RandomSelectorComposite<T> : BehaviorTreeComposite<T> where T : IComponentAgent<T, IEnumerator<TreeState>>
{
	/// <summary>RandomSelectorComposite default constructor.</summary>
	/// <param name="_children">Children to add to this Composite.</param>
	public RandomSelectorComposite(params IAgentComponent<T, IEnumerator<TreeState>>[] _children) : base(_children)
	{
		
	}

	/// <summary>Ticks the component.</summary>
	/// <param name="_agent">Component's Agent to tick.</param>
	/// <returns>Tick's result.</returns>
	public override IEnumerator<TreeState> Tick(T _agent)
	{
		IAgentComponent<T, IEnumerator<TreeState>> child = null;
		int[] randomChildIndexes = VMath.GetUniqueRandomSet(children.Count);
		IEnumerator<TreeState> currentChild = null;

		foreach(int i in randomChildIndexes)
		{
			child = children[i];
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
				break;
				
				case TreeState.Failure:
				case TreeState.Error:
				continue;
			}
		}

		yield return TreeState.Failure;
	}
}
}