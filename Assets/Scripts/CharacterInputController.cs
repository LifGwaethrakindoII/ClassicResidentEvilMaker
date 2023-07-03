using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Voidless.REMaker;

namespace Voidless.REMaker
{
public enum ControllerSchemeType
{
	Character,
	UI
}

public class CharacterInputController<T> : GamePadInputController where T : Character
{
	[Space(5f)]
	[SerializeField] private T _character;
	[SerializeField][Range(0.0f, 1.0f)] private float _turnAroundDot;

	/// <summary>Gets and Sets character property.</summary>
	public T character
	{
		get { return _character; }
		set { _character = value; }
	}

	/// <summary>Gets turnAroundDot property.</summary>
	public float turnAroundDot { get { return _turnAroundDot; } }

	/// <summary>Empties BaseCharacterInputController.</summary>
	public void Empty()
	{
		character = null;
	}
}
}