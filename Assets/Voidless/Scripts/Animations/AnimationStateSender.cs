using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class AnimationStateSender : StateMachineBehaviour
{
    [SerializeField] private AnimationState state;     /// <summary>please b0ss.</summary>
    private List<IAnimationStateListener> _listeners;   /// <summary>AnimaationState's Listeners.</summary>

    /// <summary>Gets and Sets listeners property.</summary>
    public List<IAnimationStateListener> listeners
    {
        get { return _listeners; }
        private set { _listeners = value; }
    }

    /// <summary>Adds IAnimationStateListener to the list of Listeners.</summary>
    /// <param name="_listener">IAAnimationStateListener to Add.</param>
    public void AddListener(IAnimationStateListener _listener)
    {
        if(listeners == null) listeners = new List<IAnimationStateListener>();
        if(!listeners.Contains(_listener)) listeners.Add(_listener);
    }

    /// <summary>Removes IAnimationStateListener to the list of Listeners.</summary>
    /// <param name="_listener">IAAnimationStateListener to Remove.</param>
    public void RemoveListener(IAnimationStateListener _listener)
    {
        if(listeners == null || !listeners.Contains(_listener)) return;
        listeners.Remove(_listener);
    }

    /// <summary> OnStateEnter is called when a transition starts and the state machine starts to evaluate this state.</summary>
    /// <param name="_animator">Animator's reference.</param>
    /// <param name="_stateInfo">AnimationState's Information.</param>
    /// <param name="_layerID">Layer's Index on the State Machine.</param>
    public override void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerID)
    {
        if(listeners == null) return;

        foreach(IAnimationStateListener listener in listeners)
        {
            listener.OnStateEnter(_animator, _stateInfo, _layerID);
        }
    }

    /// <summary> OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks.</summary>
    /// <param name="_animator">Animator's reference.</param>
    /// <param name="_stateInfo">AnimationState's Information.</param>
    /// <param name="_layerID">Layer's Index on the State Machine.</param>
    public override void OnStateUpdate(Animator _animator, AnimatorStateInfo _stateInfo, int _layerID)
    {
        if(listeners == null) return;

        foreach(IAnimationStateListener listener in listeners)
        {
            listener.OnStateUpdate(_animator, _stateInfo, _layerID);
            if(_stateInfo.NormalizedTime() >= 1.0f) listener.OnStateEnd(_animator, _stateInfo, _layerID);
        }
    }

    /// <summary> OnStateExit is called when a transition ends and the state machine finishes evaluating this state.</summary>
    /// <param name="_animator">Animator's reference.</param>
    /// <param name="_stateInfo">AnimationState's Information.</param>
    /// <param name="_layerID">Layer's Index on the State Machine.</param>
    public override void OnStateExit(Animator _animator, AnimatorStateInfo _stateInfo, int _layerID)
    {
        if(listeners == null) return;

        foreach(IAnimationStateListener listener in listeners)
        {
            listener.OnStateExit(_animator, _stateInfo, _layerID);
        }
    }

    /// <summary> OnStateMove is called right after Animator.OnAnimatorMove().</summary>
    /// <param name="_animator">Animator's reference.</param>
    /// <param name="_stateInfo">AnimationState's Information.</param>
    /// <param name="_layerID">Layer's Index on the State Machine.</param>
    public override void OnStateMove(Animator _animator, AnimatorStateInfo _stateInfo, int _layerID) { /* Implement code that processes and affects root motion*/ }

    /// <summary> OnStateIK is called right after Animator.OnAnimatorIK().</summary>
    /// <param name="_animator">Animator's reference.</param>
    /// <param name="_stateInfo">AnimationState's Information.</param>
    /// <param name="_layerID">Layer's Index on the State Machine.</param>
    public override void OnStateIK(Animator _animator, AnimatorStateInfo _stateInfo, int _layerID) { /* Implement code that sets up animation IK (inverse kinematics)*/ }
}
}