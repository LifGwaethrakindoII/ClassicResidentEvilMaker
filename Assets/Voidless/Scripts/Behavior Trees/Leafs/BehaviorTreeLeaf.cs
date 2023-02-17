using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public abstract class BehaviorTreeLeaf<T> : BehaviorTreeComponent<T>, IAgentLeaf<T, IEnumerator<TreeState>> where T : IComponentAgent<T, IEnumerator<TreeState>>
{
	/// <summary>Ticks the component.</summary>
	/// <param name="_agent">Component's Agent to tick.</param>
	/// <returns>Tick's result.</returns>
	public override abstract IEnumerator<TreeState> Tick(T _agent);
}
}