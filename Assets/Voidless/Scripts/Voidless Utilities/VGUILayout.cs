using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VGUILayout
{
	/// <summary>IntField for GUILayout [given that the original API does not provide it].</summary>
	/// <param name="_value">Value of the int.</param>
	/// <param name="_maxLength">Field's limit [int.MaxValue by default].</param>
	/// <param name="_options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.</param>
	/// <returns>Value parsed from Text Field [default given value if not valid].</returns>
	public static int IntField(int _value, int _maxLength = int.MaxValue, params GUILayoutOption[] _options)
	{
		string valueText = _value.ToString();
		int x = 0;

		valueText = GUILayout.TextField(valueText, _maxLength, _options);

		switch(valueText)
		{
			case "":
			case " ":
				return 0;

			default:
				return int.TryParse(valueText, out x) ? x : _value;
		}
	}

	/// <summary>FloatField for GUILayout [given that the original API does not provide it].</summary>
	/// <param name="_value">Value of the float.</param>
	/// <param name="_maxLength">Field's limit [int.MaxValue by default].</param>
	/// <param name="_options">An optional list of layout options that specify extra layouting properties. Any values passed in here will override settings defined by the style.</param>
	/// <returns>Value parsed from Text Field [default given value if not valid].</returns>
	public static float FloatField(float _value, int _maxLength = int.MaxValue, params GUILayoutOption[] _options)
	{
		string valueText = _value.ToString();
		float x = 0.0f;

		valueText = GUILayout.TextField(valueText, _maxLength, _options);

		switch(valueText)
		{
			case "":
			case " ":
				return 0.0f;

			case ".":
				return 0.1f;

			default:
				return float.TryParse(valueText, out x) ? x : _value;
		}
	}
}
}