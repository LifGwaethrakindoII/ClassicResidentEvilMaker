using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Voidless
{
public static class VBinaryFormatter
{
	private static readonly BinaryFormatter formatter; 	/// <summary>General Use BinaryFormatter.</summary>

	/// <summary>Static VBinaryFormatter's Constructor.</summary>
	static VBinaryFormatter()
	{
		formatter = new BinaryFormatter();
	}

	/// <summary>Serializes to Binary Format.</summary>
	/// <param name="_item">Item to serialize. Make sure all the items properties are serializable. Note: Unity's types, such as Vectors, Quaternions and Colors, are not serializable</param>
	/// <param name="_path">Serialization Path.</param>
	public static void SerializeToBinaryFormat<T>(this T _item, string _path)
	{
		FileStream dataStream = new FileStream(_path, FileMode.Create);
		
		try { formatter.Serialize(dataStream, _item); }
		catch(Exception exception) { Debug.LogWarning("[VBinaryFormatter] Catched Exception while trying to serialize object of type " + typeof(T) + " : " + exception.Message); }

		dataStream.Close();
	}

	/// <summary>Deserializes from Binary Format.</summary>
	/// <param name="_path">Path where the Binary Format is located.</param>
	/// <returns>Deserialized Binary Format into object of desired type.</returns>
	public static T DeserializeFromBinaryFormat<T>(string _path)
	{
		T item = default(T);

		try
		{
			if(File.Exists(_path))
			{
				FileStream dataStream = new FileStream(_path, FileMode.Open);
				item = (T)(formatter.Deserialize(dataStream));
				dataStream.Close();
			}
		}
		catch(Exception exception) { Debug.LogWarning("[VBinaryFormatter] Catched Exception while trying to deserialize object of type " + typeof(T) + " : " + exception.Message); }

		return item;
	}
}
}