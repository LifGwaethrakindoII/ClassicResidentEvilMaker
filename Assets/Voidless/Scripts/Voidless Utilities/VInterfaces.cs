using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
//using System.Type;

using Random = UnityEngine.Random;

namespace Voidless
{
public static class VInterfaces
{

/*
	State Machine's Bitwise Operations:
	
	Where:
	 - s = Current State
	 - p = Previous State
	 - x = State to Enter/Exit
	 - a = States Added from s
	 - rC = States Removed from s from a State Change
	 - rR = States Removed from s from a State Removal

	AddStates(x) = s | x
	RemoveStates(x) = s & ~x
	HasStates(x) = (s | x) == s
	HasAnyOfTheStates(x) = (s | (s & x)) == s
	
	a = ~s & x
	rC = s ^ x -> z & ~x
	rR = s & x
*/

#region IFiniteStateMachine&IStateMachine:
	/// <summary>Changes IFiniteStateMachine implementer's state. Following FSM's procedures</summary>
	/// <param name="_fsm">IFiniteStateMachine implementer.</param>
	/// <param name="_state">New State to enter.</param>
	public static void ChangeState<T>(this IFiniteStateMachine<T> _fsm, T _state)
	{
		_fsm.previousState = _fsm.state;
		_fsm.OnExitState(_fsm.state);
		_fsm.OnEnterState(_fsm.state = _state);
	}

	/// <summary>Returns to previous State, if there is.</summary>
	/// <param name="_fsm">IFiniteStateMachine implementer.</param>
	public static void ReturnToPreviousState<T>(this IFiniteStateMachine<T> _fsm)
	{
		T newState = _fsm.previousState;
		
		_fsm.previousState = _fsm.state;
		_fsm.OnExitState(_fsm.state);
		_fsm.OnEnterState(_fsm.state = newState);
	}

	/// <summary>Changes IStateMachine implementer's state. Following FSM's procedures</summary>
	/// <param name="_fsm">IStateMachine implementer.</param>
	/// <param name="_state">New State to enter.</param>
	public static void ChangeState(this IStateMachine _sm, int _state)
	{
		int states = _sm.state;
		int addedStates = (~states & _state);
		int removedStates = (states & ~_state);

		_sm.previousState = states;
		_sm.state = _state;
		_sm.OnExitState(states);
		_sm.OnEnterState(_state);
		if(addedStates != 0) _sm.OnStatesAdded(addedStates);
		if(removedStates != 0) _sm.OnStatesRemoved(removedStates);
	}

	/// <summary>Returns to previous State, if there is.</summary>
	/// <param name="_sm">FiniteStateMachine implementer.</param>
	public static void ReturnToPreviousState(this IStateMachine _sm)
	{
		int states = _sm.state;
		int previousStates = _sm.previousState;
		int addedStates = (~previousStates & states);
		int removedStates = (previousStates & ~states);
		
		_sm.previousState = states;
		_sm.state = states;
		_sm.OnExitState(states);
		_sm.OnEnterState(previousStates);
		if(addedStates != 0) _sm.OnStatesAdded(addedStates);
		if(removedStates != 0) _sm.OnStatesRemoved(removedStates);
	}

	/// <summary>Evaluates if State Machine has given state.</summary>
	/// <param name="_fsm">State Machine.</param>
	/// <param name="_state">State to evaluate.</param>
	/// <returns>True if Stete Machine has given state.</returns>
	public static bool HasState(this IFiniteStateMachine<int> _fsm, int _state)
	{
		int state = _fsm.state;
		return (state | _state) == state;
	}

	/// <summary>Evaluates if State Machine has all given states.</summary>
	/// <param name="_sm">State Machine.</param>
	/// <param name="_states">State's Flags to compare against State Machine's state.</param>
	/// <returns>True, false otherwise.</returns>
	public static bool HasStates(this IStateMachine _sm, int _states)
	{
		int states = _sm.state;
		return (states | _states) == states;
	}

	/// <summary>Evaluates if State Machine has any of the given states.</summary>
	/// <param name="_sm">State Machine.</param>
	/// <param name="_states">State's Flags to compare against State Machine's state.</param>
	/// <returns>True, false otherwise.</returns>
	public static bool HasAnyOfTheStates(this IStateMachine _sm, int _states)
	{
		return (_sm.state & _states) != 0;
	}

	/// <summary>Adds State to a State Machine.</summary>
	/// <param name="_sm">State Machine.</param>
	/// <param name="_states">States to add.</param>
	public static void AddStates(this IStateMachine _sm, int _states)
	{
		int states = _sm.state;
		int addedStates = ~states & _states;

		_sm.previousState = states;
		_sm.state |= _states;

		if(addedStates != 0) _sm.OnStatesAdded(addedStates); /// Get Added [not previously on] Flags: ~a & b 
	}

	/// <summary>Removes State to a State Machine.</summary>
	/// <param name="_sm">State Machine.</param>
	/// <param name="_states">States to remove.</param>
	public static void RemoveStates(this IStateMachine _sm, int _states)
	{
		int states = _sm.state;
		int removedStates = states & _states;

		_sm.previousState = states;
		states &= ~_states; /// Remove flags: a & ~b
		_sm.state = states;
		
		if(removedStates != 0) _sm.OnStatesRemoved(removedStates); /// Get Removed [previously on] Flags: a & b
	}

	/// <summary>Resets states by a given mask of flags.</summary>
	/// <param name="_sm">State Machine.</param>
	/// <param name="_states">Mask that contains the states to reset.</param>
	public static void ResetStates(this IStateMachine _sm, int _states)
	{
		int states = _sm.state;
		int reset = states & _states;

		if(reset != 0)
		{ /// If the mask contains flags that were on the state, proceed.
			_sm.previousState = states;
			_sm.OnStatesRemoved(reset);
			_sm.OnStatesAdded(reset);
		}
	}

	/// <summary>Removes flags and adds flags to IFiniteStateMachine's state.</summary>
	/// <param name="_sm">State Machine.</param>
	/// <param name="_removeStates">States to remove.</param>
	/// <param name="_addStates">States to add.</param>
	public static void RemoveAndAddStates(this IStateMachine _sm, int _removeStates, int _addStates)
	{
		int states = _sm.state;

		/*/// Use XOR on both flags to avoid duplicate bit entries between each flag:
		_removeStates ^= _addStates;
		_addStates ^= _removeStates;*/

		_sm.previousState = states;
		states &= ~_removeStates;
		states |= _addStates;
		_sm.state = states;

		int removedStates = states & _removeStates;
		int addedStates = ~states & _addStates;

		if(removedStates != 0) _sm.OnStatesRemoved(removedStates);
		if(addedStates != 0) _sm.OnStatesAdded(addedStates);
	}
#endregion

	/// <returns>Random Element f.</returns>
	public static T RandomElement<T>(this IEnumerable<T> _enumerable)
	{
		return _enumerable != null ? _enumerable.ElementAt(Random.Range(0, _enumerable.Count())) : default(T);
	}

	/// <summary>Iterates through all iterators and returns false once all iterators cannot move next.</summary>
	/// <param name="_iterators">Iterators to evaluate.</param>
	/// <returns>True while there is at least one iterator that keeps iterating.</returns>
	public static bool AllMoveNext(params IEnumerator[] _iterators)
	{
		int count = _iterators.Length;

		foreach(IEnumerator iterator in _iterators)
		{
			if(!iterator.MoveNext()) count--;
		}

		return count != 0;
	}

	/// <summary>Evaluates if a State Transition's Condition is met.</summary>
	/// <param name="_stateTransition">StateTransition having the condition to confirm.</param>
	/// <param name="_agent">Agent to evaluate.</param>
	/// <returns>True if the condition given the agent is met, false otherwise.</returns>
	public static bool ConditionMet<T>(this IStateTransition<T> _stateTransition, T _agent) where T : IFiniteStateAgent<T>
	{
		Predicate<T> condition = _stateTransition.TransitionPolicy;
		return condition != null ? condition(_agent) : false;
	}

	/// <summary>Utility function to subscribe object implementing IInputControllerHandler to InputController's events.</summary>
	/// <param name="_controllerHandler">IInputControllerHandler object to subscribe to events.</param>
	public static void SubscribeToInputControllerEvents(this IInputControllerHandler _controllerHandler)
	{
		InputController.onInputReceived += _controllerHandler.OnInputReceived;
		InputController.onRightAxesChange += _controllerHandler.OnRightAxesChange;
		InputController.onLeftAxesChange += _controllerHandler.OnLeftAxesChange;
		InputController.onRightTriggerAxisChange += _controllerHandler.OnRightTriggerAxisChange;
		InputController.onLeftTriggerAxisChange += _controllerHandler.OnLeftTriggerAxisChange;
		InputController.onDPadAxesChanges += _controllerHandler.OnDPadAxesChanges;
	}

	/// <summary>Utility function to unsubscribe object implementing IInputControllerHandler to InputController's events.</summary>
	/// <param name="_controllerHandler">IInputControllerHandler object to unsubscribe to events.</param>
	public static void UnsubscribeToInputControllerEvents(this IInputControllerHandler _controllerHandler)
	{
		InputController.onInputReceived -= _controllerHandler.OnInputReceived;
		InputController.onRightAxesChange -= _controllerHandler.OnRightAxesChange;
		InputController.onLeftAxesChange -= _controllerHandler.OnLeftAxesChange;
		InputController.onRightTriggerAxisChange -= _controllerHandler.OnRightTriggerAxisChange;
		InputController.onLeftTriggerAxisChange -= _controllerHandler.OnLeftTriggerAxisChange;
		InputController.onDPadAxesChanges -= _controllerHandler.OnDPadAxesChanges;
	}

	/// <summary>Subscribes to FOVSight's events.</summary>
	/// <param name="_FOVListener">IFOVListener implementer to subscribe.</param>
	/// <param name="_FOVSight">Sight to subscribe to.</param>
	public static void SubscribeToFOV(this IFOVListener _FOVListener, FOVSight _FOVSight)
	{
		_FOVSight.onTriggeredWithGameObject += _FOVListener.OnGameObjectCollision;
	}

	/// <summary>Unsubscribes to FOVSight's events.</summary>
	/// <param name="_FOVListener">IFOVListener implementer to unsubscribe.</param>
	/// <param name="_FOVSight">Sight to unsubscribe to.</param>
	public static void UnsubscribeToFOV(this IFOVListener _FOVListener, FOVSight _FOVSight)
	{
		_FOVSight.onTriggeredWithGameObject += _FOVListener.OnGameObjectCollision;
	}

	/// <summary>Updates IRayConvertible implementer's data from Ray.</summary>
	/// <param name="_ray">Ray to pass data to IRayConvertible implementer.</param>
	/// <param name="_direction">IRayConvertible implementer to pass data from Ray.</param>
	public static void UpdateToRayConvertible<T>(this Ray _ray, ref T _rayImplementer) where T : IRayConvertible 
	{
		_rayImplementer.origin = _ray.origin;
		_rayImplementer.direction = _ray.direction;
	}

	/// <summary>Updates IRay2DConvertible implementer's data from Ray2D.</summary>
	/// <param name="_ray">Ray2D to pass data to IRay2DConvertible implementer.</param>
	/// <param name="_direction">IRay2DConvertible implementer to pass data from Ray.</param>
	public static void UpdateToRayConvertible2D<T>(this Ray2D _ray, ref T _rayImplementer) where T : IRay2DConvertible 
	{
		_rayImplementer.origin = _ray.origin;
		_rayImplementer.direction = _ray.direction;
	}

	/// <summary>Creates Ray from IRayConvertible implementer.</summary>
	/// <param name="_rayImplementer">IRayConvertible implementer.</param>
	/// <returns>Ray from interface's data.</returns>
	public static Ray ToRay<T>(this T _rayImplementer) where T : IRayConvertible
	{
		return new Ray(_rayImplementer.origin, _rayImplementer.direction);
	}

	/// <summary>Creates Ray from IRay2DConvertible implementer.</summary>
	/// <param name="_rayImplementer">IRay2DConvertible implementer.</param>
	/// <returns>Ray from interface's data.</returns>
	public static Ray2D ToRay2D<T>(this T _rayImplementer) where T : IRay2DConvertible
	{
		return new Ray2D(_rayImplementer.origin, _rayImplementer.direction);
	}

	/// <summary>Gets underlying type T from IGeneric implementer.</summary>
	/// <param name="_generic">IGeneric implementer.</param>
	/// <returns>Type of IGeneric implementer.</returns>
	public static Type GetGenericType<T>(this IGeneric<T> _generic)
	{
		return typeof(T);
	}

	public static B UpCast<T, B>(this IUpCaster<T> _upCaster, B _childCast) where B : class, T
	{
		if(_upCaster.BaseCast is B)
		{
			return _childCast = _upCaster.BaseCast as B;
		}
		else
		{
			Debug.LogError("[VInterfaces] Provided BaseCast type is not of type " + typeof(B).Name + ".");
			return null;
		}
	}

#region IPoolObjectExtensions:
	/// <summary>Default Creation procedure for a MonoBehaviour implementing IPoolObject.</summary>
	/// <param name="_poolObject">Pool Object to extend.</param>
	public static void DefaultOnCreation<T>(this T _poolObject) where T : MonoBehaviour, IPoolObject
	{
		_poolObject.gameObject.SetActive(false);
		_poolObject.active = false;
	}

	/// <summary>Default Recycle procedure for a MonoBehaviour implementing IPoolObject.</summary>
	/// <param name="_poolObject">Pool Object to extend.</param>
	public static void DefaultOnRecycle<T>(this T _poolObject) where T : MonoBehaviour, IPoolObject
	{
		_poolObject.gameObject.SetActive(true);
		_poolObject.active = true;
	}

	/// <summary>Default Deactivation procedure for a MonoBehaviour implementing IPoolObject.</summary>
	/// <param name="_poolObject">Pool Object to extend.</param>
	public static void DefaultOnDeactivation<T>(this T _poolObject) where T : MonoBehaviour, IPoolObject
	{
		_poolObject.gameObject.SetActive(false);
		_poolObject.active = false;
	}

	/// <summary>Default Destruction procedure for a MonoBehaviour implementing IPoolObject.</summary>
	/// <param name="_poolObject">Pool Object to extend.</param>
	public static void DefaultOnDestruction<T>(this T _poolObject) where T : MonoBehaviour, IPoolObject
	{
		_poolObject.DefaultOnDeactivation();
		UnityEngine.Object.Destroy(_poolObject.gameObject);
	}

	/// <summary>Gets a string representing the Pool Object.</summary>
	/// <param name="_poolObject">Pool Object to debug.</param>
	/// <returns>String representing the extended Pool Object.</returns>
	public static string ToString<T>(this T _poolObject) where T : IPoolObject
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("Pool Object: ");
		builder.Append("\n");
		builder.Append("Don't Destroy on Load: ");
		builder.Append(_poolObject.dontDestroyOnLoad.ToString());
		builder.Append("\n");
		builder.Append("Active: ");
		builder.Append(_poolObject.active.ToString());

		return builder.ToString();
	}
#endregion

	/// <summary>Requests Component from IComponentEntity's implementer.</summary>
	/// <param name="_componentEntity">IComponentEntity's implementer.</param>
	/// <returns>Entity's component, if it has it attached.</returns>
	public static C RequestComponent<T, C>(this T _componentEntity) where T : MonoBehaviour, IComponentEntity<C> where C : Component
	{
		if(_componentEntity.component == null) _componentEntity.component = _componentEntity.GetComponent<C>();
		return _componentEntity.component;
	}

#region IComponentsExtensions:
	/// <summary>Adds set of children to Composite interface implementer.</summary>
	/// <param name="_composite">Composite to add the children to.</param>
	/// <param name="_children">Children to add to Composite.</param>
	public static void AddChildren<T>(this IComposite<T> _composite, params IComponent<T>[] _children)
	{
		if(_composite.children == null) _composite.children = new List<IComponent<T>>(_children.Length);
		foreach(IComponent<T> child in _children)
		{
			_composite.children.Add(child);
		}
	}

	/// <summary>Adds child to Decorator interface implementer.</summary>
	/// <param name="_decorator">Decorator to add the child to.</param>
	/// <param name="_child">Child to add to Decorator.</param>
	public static void AddChild<T>(this IDecorator<T> _decorator, IComponent<T> _child)
	{
		_decorator.child = _child;
	}
#endregion

#region IAgentComponentExtensions:
	/// <summary>Adds set of children to Composite interface implementer.</summary>
	/// <param name="_composite">Composite to add the children to.</param>
	/// <param name="_children">Children to add to Composite.</param>
	public static void AddChildren<T, R>(this IAgentComposite<T, R> _composite, params IAgentComponent<T, R>[] _children) where T : IComponentAgent<T, R>
	{
		if(_composite.children == null) _composite.children = new List<IAgentComponent<T, R>>(_children.Length);
		foreach(IAgentComponent<T, R> child in _children)
		{
			_composite.children.Add(child);
		}
	}
#endregion

#region IRangeExtensions:
	/// <summary>Gets highest value from range.</summary>
	/// <param name="_range">Range to get highest value from.</param>
	/// <returns>Highest value from Range.</returns>
	public static T GetMaximumValue<T>(this IRange<T> _range) where T : IComparable<T>
	{
		switch(_range.min.CompareTo(_range.max))
		{
			case -1:
			return _range.max;

			case 0:
			case 1:
			return _range.min;
		}

		return _range.min;
	}

	/// <summary>Gets lowest value from range.</summary>
	/// <param name="_range">Range to get lowest value from.</param>
	/// <returns>Lowest value from Range.</returns>
	public static T GetMinimumValue<T>(this IRange<T> _range) where T : IComparable<T>
	{
		switch(_range.min.CompareTo(_range.max))
		{
			case -1:
			case 0:
			return _range.min;

			case 1:
			return _range.max;
		}

		return _range.min;
	}
#endregion

	/// <summary>Debugs IGUIDebuggable's object on OnGUI's body.</summary>
	/// <param name="_debuggable">IGUIDebuggable's implementer.</param>
	public static void DebugOnGUI<T>(this T _debuggable) where T : IGUIDebuggable
	{
#if UNITY_EDITOR
		if(_debuggable.debug) GUI.Box(_debuggable.GUIRect, _debuggable.ToString());
#endif
	}
}
}