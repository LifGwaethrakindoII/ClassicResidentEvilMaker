using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;

namespace Voidless
{
public static class VEditorGUILayout
{
	private static readonly string ELEMENT = "Element ";
	private static readonly string SIZE = "Size: ";

	/// <summary>Repeats EditorGUILayout.Space's call n times.</summary>
	/// <param name="_spaces">Times to repeat EditorGUILayout.Space call. The value is internally clamped between 1 and infinity.</param>
	public static void Spaces(int _spaces)
	{
		_spaces = Mathf.Clamp(_spaces, 1, int.MaxValue);

		for(int i = 0; i < _spaces; i++)
		{
			EditorGUILayout.Space();	
		}
	}

	public static int IntSlider(string label, int x, int min, int max)
	{
		return (int)EditorGUILayout.Slider(label, x, min, max);
	}

	public static void ShowArraySizeConfiguration<T>(this T[] _array, string _label = null)
	{
		int arraySize = _array.Length;
		int newSize = Mathf.Max(0, EditorGUILayout.DelayedIntField(_label + "Size: ", arraySize));
		int difference = (newSize - arraySize);

		if(difference != 0) Array.Resize(ref _array, newSize);
	}

	public static void ShowArraysSizeConfiguration<T>(string _label = null, params T[][] _arrays)
	{
		int maxArraySize = VArray.GetArrayMaxLength(_arrays);
		int newSize = Mathf.Max(0, EditorGUILayout.DelayedIntField(_label + "Size: ", maxArraySize));
		int difference = newSize - maxArraySize;

		if(difference != 0)
		for(int i = 0; i < _arrays.Length; i++)
		{
			T[] array = _arrays[i];
			if(array == null) array = new T[newSize];
			Array.Resize(ref array, newSize);
		}
	}

	public static void ShowListSizeConfiguration<T>(ref List<T> _list, string _label = null)
	{
		if(_list == null) _list = new List<T>();

		int collectionSize = _list.Count;
		int newSize = Mathf.Max(0, EditorGUILayout.DelayedIntField(_label + "Size: ", collectionSize));
		int difference = (newSize - collectionSize);

		if(difference != 0)	ResizeList(ref _list, newSize);
	}

	public static void ShowListsSizeConfiguration<T>(string _label = null, params List<T>[] _lists)
	{
		int maxListSize = VList.GetListsMaxLength(_lists);
		int newSize = Mathf.Max(0, EditorGUILayout.DelayedIntField(_label + "Size: ", maxListSize));
		int difference = (newSize - maxListSize);

		if(difference != 0)
		for(int i = 0; i < _lists.Length; i++)
		{
			List<T> list = _lists[i];
			if(list == null) list = new List<T>();
			ResizeList(ref list, newSize);
		}
	}

	public static void ResizeList<T>(ref List<T> _list, int _newSize)
	{
		int collectionSize = _list.Count;
		int difference = _newSize - collectionSize;

		while (_newSize < _list.Count) _list.RemoveAt( _list.Count - 1 );
		while (_newSize > _list.Count) _list.Add(default(T));

		if(difference != 0)
		{
			T lastElement = _list[collectionSize - 1];

			for(int i = collectionSize - 1; i < _newSize; i++)
			{
				_list.Add(lastElement);
			}
		}
	}

	public static void ShowDicitonarySizeConfiguration<K, V>(ref SerializableDictionary<K, V> _dictionary, string _label = null)
	{
		if(_dictionary.keysList == null) _dictionary.keysList = new List<K>();
		if(_dictionary.valuesList == null) _dictionary.valuesList = new List<V>();

		List<K> keysList = _dictionary.keysList;
		List<V> valuesList = _dictionary.valuesList;
		int keysSize = _dictionary.keysList.Count;
		int valuesSize = _dictionary.valuesList.Count;
		int bestSize = Mathf.Min(keysSize, valuesSize);
		int newSize = Mathf.Max(EditorGUILayout.DelayedIntField(_label + "Size: ", bestSize));
		int difference = (newSize - bestSize);

		if(difference != 0)
		{
			while (newSize < keysList.Count) keysList.RemoveAt( keysList.Count - 1 );
			while (newSize > keysList.Count) keysList.Add(default(K));
			while (newSize < valuesList.Count) valuesList.RemoveAt( valuesList.Count - 1 );
			while (newSize > valuesList.Count) valuesList.Add(default(V));

			if(difference > 0)
			{
				K lastKey = keysList[keysSize - 1];
				V lastValue = valuesList[valuesSize - 1];

				for(int i = keysSize - 1; i < newSize; i++)
				{
					keysList.Add(lastKey);
				}
				for(int i = valuesSize - 1; i < newSize; i++)
				{
					valuesList.Add(lastValue);
				}
			}
		}
	}

	public static LayerMask LayerMaskField(string _label, LayerMask _layerMask)
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(_label);
		LayerMask tempMask = EditorGUILayout.MaskField( InternalEditorUtility.LayerMaskToConcatenatedLayersMask(_layerMask), InternalEditorUtility.layers);
		_layerMask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);
		EditorGUILayout.EndHorizontal();

		return _layerMask;
	}

	public static void ShowSerializedPropertySizeConfiguration(ref SerializedProperty _property, string _label = null)
	{
		if(!_property.isArray) return;
		int arraySize = _property.arraySize;
		int newSize = Mathf.Max(0, EditorGUILayout.DelayedIntField(_label + "Size: ", arraySize));
		int difference = (newSize - arraySize);

		if(difference != 0) _property.arraySize = newSize;
		_property.serializedObject.ApplyModifiedProperties();
	}

	public static void ShowStringStringDictionary(ref SerializableDictionary<string, string> _dictionary)
	{
		ShowDicitonarySizeConfiguration<string, string>(ref _dictionary);
		List<string> keys = _dictionary.keysList;
		List<string> values = _dictionary.valuesList;
		int size = Mathf.Min(keys.Count, values.Count);

		for(int i = 0; i < size; i++)
		{
			string originalKey = keys[i];
			EditorGUILayout.BeginHorizontal();
			keys[i] = EditorGUILayout.DelayedTextField("Key: ", keys[i]);
			values[i] = EditorGUILayout.DelayedTextField("Value: ", values[i]);
			if(GUILayout.Button("Remove"))
			{
				keys.RemoveAt(i);
				values.RemoveAt(i);
			}
			EditorGUILayout.EndHorizontal();
		}

		_dictionary.keysList = keys;
		_dictionary.valuesList = values;
	}

#region ListFields:
	/// <summary>Serializes List of class inheriting from UnityEngine.Object on the Inspector's GUI.</summary>
	/// <param name="_label">Label of the foldout.</param>
	/// <param name="_list">List to serialize.</param>
	/// <param name="_foldout">Foldout condition.</param>
	/// <returns>List given as parameter.</returns>
	public static List<T> ListField<T>(string _label, List<T> _list, bool _foldout) where T : UnityEngine.Object
	{
		if(_foldout = EditorGUILayout.Foldout(_foldout, _label))
		{
			int newCount = Mathf.Max(0, EditorGUILayout.IntField(SIZE, _list.Count));

			while (newCount < _list.Count) _list.RemoveAt( _list.Count - 1 );
			while (newCount > _list.Count) _list.Add(null);

			if(_list.Count > 0)
			{
				for(int i = 0; i < _list.Count; i++)
				{
					EditorGUILayout.Space();
					_list[i] = EditorGUILayout.ObjectField(ELEMENT + i + ": ", _list[i], typeof(T), true) as T;
				}
			}
		}

		EditorGUILayout.Space();
		return _list;
	}
#endregion

#region ArrayFields:
	/// <summary>Serializes Array of class inheriting from UnityEngine.Object on the Inspector's GUI.</summary>
	/// <param name="_label">Label of the foldout.</param>
	/// <param name="_array">Array to serialize.</param>
	/// <param name="_foldout">Foldout condition.</param>
	/// <returns>Array given as parameter.</returns>
	public static T[] ArrayField<T>(string _label, T[] _array, bool _foldout) where T : UnityEngine.Object
	{
		if(_foldout = EditorGUILayout.Foldout(_foldout, _label))
		{
			/// \TODO Check Arrays' equivalent to list methods.
			/*int newCount = Mathf.Max(0, EditorGUILayout.IntField(SIZE, _list.Count));

			while (newCount < _list.Count) _list.RemoveAt( _list.Count - 1 );
			while (newCount > _list.Count) _list.Add(null);*/

			if(_array.Length > 0)
			{
				for(int i = 0; i < _array.Length; i++)
				{
					EditorGUILayout.Space();
					_array[i] = EditorGUILayout.ObjectField(ELEMENT + i + ": ", _array[i], typeof(T), true) as T;
				}
			}
		}

		EditorGUILayout.Space();
		return _array;
	}
#endregion

	public static void DrawDictionary<K, V>(ref Dictionary<K, V> _dictionary)
	{
		if(_dictionary == null) _dictionary = new Dictionary<K, V>();
		for(int i = 0; i < _dictionary.Count; i++)
		{
			KeyValuePair<K, V> pair = _dictionary.ElementAt(i);
			V value = default(V);

			EditorGUILayout.BeginHorizontal();
			pair.Key.DrawTypeField("Key: ");
			value = pair.Value.DrawTypeField("Value: ");
			EditorGUILayout.EndHorizontal();
			_dictionary[pair.Key] = value;
		}
	}

	public static T DrawTypeField<T>(this T _object, string _label = null)
	{
		if(typeof(T) == typeof(int))
		{
			int value = (int)(object)(_object);
			return (T)(object)EditorGUILayout.DelayedIntField(_label, value);
		}
		else if(typeof(T) == typeof(string))
		{
			string value = (string)(object)(_object);
			return (T)(object)EditorGUILayout.DelayedTextField(_label, value);
		}
		return default(T);
	}

	/// \TODO Make methods to serialize List/Arrays of all TypeFields defined on EditorGUILayout's API.
}
}