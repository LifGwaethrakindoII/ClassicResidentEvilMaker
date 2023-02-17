using System;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomPropertyDrawer(typeof(LayerValue))]
public class LayerValueDrawer : VPropertyDrawer
{
	private const float WIDTH_FIELD = 100.0f;

	private SerializedProperty value;
	private SerializedProperty name;
	private SerializedProperty index;
	private string[] layers;

	/// <summary>Overrides OnGUI method.</summary>
	/// <param name="_position">Rectangle on the screen to use for the property GUI.</param>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The Label of this Property.</param>
	public override void OnGUI (Rect _position, SerializedProperty _property, GUIContent _label)
	{
		layers = VLayerMask.GetRegisteredLayerMasks().ToArray();
		BeginPropertyDrawing(_position, _property, _label);
		float width = (labelWidth * 0.5f);
		positionRect.width = width;
		positionRect.height = EditorGUIUtility.singleLineHeight;
		positionRect.x += labelWidth + SPACE_HORIZONTAL;
		index.intValue = EditorGUI.Popup(positionRect, index.intValue, layers);
		name.stringValue = layers[index.intValue];
		name.serializedObject.ApplyModifiedProperties();
		positionRect.x += width + SPACE_HORIZONTAL;
		EditorGUI.LabelField(positionRect, "Value: " + value.intValue.ToString());
		EndPropertyDrawing(_property);
	}

	/// <summary>Override this method to specify how tall the GUI for this field is in pixels.</summary>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The label of this property.</param>
	public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
	{
		value = _property.FindPropertyRelative("_value");
		name = _property.FindPropertyRelative("_name");
		index = _property.FindPropertyRelative("index");
		return SPACE_VERTICAL;
	}
}
}