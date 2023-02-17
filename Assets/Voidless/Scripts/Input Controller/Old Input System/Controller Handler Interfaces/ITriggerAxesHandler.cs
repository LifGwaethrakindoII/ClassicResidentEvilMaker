using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public interface ITriggerAxesHandler
{
	/// <summary>Method called when onLeftTriggerAxisChange event is invoked.</summary>
	/// <param name="_axis">Trigger's axis.</param>
	void OnLeftTriggerAxisChange(float _axis);

	/// <summary>Method called when onRightTriggerAxisChange event is invoked.</summary>
	/// <param name="_axis">Trigger's axis.</param>
	void OnRightTriggerAxisChange(float _axis);
}
}