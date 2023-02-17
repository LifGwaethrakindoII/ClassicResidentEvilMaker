using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public interface IInputControllerHandler : IJoystickAxesHandler, ITriggerAxesHandler, IInputReceiveHandler, IDPadAxesHandler
{
	
}
}