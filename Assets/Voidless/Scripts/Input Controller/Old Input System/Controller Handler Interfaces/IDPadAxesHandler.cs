using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public interface IDPadAxesHandler
{
	/// <summary>Method called when onDPadAxesChanges event is invoked.</summary>
	/// <param name="_axisX">D-Pad's X Axis.</param>
	/// <param name="_axisY">D-Pad's Y Axis.</param>
	void OnDPadAxesChanges(float _axisX, float _axisY);
}
}