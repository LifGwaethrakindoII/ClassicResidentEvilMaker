using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VLight
{
	public static IEnumerator StormLightningEffectRoutine(this Light _light, float _waitBeforeOscillation, float _oscillations, float _oscillationIntensity, float _oscillationDuration, float _durationTowardsMaxIntensity, float _maxIntensity, float _maxIntensityDuration, float _durationTowardsMinIntensity, Action onEffectEnds = null)
	{	
		SecondsDelayWait wait = new SecondsDelayWait(_waitBeforeOscillation);
		float originalIntensity = _light.intensity;
		float iD = 1.0f / _oscillationDuration;
		float x = (360.0f * _oscillations * Mathf.Deg2Rad);
		float intensity = 0.0f;
		float t = 0.0f;
		float n = 0.0f;

		if(_waitBeforeOscillation > 0.0f) while(wait.MoveNext()) yield return null;

		while(t < 1.0f)
		{
			n = VMath.RemapValueToNormalizedRange(Mathf.Sin(t * x), -1.0f, 1.0f);
			_light.intensity = n * _oscillationIntensity;
			t += (Time.deltaTime * iD);
			yield return null;
		}

		intensity = _light.intensity;
		t = 0.0f;
		iD = 1.0f / _durationTowardsMaxIntensity;

		while(t < 0.0f)
		{
			_light.intensity = Mathf.Lerp(intensity, _maxIntensity, t);;
			t += (Time.deltaTime * iD);
			yield return null;
		}

		_light.intensity = _maxIntensity;
		wait.ChangeDurationAndReset(_maxIntensityDuration);
		
		if(_maxIntensityDuration > 0.0f) while(wait.MoveNext()) yield return null;

		intensity = _light.intensity;
		t = 0.0f;
		iD = 1.0f / _durationTowardsMinIntensity;

		while(t < 1.0f)
		{
			_light.intensity = Mathf.Lerp(intensity, originalIntensity, t);;
			t += (Time.deltaTime * iD);
			yield return null;
		}

		_light.intensity = originalIntensity;

		if(onEffectEnds != null) onEffectEnds();
	}	
}
}