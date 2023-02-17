using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public enum AnimationAttackState
{
	None,
	StartUp,
	Active,
	Recovery
}

public delegate void OnAnimationAttackStateChange(AnimationAttackState _state, int _index, int _subIndex);

[Serializable]
public class AnimationAttackInfo
{
	public event OnAnimationAttackStateChange onAnimationAttackStateChange;

	[SerializeField] private int _layerID; 				/// <summary>Animation's Layer in the AnimatorController.</summary>
	[SerializeField] private int _attacksLimit; 		/// <summary>Attacks' Limit.</summary>
	[SerializeField] private string _subStateMachine; 	/// <summary>Sub-State Machine's Name [optional].</summary>
	[SerializeField] private string _stateName; 		/// <summary>Name of the AnimatorController's State.</summary>
	private int _currentStateID; 						/// <summary>Current State's ID.</summary>
	private int _fullNameHash; 						/// <summary>Animation's Full Name's Hash.</summary>
	private AnimationAttackState _state; 				/// <summary>Current Animation Attack's State.</summary>
	private StringBuilder _builder; 					/// <summary>StringBuilder reference used to construct strings.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets layerID property.</summary>
	public int layerID
	{
		get { return _layerID; }
		set { _layerID = value; }
	}

	/// <summary>Gets and Sets attacksLimit property.</summary>
	public int attacksLimit
	{
		get { return _attacksLimit; }
		set { _attacksLimit = value; }
	}

	/// <summary>Gets and Sets currentStateID property.</summary>
	public int currentStateID
	{
		get { return _currentStateID; }
		set { _currentStateID = value; }
	}

	/// <summary>Gets and Sets fullNameHash property.</summary>
	public int fullNameHash
	{
		get { return _fullNameHash; }
		private set { _fullNameHash = value; }
	}

	/// <summary>Gets and Sets subStateMachine property.</summary>
	public string subStateMachine
	{
		get { return _subStateMachine; }
		set { _subStateMachine = value; }
	}

	/// <summary>Gets and Sets stateName property.</summary>
	public string stateName
	{
		get { return _stateName; }
		set { _stateName = value; }
	}

	/// <summary>Gets and Sets state property.</summary>
	public AnimationAttackState state
	{
		get { return _state; }
		set { _state = value; }
	}

	/// <summary>Gets and Sets builder property.</summary>
	public StringBuilder builder
	{
		get { return _builder; }
		private set { _builder = value; }
	}
#endregion

	/// <summary>AnimationAttackInfo default constructor.</summary>
	public AnimationAttackInfo()
	{
		builder = new StringBuilder();
	}

	/// <summary>Resets AnimationAttackInfo's instance to its default values.</summary>
	public void Reset()
	{
		state = AnimationAttackState.None;
		currentStateID = 0;
	}

	public string GetAnimationFullPath(Animator _animator)
	{
		builder.Clear();

		builder.Append(_animator.GetLayerName(layerID));
		builder.Append(".");
		if(!string.IsNullOrEmpty(subStateMachine))
		{
			builder.Append(subStateMachine);
			builder.Append(".");
		}
		builder.Append(stateName);
		builder.Append("_");
		builder.Append(currentStateID.ToString());

		string path = builder.ToString();

		fullNameHash = Animator.StringToHash(path);

		return path;
	}

	public void InvokeAnimationAttackStateChange(AnimationAttackState _state, int _index, int _subIndex)
	{
		state = _state;
		if(onAnimationAttackStateChange != null) onAnimationAttackStateChange(_state, _index, _subIndex);
	}
}
}