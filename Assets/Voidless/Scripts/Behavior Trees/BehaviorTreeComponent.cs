using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public enum TreeState 	/// <summary>Tree States.</summary>
{
	Unassigned, 		/// <summary>Unassigned Tree State.</summary>
	Failure, 			/// <summary>Failure Tree State.</summary>
	Success, 			/// <summary>Success Tree State.</summary>
	Running, 			/// <summary>Running Tree State.</summary>
	Error 				/// <summary>Error Tree State.</summary>
}

/*public enum TickMode
{
	AllPerFrame,
	OnePerFrame
}*/

public abstract class BehaviorTreeComponent<T> : IAgentComponent<T, IEnumerator<TreeState>> where T : IComponentAgent<T, IEnumerator<TreeState>>/*, IEnumerator<TreeState>*/
{
	/*Object IEnumerator.Current { return null; } 				/// <summary>Current's Object.</summary>

	TreeState IEnumerator<TreeState>.Current { return null; } 	/// <summary>Current Tree State.</summary>*/

	/// <summary>Ticks the component.</summary>
	/// <param name="_agent">Component's Agent to tick.</param>
	/// <returns>Tick's result.</returns>
	public abstract IEnumerator<TreeState> Tick(T _agent);

	public virtual bool MoveNext()
	{
		return false;
	}

	public virtual void Reset()
	{
		
	}

	public virtual void Dispose()
	{

	}
}
}