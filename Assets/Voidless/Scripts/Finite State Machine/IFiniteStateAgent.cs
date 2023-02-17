using System.Collections;

namespace Voidless
{
public interface IFiniteStateAgent<T> where T : IFiniteStateAgent<T>
{
	IState<T> currentState { get; set; } 			/// <summary>Agent's Current State.</summary>
	IState<T> previousState { get; set; } 			/// <summary>Agent's Previous State.</summary>
	IEnumerator executionEnumerator { get; set; } 	/// <summary>Agent's Execution Enumerator.</summary>
}	
}