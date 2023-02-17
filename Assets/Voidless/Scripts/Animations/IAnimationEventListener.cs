using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public interface IAnimationEventListener
{
	/// <summary>Callback invoked when an AnimationEvent Action occurs.</summary>
	void OnAnimationEventAction();

	/// <summary>Callback invoked when an AnimationEvent Int Action occurs.</summary>
	/// <param name="x">Integer argument.</param>
	void OnAnimationEventIntAction(int x);

	/// <summary>Callback invoked when an AnimationEvent Float Action occurs.</summary>
	/// <param name="x">Float argument.</param>
	void OnAnimationEventFloatAction(float x);

	/// <summary>Callback invoked when an AnimationEvent String Action occurs.</summary>
	/// <param name="x">String argument.</param>
	void OnAnimationEventStringAction(string x);

	/// <summary>Callback invoked when an AnimationEvent AnimationEvent Action occurs.</summary>
	/// <param name="x">AnimationEvent argument.</param>
	void OnAnimationEvent(AnimationEvent x);
}
}