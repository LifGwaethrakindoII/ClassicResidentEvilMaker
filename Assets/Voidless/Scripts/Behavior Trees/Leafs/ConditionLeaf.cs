using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class ConditionLeaf<T> : BehaviorTreeLeaf<T> where T : IComponentAgent<T, IEnumerator<TreeState>>
{
	private Predicate<T> _AgentCondition; 	/// <summary>Agent's Condition.</summary>

	/// <summary>Gets and Sets AgentCondition property.</summary>
	public Predicate<T> AgentCondition
	{
		get { return _AgentCondition; }
		set { _AgentCondition = value; }
	}

	/// <summary>ConditionLeaf default constructor.</summary>
	public ConditionLeaf(Predicate<T> agentCondition)
	{
		AgentCondition = agentCondition;
	}

	/// <summary>Ticks the component.</summary>
	/// <param name="_agent">Component's Agent to tick.</param>
	/// <returns>Tick's result.</returns>
	public override IEnumerator<TreeState> Tick(T _agent)
	{
		yield return AgentCondition(_agent) ? TreeState.Success : TreeState.Failure;
	}
}
}