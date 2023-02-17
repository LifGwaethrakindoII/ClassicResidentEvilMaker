using System;
using System.Text;
using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public class SerializableDictionary<K, V> : Dictionary<K, V>, ISerializationCallbackReceiver
#if UNITY_EDITOR
        , ISerializable
#endif
{
	[HideInInspector] public List<K> _keysList; 	/// <summary>Dictionary's Keys on a List.</summary>
	[HideInInspector] public List<V> _valuesList; 	/// <summary>Dictionary's Values on a List.</summary>

	/// <summary>Gets and Sets keysList property.</summary>
	public List<K> keysList
	{
		get { return _keysList; }
		set { _keysList = value; }
	}

	/// <summary>Gets and Sets valuesList property.</summary>
	public List<V> valuesList
	{
		get { return _valuesList; }
		set { _valuesList = value; }
	}

	/*public static implicit operator Dictionary<K, V>(SerializableDictionary<K, V> _dictionary)
	{
		Dictionary
	}*/

	/// <summary>SerializableDictionary default constructor.</summary>
	public SerializableDictionary() : base()
	{
		keysList = new List<K>();
		valuesList = new List<V>();
	}

#if UNITY_EDITOR
        public SerializableDictionary(SerializationInfo _information, StreamingContext _context) : base(_information, _context)
	{
		keysList = new List<K>();
		valuesList = new List<V>();
	}
#endif

        /// <summary>SerializableDictionary destructor.</summary>
        ~SerializableDictionary()
	{
		
	}

	/// <summary>Implement this method to receive a callback before Unity serializes your object.</summary>
	/// <summary>Saves Dictionary to both Lists.</summary>
	public void OnBeforeSerialize()
    	{
	    	keysList.Clear();
	    	valuesList.Clear();

	    	foreach(KeyValuePair<K, V> pair in this)
	    	{
	    		keysList.Add(pair.Key);
	    		valuesList.Add(pair.Value);
	    	}
    	}

    /// <summary>Implement this method to receive a callback after Unity deserializes your object.</summary>
    public void OnAfterDeserialize()
    {
    	Clear();
    	int minSize = Mathf.Min(keysList.Count, valuesList.Count);

    	if(keysList.Count != valuesList.Count)
    	{
    		Debug.LogWarning("[SerializableDictionary] Counts differ. Resizing Lists...");

    		while(minSize > keysList.Count) keysList.RemoveAt(keysList.Count - 1);
    		while(minSize > valuesList.Count) valuesList.RemoveAt(valuesList.Count - 1);

    		minSize = Mathf.Min(keysList.Count, valuesList.Count);
    	}

    	for(int i = 0; i < minSize; i++)
    	{
    		if(ContainsKey(keysList[i]))
    		{
    			Debug.LogWarning("[SerializableDictionary] Dictionary already contains key: " + keysList[i].ToString() + ", thus a random key will be generated. Insert a proper key.");
    			K temporalKey = ProvideTemporalKey(keysList[i]);
    			Add(temporalKey, valuesList[i]);
    			if(i + 1 < minSize) keysList[i + 1] = temporalKey;
    		} else Add(keysList[i], valuesList[i]);	
    	}
    }

    /// <summary>Clears internal dictionary and Keys' and Values' Lists.</summary>
    public void ClearAll()
    {
    	Clear();
    	keysList.Clear();
    	valuesList.Clear();
    }

#if UNITY_EDITOR
        /// <summary>Populates a SerializationInfo with the data needed to serialize the target object.</summary>
        public override void GetObjectData( SerializationInfo _info, StreamingContext _context)
	{
	        foreach(KeyValuePair<K, V> pair in this)
		{
	            _info.AddValue(pair.Key.ToString(), pair.Value);
	        }
	}
#endif

        /// <param name="_repeatedKey">Repeated Key's Reference.</param>
        /// <returns>Random temporal key.</returns>
        protected virtual K ProvideTemporalKey(K _repeatedKey) { return default(K); }

    	/// <returns>String representing this Dictionary's Entries.</returns>
    	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("Dictionary: ");
		builder.Append("\n{");
		builder.Append("\n");
		foreach(KeyValuePair<K, V> pair in this)
		{
			builder.Append("\t[ ");
			builder.Append("Key: ");
			builder.Append(pair.Key.ToString());
			builder.Append(", Value: ");
			builder.Append(pair.Value.ToString());
			builder.Append(" ]");
			builder.Append("\n");
		}
		builder.Append("}");

		return builder.ToString();
	}
}

[Serializable] public class StringKeyDictionary<V> : SerializableDictionary<string, V>
{
	/// <param name="_repeatedKey">Repeated Key's Reference.</param>
	/// <returns>Random temporal key.</returns>
    protected override string ProvideTemporalKey(string _repeatedKey)
    {
    	string copy_sufix = "COPY_";
    	return copy_sufix + _repeatedKey;
    }
}

[Serializable] public struct StringArray { public string[] array; }

// Char Key Dictionaries:
[Serializable] public class CharKeyDictionary<V> : SerializableDictionary<char, V> { /*...*/ }
[Serializable] public class CharFloatDictionary : CharKeyDictionary<float> { /*...*/ }
[Serializable] public class CharTransformDictionary : CharKeyDictionary<Transform> { /*...*/ }
[Serializable] public class CharAudioClipDictionary : CharKeyDictionary<AudioClip> { /*...*/ }

// String Key Dictionaries:
[Serializable] public class StringStringDictionary : StringKeyDictionary<string> { /*...*/ }
//[Serializable] public class StringStringArrayDictionary : StringKeyDictionary<string[]> { /*...*/ }
[Serializable] public class StringStringArrayDictionary : StringKeyDictionary<StringArray> { /*...*/ }
[Serializable] public class StringBoolDictionary : StringKeyDictionary<bool> { /*...*/ }
[Serializable] public class StringIntDictionary : StringKeyDictionary<int> { /*...*/ }
[Serializable] public class StringFloatDictionary : StringKeyDictionary<float> { /*...*/ }
[Serializable] public class StringVector2Dictionary : StringKeyDictionary<Vector2> { /*...*/ }
[Serializable] public class StringVector3Dictionary : StringKeyDictionary<Vector3> { /*...*/ }
[Serializable] public class StringQuaternionDictionary : StringKeyDictionary<Quaternion> { /*...*/ }
[Serializable] public class StringEulerRotationDictionary : StringKeyDictionary<EulerRotation> { /*...*/ }
[Serializable] public class StringAudioClipDictionary : StringKeyDictionary<AudioClip> { /*...*/ }
[Serializable] public class StringColorDictionary : StringKeyDictionary<Color> { /*...*/ }
[Serializable] public class StringTransformDictionary : StringKeyDictionary<Transform> { /*...*/ }
[Serializable] public class StringRendererDictionary : StringKeyDictionary<Renderer> { /*...*/ }
[Serializable] public class StringSkinnedMeshRendererDictionary : StringKeyDictionary<SkinnedMeshRenderer> { /*...*/ }
[Serializable] public class StringMaterialDictionary : StringKeyDictionary<Material> { /*...*/ }

// Int Key Dictionaries:
[Serializable] public class IntStringDictionary : SerializableDictionary<int, string> { /*...*/ }
[Serializable] public class IntFloatDictionary : SerializableDictionary<int, float> { /*...*/ }

// Float Key Dictionaries:
[Serializable] public class FloatStringDictionary : SerializableDictionary<float, string> { /*...*/ }
[Serializable] public class FloatIntDictionary : SerializableDictionary<float, int> { /*...*/ }
[Serializable] public class FloatColorDictionary : SerializableDictionary<float, Color> { /*...*/ }

// GameObjectTag Key Dictionaries:
[Serializable] public class TagIndexDictionary : SerializableDictionary<GameObjectTag, CollectionIndex> { /*...*/ }
[Serializable] public class TagIntDictionary : SerializableDictionary<GameObjectTag, int> { /*...*/ }
[Serializable] public class TagCollectionIndexDictionary : SerializableDictionary<GameObjectTag, CollectionIndex> { /*...*/ }

}