using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Voidless
{
public enum HealthEvent
{
    Depleted,
    Replenished,
    HitStunEnds,
    InvincibilityEnds,
    FullyDepleted,
    FullyReplenished
}

/// <summary>Event invoked when a Health's event has occured.</summary>
/// <param name="_event">Type of Health Event.</param>
/// <param name="_amount">Amount of health that changed [0.0f by default].</param>
/// <param name="_object">GameObject that caused the event, null be default.</param>
public delegate void OnHealthEvent(HealthEvent _event, float _amount = 0.0f, GameObject _object = null);

/// <summary>Event invoked when an event of a Health's instance has occured.</summary>
/// <param name="_health">Health's Instance.</param>
/// <param name="_event">Type of Health Event.</param>
/// <param name="_amount">Amount of health that changed [0.0f by default].</param>
/// <param name="_object">GameObject that caused the event, null be default.</param>
public delegate void OnHealthInstanceEvent(Health _health, HealthEvent _event, float _amount = 0.0f, GameObject _object = null);

public class Health : MonoBehaviour, IStateMachine
{
	public event OnHealthEvent onHealthEvent;                   /// <summary>OnHealthEvent Event Delegate.</summary>
    public event OnHealthInstanceEvent onHealthInstanceEvent;   /// <summary>OnHealthInstanceEvent Event Delegate.</summary>

    public const int ID_STATE_ONHITSTUN = 1 << 0;               /// <summary>On Hit-Stun State's ID.</summary>
    public const int ID_STATE_ONINVINCIBILITY = 1 << 1;         /// <summary>On Invincibility State's ID.</summary>

    [InfoBox("@ToString()")]
    [SerializeField] private float _maxHP;                      /// <summary>Maximum amount of Health.</summary>
    [SerializeField] private float _hitStunDuration;            /// <summary>Cooldown duration when on Hit-Stun mode.</summary>
    [SerializeField] private float _invincibilityDuration;      /// <summary>Cooldown duration when on Invincible mode.</summary>
    [SerializeField] private GameObjectTag[] _inmunities;       /// <summary>Inmunities' Set.</summary>
    private Cooldown _invincibilityCooldown;                    /// <summary>Invincibility Cooldown's reference.</summary>
    private Cooldown _hitStunCooldown;                          /// <summary>Hit-Stun Cooldown's reference.</summary>
    private float _hp;                                          /// <summary>Current HP.</summary>
    private int _state;                                         /// <summary>Health's Current State.</summary>
    private int _previousState;                                 /// <summary>Health's Previous State.</summary>
    public int ignoreResetMask { get; set; }                    /// <summary>Mask that selectively contains state to ignore resetting if they were added again [with AddState's method]. As it is 0 by default, it won't ignore resetting any state [~0 = 11111111]</summary>

#region Getters/Setters:
    /// <summary>Gets and Sets maxHP property.</summary>
    public float maxHP
    {
    	get { return _maxHP; }
    	set { _maxHP = value; }
    }

    /// <summary>Gets and Sets invincibilityDuration property.</summary>
    public float invincibilityDuration
    {
        get { return _invincibilityDuration; }
        set { _invincibilityDuration = value; }
    }

    /// <summary>Gets and Sets hitStunDuration property.</summary>
    public float hitStunDuration
    {
        get { return _hitStunDuration; }
        set { _hitStunDuration = value; }
    }

    /// <summary>Gets and Sets hp property.</summary>
    public float hp
    {
    	get { return _hp; }
    	set { _hp = value; }
    }

    /// <summary>Gets and Sets inmunities property.</summary>
    public GameObjectTag[] inmunities
    {
        get { return _inmunities; }
        set { _inmunities = value; }
    }

    /// <summary>Gets hpRatio property.</summary>
    public float hpRatio { get { return hp / maxHP; } }

    /// <summary>Gets invincibilityProgress property.</summary>
    public float invincibilityProgress { get { return invincibilityCooldown.progress; } }

    /// <summary>Gets and Sets invincibilityCooldown property.</summary>
    public Cooldown invincibilityCooldown
    {
        get { return _invincibilityCooldown; }
        private set { _invincibilityCooldown = value; }
    }

    /// <summary>Gets and Sets hitStunCooldown property.</summary>
    public Cooldown hitStunCooldown
    {
        get { return _hitStunCooldown; }
        private set { _hitStunCooldown = value; }
    }

    /// <summary>Gets and Sets state property.</summary>
    public int state
    {
        get { return _state; }
        set { _state = value; }
    }

    /// <summary>Gets and Sets previousState property.</summary>
    public int previousState
    {
        get { return _previousState; }
        set { _previousState = value; }
    }

    /// <summary>Gets onInvincibility property.</summary>
    public bool onInvincibility { get { return invincibilityCooldown != null ? invincibilityCooldown.onCooldown : false; } }

    /// <summary>Gets onHitStun property.</summary>
    public bool onHitStun { get { return hitStunCooldown != null ? hitStunCooldown.onCooldown : false; } }
#endregion

    /// <summary>Resets Health's instance to its default values.</summary>
    public void Reset()
    {
        if(this.HasStates(ID_STATE_ONINVINCIBILITY)) OnInvincibilityCooldownEnds();
        hp = maxHP;
    }

    /// <summary>Health's instance initialization when loaded [Before scene loads].</summary>
    private void Awake()
    {
    	hp = maxHP;
        hitStunCooldown = new Cooldown(this, hitStunDuration);
        invincibilityCooldown = new Cooldown(this, invincibilityDuration);
        hitStunCooldown.OnCooldownEnds.AddListener(OnHitStunCooldownEnds);
        invincibilityCooldown.OnCooldownEnds.AddListener(OnInvincibilityCooldownEnds);
    }

    /// <summary>Assigns damage to this Health Container.</summary>
    /// <param name="_damage">Damage to inflict.</param>
    /// <param name="_applyInvincibility">Apply Hit-Stun? True by default.</param>
    /// <param name="_applyInvincibility">Apply Invincibility? True by default.</param>
    /// <param name="_object">GameObject that applies the damage, null be default.</param>
    public void GiveDamage(float _damage, bool _applyHitStun = true, bool _applyInvincibility = true, GameObject _object = null)
    {
        if(inmunities != null && _object != null) foreach(GameObjectTag tag in inmunities)
        {
            if(_object.CompareTag(tag)) return;
        }

        /// If the current state is onInvincibility or the damage to receive is less or equal than '0', do nothing.
        if(onInvincibility || _damage <= 0.0f) return;

        _damage = _damage.Clamp(0.0f, hp);
    	hp -= _damage;
    	
    	if(hp > 0.0f)
        {
            if(onHealthEvent != null) onHealthEvent(HealthEvent.Depleted, _damage, _object);
            if(onHealthInstanceEvent != null) onHealthInstanceEvent(this, HealthEvent.Depleted, _damage, _object);

        } else if(hp == 0.0)
        {
            if(onHealthEvent != null) onHealthEvent(HealthEvent.FullyDepleted, _damage, _object);
            if(onHealthInstanceEvent != null) onHealthInstanceEvent(this, HealthEvent.FullyDepleted, _damage, _object);
        }

        if(hitStunDuration > 0.0f && _applyHitStun && hp > 0.0f)
        BeginHitStunCooldown();

        if(invincibilityDuration > 0.0f && _applyInvincibility && hp > 0.0f)
        BeginInvincibilityCooldown();
    }

    /// <summary>Replenishes Health.</summary>
    /// <param name="_amount">Amount to replenish.</param>
    public void ReplenishHealth(float _amount)
    {
        if(_amount <= 0.0f) return;

        _amount = _amount.Clamp(0.0f, maxHP - hp);
        hp += _amount;

        if(hp < maxHP)
        {
            if(onHealthEvent != null) onHealthEvent(HealthEvent.Replenished, _amount);
            if(onHealthInstanceEvent != null) onHealthInstanceEvent(this, HealthEvent.Replenished, _amount);

        } else if(hp == maxHP)
        {
            if(onHealthEvent != null) onHealthEvent(HealthEvent.FullyReplenished, _amount);
            if(onHealthInstanceEvent != null) onHealthInstanceEvent(this, HealthEvent.FullyReplenished, _amount);
        }
    }

    /// <summary>Sets Maximum HP.</summary>
    /// <param name="_maxHP">Max HP.</param>
    /// <param name="_resetHP">Reset current HP [false by default].</param>
    public void SetMaxHP(float _maxHP, bool _resetHP = false)
    {
        maxHP = _maxHP;
        if(_resetHP) Reset();
    }

    /// <summary>Begins Hit-Stun's Cooldown.</summary>
    public void BeginHitStunCooldown()
    {
        this.AddStates(ID_STATE_ONHITSTUN);
        hitStunCooldown.Begin();
    }

    /// <summary>Begins Invincibility's Cooldown.</summary>
    public void BeginInvincibilityCooldown()
    {
        this.AddStates(ID_STATE_ONINVINCIBILITY);
        invincibilityCooldown.Begin();
    }

    /// <summary>Callback internally invoked when the Hit-Stun Cooldown ends.</summary>
    public void OnHitStunCooldownEnds()
    {
        this.RemoveStates(ID_STATE_ONHITSTUN);
        if(onHealthEvent != null) onHealthEvent(HealthEvent.HitStunEnds);
        if(onHealthInstanceEvent != null) onHealthInstanceEvent(this, HealthEvent.HitStunEnds);
    }

    /// <summary>Callback internally invoked when the Invincibility Cooldown ends.</summary>
    public void OnInvincibilityCooldownEnds()
    {
        this.RemoveStates(ID_STATE_ONINVINCIBILITY);
        //if(invincibilityCooldown != null) invincibilityCooldown.End(); /// This provokes a StackOverflowException...
        if(onHealthEvent != null) onHealthEvent(HealthEvent.InvincibilityEnds);
        if(onHealthInstanceEvent != null) onHealthInstanceEvent(this, HealthEvent.InvincibilityEnds);
    }

#region IFiniteStateMachine:
    /// <summary>Enters int State.</summary>
    /// <param name="_state">int State that will be entered.</param>
    public virtual void OnEnterState(int _state) {/*...*/}

    /// <summary>Exits int State.</summary>
    /// <param name="_state">int State that will be left.</param>
    public virtual void OnExitState(int _state) {/*...*/}

    /// <summary>Callback invoked when new state's flags are added.</summary>
    /// <param name="_state">State's flags that were added.</param>
    public virtual void OnStatesAdded(int _state) {/*...*/}

    /// <summary>Callback invoked when new state's flags are removed.</summary>
    /// <param name="_state">State's flags that were removed.</param>
    public virtual void OnStatesRemoved(int _state) {/*...*/}
#endregion

    /// <returns>String representing this Health's instance.</returns>
    public string ToString()
    {
        StringBuilder builder = new StringBuilder();

        builder.AppendLine("Health: \n{");
        builder.Append("\tMax HP: ");
        builder.AppendLine(maxHP.ToString());

        if(Application.isPlaying)
        {
            builder.Append("\tCurrent HP: ");
            builder.AppendLine(hp.ToString());
            builder.Append("\tHP Ratio (HP / MaxHP): ");
            builder.AppendLine(hpRatio.ToString());
            builder.Append("\tOn Hit-Stun: ");
            builder.AppendLine(onHitStun.ToString());
            builder.Append("\tOn Invincibility: ");
            builder.AppendLine(onInvincibility.ToString());
        }

        builder.Append("}");

        return builder.ToString();
    }
}
}