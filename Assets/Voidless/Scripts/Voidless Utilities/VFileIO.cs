using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VFileIO
{
	/// <summary>Writes file into desired path.</summary>
	/// <param name="text">Text that will be written into the file.</param>
	/// <param name="path">File's Path [file extension must be included in the path].</param>
	public static void WriteFile(string text, string path)
	{
		try { File.WriteAllText(path, text); }
		catch(Exception exception) { Debug.LogWarning("[VFileIO] Catched Exception while trying to write file: " + exception.Message ); }
	}
}
}