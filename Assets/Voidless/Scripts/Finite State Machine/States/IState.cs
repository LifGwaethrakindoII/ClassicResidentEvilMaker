using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public interface IState<T> where T : IFiniteStateAgent<T>
{
	List<IStateTransition<T>> transitions { get; set; } 	/// <summary>State's Transitions.</summary>

	/// <summary>Message invoked when entering state.</summary>
	/// <param name="_instance">IFiniteStateAget's instance invoking the callback.</param>
	/// <param name="_deltaTime">Optional Delta Time information.</param>
	void OnEnterState(T _instance);

	/// <summary>Message invoked when executing state.</summary>
	/// <param name="_instance">IFiniteStateAget's instance invoking the callback.</param>
	/// <param name="_deltaTime">Optional Delta Time information.</param>
	IEnumerator OnExecuteState(T _instance);

	/// <summary>Message invoked when leaving state.</summary>
	/// <param name="_instance">IFiniteStateAget's instance invoking the callback.</param>
	/// <param name="_deltaTime">Optional Delta Time information.</param>
	void OnExitState(T _instance);
}
}