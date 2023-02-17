using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public interface IStateMachine : IFiniteStateMachine<int>
{
	int ignoreResetMask { get; set; } 	/// <summary>Mask that selectively contains state to ignore resetting if they were added again [with AddState's method]. As it is 0 by default, it won't ignore resetting any state [~0 = 11111111]</summary>

	/// <summary>Callback invoked when new state's flags are added.</summary>
	/// <param name="_state">State's flags that were added.</param>
	void OnStatesAdded(int _state);

	/// <summary>Callback invoked when new state's flags are removed.</summary>
	/// <param name="_state">State's flags that were removed.</param>
	void OnStatesRemoved(int _state);
}
}