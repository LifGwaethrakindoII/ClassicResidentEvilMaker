using System.Collections;
using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Flags]
public enum AnimationFlags
{
    Undefined = 0,
    _0 = 1 << 0,
    _1 = 1 << 1,
    _2 = 1 << 2,
    _3 = 1 << 3,
    _4 = 1 << 4,
    _5 = 1 << 5,
    _6 = 1 << 6,
    _7 = 1 << 7,
    _8 = 1 << 8,
    _9 = 1 << 9,
    _10 = 1 << 10,
    _11 = 1 << 11,
    _12 = 1 << 12,
    _13 = 1 << 13,
    _14 = 1 << 14,
    _15 = 1 << 15,
    _16 = 1 << 16,
    _17 = 1 << 17,
    _18 = 1 << 18,
    _19 = 1 << 19,
    _20 = 1 << 20,
    _21 = 1 << 21,
    _22 = 1 << 22,
    _23 = 1 << 23,
    _24 = 1 << 24,
    _25 = 1 << 25,
    _26 = 1 << 26,
    _27 = 1 << 27,
    _28 = 1 << 28,
    _29 = 1 << 29,
    _30 = 1 << 30,
    _31 = 1 << 31,
}

public class AnimationCommandSender : AnimationStateSender
{
    public const int FLAG_STARTUP = 1 << 0;                                         /// <summary>Startup's Flag.</summary>
    public const int FLAG_ACTIVE = 1 << 1;                                          /// <summary>Active's Flag.</summary>
    public const int FLAG_RECOVERY = 1 << 2;                                        /// <summary>Recovery's Flag.</summary>
    public const int FLAG_END = 1 << 4;                                             /// <summary>End's Flag.</summary>

    [SerializeField] private AnimationFlags _flags;                                 /// <summary>Animation's Flags.</summary>
    [SerializeField] private int _subID;                                            /// <summary>Sub-ID [Additional to the Animation Flags].</summary>
    [Space(50f)]
    [SerializeField] private FloatWrapper _startupPercentage;                       /// <summary>Startup's Percentage on the Animation.</summary>
    [SerializeField] private FloatWrapper _activePercentage;                        /// <summary>Active's Percentage on the Animation.</summary>
    [SerializeField] private FloatWrapper _recoveryPercentage;                      /// <summary>Recovery's Percentage on the Animation.</summary>
    [SerializeField] private float _additionalWindow;                               /// <summary>Additional Command window's duration after the recovery.</summary>
    [SerializeField] private List<IAnimationCommandListener> _commandListeners;     /// <summary>AnimationState's Listeners.</summary>
    [SerializeField] private int _state;                                            /// <summary>State's Mask.</summary>
#if UNITY_EDITOR
    [HideInInspector] public AnimationClip clip;                                    /// <summary>Animation's Clip.</summary>
    [HideInInspector] public bool toggleAnimationStateData;                         /// <summary>Toggle Animation's State Data?.</summary>
    [HideInInspector] public float speed;                                           /// <summary>AnimationClip's Speed scalar.</summary>
#endif

#region Getters/Setters:
    /// <summary>Gets and Sets flags property.</summary>
    public AnimationFlags flags
    {
        get { return _flags; }
        set { _flags = value; }
    }

    /// <summary>Gets and Sets subID property.</summary>
    public int subID
    {
        get { return _subID; }
        set { _subID = value; }
    }

    /// <summary>Gets and Sets startupPercentage property.</summary>
    public FloatWrapper startupPercentage
    {
        get { return _startupPercentage; }
        set { _startupPercentage = value; }
    }

    /// <summary>Gets and Sets activePercentage property.</summary>
    public FloatWrapper activePercentage
    {
        get { return _activePercentage; }
        set { _activePercentage = value; }
    }

    /// <summary>Gets and Sets recoveryPercentage property.</summary>
    public FloatWrapper recoveryPercentage
    {
        get { return _recoveryPercentage; }
        set { _recoveryPercentage = value; }
    }

    /// <summary>Gets and Sets additionalWindow property.</summary>
    public float additionalWindow
    {
        get { return _additionalWindow; }
        set { _additionalWindow = value; }
    }

    /// <summary>Gets and Sets commandListeners property.</summary>
    public List<IAnimationCommandListener> commandListeners
    {
        get { return _commandListeners; }
        private set { _commandListeners = value; }
    }

    /// <summary>Gets and Sets state property.</summary>
    public int state
    {
        get { return _state; }
        protected set { _state = value; }
    }
#endregion

    /// <summary>Resets AnimationCommandSender's instance to its default values.</summary>
    public void Reset()
    {
        if(startupPercentage == null) startupPercentage = new FloatWrapper(0.0f);
        if(activePercentage == null) activePercentage = new FloatWrapper(0.0f);
        if(recoveryPercentage == null) recoveryPercentage = new FloatWrapper(0.0f);
#if UNITY_EDITOR
        speed = 1.0f;
#endif
    }

    /// <summary>Adds IAnimationCommandListener to the list of Listeners.</summary>
    /// <param name="_listener">IAAnimationStateListener to Add.</param>
    public void AddListener(IAnimationCommandListener _listener)
    {
        base.AddListener(_listener);
        if(commandListeners == null) commandListeners = new List<IAnimationCommandListener>();
        if(!commandListeners.Contains(_listener)) commandListeners.Add(_listener);
    }

    /// <summary>Removes IAnimationCommandListener to the list of Listeners.</summary>
    /// <param name="_listener">IAAnimationStateListener to Remove.</param>
    public void RemoveListener(IAnimationCommandListener _listener)
    {
        base.RemoveListener(_listener);
        if(commandListeners == null || !commandListeners.Contains(_listener)) return;
        commandListeners.Remove(_listener);
    }

    /// <summary> OnStateEnter is called when a transition starts and the state machine starts to evaluate this state.</summary>
    /// <param name="_animator">Animator's reference.</param>
    /// <param name="_stateInfo">AnimationState's Information.</param>
    /// <param name="_layerID">Layer's Index on the State Machine.</param>
    public override void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerID)
    {
        state = 0;

        if(commandListeners == null) return;

        foreach(IAnimationCommandListener listener in commandListeners)
        {
            listener.OnStateEnter(_animator, _stateInfo, _layerID);
            state |= FLAG_STARTUP;
            if(startupPercentage > 0.0f) InvokeState(listener, FLAG_STARTUP, _animator, _stateInfo, flags, subID, _layerID);

            listener.OnAnimationCommandEnter(
                _animator,
                startupPercentage.value,
                activePercentage.value,
                recoveryPercentage.value,
                additionalWindow,
                _layerID
            );
        }
    }

    /// <summary> OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks.</summary>
    /// <param name="_animator">Animator's reference.</param>
    /// <param name="_stateInfo">AnimationState's Information.</param>
    /// <param name="_layerID">Layer's Index on the State Machine.</param>
    public override void OnStateUpdate(Animator _animator, AnimatorStateInfo _stateInfo, int _layerID)
    {
        if(commandListeners == null || (state | FLAG_END) == state) return;

        float t = _stateInfo.NormalizedTime();
        bool invokeActive = ((state | FLAG_ACTIVE) != state && t >= startupPercentage && t < activePercentage);
        bool invokeRecovery = ((state | FLAG_RECOVERY) != state && t >= activePercentage && t <= recoveryPercentage);
        bool invokeEnd = ((state | FLAG_END) != state && t >= 1.0f);

        if(invokeActive) state |= FLAG_ACTIVE;
        if(invokeRecovery) state |= FLAG_RECOVERY;
        if(invokeEnd) state |= FLAG_END;

        foreach(IAnimationCommandListener listener in commandListeners)
        {
            if(!invokeEnd) listener.OnStateUpdate(_animator, _stateInfo, _layerID);

            if(invokeActive) InvokeState(listener, FLAG_ACTIVE, _animator, _stateInfo, flags, subID, _layerID);
            if(invokeRecovery) InvokeState(listener, FLAG_RECOVERY, _animator, _stateInfo, flags, subID, _layerID);
            if(invokeEnd)
            {
                if(additionalWindow > 0.0f) listener.OnAdditionalWindow(_animator, _stateInfo, additionalWindow, _flags, _subID, _layerID);
                else InvokeState(listener, FLAG_END, _animator, _stateInfo, flags, subID, _layerID);
            }
        }
    }

    /// <summary> OnStateExit is called when a transition ends and the state machine finishes evaluating this state.</summary>
    /// <param name="_animator">Animator's reference.</param>
    /// <param name="_stateInfo">AnimationState's Information.</param>
    /// <param name="_layerID">Layer's Index on the State Machine.</param>
    public override void OnStateExit(Animator _animator, AnimatorStateInfo _stateInfo, int _layerID)
    {
        state = 0;

        if(commandListeners == null) return;

        foreach(IAnimationCommandListener listener in commandListeners)
        {
            listener.OnStateExit(_animator, _stateInfo, _layerID);
        }
    }

    /// <summary>Invokes Animation Event.</summary>
    /// <param name="_listener">Listener to invoke the event to.</param>
    /// <param name="_stateID">State's ID.</param>
    /// <param name="_animator">Animator's reference.</param>
    /// <param name="_stateInfo">AnimationState's Information.</param>
    /// <param name="_flags">Additional Animation's Flags.</param>
    /// <param name="_subID">Additional Animation's Sub-ID.</param>
    /// <param name="_layerID">Layer's Index on the State Machine.</param>
    private void InvokeState(IAnimationCommandListener _listener, int _stateID, Animator _animator, AnimatorStateInfo _stateInfo, AnimationFlags _flags, int _subID, int _layerID)
    {
        string state = string.Empty;

        switch(_stateID)
        {
            case FLAG_STARTUP:
            state = "Startup";
            _listener.OnStartup(_animator, _stateInfo, _flags, _subID, _layerID);
            break;

            case FLAG_ACTIVE:
            state = "Active";
            _listener.OnActive(_animator, _stateInfo, _flags, _subID, _layerID);
            break;

            case FLAG_RECOVERY:
            state = "Recovery";
            _listener.OnRecovery(_animator, _stateInfo, _flags, _subID, _layerID);
            break;

            case FLAG_END:
            state = "End";
            _listener.OnStateEnd(_animator, _stateInfo, _layerID);
            break;
        }
    }
}
}