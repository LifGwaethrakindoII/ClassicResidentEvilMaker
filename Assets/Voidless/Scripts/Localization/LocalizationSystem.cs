using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
/// <summary>Event invoked when the System Language's Changes.</summary>
public delegate void OnLanguageChanged(SystemLanguage _language);

public class LocalizationSystem : Singleton<LocalizationSystem>
{
	public static event OnLanguageChanged onLanguageChanged; 	/// <summary>OnLanguageChanged's Delegate.</summary>

	[SerializeField] private SystemLanguage _language; 			/// <summary>Current's Language.</summary>
	[SerializeField] private LocalizationMapping _mapping; 		/// <summary>Localization's Mapping.</summary>
	private SystemLanguage _previousLanguage; 					/// <summary>Previous' Language.</summary>

	/// <summary>Gets and Sets language property.</summary>
	public static SystemLanguage language
	{
		get { return Instance._language; }
		set 
		{
			if(previousLanguage != language && onLanguageChanged != null) onLanguageChanged(value);
			previousLanguage = language;
			Instance._language = value;
		}
	}

	/// <summary>Gets and Sets mapping property.</summary>
	public LocalizationMapping mapping
	{
		get { return _mapping; }
		set { _mapping = value; }
	}

	/// <summary>Gets and Sets previousLanguage property.</summary>
	public static SystemLanguage previousLanguage
	{
		get { return Instance._previousLanguage; }
		private set { Instance._previousLanguage = value; }
	}

	/// <summary>Resets LocalizationSystem's instance to its default values.</summary>
	private void Reset()
	{
		language = Application.systemLanguage;
	}

	/// <summary>Localizes given Key.</summary>
	/// <returns>Localized Key.</returns>
	public static string Localize(string _key)
	{
		return Instance != null ? Instance.mapping.Localize(_key, language) : LocalizationMapping.errorMessages[Application.systemLanguage];
	}
}
}