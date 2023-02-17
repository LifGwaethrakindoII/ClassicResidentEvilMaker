using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace Voidless
{
public class Typewriter : MonoBehaviour
{
	[SerializeField] private CharTransformDictionary _keysDictionary; 			/// <summary>Keys' Dictionary.</summary>
	[SerializeField] private CharAudioClipDictionary _keyPressClipsDictionary; 	/// <summary>Key Presses' Dictionary [for special cases].</summary>
	[SerializeField] private CharFloatDictionary _writingDurationDictionary; 		/// <summary>Dictionary for additional writing speed per special key case.</summary>
	[SerializeField] private float _writingDuration; 								/// <summary>Default Writing's Speed.</summary>
	[SerializeField] private float _keyPressDisplacement; 						/// <summary>Key Press' Displacement Magnitude.</summary>
	[SerializeField] private AudioClip _keyPressClip; 							/// <summary>Key Press's AudioClip.</summary>
	[Space(5f)]
	[Header("TEST:")]
	[SerializeField] private string testText; 									/// <summary>Test's Text.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets keysDictionary property.</summary>
	public CharTransformDictionary keysDictionary
	{
		get { return _keysDictionary; }
		set { _keysDictionary = value; }
	}

	/// <summary>Gets and Sets keyPressClipsDictionary property.</summary>
	public CharAudioClipDictionary keyPressClipsDictionary
	{
		get { return _keyPressClipsDictionary; }
		set { _keyPressClipsDictionary = value; }
	}

	/// <summary>Gets and Sets writingDurationDictionary property.</summary>
	public CharFloatDictionary writingDurationDictionary
	{
		get { return _writingDurationDictionary; }
		set { _writingDurationDictionary = value; }
	}

	/// <summary>Gets and Sets writingDuration property.</summary>
	public float writingDuration
	{
		get { return _writingDuration; }
		set { _writingDuration = value; }
	}

	/// <summary>Gets and Sets keyPressDisplacement property.</summary>
	public float keyPressDisplacement
	{
		get { return _keyPressDisplacement; }
		set { _keyPressDisplacement = value; }
	}

	/// <summary>Gets and Sets keyPressClip property.</summary>
	public AudioClip keyPressClip
	{
		get { return _keyPressClip; }
		set { _keyPressClip = value; }
	}
#endregion

	/// <summary>Draws Gizmos on Editor mode when Typewriter's instance is selected.</summary>
	private void OnDrawGizmosSelected()
	{
		if(keysDictionary != null) foreach(Transform key in keysDictionary.Values)
		{
			Gizmos.DrawRay(key.transform.position, (-key.forward * keyPressDisplacement));
		}
	}
	 private Coroutine coroutine;

	[Button("Test Text")]
	private void TestText()
	{
		this.StartCoroutine(WriteText(), ref coroutine);
	}

	private IEnumerator WriteText()
	{
		if(string.IsNullOrEmpty(testText)) yield break;

		Text text = GameplayUIController.mainText;
		StringBuilder builder = new StringBuilder();
		int i = 0;
		Action<char> OnKeyPressed = (c)=>
		{
			builder.Append(c);
			text.text = builder.ToString();
		};

		text.text = "";

		while(i < testText.Length)
		{
			char current = testText[i];
			IEnumerator keyDisplacement = DisplaceKeyButton(current, OnKeyPressed);
			while(keyDisplacement.MoveNext()) yield return null;
			i++;
		}
	}

	private IEnumerator DisplaceKeyButton(char key, Action<char> onKeyPressed = null)
	{
		if(keysDictionary == null || !keysDictionary.ContainsKey(key))
		{
			if(onKeyPressed != null) onKeyPressed(key);
			yield break;
		}

		Transform keyTransform = keysDictionary[key];
		Vector3 a = keyTransform.transform.position;
		Vector3 b = a + (-keyTransform.up * keyPressDisplacement);
		float duration = writingDuration;
		if(writingDurationDictionary != null && writingDurationDictionary.ContainsKey(key)) duration += writingDurationDictionary[key];
		float inverseDuration = 1.0f / (duration * 0.5f);
		float t = 0.0f;

		while(t < 1.0f)
		{
			keyTransform.position = Vector3.Lerp(a, b, t * t);
			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		keyTransform.position = b;
		t = 0.0f;

		if(onKeyPressed != null) onKeyPressed(key);

		while(t < 1.0f)
		{
			keyTransform.position = Vector3.Lerp(b, a, t * t);
			t += (Time.deltaTime * inverseDuration);
			yield return null;
		}

		keyTransform.position = a;
	}
}
}