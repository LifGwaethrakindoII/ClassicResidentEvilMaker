using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VGUIUtility
{
	/// <summary>Copies provided text to Clipboard.</summary>
	/// <param name="_text">Text to store on Clipboard.</param>
	public static void CopyToClipboard(string _text)
	{
		GUIUtility.systemCopyBuffer = _text;
	}

	/// <returns>Content stored on Clipboard.</returns>
	public static string GetClipboard()
	{
		return GUIUtility.systemCopyBuffer;
	}
}
}