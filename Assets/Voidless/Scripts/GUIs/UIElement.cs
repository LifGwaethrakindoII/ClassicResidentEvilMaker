using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Voidless
{
public class UIElement : MonoBehaviour
{
	private RectTransform _rectTransform; 	/// <summary>RectTransform Component.</summary>

	/// <summary>Gets rectTransform property.</summary>
	public RectTransform rectTransform
	{
		get
		{
			if(_rectTransform == null)
			{
				_rectTransform = GetComponent<RectTransform>();
			}

			return _rectTransform;
		}
	}
}
}