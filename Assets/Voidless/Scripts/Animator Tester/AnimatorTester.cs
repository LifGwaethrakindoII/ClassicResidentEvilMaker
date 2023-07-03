using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Voidless
{
public class AnimatorTester : MonoBehaviour
{
	[SerializeField] private Animator _animator; 					/// <summary>Animator's Component.</summary>
	[SerializeField] private List<AnimatorParameter> _parameters; 	/// <summary>Animator's Parameters.</summary>

	/// <summary>Gets And Sets animator Component.</summary>
	public Animator animator
	{ 
		get
		{
			if(_animator == null) _animator = GetComponent<Animator>();
			return _animator;
		}
		set { _animator = value; }
	}

	/// <summary>Gets and Sets parameters property.</summary>
	public List<AnimatorParameter> parameters
	{
		get { return _parameters; }
		set { _parameters = value; }
	}

	/// <summary>AnimatorTester's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		if(parameters == null) parameters = new List<AnimatorParameter>();
	}

	/// <summary>Updates AnimatorTester's instance at each frame.</summary>
	private void Update()
	{
		foreach(AnimatorParameter parameter in parameters)
		{
			switch(parameter.type)
			{
				case AnimatorParameterType.Bool:
				animator.SetBool(parameter.key, parameter.boolValue);
				break;

				case AnimatorParameterType.Int:
				animator.SetInteger(parameter.key, parameter.intValue);
				break;

				case AnimatorParameterType.Float:
				case AnimatorParameterType.BlendTreeFloat:
				animator.SetFloat(parameter.key, parameter.floatValue);
				break;
			}
		}
	}

	[Button("Play")]
	/// <summary>Plays target animation.</summary>
	/// <param name="_animation">Animation's Name in the AnimatorController.</param>
	/// <param name="_layer">Animation's layer [0 by default].</param>
	private void Play(string _animation, int _layer = 0)
	{
		animator.Play(_animation, _layer);
	}

	[Button("Cross-Fade")]
	/// <summary>Cross-Fades to target animation.</summary>
	/// <param name="_animation">Animation's Name in the AnimatorController.</param>
	/// <param name="_fadeDuration">Normalized Fade's duration.</param>
	/// <param name="_layer">Animation's layer [0 by default].</param>
	private void CrossFade(string _animation, float _fadeDuration = 0.3f, int _layer = 0)
	{
		animator.CrossFade(_animation, _fadeDuration, _layer);
	}
}
}