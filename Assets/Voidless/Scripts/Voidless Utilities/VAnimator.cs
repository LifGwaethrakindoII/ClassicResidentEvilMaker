using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
/// <summary>Animation Frame-Rates' Enumerator.</summary>
public enum AnimationFrameRate
{
	_24 = 24,
	_30 = 30,
	_60 = 60,
	_120 = 120
}

public static class VAnimator
{
	public static readonly AnimatorCredential CREDENTIAL_EMPTY; 	/// <summary>Empty's Animator-Credential.</summary>

	/// <summary>VAnimator's static constructor.</summary>
	static VAnimator()
	{
		CREDENTIAL_EMPTY = "Empty";
	}

	/// <returns>AnimationFrameRate's enum into frame rate [as integer].</returns>
	public static int ToFrameRate(this AnimationFrameRate _frameRate)
	{
		return (int)_frameRate;
	}

	/// <summary>Subscribes IAnimationStateListener into Animator's AnimationStateSenders.</summary>
	/// <param name="_animator">Animator that contains AnimationStateSenders.</param>
	/// <param name="_listener">IAnimationStateListener's to subscribe.</param>
	/// <returns>True if successfully subscribed to at least an AnimationStateSender.</returns>
	public static bool SubscribeToAnimationStateSenders(this Animator _animator, IAnimationStateListener _listener)
	{
		AnimationStateSender[] senders = _animator.GetBehaviours<AnimationStateSender>();

		if(senders == null) return false;
		foreach(AnimationStateSender sender in senders)
		{
			sender.AddListener(_listener);
		}

		return true;
	}

	/// <summary>Subscribes IAnimationCommandListener into Animator's AnimationCommandeSenders.</summary>
	/// <param name="_animator">Animator that contains AnimationCommandeSenders.</param>
	/// <param name="_listener">IAnimationCommandListener's to subscribe.</param>
	/// <returns>True if successfully subscribed to at least an AnimationCommandSender.</returns>
	public static bool SubscribeToAnimationCommandSenders(this Animator _animator, IAnimationCommandListener _listener)
	{
		AnimationCommandSender[] senders = _animator.GetBehaviours<AnimationCommandSender>();

		if(senders == null) return false;
		foreach(AnimationCommandSender sender in senders)
		{
			sender.AddListener(_listener);
		}

		return true;
	}

	/// <summary>Gets Animator's current animation time.</summary>
	/// <param name="_animator">Animator Reference.</param>
	/// <param name="_layer">Animation's Layer [0 by default].</param>
	/// <returns>Current Animation's Time.</returns>
	public static float GetCurrentTime(this Animator _animator, int _layer = 0)
	{
		AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(_layer);
		
		return info.normalizedTime % 1.0f;
	}

	/// <param name="_animator">Animator to retrieve current AnimatorStateInfo from.</param>
	/// <param name="_layerID">Layer's ID [0 by default].</param>
	/// <returns>Animator's Current AnimatorStateInfo's normalizedTime property, but clamped at 1.0f.</returns>
	public static float NormalizedTime(this Animator _animator, int _layerID = 0)
	{
		return _animator.GetCurrentAnimatorStateInfo(_layerID).NormalizedTime();
	}

	/// <returns>AnimatorStateInfo's normalizedTime property, but clamped at 1.0f.</returns>
	public static float NormalizedTime(this AnimatorStateInfo _info)
	{
		float t = _info.normalizedTime;
		return _info.loop ? t %= 1.00000000001f : Mathf.Min(t, 1.0f);
	}

	/// <summary>Evaluates of current Animator's State is on given Layer.</summary>
	/// <param name="_animator">Animator to evaluate.</param>
	/// <param name="_fullPathHash">Full Path's Hash.</param>
	/// <param name="_layerID">Layer's ID [0 by default].</param>
	/// <returns>True if current Animator's State is on given layer.</returns>
	public static bool IsPlayingOnLayer(this Animator _animator, int _fullPathHash, int _layerID = 0)
	{
		return _animator.GetCurrentAnimatorStateInfo(_layerID).fullPathHash == _fullPathHash;
	}

	/// <summary>Sets IK position of this Joint if it is one.</summary>
	/// <param name="_animator">Animator to Extend.</param>
	/// <param name="_IKGoal">IK Goal to displace.</param>
	/// <param name="_position">Position.</param>
	/// <param name="_weight">Position's Weight [1.0f by default].</param>
	public static void SetIKPosition(this Animator _animator, AvatarIKGoal _IKGoal, Vector3 _position, float _weight = 1.0f)
	{
		_animator.SetIKPosition(_IKGoal, _position);
        _animator.SetIKPositionWeight(_IKGoal, _weight);
	}

	/// <summary>Sets weight to all layers on an Animator.</summary>
	/// <param name="_animator">Animator's reference.</param>
	/// <param name="_weight">Desired Weight for all Animator's Layers.</param>
	public static void SetAllLayersWeight(this Animator _animator, float _weight)
	{
		if(_animator == null) return;
		
		for(int i = 0; i < _animator.layerCount ; i++)
		{
			_animator.SetLayerWeight(i, _weight);
		}
	}

	/// <summary>Gets AnimatorStateInfo's name from a set of possible names.</summary>
	/// <param name="_info">AnimatorState's Information.</param>
	/// <param name="_names">Set of possible names.</param>
	/// <returns>Name of current Animator's state out of a set of possible names.</returns>
	public static string GetAnimatorStateName(this AnimatorStateInfo _info, params string[] _names)
	{
		if(_names == null || _names.Length == 0) return string.Empty;

		foreach(string name in _names)
		{
			if(_info.IsName(name)) return name;
		}

		return string.Empty;
	}

	/// <summary>Evaluates if Animator is Playing on given layer.</summary>
	/// <param name="_animator">Animator to evaluate.</param>
	/// <param name="_layer">Layer to evaluate [0 by default].</param>
	public static bool IsPlaying(this Animator _animator, int _layer = 0)
	{
		AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(_layer);
		return info.length > info.normalizedTime;
	}

	/// <summary>Evaluates if Animator's Layer is on given AnimatorCredential.</summary>
	/// <param name="_animator">Animator's Component.</param>
	/// <param name="_credential">Animator's Credential.</param>
	/// <param name="_layer">Animation's Layer [0 by default].</param>
	/// <returns>True if current AnimationState is on given name.</returns>
	public static bool IsOnAnimation(this Animator _animator, AnimatorCredential _credential, int _layer = 0, bool _fullPathHash = false)
	{
		/*string animation = _fullPathHash ? _animator.GetLayerName(_layer) + "." + _credential.tag : _credential.tag;
		return  _animator.IsPlaying() && _animator.GetCurrentAnimatorStateInfo(_layer).IsName(animation);*/

		AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(_layer);
		return _fullPathHash ? info.HasFullHash(_credential.ID) : info.HasShortHash(_credential.ID);
	}

	/// <summary>Evaluates if Animator's Layer is on Animation with Tag.</summary>
	/// <param name="_animator">Animator's Component.</param>
	/// <param name="_credential">Animation's Tag.</param>
	/// <param name="_layer">Animation's Layer [0 by default].</param>
	/// <returns>True if current AnimationState is on given tag.</returns>
	public static bool IsOnAnimationWithTag(this Animator _animator, string _tag, int _layer = 0)
	{
		return _animator.GetCurrentAnimatorStateInfo(_layer).IsTag(_tag);
	}

	public static bool HasFullHash(this AnimatorStateInfo _info, int _fullPathHash)
	{
		return _info.fullPathHash == _fullPathHash;
	}

	public static bool HasShortHash(this AnimatorStateInfo _info, int _shortNameHash)
	{
		return _info.shortNameHash == _shortNameHash;
	}

	/// <summary>Creates a string that shows all the Animator State's Info.</summary>
	/// <param name="_info">Animator State's Info.</param>
	/// <returns>Animator State Info into a string.</returns>
	public static string StateInfoToString(this AnimatorStateInfo _info)
	{
		StringBuilder builder = new StringBuilder();

		builder.AppendLine("Animator State Info: ");
		builder.AppendLine();
		builder.Append("Full Path Hash: ");
		builder.AppendLine(_info.fullPathHash.ToString());
		builder.Append("Length (Duration): ");
		builder.AppendLine(_info.length.ToString());
		builder.Append("Looping? ");
		builder.AppendLine(_info.loop.ToString());
		builder.Append("Normalized Time: ");
		builder.AppendLine(_info.normalizedTime.ToString());
		builder.Append("Short Name Hash: ");
		builder.AppendLine(_info.shortNameHash.ToString());
		builder.Append("Playback Speed: ");
		builder.AppendLine(_info.speed.ToString());
		builder.Append("Speed Multiplier: ");
		builder.AppendLine(_info.speedMultiplier.ToString());
		builder.Append("Tag Hash: ");
		builder.Append(_info.tagHash.ToString());

		return builder.ToString();
	}
}
}