using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public interface IJoystickAxesHandler
{
	/// <summary>Method called when on LeftAxesChange is invoked.</summary>
	/// <param name="_axisX">X's axis.</param>
	/// <param name="_axisY">Y's axis.</param>
	void OnLeftAxesChange(float _axisX, float _axisY);

	/// <summary>Method called when on RightAxesChange is invoked.</summary>
	/// <param name="_axisX">X's axis.</param>
	/// <param name="_axisY">Y's axis.</param>
	void OnRightAxesChange(float _axisX, float _axisY);
}
}