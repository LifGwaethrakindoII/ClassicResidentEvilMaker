using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Voidless
{
public class ImageTransitionEffector : MonoBehaviour
{
	[SerializeField] private Image _maskImage; 			/// <summary>Transition Effector's Mask Image.</summary>
	[SerializeField] private Image _backgroundImage; 	/// <summary>Background's Image.</summary>
	private Coroutine imageTransition; 					/// <summary>Fade Transition's Coroutine.</summary>

	/// <summary>Gets maskImage property.</summary>
	public Image maskImage { get { return _maskImage; } }

	/// <summary>Gets backgroundImage property.</summary>
	public Image backgroundImage { get { return _backgroundImage; } }

	/// <summary>Gets imageTransitionRunning property.</summary>
	public bool imageTransitionRunning { get { return imageTransition != null; } }

	/// <summary>Begins Image's In Effect.</summary>
	/// <param name="_data">Image Transition's Data.</param>
	/// <param name="onTransitionEnds">Callback invoked when the transition ends.</param>
	public void BeginImageInTransitionEffect(ImageTransitionData _data, Action onTransitionEnds)
	{
		maskImage.sprite = _data.sprite;
		backgroundImage.color = backgroundImage.color.WithAlpha(1.0f);
		this.StartCoroutine(ImageTransitionEffect(_data, Fade.In, onTransitionEnds), ref imageTransition);
	}

	/// <summary>Begins Image's Out Effect.</summary>
	/// <param name="_data">Image Transition's Data.</param>
	/// <param name="onTransitionEnds">Callback invoked when the transition ends.</param>
	public void BeginImageOutTransitionEffect(ImageTransitionData _data, Action onTransitionEnds)
	{
		maskImage.sprite = _data.sprite;
		backgroundImage.color = backgroundImage.color.WithAlpha(1.0f);
		this.StartCoroutine(ImageTransitionEffect(_data, Fade.Out, onTransitionEnds), ref imageTransition);	
	}

	/// <summary>Image Transition Effect's Iterator.</summary>
	/// <param name="_data">Image Transition's Data.</param>
	/// <param name="_fadeType">Type of Transition [whether in or out].</param>
	/// <param name="onTransitionEnds">Callback invoked when the transition ends.</param>
	private IEnumerator ImageTransitionEffect(ImageTransitionData _data, Fade _fadeType, Action onTransitionEnds)
	{
		RectTransform rectTransform = maskImage.transform as RectTransform;
		float t = 0.0f;
		float duration = _fadeType == Fade.In ? _data.fadeInDuration : _data.fadeOutDuration;
		Vector2 initialSize = _fadeType == Fade.In ? _data.maxSize : _data.minSize;
		Vector2 targetSize = _fadeType == Fade.In ? _data.minSize : _data.maxSize;
		NormalizedPropertyFunction f = _fadeType == Fade.In ? _data.fadeInFunction : _data.fadeOutFunction;

		while(t < (1.0f + Mathf.Epsilon))
		{
			rectTransform.sizeDelta = Vector2.Lerp(initialSize, targetSize, f != null ? f.Evaluate(t) : t);

			if(_fadeType == Fade.Out)
			{
				Color color = backgroundImage.color;
				
				color.a = Mathf.Lerp(1.0f, 0.0f, f != null ? f.Evaluate(t) : t);
				backgroundImage.color = color;
			}

			t += (Time.deltaTime / duration);
			yield return null;
		}

		if(onTransitionEnds != null) onTransitionEnds();
		this.DispatchCoroutine(ref imageTransition);
	}
}
}