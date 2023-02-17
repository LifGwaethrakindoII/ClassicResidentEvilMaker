using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public interface IInputReceiveHandler
{
	/// <summary>Method called when onInputReceived event is invoked.</summary>
	/// <param name="_inputID">ID of the input received.</param>
	/// <param name="_state">State of the input received.</param>
	void OnInputReceived(int _inputID, InputState _state);
}
}