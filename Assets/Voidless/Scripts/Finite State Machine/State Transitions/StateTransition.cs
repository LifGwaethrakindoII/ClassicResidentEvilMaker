using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class StateTransition<T> : IStateTransition<T> where T : IFiniteStateAgent<T>
{
	public IState<T> _transitionState; 		/// <summary>Desired's Trabsition State.</summary>
	public Predicate<T> transitionPolicy; 	/// <summary>Transition Policy predicate that will determine whether the transition will be made or not.</summary>

	/// <summary>Gets and Sets transitionState property.</summary>
	public IState<T> transitionState
	{
		get { return _transitionState; }
		set { _transitionState = value; }
	}

	/// <summary>Gets and Sets TransitionPolicy property.</summary>
	public Predicate<T> TransitionPolicy
	{
		get { return transitionPolicy; }
		set { transitionPolicy = value; }
	}

	/// <summary>StateTransition constructor.</summary>
	/// <param name="transitionPolicy">Transition's Condition.</param>
	/// <param name="_transitionState">Transition's State.</param>
	public StateTransition(Predicate<T> transitionPolicy, IState<T> _transitionState)
	{
		TransitionPolicy = transitionPolicy;
		transitionState = _transitionState;
	}
}
}