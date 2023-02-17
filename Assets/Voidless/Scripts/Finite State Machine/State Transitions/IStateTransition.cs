using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public interface IStateTransition<T> where T : IFiniteStateAgent<T>
{
	IState<T> transitionState { get; set; } 		/// <summary>Desired's Transition State.</summary>
	Predicate<T> TransitionPolicy { get; set; } 	/// <summary>Transition Policy's Condition.</summary>	
}
}