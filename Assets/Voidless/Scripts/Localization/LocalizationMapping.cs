using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable] public class SystemLanguageStringTuple : VTuple<SystemLanguage, string> { /*...*/ }

[Serializable]
public class LocalizationMapping
{
	public static readonly Dictionary<SystemLanguage, string> errorMessages; 	/// <summary>Error Messages per System Language.</summary>

	[SerializeField] private string[] _keys; 									/// <summary>Mapping's Keys.</summary>
	[SerializeField] private LocalizationText[] _localizationTexts; 			/// <summary>Localization's Texts.</summary>
	[SerializeField] private SystemLanguageStringTuple[] _tuples; 				/// <summary>Tuples containing text associated with a System Language.</summary>
	private Dictionary<string, Dictionary<SystemLanguage, string>> _map; 		/// <summary>Localization's Map.</summary>

	/// <summary>Gets and Sets keys property.</summary>
	public string[] keys
	{
		get { return _keys; }
		set { _keys = value; }
	}

	/// <summary>Gets and Sets localizationTexts property.</summary>
	public LocalizationText[] localizationTexts
	{
		get { return _localizationTexts; }
		set { _localizationTexts = value; }
	}

	/// <summary>Gets and Sets tuples property.</summary>
	public SystemLanguageStringTuple[] tuples
	{
		get { return _tuples; }
		set { _tuples = value; }
	}

	/// <summary>Gets and Sets map property.</summary>
	public Dictionary<string, Dictionary<SystemLanguage, string>> map
	{
		get { return _map; }
		set { _map = value; }
	}

	/// <summary>Static LocalizationMapping's Constructor.</summary>
	static LocalizationMapping()
	{
		errorMessages = new Dictionary<SystemLanguage, string>()
		{
			{ SystemLanguage.Afrikaans, "#FOUT!"},
			{ SystemLanguage.Arabic, "#خطأ!"},
			{ SystemLanguage.Basque, "#ERROREA!"},
			{ SystemLanguage.Belarusian, "#ПАМЫЛКА!"},
			{ SystemLanguage.Bulgarian, "#ГРЕШКА!"},
			{ SystemLanguage.Catalan, "#ERROR!"},
			{ SystemLanguage.Chinese, "＃错误！"},
			{ SystemLanguage.Czech, "#CHYBA!"},
			{ SystemLanguage.Danish, "#FEJL!"},
			{ SystemLanguage.Dutch, "#FOUT!"},
			{ SystemLanguage.English, "#ERROR!"},
			{ SystemLanguage.Estonian, "#VIGA!"},
			{ SystemLanguage.Faroese, "#MISTAK!"},
			{ SystemLanguage.Finnish, "#VIRHE!"},
			{ SystemLanguage.French, "#ERREUR!"},
			{ SystemLanguage.German, "#ERROR!"},
			{ SystemLanguage.Greek, "#ΛΑΘΟΣ!"},
			{ SystemLanguage.Hebrew, "#שְׁגִיאָה!"},
			{ SystemLanguage.Icelandic, "#VILLA!"},
			{ SystemLanguage.Indonesian, "#KESALAHAN!"},
			{ SystemLanguage.Italian, "#ERRORE!"},
			{ SystemLanguage.Japanese, "＃エラー！"},
			{ SystemLanguage.Korean, "#오류!"},
			{ SystemLanguage.Latvian, "#KĻŪDA!"},
			{ SystemLanguage.Lithuanian, "#KLAIDA!"},
			{ SystemLanguage.Norwegian, "#FEIL!"},
			{ SystemLanguage.Polish, "#BŁĄD!"},
			{ SystemLanguage.Portuguese, "#ERRO!"},
			{ SystemLanguage.Romanian, "#EROARE!"},
			{ SystemLanguage.Russian, "#ОШИБКА!"},
			{ SystemLanguage.SerboCroatian, "#ГРЕШКА!"},
			{ SystemLanguage.Slovak, "#CHYBA!"},
			{ SystemLanguage.Slovenian, "#CHYBA!"},
			{ SystemLanguage.Spanish, "#ERROR!"},
			{ SystemLanguage.Swedish, "#FEL!"},
			{ SystemLanguage.Thai, "#ข้อผิดพลาด!"},
			{ SystemLanguage.Turkish, "#HATA!"},
			{ SystemLanguage.Ukrainian, "#ПОМИЛКА!"},
			{ SystemLanguage.Vietnamese, "#LỖI!"},
			{ SystemLanguage.ChineseSimplified, "＃错误！"},
			{ SystemLanguage.ChineseTraditional, "＃錯誤！"},
			{ SystemLanguage.Unknown, "#ERROR!"},
			{ SystemLanguage.Hungarian, "#HIBA!"}
		};
	}

	/// <summary>LocalizationMapping default constructor.</summary>
	public LocalizationMapping()
	{
		keys = new string[0];
		tuples = new SystemLanguageStringTuple[0];
	}

	/// <summary>Localizes given Key to the provided System's Language.</summary>
	/// <param name="_key">Key to Localize.</param>
	/// <param name="_language">System's Language.</param>
	/// <returns>Localized Key. If the key is not registered on the mapping, it will return an error message on the selected language.</returns>
	public string Localize(string _key, SystemLanguage _language)
	{
		return (map != null && map.ContainsKey(_key)) ? map[_key][_language] : errorMessages[_language];
	}
}
}