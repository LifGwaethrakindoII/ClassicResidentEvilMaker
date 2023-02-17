using System;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomPropertyDrawer(typeof(FloatRange))]
[CustomPropertyDrawer(typeof(IntRange))]
public class RangeDrawer : VPropertyDrawer
{
	private const float RATIO_WIDTH_LABEL = 0.2f;
	private const float RATIO_WIDTH_FIELD = 0.4f;

	private SerializedProperty min;
	private SerializedProperty max;

	/// <summary>Overrides OnGUI method.</summary>
	/// <param name="_position">Rectangle on the screen to use for the property GUI.</param>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The Label of this Property.</param>
	public override void OnGUI (Rect _position, SerializedProperty _property, GUIContent _label)
	{
		BeginPropertyDrawing(_position, _property, _label);

		float updatedLabelWidth = (labelWidth - (SPACE_HORIZONTAL * 2.0f));
		float labelDimension = (updatedLabelWidth * RATIO_WIDTH_LABEL);
		float fieldWidth = (updatedLabelWidth * RATIO_WIDTH_FIELD);
		
		min = _property.FindPropertyRelative("_min");
		max = _property.FindPropertyRelative("_max");
		
		positionRect.x += labelWidth * 0.5f;
		positionRect.width = labelDimension;
		EditorGUI.LabelField(positionRect, "Min: ");
		positionRect.x += labelDimension + SPACE_HORIZONTAL;
		positionRect.width = fieldWidth;
		EditorGUI.PropertyField(positionRect, min, new GUIContent());
		positionRect.x += fieldWidth + SPACE_HORIZONTAL;
		EditorGUI.LabelField(positionRect, "Max: ");
		positionRect.x += labelDimension + SPACE_HORIZONTAL;
		positionRect.width = fieldWidth;
		EditorGUI.PropertyField(positionRect, max, new GUIContent());

		EndPropertyDrawing(_property);
	}

	/// <summary>Override this method to specify how tall the GUI for this field is in pixels.</summary>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The label of this property.</param>
	public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
	{
		min = _property.FindPropertyRelative("_min");
		max = _property.FindPropertyRelative("_max");

		return base.GetPropertyHeight(_property, _label);
	}
}
}