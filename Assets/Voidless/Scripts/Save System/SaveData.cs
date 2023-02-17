using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public class SaveData
{
	[SerializeField] private StringStringDictionary _stringDictionary; 				/// <summary>String's Dictionary.</summary>
	[SerializeField] private StringBoolDictionary _boolDictionary; 					/// <summary>Bool's Dictionary.</summary>
	[SerializeField] private StringIntDictionary _intDictionary; 					/// <summary>Int's Dictionary.</summary>
	[SerializeField] private StringFloatDictionary _floatDictionary; 				/// <summary>Float's Dictionary.</summary>
	[SerializeField] private StringVector2Dictionary _vector2Dictionary; 			/// <summary>Vector2's Dictionary.</summary>
	[SerializeField] private StringVector3Dictionary _vector3Dictionary; 			/// <summary>Vector3's Dictionary.</summary>
	[SerializeField] private StringEulerRotationDictionary _rotationDictionary; 	/// <summary>EulerRotation's Dictionary.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets stringDictionary property.</summary>
	public StringStringDictionary stringDictionary
	{
		get { return _stringDictionary; }
		private set { _stringDictionary = value; }
	}

	/// <summary>Gets and Sets boolDictionary property.</summary>
	public StringBoolDictionary boolDictionary
	{
		get { return _boolDictionary; }
		private set { _boolDictionary = value; }
	}

	/// <summary>Gets and Sets intDictionary property.</summary>
	public StringIntDictionary intDictionary
	{
		get { return _intDictionary; }
		private set { _intDictionary = value; }
	}

	/// <summary>Gets and Sets floatDictionary property.</summary>
	public StringFloatDictionary floatDictionary
	{
		get { return _floatDictionary; }
		private set { _floatDictionary = value; }
	}

	/// <summary>Gets and Sets vector2Dictionary property.</summary>
	public StringVector2Dictionary vector2Dictionary
	{
		get { return _vector2Dictionary; }
		private set { _vector2Dictionary = value; }
	}

	/// <summary>Gets and Sets vector3Dictionary property.</summary>
	public StringVector3Dictionary vector3Dictionary
	{
		get { return _vector3Dictionary; }
		private set { _vector3Dictionary = value; }
	}

	/// <summary>Gets and Sets rotationDictionary property.</summary>
	public StringEulerRotationDictionary rotationDictionary
	{
		get { return _rotationDictionary; }
		private set { _rotationDictionary = value; }
	}
#endregion

	/// <summary>SaveData's Constructor.</summary>
	public SaveData()
	{
		stringDictionary = new StringStringDictionary();
		boolDictionary = new StringBoolDictionary();
		intDictionary = new StringIntDictionary();
		floatDictionary = new StringFloatDictionary();
		vector2Dictionary = new StringVector2Dictionary();
		vector3Dictionary = new StringVector3Dictionary();
		rotationDictionary = new StringEulerRotationDictionary();
	}

#region SaveFunctions:
	/// <summary>Saves string value into entry.</summary>
	/// <param name="key">Entry's Key.</param>
	/// <param name="value">Value to introduce.</param>
	public void SaveString(string key, string value)
	{
		if(!stringDictionary.ContainsKey(key)) stringDictionary.Add(key, value);
		else stringDictionary[key] = value;
	}

	/// <summary>Saves bool value into entry.</summary>
	/// <param name="key">Entry's Key.</param>
	/// <param name="value">Value to introduce.</param>
	public void SaveBool(string key, bool value)
	{
		if(!boolDictionary.ContainsKey(key)) boolDictionary.Add(key, value);
		else boolDictionary[key] = value;
	}

	/// <summary>Saves int value into entry.</summary>
	/// <param name="key">Entry's Key.</param>
	/// <param name="value">Value to introduce.</param>
	public void SaveInt(string key, int value)
	{
		if(!intDictionary.ContainsKey(key)) intDictionary.Add(key, value);
		else intDictionary[key] = value;
	}

	/// <summary>Saves float value into entry.</summary>
	/// <param name="key">Entry's Key.</param>
	/// <param name="value">Value to introduce.</param>
	public void SaveFloat(string key, float value)
	{
		if(!floatDictionary.ContainsKey(key)) floatDictionary.Add(key, value);
		else floatDictionary[key] = value;
	}

	/// <summary>Saves Vector2 value into entry.</summary>
	/// <param name="key">Entry's Key.</param>
	/// <param name="value">Value to introduce.</param>
	public void SaveVector2(string key, Vector2 value)
	{
		if(!vector2Dictionary.ContainsKey(key)) vector2Dictionary.Add(key, value);
		else vector2Dictionary[key] = value;
	}

	/// <summary>Saves Vector3 value into entry.</summary>
	/// <param name="key">Entry's Key.</param>
	/// <param name="value">Value to introduce.</param>
	public void SaveVector3(string key, Vector3 value)
	{
		if(!vector3Dictionary.ContainsKey(key)) vector3Dictionary.Add(key, value);
		else vector3Dictionary[key] = value;
	}

	/// <summary>Saves EulerRotation value into entry.</summary>
	/// <param name="key">Entry's Key.</param>
	/// <param name="value">Value to introduce.</param>
	public void SaveRotation(string key, EulerRotation value)
	{
		if(!rotationDictionary.ContainsKey(key)) rotationDictionary.Add(key, value);
		else rotationDictionary[key] = value;
	}
#endregion

#region DeleteFunctions:
	/// <summary>Deletes string value on given entry.</summary>
	/// <param name="key">Entry's Key.</param>
	public void DeleteString(string key, string value)
	{
		if(stringDictionary.ContainsKey(key)) stringDictionary.Remove(key);
	}

	/// <summary>Deletes bool value on given entry.</summary>
	/// <param name="key">Entry's Key.</param>
	public void DeleteBool(string key, bool value)
	{
		if(boolDictionary.ContainsKey(key)) boolDictionary.Remove(key);
	}

	/// <summary>Deletes int value on given entry.</summary>
	/// <param name="key">Entry's Key.</param>
	public void DeleteInt(string key, int value)
	{
		if(intDictionary.ContainsKey(key)) intDictionary.Remove(key);
	}

	/// <summary>Deletes float value on given entry.</summary>
	/// <param name="key">Entry's Key.</param>
	public void DeleteFloat(string key, float value)
	{
		if(floatDictionary.ContainsKey(key)) floatDictionary.Remove(key);
	}

	/// <summary>Deletes Vector2 value on given entry.</summary>
	/// <param name="key">Entry's Key.</param>
	public void DeleteVector2(string key, Vector2 value)
	{
		if(vector2Dictionary.ContainsKey(key)) vector2Dictionary.Remove(key);
	}

	/// <summary>Deletes Vector3 value on given entry.</summary>
	/// <param name="key">Entry's Key.</param>
	public void DeleteVector3(string key, Vector3 value)
	{
		if(vector3Dictionary.ContainsKey(key)) vector3Dictionary.Remove(key);
	}

	/// <summary>Deletes EulerRotation value on given entry.</summary>
	/// <param name="key">Entry's Key.</param>
	public void DeleteRotation(string key, EulerRotation value)
	{
		if(rotationDictionary.ContainsKey(key)) rotationDictionary.Remove(key);
	}
#endregion

#region LoadFunctions:
	/// <summary>Loads string registered on given entry.</summary>
	/// <param name="key">Entry's Key.</param>
	/// <param name="_default">[Optional] Default value returned if there is no entry registered.</param>
	/// <returns>String value.</returns>
	public string LoadString(string key, string _default = default(string))
	{
		return stringDictionary.ContainsKey(key) ? stringDictionary[key] : _default;
	}

	/// <summary>Loads bool registered on given entry.</summary>
	/// <param name="key">Entry's Key.</param>
	/// <param name="_default">[Optional] Default value returned if there is no entry registered.</param>
	/// <returns>Bool value.</returns>
	public bool LoadBool(string key, bool _default = default(bool))
	{
		return boolDictionary.ContainsKey(key) ? boolDictionary[key] : _default;
	}

	/// <summary>Loads int registered on given entry.</summary>
	/// <param name="key">Entry's Key.</param>
	/// <param name="_default">[Optional] Default value returned if there is no entry registered.</param>
	/// <returns>Int value.</returns>
	public int LoadInt(string key, int _default = default(int))
	{
		return intDictionary.ContainsKey(key) ? intDictionary[key] : _default;
	}

	/// <summary>Loads float registered on given entry.</summary>
	/// <param name="key">Entry's Key.</param>
	/// <param name="_default">[Optional] Default value returned if there is no entry registered.</param>
	/// <returns>Float value.</returns>
	public float LoadFloat(string key, float _default = default(float))
	{
		return floatDictionary.ContainsKey(key) ? floatDictionary[key] : _default;
	}

	/// <summary>Loads Vector2 registered on given entry.</summary>
	/// <param name="key">Entry's Key.</param>
	/// <param name="_default">[Optional] Default value returned if there is no entry registered.</param>
	/// <returns>Vector2 value.</returns>
	public Vector2 LoadVector2(string key, Vector2 _default = default(Vector2))
	{
		return vector2Dictionary.ContainsKey(key) ? vector2Dictionary[key] : _default;
	}

	/// <summary>Loads Vector3 registered on given entry.</summary>
	/// <param name="key">Entry's Key.</param>
	/// <param name="_default">[Optional] Default value returned if there is no entry registered.</param>
	/// <returns>Vector3 value.</returns>
	public Vector3 LoadVector3(string key, Vector3 _default = default(Vector3))
	{
		return vector3Dictionary.ContainsKey(key) ? vector3Dictionary[key] : _default;
	}

	/// <summary>Loads EulerRotation registered on given entry.</summary>
	/// <param name="key">Entry's Key.</param>
	/// <param name="_default">[Optional] Default value returned if there is no entry registered.</param>
	/// <returns>EulerRotation value.</returns>
	public EulerRotation LoadRotation(string key, EulerRotation _default = default(EulerRotation))
	{
		return rotationDictionary.ContainsKey(key) ? rotationDictionary[key] : _default;
	}
#endregion

	/// <returns>JSon representation of this SaveData.</returns>
	public virtual string ToJSON()
	{
		return JsonUtility.ToJson(this);
	}

	/// <returns>String representign this SaveData.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("Save Data: ");
		builder.AppendLine();
		builder.Append("String Entries: ");
		builder.Append(stringDictionary.ToString());
		builder.AppendLine();
		builder.Append("Bool Entries: ");
		builder.Append(boolDictionary.ToString());
		builder.AppendLine();
		builder.Append("Int Entries: ");
		builder.Append(intDictionary.ToString());
		builder.AppendLine();
		builder.Append("Float Entries: ");
		builder.Append(floatDictionary.ToString());
		builder.AppendLine();
		builder.Append("Vector2 Entries: ");
		builder.Append(vector2Dictionary.ToString());
		builder.AppendLine();
		builder.Append("Vector3 Entries: ");
		builder.Append(vector3Dictionary.ToString());
		builder.AppendLine();
		builder.Append("Rotation Entries: ");
		builder.Append(rotationDictionary.ToString());

		return builder.ToString();
	}
}
}