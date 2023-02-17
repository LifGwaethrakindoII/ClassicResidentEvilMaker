using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class LightningController : Singleton<LightningController>
{
	public static Coroutine SwitchLightmapSettings(LightmapData[] data, float duration = 0.0f, Action onSwitchEnds = null, Func<float, float> f = null)
	{
		return SwitchLightmapSettings(LightmapSettings.lightmaps, data, duration, onSwitchEnds, f);
	}

	public static Coroutine SwitchLightmapSettings(LightmapData[] a, LightmapData[] b, float duration = 0.0f, Action onSwitchEnds = null, Func<float, float> f = null)
	{
		if(a == null || b == null) return null;

		VArray.Balance(ref a, ref b);

		return Instance.StartCoroutine(SwitchLightmapSettingsRoutine(a, b, duration, onSwitchEnds, f));
	}

	public static IEnumerator SwitchLightmapSettingsRoutine(LightmapData[] a, LightmapData[] b, float duration = 0.0f, Action onSwitchEnds = null, Func<float, float> f = null)
	{
		LightmapSettings.lightmaps = a;

		float t = 0.0f;
		float inverseDuration = 1.0f / duration;
		LightmapData dataA = null;	
		LightmapData dataB = null;	
		int size = a.Length;
		IEnumerator<Texture2D>[] colorIterators = new IEnumerator<Texture2D>[size];
		IEnumerator<Texture2D>[] dirIterators = new IEnumerator<Texture2D>[size];
		LightmapData[] lightmapsData = new LightmapData[size];
		VTuple<IEnumerator<Texture2D>, IEnumerator<Texture2D>>[] tuples = new VTuple<IEnumerator<Texture2D>, IEnumerator<Texture2D>>[size];

		for(int i = 0; i < size; i++)
		{
			dataA = a[i];
			dataB = b[i];

			tuples[i] = new VTuple<IEnumerator<Texture2D>, IEnumerator<Texture2D>>();
			tuples[i].Item1 = dataA.lightmapColor.InterpolateToTexture2D(dataB.lightmapColor, duration, null, f);
			tuples[i].Item2 = dataA.lightmapDir.InterpolateToTexture2D(dataB.lightmapDir, duration, null, f);
			lightmapsData[i] = new LightmapData();
		}

		while(t < 1.0f)
		{
			for(int i = 0; i < size; i++)
			{
				tuples[i].Item1.MoveNext();
				tuples[i].Item2.MoveNext();

				lightmapsData[i].lightmapColor = tuples[i].Item1.Current;
				lightmapsData[i].lightmapDir = tuples[i].Item2.Current;
			}

			LightmapSettings.lightmaps = lightmapsData;

			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		LightmapSettings.lightmaps = b;

		if(onSwitchEnds != null) onSwitchEnds();
	}
}
}