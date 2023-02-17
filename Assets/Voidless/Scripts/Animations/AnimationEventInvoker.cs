using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class AnimationEventInvoker : MonoBehaviour
{
	private List<Action> _actions; 									/// <summary>Invokable Actions.</summary>
	private List<Action<int>> _intActions; 							/// <summary>Invokable Int Actions.</summary>
	private List<Action<float>> _floatActions; 						/// <summary>Invokable Float Actions.</summary>
	private List<Action<string>> _stringActions; 					/// <summary>Invokable String Actions.</summary>
	private List<Action<AnimationEvent>> _animationEventActions; 	/// <summary>Invokable AnimationEvent Actions.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets actions property.</summary>
	public List<Action> actions
	{
		get
		{
			if(_actions == null) _actions = new List<Action>();
			return _actions;
		}
		private set { _actions = value; }
	}

	/// <summary>Gets and Sets intActions property.</summary>
	public List<Action<int>> intActions
	{
		get
		{
			if(_intActions == null) _intActions = new List<Action<int>>();
			return _intActions;
		}
		private set { _intActions = value; }
	}

	/// <summary>Gets and Sets floatActions property.</summary>
	public List<Action<float>> floatActions
	{
		get
		{
			if(_floatActions == null) _floatActions = new List<Action<float>>();
			return _floatActions;
		}
		private set { _floatActions = value; }
	}

	/// <summary>Gets and Sets stringActions property.</summary>
	public List<Action<string>> stringActions
	{
		get
		{
			if(_stringActions == null) _stringActions = new List<Action<string>>();
			return _stringActions;
		}
		private set { _stringActions = value; }
	}

	/// <summary>Gets and Sets animationEventActions property.</summary>
	public List<Action<AnimationEvent>> animationEventActions
	{
		get
		{
			if(_animationEventActions == null) _animationEventActions = new List<Action<AnimationEvent>>();
			return _animationEventActions;
		}
		private set { _animationEventActions = value; }
	}
#endregion

#region AddMethods:
	/// <summary>Adds IAnimationEventListener's methods into all list of listeners.</summary>
	/// <param name="_listener">IAnimationEventListener's intance.</param>
	public void AddListener(IAnimationEventListener _listener)
	{
		if(_listener == null) return;

		AddActionListener(_listener.OnAnimationEventAction);
		AddIntActionListener(_listener.OnAnimationEventIntAction);
		AddFloatActionListener(_listener.OnAnimationEventFloatAction);
		AddStringActionListener(_listener.OnAnimationEventStringAction);
		AddAnimationEventActionListener(_listener.OnAnimationEvent);
	}

	/// <summary>Adds Action Listener.</summary>
	/// <param name="action">Action to add.</param>
	public void AddActionListener(Action action)
	{
		if(action != null) actions.Add(action);
	}

	/// <summary>Adds Int Action Listener.</summary>
	/// <param name="action">Int Action to add.</param>
	public void AddIntActionListener(Action<int> action)
	{
		if(action != null) intActions.Add(action);
	}

	/// <summary>Adds Float Action Listener.</summary>
	/// <param name="action">Float Action to add.</param>
	public void AddFloatActionListener(Action<float> action)
	{
		if(action != null) floatActions.Add(action);
	}

	/// <summary>Adds String Action Listener.</summary>
	/// <param name="action">String Action to add.</param>
	public void AddStringActionListener(Action<string> action)
	{
		if(action != null) stringActions.Add(action);
	}

	/// <summary>Adds AnimationEvent Action Listener.</summary>
	/// <param name="action">AnimationEvent Action to add.</param>
	public void AddAnimationEventActionListener(Action<AnimationEvent> action)
	{
		if(action != null) animationEventActions.Add(action);
	}
#endregion

#region RemoveMethods:
	/// <summary>Removes IAnimationEventListener's methods into all list of listeners.</summary>
	/// <param name="_listener">IAnimationEventListener's intance.</param>
	public void RemoveListener(IAnimationEventListener _listener)
	{
		if(_listener == null) return;

		RemoveActionListener(_listener.OnAnimationEventAction);
		RemoveIntActionListener(_listener.OnAnimationEventIntAction);
		RemoveFloatActionListener(_listener.OnAnimationEventFloatAction);
		RemoveStringActionListener(_listener.OnAnimationEventStringAction);
		RemoveAnimationEventActionListener(_listener.OnAnimationEvent);
	}

	/// <summary>Removes Action Listener.</summary>
	/// <param name="action">Action to add.</param>
	public void RemoveActionListener(Action action)
	{
		if(action != null) actions.Remove(action);
	}

	/// <summary>Removes Int Action Listener.</summary>
	/// <param name="action">Int Action to add.</param>
	public void RemoveIntActionListener(Action<int> action)
	{
		if(action != null) intActions.Remove(action);
	}

	/// <summary>Removes Float Action Listener.</summary>
	/// <param name="action">Float Action to add.</param>
	public void RemoveFloatActionListener(Action<float> action)
	{
		if(action != null) floatActions.Remove(action);
	}

	/// <summary>Removes String Action Listener.</summary>
	/// <param name="action">String Action to add.</param>
	public void RemoveStringActionListener(Action<string> action)
	{
		if(action != null) stringActions.Remove(action);
	}

	/// <summary>Removes AnimationEvent Action Listener.</summary>
	/// <param name="action">AnimationEvent Action to add.</param>
	public void RemoveAnimationEventActionListener(Action<AnimationEvent> action)
	{
		if(action != null) animationEventActions.Remove(action);
	}
#endregion

#region Callbacks:
	/// <summary>Invokes Actions.</summary>
	public void InvokeActions()
	{
		foreach(Action action in actions)
		{
			if(action != null) action();
		}
	}

	/// <summary>Invokes Actions.</summary>
	/// <param name="x">Argument of type int.</param>
	public void InvokeIntActions(int x)
	{
		foreach(Action<int> action in intActions)
		{
			if(action != null) action(x);
		}
	}

	/// <summary>Invokes Actions.</summary>
	/// <param name="x">Argument of type float.</param>
	public void InvokeFloatActions(float x)
	{
		foreach(Action<float> action in floatActions)
		{
			if(action != null) action(x);
		}
	}

	/// <summary>Invokes Actions.</summary>
	/// <param name="x">Argument of type string.</param>
	public void InvokeStringActions(string x)
	{
		foreach(Action<string> action in stringActions)
		{
			if(action != null) action(x);
		}
	}

	/// <summary>Invokes Actions.</summary>
	/// <param name="x">Argument of type animationEvent.</param>
	public void InvokeAnimationEventActions(AnimationEvent x)
	{
		foreach(Action<AnimationEvent> action in animationEventActions)
		{
			if(action != null) action(x);
		}
	}
#endregion

}
}