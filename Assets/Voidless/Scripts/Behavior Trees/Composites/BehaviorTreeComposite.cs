using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public abstract class BehaviorTreeComposite<T> : BehaviorTreeComponent<T>, IAgentComposite<T, IEnumerator<TreeState>> where T : IComponentAgent<T, IEnumerator<TreeState>>
{
	private List<IAgentComponent<T, IEnumerator<TreeState>>> _children; 	/// <summary>Composite's Children.</summary>

	/// <summary>Gets and Sets children property.</summary>
	public List<IAgentComponent<T, IEnumerator<TreeState>>> children
	{
		get { return _children; }
		set { _children = value; }
	}

	/// <summary>BehaviorTreeComposite default constructor.</summary>
	/// <param name="_children">Children to add to this Composite.</param>
	public BehaviorTreeComposite(params IAgentComponent<T, IEnumerator<TreeState>>[] _children)
	{
		children = new List<IAgentComponent<T, IEnumerator<TreeState>>>(_children.Length);
		this.AddChildren(_children);
	}

	/// <summary>Ticks the component.</summary>
	/// <param name="_agent">Component's Agent to tick.</param>
	/// <returns>Tick's result.</returns>
	public override abstract IEnumerator<TreeState> Tick(T _agent);
}
}