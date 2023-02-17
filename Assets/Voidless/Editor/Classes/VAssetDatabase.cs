using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Object = UnityEngine.Object;

namespace Voidless
{
public static class VAssetDatabase
{
	/// <returns>Folder path of this Object [without this Object's name and extension].</returns>
	public static string GetAssetFolderPath(Object _object)
	{
		string path = AssetDatabase.GetAssetPath(_object);

		if(path == string.Empty) return string.Empty;

		int length = path.Length;
		string extension = Path.GetExtension(path);
		int objectNameLength = _object.name.Length;
		int extensionLength = extension.Length;

		return path.Substring(0, length - (objectNameLength + extensionLength));
	}
}
}