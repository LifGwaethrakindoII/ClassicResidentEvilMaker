using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VJSONSerializer
{
	/// <summary>Serializes given item into JSON format to a file located at provided path.</summary>
	/// <param name="_item">Item to serialize.</param>
	/// <param name="_path">Path to serialize the JSON's content.</param>
	public static void SerializeToJSON<T>(this T _item, string _path)
	{
		string json = JsonUtility.ToJson(_item);
		try { File.WriteAllText(_path, json); }
		catch(Exception exception) { Debug.LogWarning("[VJSONSerializer] Catched Exception while trying to serialize to JSON: " + exception.Message ); }
	}

	/// <summary>Deserializes JSON content from file located at provided path.</summary>
	/// <param name="_path">Path where the JSON should be located.</param>
	/// <returns>Deserialized item from JSON's content, if such exists.</returns>
	public static T DeserializeFromJSONFromPath<T>(string _path)
	{
		T item = default(T);
		string json = null;

		try
		{
			json = File.ReadAllText(_path);
			item = JsonUtility.FromJson<T>(json);
		}
		catch(Exception exception) { Debug.LogWarning("[VJSONSerializer] Catched Exception while trying to deserialize object of type " + typeof(T) + " : " + exception.Message); }

		return item;
	}

	/// <summary>Deserializes JSON content from TextAsset file.</summary>
	/// <param name="_file">File that should contain the JSON's content.</param>
	/// <returns>Deserialized item from JSon's content, if such exists.</returns>
	public static T DeserializeFromJSONFromTextAsset<T>(TextAsset _file)
	{
		T item = default(T);

		try { item = JsonUtility.FromJson<T>(_file.text); }
		catch(Exception exception) { Debug.LogWarning("[VJSONSerializer] Catched Exception while trying to deserialize object of type " + typeof(T) + " from TextAsset: " + exception.Message); }

		return item;
	}

#if UNITY_EDITOR
	/// <summary>Deserializes item to JSON format to a TextAsset file.</summary>
	/// <param name="_item">Item to serialize into JSON format.</param>
	/// <param name="_file">File to overwrite the JSON's content to.</param>
	public static void SerializeToJSONIntoTextAsset<T>(this T _item, TextAsset _file)
	{
		string json = JsonUtility.ToJson(_item);
		try { File.WriteAllText(UnityEditor.AssetDatabase.GetAssetPath(_file), json); }
		catch(Exception exception) { Debug.LogWarning("[VJSONSerializer] Catched Exception while trying to serialize JSON into TextAsset: " + exception.Message); }
	}
#endif
}
}