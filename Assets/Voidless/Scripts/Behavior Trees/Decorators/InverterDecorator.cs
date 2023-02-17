using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class InverterDecorator<T> : BehaviorTreeDecorator<T> where T : IComponentAgent<T, IEnumerator<TreeState>>
{
	private InvertPolicy[] _invertPolicies; 	/// <summary>Wait duration [in seconds].</summary>

	/// <summary>Gets and Sets invertPolicies property.</summary>
	public InvertPolicy[] invertPolicies
	{
		get { return _invertPolicies; }
		set { _invertPolicies = value; }
	}

	/// <summary>InverterDecorator default constructor.</summary>
	/// <param name="_invertPolicies">Wait duration.</param>
	/// <param name="_child">Child to decorate.</param>
	public InverterDecorator(IAgentLeaf<T, IEnumerator<TreeState>> _child, params InvertPolicy[] _invertPolicies) : base(_child)
	{
		invertPolicies = _invertPolicies;
	}

	/// <summary>Ticks the component.</summary>
	/// <param name="_agent">Component's Agent to tick.</param>
	/// <returns>Tick's result.</returns>
	public override IEnumerator<TreeState> Tick(T _agent)
	{
		bool inverted = false;
		IEnumerator<TreeState> currentChild = child.Tick(_agent);
		currentChild.MoveNext();

		foreach(InvertPolicy policy in invertPolicies)
		{
			TreeState invertedResult = policy.EvaluatePolicyTerms(currentChild.Current);
			if(invertedResult != currentChild.Current)
			{
				inverted = true;
				yield return invertedResult;
				break;
			}
		}

		if(!inverted) yield return currentChild.Current;
	}
}
}