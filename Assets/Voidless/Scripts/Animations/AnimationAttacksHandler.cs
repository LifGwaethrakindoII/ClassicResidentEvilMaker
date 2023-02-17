using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public enum AnimationCommandState
{
    None,
    Startup,
    Active,
    Recovery,
    End
}

public enum AttackState
{
	None,
	Waiting,
	Attacking,
	AttackWindow,
}

/// <summary>Event invoked whan an Animation Attack event occurs.</summary>
/// <param name="_state">Animation Attack's Event/State.</param>
public delegate void OnAnimationAttackEvent(AnimationCommandState _state);

public class AnimationAttacksHandler : MonoBehaviour, IAnimationCommandListener
{
	public event OnAnimationAttackEvent onAnimationAttackEvent; 	/// <summary>OnAnimationAttackState event delegate.</summary>
	
	public const int FLAG_STARTUP = 1 << 0;                         /// <summary>Startup's Flag.</summary>
    public const int FLAG_ACTIVE = 1 << 1;                          /// <summary>Active's Flag.</summary>
    public const int FLAG_RECOVERY = 1 << 2;                        /// <summary>Recovery's Flag.</summary>
    public const int FLAG_END = 1 << 4;                             /// <summary>End's Flag.</summary>

    [SerializeField] private AnimationFlags _flags; 				/// <summary>Animation Flags that this listener accepts on events' callbacks.</summary>
	[SerializeField] private Animator _animator; 					/// <summary>Animator's Component.</summary>
	[SerializeField] private int[] _comboLimits; 					/// <summary>Combos' Limits.</summary>
	private int _attackID; 											/// <summary>Current Attacks' ID.</summary>
	private int _previousComboIndex; 								/// <summary>Previous Combo's Index.</summary>
	private AttackState _state; 									/// <summary>Current State.</summary>
	private Coroutine attackHandler; 								/// <summary>Attack's Cooldown Coroutine reference.</summary>
	private Coroutine attackWindow; 								/// <summary>AttackWindow Coroutine's reference.</summary>						

#region Getters/Setters:
	/// <summary>Gets and Sets flags property.</summary>
	public AnimationFlags flags
	{
		get { return _flags; }
		set { _flags = value; }
	}

	/// <summary>Gets and Sets comboLimits property.</summary>
	public int[] comboLimits
	{
		get { return _comboLimits; }
		set { _comboLimits = value; }
	}

	/// <summary>Gets and Sets attackID property.</summary>
	public int attackID
	{
		get { return _attackID; }
		set { _attackID = value; }
	}

	/// <summary>Gets and Sets previousComboIndex property.</summary>
	public int previousComboIndex
	{
		get { return _previousComboIndex; }
		set { _previousComboIndex = value; }
	}

	/// <summary>Gets and Sets state property.</summary>
	public AttackState state
	{
		get { return _state; }
		set { _state = value; }
	}

	/// <summary>Gets and Sets animator property.</summary>
	public Animator animator
	{
		get { return _animator; }
		set { _animator = value; }
	}
#endregion


	/// <summary>AnimationAttacksHandler's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		if(animator != null) animator.SubscribeToAnimationCommandSenders(this);
		previousComboIndex = -1;
	}

	/// <summary>Begins Attack.</summary>
	public bool BeginAttack(int _index = 0)
	{
		if(state == AttackState.Attacking
		|| state == AttackState.Waiting
		|| comboLimits == null) return false;

		//if(previousComboIndex > -1 && _index != previousComboIndex) CancelAttack();

		int limit = comboLimits[Mathf.Clamp(_index, 0, comboLimits.Length - 1)];

		if(attackID > limit) return false;

		attackID = Mathf.Min(++attackID, limit);
		state = AttackState.Waiting;
		previousComboIndex = _index;
		CancelAttackWindow();

		return true;
	}

	/// <summary>Cancels Attack.</summary>
	public void CancelAttack()
	{
		CancelAttackWindow();
		CancelAttackHandler();
		state = AttackState.None;
		attackID = 0;
	}

	/// <summary>Cancels Attack's Window.</summary>
	private void CancelAttackWindow()
	{
		this.DispatchCoroutine(ref attackWindow);
	}

	/// <summary>Cancels Attack's Handler.</summary>
	private void CancelAttackHandler()
	{
		this.DispatchCoroutine(ref attackHandler);
	}

	/// <summary>Invokes Animation's Event.</summary>
	/// <param name="_event">Animation's Event.</param>
	private void InvokeAnimationAttackEvent(AnimationCommandState _state)
	{
		if(onAnimationAttackEvent != null) onAnimationAttackEvent(_state);	
	}

#region Callbacks:
	/// <summary>Callback invoked when an animation begins.</summary>
	/// <param name="_animator">Animator's reference.</param>
    /// <param name="_startUpPercentage">Startup's Percentage.</param>
    /// <param name="_activePercentage">Active's Percentage.</param>
    /// <param name="_recoveryPercentage">Recovery's Percentage.</param>
    /// <param name="_additionalWindow">Additional Window's duration [0.0f by default].</param>
    /// <param name="_layerID">Layer's Index on the State Machine [0 by default].</param>
    public void OnAnimationCommandEnter(Animator _animator, float _startUpPercentage, float _activePercentage, float _recoveryPercentage, float _additionalWindow = 0.0f, int _layerID = 0)
    {
    	state = AttackState.Attacking;
    	this.StartCoroutine(CommandHandler(_animator, _startUpPercentage, _activePercentage, _recoveryPercentage, _additionalWindow, _layerID), ref attackHandler);
    }

	/// <summary>OnStateEnter is called when a transition starts and the state machine starts to evaluate this state.</summary>
	/// <param name="_animator">Animator's reference.</param>
	/// <param name="_stateInfo">AnimationState's Information.</param>
	/// <param name="_layerID">Layer's Index on the State Machine.</param>
    public void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerID)
	{

	}

    /// <summary>OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks.</summary>
    /// <param name="_animator">Animator's reference.</param>
	/// <param name="_stateInfo">AnimationState's Information.</param>
	/// <param name="_layerID">Layer's Index on the State Machine.</param>
    public void OnStateUpdate(Animator _animator, AnimatorStateInfo _stateInfo, int _layerID)
	{

	}

    /// <summary>OnStateExit is called when a transition ends and the state machine finishes evaluating this state.</summary>
    /// <param name="_animator">Animator's reference.</param>
	/// <param name="_stateInfo">AnimationState's Information.</param>
	/// <param name="_layerID">Layer's Index on the State Machine.</param>
    public void OnStateExit(Animator _animator, AnimatorStateInfo _stateInfo, int _layerID)
    {

    }

    /// <summary>OnStateEnd is called when an animation ends.</summary>
    /// <param name="_animator">Animator's reference.</param>
    /// <param name="_stateInfo">AnimationState's Information.</param>
    /// <param name="_layerID">Layer's Index on the State Machine.</param>
    public void OnStateEnd(Animator _animator, AnimatorStateInfo _stateInfo, int _layerID)
    {
    	CancelAttack();
    	InvokeAnimationAttackEvent(AnimationCommandState.End);
    }

    /// <summary>OnStateMove is called right after Animator.OnAnimatorMove().</summary>
    /// <param name="_animator">Animator's reference.</param>
	/// <param name="_stateInfo">AnimationState's Information.</param>
	/// <param name="_layerID">Layer's Index on the State Machine.</param>
    public void OnStateMove(Animator _animator, AnimatorStateInfo _stateInfo, int _layerID) { /*...*/ }

    /// <summary>OnStateIK is called right after Animator.OnAnimatorIK().</summary>
    /// <param name="_animator">Animator's reference.</param>
	/// <param name="_stateInfo">AnimationState's Information.</param>
	/// <param name="_layerID">Layer's Index on the State Machine.</param>
    public void OnStateIK(Animator _animator, AnimatorStateInfo _stateInfo, int _layerID) { /*...*/ }

    /// <summary>Callback invoked when a command setup begins.</summary>
    /// <param name="_animator">Animator's reference.</param>
    /// <param name="_stateInfo">AnimationState's Information.</param>
    /// <param name="_flags">Additional Animation's Flags.</param>
    /// <param name="_subID">Additional Animation's Sub-ID.</param>
    /// <param name="_layerID">Layer's Index on the State Machine.</param>
    public void OnStartup(Animator _animator, AnimatorStateInfo _stateInfo, AnimationFlags _flags, int  _subID, int _layerID)
    {
    	if((_flags | flags) != _flags) return;

    	InvokeAnimationAttackEvent(AnimationCommandState.Startup);
    }

    /// <summary>Callback invoked whan a command activation begins.</summary>
    /// <param name="_animator">Animator's reference.</param>
    /// <param name="_stateInfo">AnimationState's Information.</param>
    /// <param name="_flags">Additional Animation's Flags.</param>
    /// <param name="_subID">Additional Animation's Sub-ID.</param>
    /// <param name="_layerID">Layer's Index on the State Machine.</param>
    public void OnActive(Animator _animator, AnimatorStateInfo _stateInfo, AnimationFlags _flags, int  _subID, int _layerID)
    {
    	if((_flags | flags) != _flags) return;

    	InvokeAnimationAttackEvent(AnimationCommandState.Active);
    }

    /// <summary>Callback invoked when a command recovery begins.</summary>
    /// <param name="_animator">Animator's reference.</param>
    /// <param name="_stateInfo">AnimationState's Information.</param>
    /// <param name="_flags">Additional Animation's Flags.</param>
    /// <param name="_subID">Additional Animation's Sub-ID.</param>
    /// <param name="_layerID">Layer's Index on the State Machine.</param>
    public void OnRecovery(Animator _animator, AnimatorStateInfo _stateInfo, AnimationFlags _flags, int  _subID, int _layerID)
    {
    	if((_flags | flags) != _flags) return;

    	state = AttackState.AttackWindow;
    	InvokeAnimationAttackEvent(AnimationCommandState.Recovery);
    }

    /// <summary>Callback invoked when [and if] the activation window begins.</summary>
    /// <param name="_animator">Animator's reference.</param>
    /// <param name="_stateInfo">AnimationState's Information.</param>
    /// <param name="_duration">Window's Duration.</param>
    /// <param name="_flags">Additional Animation's Flags.</param>
    /// <param name="_subID">Additional Animation's Sub-ID.</param>
    /// <param name="_layerID">Layer's Index on the State Machine.</param>
    public void OnAdditionalWindow(Animator _animator, AnimatorStateInfo _stateInfo, float _duration, AnimationFlags _flags, int _subID, int _layerID)
    {
    	state = AttackState.AttackWindow;
    	this.StartCoroutine(AttackWindow(_duration, attackID), ref attackWindow);
    }
#endregion

    /// <param name="_animator">Animator's reference.</param>
    /// <param name="_startUpPercentage">Startup's Percentage.</param>
    /// <param name="_activePercentage">Active's Percentage.</param>
    /// <param name="_recoveryPercentage">Recovery's Percentage.</param>
    /// <param name="_additionalWindow">Additional Window's duration [0.0f by default].</param>
    /// <param name="_layerID">Layer's Index on the State Machine [0 by default].</param>
    private IEnumerator CommandHandler(Animator _animator, float _startUpPercentage, float _activePercentage, float _recoveryPercentage, float _additionalWindow = 0.0f, int _layerID = 0)
    {
    	AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(_layerID);
    	SecondsDelayWait wait = new SecondsDelayWait(info.length);
    	int stateFlags = 0;
    	float t = 0.0f;

    	while(wait.MoveNext())
    	{
    		t = info.normalizedTime;

    		if((stateFlags | FLAG_STARTUP) != stateFlags && _startUpPercentage > 0.0f)
    		{
    			stateFlags |= FLAG_STARTUP;
    			InvokeAnimationAttackEvent(AnimationCommandState.Startup);
    		
    		} else if((stateFlags | FLAG_ACTIVE) != stateFlags && t >= _startUpPercentage  && t < _activePercentage)
    		{
    			stateFlags |= FLAG_ACTIVE;
    			InvokeAnimationAttackEvent(AnimationCommandState.Active);
    		
    		} else if((stateFlags | FLAG_RECOVERY) != stateFlags && t >= _activePercentage  && t < _recoveryPercentage)
    		{
    			stateFlags |= FLAG_RECOVERY;
    			state = AttackState.AttackWindow;
    			InvokeAnimationAttackEvent(AnimationCommandState.Recovery);
    		}

    		yield return null;

    		info = _animator.GetCurrentAnimatorStateInfo(_layerID);
    	}

    	if(_additionalWindow > 0.0f)
    	{
    		state = AttackState.AttackWindow;
    		wait.ChangeDurationAndReset(_additionalWindow);
    		while(wait.MoveNext()) yield return null;
    	}

    	InvokeAnimationAttackEvent(AnimationCommandState.End);
    }

    /// <summary>Performs an attack window.</summary>
	/// <param name="_duration">Attack Window's Duration.</param>
	/// <param name="_ID">Attack's ID at the moment the Windoe coroutine was created.</param>
	public IEnumerator AttackWindow(float _duration, int _ID)
	{
		SecondsDelayWait wait = new SecondsDelayWait(_duration);
		while(wait.MoveNext()) yield return null;
		if(state == AttackState.AttackWindow || attackID == _ID)
		{
			CancelAttack();
			InvokeAnimationAttackEvent(AnimationCommandState.End);
		}
	}
}
}