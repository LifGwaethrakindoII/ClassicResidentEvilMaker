using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Voidless
{
public static class VAudioMixer
{
	/// <summary>Sets AudioMixer's Volume.</summary>
	/// <param name="_mixer">AudioMixer to modify.</param>
	/// <param name="_parameter">Exposed parameter to modify.</param>
	/// <param name="_volume">Desired volume [on a normalized range].</param>
	public static void SetVolume(this AudioMixer _mixer, string _parameter, float _volume)
	{
		if(_mixer == null) return;

		_volume = Mathf.Clamp(_volume, 0.0001f, 1.0f);
		_mixer.SetFloat(_parameter, Mathf.Log10(_volume) * 20.0f);
	}

	/// \TODO Understand what is going on. Link: https://gamedevbeginner.com/how-to-fade-audio-in-unity-i-tested-every-method-this-ones-the-best/
	/// <summary>Fades AudioMixer's volume to desired value.</summary>
	/// <param name="_mixer">AudioMixer's reference.</param>
	/// <param name="_parameter">Exposed parameter to modify.</param>
	/// <param name="_duration">Fading's Duration.</param>
	/// <param name="_volume">Desired's volume value.</param>
	/// <param name="onFadeEnds">Optionall callback invoked when the fade ends [null by default].</param>
	public static IEnumerator FadeVolume(this AudioMixer _mixer, string _parameter, float _duration, float _volume, Action onFadeEnds = null)
	{
		if(_mixer == null) yield break;

		float targetVolume = Mathf.Clamp(_volume, 0.0001f, 1.0f);
		float newVolume = 0.0f;
		float t = 0.0f;
		float v = 0.0f;
		float inverseDuration = 1.0f / _duration;

		_mixer.GetFloat(_parameter, out v);
		v = Mathf.Pow(10.0f, v / 20.0f);

		while(t < 1.0f)
		{
			newVolume = Mathf.Lerp(v, targetVolume, t);
			_mixer.SetFloat(_parameter, Mathf.Log10(newVolume) * 20.0f);
			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		_mixer.SetFloat(_parameter, Mathf.Log10(targetVolume) * 20.0f);
		if(onFadeEnds != null) onFadeEnds();
	}
}
}