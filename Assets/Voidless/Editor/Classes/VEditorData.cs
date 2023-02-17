using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[Serializable]
public class VEditorData
{
	private const string PATH_ROOT_FOLDER = "/Voidless/Editor Default Resources"; 	/// <summary>Default Root's Path.</summary>
	private const string NAME_DATA = "/Data_VoidlessEditor.txt"; 					/// <summary>JSON's file default name.</summary>

	private static VEditorData editorData; 											/// <summary>EditorData's internal static reference.</summary>

	[SerializeField] private StringStringDictionary StringStringDictionary; 		/// <summary>Dictionary of String Keys and String Values.</summary>
	[SerializeField] private StringIntDictionary StringIntDictionary; 				/// <summary>Dictionary of String Keys and Int Values.</summary>
	[SerializeField] private StringFloatDictionary StringFloatDictionary; 			/// <summary>Dictionary of String Keys and Float Values.</summary>

	/// <summary>VEditorData's Constructor.</summary>
	private VEditorData()
	{
		StringStringDictionary = new StringStringDictionary();
		StringIntDictionary = new StringIntDictionary();
		StringFloatDictionary = new StringFloatDictionary();
	}

#region StringStringMethods:
	/// <summary>Saves String Entry.</summary>
	/// <param name="_key">Entry's Key.</param>
	/// <param name="_value">Value to save.</param>
	public static void SaveString(string _key, string _value)
	{
		InitializeEditorData();
		if(editorData.StringStringDictionary.ContainsKey(_key)) editorData.StringStringDictionary[_key] = _value;
		else editorData.StringStringDictionary.Add(_key, _value);
		Save();
	}

	/// <summary>Loads String Entry [if registered].</summary>
	/// <param name="_key">Entry's Key.</param>
	/// <param name="_default">Default value to return if there was no entry stored with given key [null by default].</param>
	/// <returns>String located on entry if stored, default otherwise.</returns>
	public static string LoadString(string _key, string _default = null)
	{
		InitializeEditorData();
		if(editorData.StringStringDictionary.ContainsKey(_key)) return editorData.StringStringDictionary[_key];
		else return _default;
	}

	/// <summary>Deletes String's Entry on given Key.</summary>
	/// <param name="_key">Key where the entry should be stored.</param>
	/// <returns>True if an entry with given key existed, false otherwise.</returns>
	public static bool DeleteString(string _key)
	{
		InitializeEditorData();
		if(editorData.StringStringDictionary.ContainsKey(_key))
		{
			editorData.StringStringDictionary.Remove(_key);
			Save();
			return true;
		}
		else return false;
	}

	/// <summary>Deletes all String's Entries.</summary>
	public static void DeleteAllStringEntries()
	{
		InitializeEditorData();
		editorData.StringStringDictionary.Clear();
		Save();
	}
#endregion

#region StringIntMethods:
	/// <summary>Saves Int Entry.</summary>
	/// <param name="_key">Entry's Key.</param>
	/// <param name="_value">Value to save.</param>
	public static void SaveInt(string _key, int _value)
	{
		InitializeEditorData();
		if(editorData.StringIntDictionary.ContainsKey(_key)) editorData.StringIntDictionary[_key] = _value;
		else editorData.StringIntDictionary.Add(_key, _value);
		Save();
	}

	/// <summary>Loads Int Entry [if registered].</summary>
	/// <param name="_key">Entry's Key.</param>
	/// <param name="_default">Default value to return if there was no entry stored with given key [0 by default].</param>
	/// <returns>Int located on entry if stored, default otherwise.</returns>
	public static int LoadInt(string _key, int _default = 0)
	{
		InitializeEditorData();
		if(editorData.StringIntDictionary.ContainsKey(_key)) return editorData.StringIntDictionary[_key];
		else return _default;
	}

	/// <summary>Deletes Int's Entry on given Key.</summary>
	/// <param name="_key">Key where the entry should be stored.</param>
	/// <returns>True if an entry with given key existed, false otherwise.</returns>
	public static bool DeleteInt(string _key)
	{
		InitializeEditorData();
		if(editorData.StringIntDictionary.ContainsKey(_key))
		{
			editorData.StringIntDictionary.Remove(_key);
			Save();
			return true;
		}
		else return false;
	}

	/// <summary>Deletes all Int's Entries.</summary>
	public static void DeleteAllIntEntries()
	{
		InitializeEditorData();
		editorData.StringIntDictionary.Clear();
		Save();
	}
#endregion

#region StringFloatMethods:
	/// <summary>Saves Float Entry.</summary>
	/// <param name="_key">Entry's Key.</param>
	/// <param name="_value">Value to save.</param>
	public static void SaveFloat(string _key, float _value)
	{
		InitializeEditorData();
		if(editorData.StringFloatDictionary.ContainsKey(_key)) editorData.StringFloatDictionary[_key] = _value;
		else editorData.StringFloatDictionary.Add(_key, _value);
		Save();
	}

	/// <summary>Loads Float Entry [if registered].</summary>
	/// <param name="_key">Entry's Key.</param>
	/// <param name="_default">Default value to return if there was no entry stored with given key [0.0f by default].</param>
	/// <returns>Float located on entry if stored, default otherwise.</returns>
	public static float LoadFloat(string _key, float _default = 0.0f)
	{
		InitializeEditorData();
		if(editorData.StringFloatDictionary.ContainsKey(_key)) return editorData.StringFloatDictionary[_key];
		else return _default;
	}

	/// <summary>Deletes Float's Entry on given Key.</summary>
	/// <param name="_key">Key where the entry should be stored.</param>
	/// <returns>True if an entry with given key existed, false otherwise.</returns>
	public static bool DeleteFloat(string _key)
	{
		InitializeEditorData();
		if(editorData.StringFloatDictionary.ContainsKey(_key))
		{
			editorData.StringFloatDictionary.Remove(_key);
			Save();
			return true;
		}
		else return false;
	}

	/// <summary>Deletes all Float's Entries.</summary>
	public static void DeleteAllFloatEntries()
	{
		InitializeEditorData();
		editorData.StringFloatDictionary.Clear();
		Save();
	}
#endregion

	/// <summary>Clears all Data.</summary>
	public static void DeleteAllData()
	{
		InitializeEditorData();
		editorData.StringStringDictionary.ClearAll();
		editorData.StringIntDictionary.ClearAll();
		editorData.StringFloatDictionary.ClearAll();
		Save();
	}

	/// <summary>Loads Editor's Data.</summary>
	private static void Load()
	{
		string folderPath = Application.dataPath + PATH_ROOT_FOLDER;
		string filePath = folderPath + NAME_DATA;
		bool directoryExists = Directory.Exists(folderPath);
		bool fileExists = File.Exists(filePath);

		if(directoryExists && fileExists)
		{
			editorData = VJSONSerializer.DeserializeFromJSONFromPath<VEditorData>(filePath);
			if(editorData == null) editorData = new VEditorData();
		}
		else
		{
			if(!directoryExists) Directory.CreateDirectory(folderPath);
			File.Create(filePath);
			editorData = new VEditorData();
			editorData.SerializeToJSON(filePath);
		}
	}

	/// <summary>Saves Editor's Data into JSON format.</summary>
	private static void Save()
	{
		if(editorData == null) Load();
		else editorData.SerializeToJSON(Application.dataPath + PATH_ROOT_FOLDER + NAME_DATA);
		AssetDatabase.Refresh();
	}

	/// <summary>Initializes Editor's Data if non-existing.</summary>
	private static void InitializeEditorData()
	{
		if(editorData == null) Load();
	}

	/// <returns>String representing Editor Data's Dictionaries.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.AppendLine("Editor's Data: ");
		builder.AppendLine();
		builder.Append("<String, String> ");
		builder.AppendLine(StringStringDictionary.ToString());
		builder.Append("<String, Int> ");
		builder.AppendLine(StringIntDictionary.ToString());
		builder.Append("<String, Float> ");
		builder.AppendLine(StringFloatDictionary.ToString());

		return builder.ToString();
	}

	public static void ShowDictionary()
	{
		Load();
		//VEditorGUILayout.ShowStringStringDictionary(ref editorData.StringStringDictionary);
	}
}
}