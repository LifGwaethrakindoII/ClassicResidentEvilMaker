using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class ActionLeaf<T> : BehaviorTreeLeaf<T> where T : IComponentAgent<T, IEnumerator<TreeState>>
{
	private Func<T, TreeState> _AgentAction; 	/// <summary>Agent's Action defined on this leaf.</summary>

	/// <summary>Gets and Sets AgentAction property.</summary>
	public Func<T, TreeState> AgentAction
	{
		get { return _AgentAction; }
		set { _AgentAction = value; }
	}

	/// <summary>ActionLeaf default constructor.</summary>
	/// <param name="agentAction">Agent's Action.</param>
	public ActionLeaf(Func<T, TreeState> agentAction)
	{
		AgentAction = agentAction;
	}

	/// <summary>Ticks the component.</summary>
	/// <param name="_agent">Component's Agent to tick.</param>
	/// <returns>Tick's result.</returns>
	public override IEnumerator<TreeState> Tick(T _agent)
	{
		yield return AgentAction(_agent);
	}
}
}