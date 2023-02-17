using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VAnimation
{
	public static readonly int HASH_ANIMATION_EMPTY; 	/// <summary>Empty Animation's Hash.</summary>

	/// <summary>VAnimation's static constructor.</summary>
	static VAnimation()
	{
		HASH_ANIMATION_EMPTY = Animator.StringToHash("Empty");
	}

	/// <summary>Adds AnimationClips into Animation Component.</summary>
	/// <param name="_animation">Animation's Component.</param>
	/// <param name="_clips">AnimationClips to add.</param>
	public static void AddClips(this Animation _animation, params AnimationClip[] _clips)
	{
		if(_clips == null) return;

		foreach(AnimationClip clip in _clips)
		{
			if(clip != null)
			{
				clip.legacy = true;
				_animation.AddClip(clip, clip.name);
			}
		}
	}

	/// <summary>Adds AnimationClip into Animation Component with the same AnimationClip's name.</summary>
	/// <param name="_animation">Animation Component.</param>
	/// <param name="_clip">Animation clip to add into the Animation Component.</param>
	public static void AddClip(this Animation _animation, AnimationClip _clip)
	{
		if(_clip == null) return;

		_clip.legacy = true;
		_animation.AddClip(_clip, _clip.name);
	}

	/// <summary>Gets animation state from AnimationClip.</summary>
	/// <param name="_animation">Animation's Component.</param>
	/// <param name="_clip">AnimationClip.</param>
	/// <returns>AnimationState from given AnimationClip.</returns>
	public static AnimationState GetAnimationState(this Animation _animation, AnimationClip _clip)
	{
		return _clip != null ? _animation[_clip.name] : null;
	}

	/// <summary>Plays an animation without blending.</summary>
	/// <param name="_animation">Animation's Component.</param>
	/// <param name="_clip">AnimationClip to play.</param>
	/// <param name="_mode">The optional PlayMode lets you choose how this animation will affect others already playing.</param>
	public static bool Play(this Animation _animation, AnimationClip _clip, PlayMode _mode = PlayMode.StopSameLayer)
	{
		return _animation.Play(_clip.name, _mode);
	}

	/// <summary>Cross Fades towards animation.</summary>
	/// <param name="_animation">Animation Component.</param>
	/// <param name="_clip">Destinatin Animation.</param>
	/// <param name="_fadeDuration">fade's Duration.</param>
	/// <param name="_mode">Play Mode [StopSameLayer by default].</param>
	public static void CrossFade(this Animation _animation, AnimationClip _clip, float _fadeDuration = 0.3f, PlayMode _mode = PlayMode.StopSameLayer)
	{
		_animation.CrossFade(_clip.name, _fadeDuration, _mode);
	}

	/// <summary>Rewinds an animation without blending.</summary>
	/// <param name="_animation">Animation's Component.</param>
	/// <param name="_clip">AnimationClip to rewind.</param>
	public static void Rewind(this Animation _animation, AnimationClip _clip)
	{
		_animation.Rewind(_clip.name);
	}

	/// <summary>Plays an animation after previous animations has finished playing.</summary>
	/// <param name="_animation">Animation's Component.</param>
	/// <param name="_clip">Clip to play.</param>
	/// <param name="_queue">Queue Mode.</param>
	/// <param name="_mode">The optional PlayMode lets you choose how this animation will affect others already playing.</param>
	public static void PlayQueued(this Animation _animation, AnimationClip _clip, QueueMode _queue = QueueMode.CompleteOthers, PlayMode _mode = PlayMode.StopSameLayer)
	{
		_animation.PlayQueued(_clip.name, _queue, _mode);
	}

	/// <summary>Blends 2 animations.</summary>
	/// <param name="_animation">Animation's Component.</param>
	/// <param name="_clip">Destination AnimationClip.</param>
	/// <param name="_weight">target Blending Weight.</param>
	/// <param name="_fadeDuration">Fade's Duration.</param>
	public static void Blend(this Animation _animation, AnimationClip _clip, float _weight = 1.0f, float _fadeDuration = 0.3f)
	{
		_animation.Blend(_clip.name, _weight, _fadeDuration);
	}
}
}