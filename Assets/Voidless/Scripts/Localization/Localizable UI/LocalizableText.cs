using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Voidless
{
[RequireComponent(typeof(Text))]
public class LocalizableText : MonoBehaviour
{
	[SerializeField] private string _key; 	/// <summary>Localizable's Key.</summary>
	private Text _text; 					/// <summary>Text's Component.</summary>

	/// <summary>Gets and Sets key property.</summary>
	public string key
	{
		get { return _key; }
		set
		{
			_key = value;
			UpdateText();
		}
	}

	/// <summary>Gets and Sets text Component.</summary>
	public Text text
	{ 
		get
		{
			if(_text == null) _text = GetComponent<Text>();
			return _text;
		}
		set { _text = value; }
	}

	/// <summary>Callback invoked when LocalizableText's instance is enabled.</summary>
	private void OnEnable()
	{
		if(LocalizationSystem.Instance != null) UpdateText();
		LocalizationSystem.onLanguageChanged += OnLanguageChanged;
	}

	/// <summary>Callback invoked when LocalizableText's instance is disabled.</summary>
	private void OnDisable()
	{
		LocalizationSystem.onLanguageChanged -= OnLanguageChanged;
	}

	/// <summary>LocalizableText's starting actions before 1st Update frame.</summary>
	private void Start ()
	{
		UpdateText();
	}

	/// <summary>Callback invoked when the Language changes.</summary>
	/// <param name="_language">New Language.</param>
	private void OnLanguageChanged(SystemLanguage _language)
	{
		UpdateText();
	}

	/// <summary>Updates Text.</summary>
	public void UpdateText()
	{
		text.text = LocalizationSystem.Localize(_key);
	}
}
}