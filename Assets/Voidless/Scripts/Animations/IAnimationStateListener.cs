using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public interface IAnimationStateListener
{
	/// <summary>OnStateEnter is called when a transition starts and the state machine starts to evaluate this state.</summary>
	/// <param name="_animator">Animator's reference.</param>
	/// <param name="_stateInfo">AnimationState's Information.</param>
	/// <param name="_layerID">Layer's Index on the State Machine.</param>
    void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerID);

    /// <summary>OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks.</summary>
    /// <param name="_animator">Animator's reference.</param>
	/// <param name="_stateInfo">AnimationState's Information.</param>
	/// <param name="_layerID">Layer's Index on the State Machine.</param>
    void OnStateUpdate(Animator _animator, AnimatorStateInfo _stateInfo, int _layerID);

    /// <summary>OnStateExit is called when a transition ends and the state machine finishes evaluating this state.</summary>
    /// <param name="_animator">Animator's reference.</param>
	/// <param name="_stateInfo">AnimationState's Information.</param>
	/// <param name="_layerID">Layer's Index on the State Machine.</param>
    void OnStateExit(Animator _animator, AnimatorStateInfo _stateInfo, int _layerID);

    /// <summary>OnStateEnd is called when an animation ends.</summary>
    /// <param name="_animator">Animator's reference.</param>
    /// <param name="_stateInfo">AnimationState's Information.</param>
    /// <param name="_layerID">Layer's Index on the State Machine.</param>
    void OnStateEnd(Animator _animator, AnimatorStateInfo _stateInfo, int _layerID);

    /// <summary>OnStateMove is called right after Animator.OnAnimatorMove().</summary>
    /// <param name="_animator">Animator's reference.</param>
	/// <param name="_stateInfo">AnimationState's Information.</param>
	/// <param name="_layerID">Layer's Index on the State Machine.</param>
    void OnStateMove(Animator _animator, AnimatorStateInfo _stateInfo, int _layerID);

    /// <summary>OnStateIK is called right after Animator.OnAnimatorIK().</summary>
    /// <param name="_animator">Animator's reference.</param>
	/// <param name="_stateInfo">AnimationState's Information.</param>
	/// <param name="_layerID">Layer's Index on the State Machine.</param>
    void OnStateIK(Animator _animator, AnimatorStateInfo _stateInfo, int _layerID);
}
}