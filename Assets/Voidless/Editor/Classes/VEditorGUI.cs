using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
public static class VEditorGUI
{
	/// <summary>Shows Serialized Property configurations for size, if it is a collection.</summary>
	/// <param name="_property">Serialized Property.</param>
	/// <param name="_rect">Positions' Rect.</param>
	/// <param name="_label">Optional additional label.</param>
	public static void ShowSerializedPropertySizeConfiguration(ref SerializedProperty _property, Rect _rect, string _label = null)
	{
		if(!_property.isArray) return;
		int arraySize = _property.arraySize;
		int newSize = Mathf.Max(0, EditorGUI.DelayedIntField(_rect, _label + "Size: ", arraySize));
		int difference = (newSize - arraySize);

		if(difference != 0) _property.arraySize = newSize;
		_property.serializedObject.ApplyModifiedProperties();
	}

	/// <summary>Shows Serialized Property configurations for size in a SerializableDictionary's keys and values SerializedProperties.</summary>
	/// <param name="_keys">Keys' Serialized Property.</param>
	/// <param name="_values">Values' Serialized Property.</param>
	/// <param name="_rect">Positions' Rect.</param>
	/// <param name="_label">Optional additional label.</param>
	public static void ShowDictionarySerializedPropertySizeConfiguration(ref SerializedProperty _keys, ref SerializedProperty _values, Rect _rect, string _label = null)
	{
		if(!_keys.isArray || !_values.isArray) return;
		int bestSize = Mathf.Max(_keys.arraySize, _values.arraySize);
		int newSize = (Mathf.Max(0, EditorGUI.DelayedIntField(_rect, _label + "Size: ", bestSize)));
		int difference = (newSize - bestSize);

		if(difference != 0)
		{
			_keys.arraySize = newSize;
			_values.arraySize = newSize;
		}

		_keys.serializedObject.ApplyModifiedProperties();
		_values.serializedObject.ApplyModifiedProperties();
	}
}
}