using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public abstract class BehaviorTreeDecorator<T> : BehaviorTreeComponent<T>, IAgentDecorator<T, IEnumerator<TreeState>> where T : IComponentAgent<T, IEnumerator<TreeState>>
{
	private IAgentComponent<T, IEnumerator<TreeState>> _child; 	/// <summary>Decorator's Child.</summary>

	/// <summary>Gets and Sets child property.</summary>
	public IAgentComponent<T, IEnumerator<TreeState>> child
	{
		get { return _child; }
		set { _child = value; }
	}

	/// <summary>BehaviorTreeDecorator default constructor.</summary>
	/// <param name="_child">Child to decorate.</param>
	public BehaviorTreeDecorator(IAgentComponent<T, IEnumerator<TreeState>> _child)
	{
		child = _child;
	}

	/// <summary>Ticks the component.</summary>
	/// <param name="_agent">Component's Agent to tick.</param>
	/// <returns>Tick's result.</returns>
	public abstract override IEnumerator<TreeState> Tick(T _agent);
}
}