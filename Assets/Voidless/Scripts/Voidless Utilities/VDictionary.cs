using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

namespace Voidless
{
public static class VDictionary
{
#region Dictionaries:
	/// <returns>Random Dictionary Element.</returns>
	public static KeyValuePair<K, V> RandomElement<K, V>(this Dictionary<K, V> _dictionary)
	{
		return _dictionary.ElementAt(Random.Range(0, _dictionary.Count));
	}
	
	/// <returns>Random Dictionary Key.</returns>
	public static K RandomKey<K, V>(this Dictionary<K, V> _dictionary)
	{
		return _dictionary.RandomElement().Key;
	}

	/// <returns>Random Dictionary Value.</returns>
	public static V RandomEntry<K, V>(this Dictionary<K, V> _dictionary)
	{
		return _dictionary.RandomElement().Value;
	}
	
	/// <summary>Initializes Dictionary from Collection of SerializableKeyValuePair of the same KeyValuePair.</summary>
	/// <param name="_dictionary">Dictionary to initialize.</param>
	/// <param name="_SerializableKeyValuePairs">Collection of SerializableKeyValuePair of the same KeyValuePair as the SerializableKeyValuePair.</param>
	/// <param name="onInitializationEnds">[Optional] Action to invoke after the Initialization ends.</param>
	/// <returns>Initialized Dictionary.</returns>
	public static Dictionary<T, U> InitializeFrom<T, U>(this Dictionary<T, U> _dictionary, List<SerializableKeyValuePair<T, U>> _SerializableKeyValuePairs, System.Action onInitializationEnds)
	{
		_dictionary = new Dictionary<T, U>();

		for(int i = 0; i < _SerializableKeyValuePairs.Count; i++)
		{
			_dictionary.Add(_SerializableKeyValuePairs[i].key, _SerializableKeyValuePairs[i].value);
		}

		if(onInitializationEnds != null) onInitializationEnds();

		return _dictionary;
	}

	/// <summary>Initializes Dictionary from Collection of SerializableKeyValuePair of the same KeyValuePair.</summary>
	/// <param name="_dictionary">Dictionary to initialize.</param>
	/// <param name="_SerializableKeyValuePairs">Collection of SerializableKeyValuePair of the same KeyValuePair as the SerializableKeyValuePair.</param>
	/// <param name="onInitializationEnds">[Optional] Action to invoke after the Initialization ends.</param>
	/// <returns>Initialized Dictionary.</returns>
	public static Dictionary<T, U> InitializeFrom<T, U>(this Dictionary<T, U> _dictionary, SerializableKeyValuePair<T, U>[] _SerializableKeyValuePairs, System.Action onInitializationEnds)
	{
		_dictionary = new Dictionary<T, U>();

		for(int i = 0; i < _SerializableKeyValuePairs.Length; i++)
		{
			_dictionary.Add(_SerializableKeyValuePairs[i].key, _SerializableKeyValuePairs[i].value);
		}

		if(onInitializationEnds != null) onInitializationEnds();

		return _dictionary;
	}

	/// <summary>Deletes entries if their values are already registered under another key.</summary>
	/// <param name="_dictionary">Target Dictionary.</param>
	/// <returns>Dictionary with duplicated value entries deleted.</returns>
	public static Dictionary<K, V> DeleteDuplicateValueEntries<K, V>(this Dictionary<K, V> _dictionary)
	{
		HashSet<V> registered = new HashSet<V>();
		List<K> keys = new List<K>();

		foreach(KeyValuePair<K, V> pair in _dictionary)
		{
			if(registered.Contains(pair.Value)) keys.Add(pair.Key);
			registered.Add(pair.Value);
		}

		foreach(K key in keys)
		{
			_dictionary.Remove(key);
		}

		return _dictionary;
	}
#endregion
}
}