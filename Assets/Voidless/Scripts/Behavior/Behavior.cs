using System.Collections;
using UnityEngine;
using System.Text;

namespace Voidless
{
public enum BehaviorState 								/// <summary>Behavior's possible states.</summary>
{
    Ready, 												/// <summary>Ready Behavior's state.</summary>
    Running, 											/// <summary>Ready Behavior's state.</summary>
    Paused, 											/// <summary>Paused Behavior's state.</summary>
    Restarted, 											/// <summary>Restarted Behavior's state.</summary>
    Finished 											/// <summary>Finished Behavior's state.</summary>
}

/// <summary>Event invoked when the Behavior changes state.</summary>
/// <param name="_behavior">Behavior that changed state.</param>
/// <param name="_state">New Behavior's state.</param>
public delegate void OnBehaviorEvent(Behavior _behavior, BehaviorState _state);

public class Behavior : IEnumerator, IFiniteStateMachine<BehaviorState>
{
	public event OnBehaviorEvent onBehaviorEvent; 		/// <summary>OnBehaviorEvent's subscription event.</summary>

	private IEnumerator _enumerator; 					/// <summary>Behavior's IEnumerator.</summary>
	private Coroutine _coroutine; 						/// <summary>Last Coroutine store, allows control of the Behavior Class.</summary>
	private MonoBehaviour _monoBehaviour; 				/// <summary>MonoBehaviour from where Behavior is instantiated.</summary>
	private bool _monoBehaviourDependency;  			/// <summary>Does this Behavior have a dependency on a MonoBehaviour's class?.</summary>

	public BehaviorState previousState { get; set; } 	/// <summary>Previous's state.</summary>
	public BehaviorState state { get; set; } 			/// <summary>Current's state.</summary>

	/// <summary>Gets and Sets enumerator property.</summary>
	public virtual IEnumerator enumerator
	{
		get { return _enumerator; }
		protected set { _enumerator = value; }
	}

	/// <summary>Gets and Sets coroutine property.</summary>
	public Coroutine coroutine
	{
		get { return _coroutine; }
		protected set { _coroutine = value; }
	}

	public MonoBehaviour monoBehaviour
	{
		get { return _monoBehaviour; }
		protected set
		{
			_monoBehaviour = value;
			monoBehaviourDependency = (_monoBehaviour != null);
		}
	}

	/// <summary>Gets and Sets monoBehaviourDependency property.</summary>
	public bool monoBehaviourDependency
	{
		get { return _monoBehaviourDependency; }
		protected set { _monoBehaviourDependency = value; }
	}

	/// <summary>Gets Current property.</summary>
	public System.Object Current { get { return enumerator != null ? enumerator.Current : null; } }

#region FiniteStateMachine:
	/// <summary>Enters BehaviorState State.</summary>
	/// <param name="_state">BehaviorState State that will be entered.</param>
	public virtual void OnEnterState(BehaviorState _state)
	{
		if(onBehaviorEvent != null) onBehaviorEvent(this, _state);
	}
	
	/// <summary>Leaves BehaviorState State.</summary>
	/// <param name="_state">BehaviorState State that will be left.</param>
	public virtual void OnExitState(BehaviorState _state)
	{
		
	}
#endregion

	/// <summary>Parameterless Behavior's constructor.</summary>
	protected Behavior() {  }

	/// <summary>Behavior class Constructor.</summary>
	/// <param name="_monoBehaviour">MonoBehaviour from where the coroutine belongs.</param>
	/// <param name="_enumerator">Coroutine that will be initialized.</param>
	/// <param name="_startAutomagically">Start this coroutine as soon as you instantiate this behavior? Automatically set to true.</param>
	public Behavior(MonoBehaviour _monoBehaviour, IEnumerator _enumerator, bool _startAutomagically = true)
	{
		state = BehaviorState.Ready;
		monoBehaviour = _monoBehaviour;
		enumerator = _enumerator;

		if(_startAutomagically) StartBehavior();
	}

	/// <summary>Starts the Behavior's Coroutine.</summary>
	public virtual void StartBehavior()
	{
		if(state == BehaviorState.Ready)
		{
			if(monoBehaviourDependency) coroutine = monoBehaviour.StartCoroutine(this);
			this.ChangeState(BehaviorState.Running);
		}
	}

	/// <summary>Pauses the Behavior's Coroutine.</summary>
	public virtual void PauseBehavior()
	{
		if(state == BehaviorState.Running || state == BehaviorState.Ready || state == BehaviorState.Restarted) this.ChangeState(BehaviorState.Paused);
	}

	/// <summary>Resumes the Behavior [if it was paused].</summary>
	public virtual void ResumeBehavior()
	{
		if(state == BehaviorState.Paused) this.ChangeState(BehaviorState.Running);
	}

	/// <summary>Stops the current Coroutine, then it starts it again.</summary>
	public virtual void ResetBehavior()
	{
		if(state != BehaviorState.Restarted)
		{
			this.ChangeState(BehaviorState.Restarted);	
			EndBehavior();
			StartBehavior();
		}
	}

	/// <summary>Ends the Behavior.</summary>
	public virtual void EndBehavior()
	{
		if(state != BehaviorState.Finished)
		{
			if(monoBehaviourDependency && coroutine != null) monoBehaviour.StopCoroutine(coroutine);
			coroutine = null;
			this.ChangeState(BehaviorState.Finished);
			this.ChangeState(BehaviorState.Ready);
		}
	}

#region IEnumeratorMethods:
	/// <summary>Advances the enumerator to the next element of the collection.</summary>
	public virtual bool MoveNext()
	{
		if(enumerator != null)
		{
			switch(state)
			{
				case BehaviorState.Ready:
				return true;

				case BehaviorState.Running:
				return enumerator.MoveNext();

				case BehaviorState.Paused:
				return true;

				case BehaviorState.Finished:
				return false;
			}

			return true;
		}	
		else return false;
	}

	/// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
	public virtual void Reset()
	{
		enumerator.Reset();
	}
#endregion

	/// <summary>Returns a string that represents the current object.</summary>
	/// <returns>A string that represents the current object..</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();
		builder.Append("Behavior State: ");
		builder.Append(state.ToString());

		return builder.ToString();
	}
}
}