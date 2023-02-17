using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class RepeatNTimesDecorator<T> : BehaviorTreeDecorator<T> where T : IComponentAgent<T, IEnumerator<TreeState>>
{
	private int _repeatTimes; 	/// <summary>Times to repeat a child's ticking [in seconds].</summary>

	/// <summary>Gets and Sets repeatTimes property.</summary>
	public int repeatTimes
	{
		get { return _repeatTimes; }
		set { _repeatTimes = value; }
	}

	/// <summary>RepeatNTimesDecorator default constructor.</summary>
	/// <param name="_repeatTimes">Times to repeat a child's ticking.</param>
	/// <param name="_child">Child to decorate.</param>
	public RepeatNTimesDecorator(int _repeatTimes, IAgentLeaf<T, IEnumerator<TreeState>> _child) : base(_child)
	{
		repeatTimes = _repeatTimes;
	}

	/// <summary>Ticks the component.</summary>
	/// <param name="_agent">Component's Agent to tick.</param>
	/// <returns>Tick's result.</returns>
	public override IEnumerator<TreeState> Tick(T _agent)
	{
		int currentRepeats = 1;
		IEnumerator<TreeState> currentChild = null;
		
		while(currentRepeats < repeatTimes)
		{
			currentChild = child.Tick(_agent);
			currentChild.MoveNext();
			currentRepeats++;
			yield return TreeState.Running;
		}

		currentChild = child.Tick(_agent);
		currentChild.MoveNext();

		while(currentChild.Current == TreeState.Running)
		{
			yield return TreeState.Running;
			currentChild.MoveNext();
		}
		
		yield return currentChild.Current;
	}
}
}