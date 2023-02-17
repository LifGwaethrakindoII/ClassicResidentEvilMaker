using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VTime
{
	/// <summary>Sets Fixed Delta Time equal to the ideal main Delta Time, and sets frame-rate beforehand.</summary>
	/// <param name="frameRate">Target Frame-Rate.</param>
	public static void EqualizeFixedDeltaTime(int frameRate)
	{
		Application.targetFrameRate = Mathf.Max(0, frameRate);
		EqualizeFixedDeltaTime();
	}

	/// <summary>Sets Fixed Delta Time equal to the ideal main Delta Time.</summary>
	public static void EqualizeFixedDeltaTime()
	{
		float f = (float)Application.targetFrameRate;
		float dt = 1.0f / f;
		Time.maximumDeltaTime = dt;
		Time.fixedDeltaTime = dt;

		//Debug.Log("[VTime] With a Frame-Rate of " + f + ", Setted both Fixed and Maximum Delta Time to " + dt);
	}

	/// <returns>Current's Frame-Rate [as integer].</returns>
	public static int GetFrameRate()
	{
		return (int)(1.0f / Time.smoothDeltaTime);
	}
}
}