using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomPropertyDrawer(typeof(StringStringDictionary))]
[RequireComponent(typeof(StringStringArrayDictionary))]
[CustomPropertyDrawer(typeof(StringIntDictionary))]
[CustomPropertyDrawer(typeof(StringFloatDictionary))]
[CustomPropertyDrawer(typeof(StringAudioClipDictionary))]
public class SerializableDictionaryDrawer : VPropertyDrawer
{
	private const float RADIUS_REMOVE_BUTTON = 0.2f;
	private const float RADIUS_FIELD_KEY = 0.4f;
	private const float RADIUS_FIELD_VALUE = 0.4f;
	private const float WIDTH_MIN_REMOVE_BUTTON = 80.0f;
	private const float WIDTH_MIN_FIELD_KEY = 90.0f;
	private const float WIDTH_MIN_FIELD_VALUE = 90.0f;

	private SerializedProperty keys;
	private SerializedProperty values;

	/// <summary>Overrides OnGUI method.</summary>
	/// <param name="_position">Rectangle on the screen to use for the property GUI.</param>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The Label of this Property.</param>
	public override void OnGUI (Rect _position, SerializedProperty _property, GUIContent _label)
	{
		BeginPropertyDrawing(_position, _property, _label);

		//AddVerticalSpace();
		positionRect.height = SPACE_VERTICAL;
		_property.isExpanded = EditorGUI.Foldout(positionRect, _property.isExpanded, _label);
		
		if(_property.isExpanded)
		{
			AddIndent();
			AddVerticalSpace();
			positionRect.height = SPACE_VERTICAL;
			VEditorGUI.ShowDictionarySerializedPropertySizeConfiguration(ref keys, ref values, positionRect.WithHeight(EditorGUIUtility.singleLineHeight));
			AddVerticalSpace(SPACE_VERTICAL * 2.0f);

			int dictionarySize = Mathf.Min(keys.arraySize, values.arraySize);
			float fieldWidth = Mathf.Max((positionRect.width * RADIUS_FIELD_KEY), WIDTH_MIN_FIELD_KEY);
			float buttonWidth = Mathf.Max(positionRect.width * RADIUS_REMOVE_BUTTON, WIDTH_MIN_REMOVE_BUTTON);
			float halfHorizontalSpace = (SPACE_HORIZONTAL * 0.5f);
			Rect keyRect = new Rect(positionRect.x, positionRect.y, (fieldWidth - halfHorizontalSpace), EditorGUIUtility.singleLineHeight);
			Rect valueRect = keyRect.WithAddedX(fieldWidth - halfHorizontalSpace);
			Rect buttonRect = keyRect;
			buttonRect.x += (fieldWidth * 2.0f);
			buttonRect.width = buttonWidth - halfHorizontalSpace;
			
			if(dictionarySize > 0)
			{
				EditorGUI.LabelField(keyRect, "Key: ");
				EditorGUI.LabelField(valueRect, "Value: ");

				for(int i = 0; i < dictionarySize; i++)
				{
					AddVerticalSpace();
					keyRect.y += SPACE_VERTICAL;
					valueRect.y += SPACE_VERTICAL;
					buttonRect.y += SPACE_VERTICAL;
					SerializedProperty key = keys.GetArrayElementAtIndex(i);
					EditorGUI.PropertyField(keyRect, key, new GUIContent());
					SerializedProperty value = values.GetArrayElementAtIndex(i);
					EditorGUI.PropertyField(valueRect, value, new GUIContent());
					if(GUI.Button(buttonRect, "Remove"))
					{
						keys.DeleteArrayElementAtIndex(i);
						values.DeleteArrayElementAtIndex(i);
					}
				}
			}
		}	
		
		EndPropertyDrawing(_property);
	}

	public override float GetPropertyHeight(SerializedProperty _property, GUIContent label)
	{
		float height = 0.0f;
		keys = _property.FindPropertyRelative("_keysList");
		values = _property.FindPropertyRelative("_valuesList");

		height = SPACE_VERTICAL;

		if(_property.isExpanded)
		{
			height += SPACE_VERTICAL * 3.0f;

			for(int i = 0; i < keys.arraySize; i++)
			{
				height += Mathf.Max(
					EditorGUI.GetPropertyHeight(keys, true),
					EditorGUI.GetPropertyHeight(values, true)
				);
			}

			height += SPACE_VERTICAL;
		}	

		return height;
	}
}
}