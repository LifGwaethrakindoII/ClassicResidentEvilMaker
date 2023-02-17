using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Voidless
{
public static class VXMLSerializer
{
	public const string EXTENSION_SUFIX_XML = ".xml";

	/// <summary>Serialized Object on Path given.</summary>
	/// <param name="_item">Object to serialize.</param>
	/// <param name="_path">Path to save the Object.</param>
	public static void SerializeToXML<T>(this T _item, string _path)
	{
#if UNITY_EDITOR
        XmlSerializer serializer = new XmlSerializer(typeof(T), _path);

		using(StreamWriter writer = new StreamWriter(_path))
		{
			try { serializer.Serialize(writer.BaseStream, _item); }
			catch(Exception e) { Debug.LogError("Error while trying to save " + typeof(T).Name + ": " + e.Message); }
			finally { writer.Close(); }
		}
#endif
    }

	/// <summary>Deserializes object on given path as T.</summary>
	/// <param name="_path">Path to deserialize the object from.</param>
	/// <param name="_root">Optional Root. Null by default.</param>
	/// <returns>Deserialized Object [as T].</returns>
	public static T DeserializeFromXML<T>(string _path, string _root = null)
	{
		XmlSerializer serializer = string.IsNullOrEmpty(_root) ? new XmlSerializer(typeof(T)) : new XmlSerializer(typeof(T), new XmlRootAttribute(_root));
		T _item = default(T);

#if UNITY_EDITOR
        using (StreamReader reader = new StreamReader(_path))
		{
			try { _item = (T)serializer.Deserialize(reader.BaseStream); }
			catch(Exception e) { Debug.LogError("Error while trying to load " + typeof(T).Name + ": " + e.Message); }
			finally { reader.Close(); }
		}
#endif

        return _item;
	}

#if UNITY_EDITOR
	/// \TODO Serialize TextAsset outside from Editor.
	/// <summary>Serializes Item into TextAsset.</summary>
	/// <param name="_item">Item to serialize.</param>
	/// <param name="_file">File to serialize the Item to.</param>
	public static void SerializeToXMLIntoTextAsset<T>(this T _item, TextAsset _file)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(T));

#if UNITY_EDITOR
        using (StreamWriter writer = new StreamWriter(UnityEditor.AssetDatabase.GetAssetPath(_file)))
		{
			try { serializer.Serialize(writer.BaseStream, _item); }
			catch(Exception e) { Debug.LogError("Error while trying to save " + typeof(T).Name + ": " + e.Message); }
			finally { writer.Close(); }
		}
#endif
    }
#endif

	/// <summary>Deserializes TextAsset's Content into Item.</summary>
	/// <param name="_file">File to deserialize Item from.</param>
	/// <param name="_root">Optional Root [null by default].</param>
	/// <returns>Deserialized Item, if existing.</returns>
	public static T DeserializeXMLFromTextAsset<T>(TextAsset _file, string _root = null)
	{
		XmlSerializer serializer = string.IsNullOrEmpty(_root) ? new XmlSerializer(typeof(T)) : new XmlSerializer(typeof(T), new XmlRootAttribute(_root));
		T _item = default(T);

#if UNITY_EDITOR
        using (StreamReader reader = new StreamReader(_file.ToString()))
		{
			try { _item = (T)serializer.Deserialize(reader); }
			catch(Exception e) { Debug.LogError("Error while trying to load " + typeof(T).Name + ": " + e.Message); }
			finally { reader.Close(); }
		}
#endif
        return _item;
	}
}
}