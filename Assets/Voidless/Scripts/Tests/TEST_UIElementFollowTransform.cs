using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_UIElementFollowTransform : MonoBehaviour
{
	[SerializeField] private Transform target; 	/// <summary>Target's Transform.</summary>
	private RectTransform _rectTransform; 		/// <summary>RectTransform's Component.</summary>

	/// <summary>Gets rectTransform Component.</summary>
	public RectTransform rectTransform
	{ 
		get
		{
			if(_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
			return _rectTransform;
		}
	}
	
	/// <summary>TEST_UIElementFollowTransform's tick at each frame.</summary>
	private void Update ()
	{
		if(target == null) return;

		Vector2 screenPosition = Camera.main.WorldToScreenPoint(target.position);
		rectTransform.position = screenPosition;
	}
}