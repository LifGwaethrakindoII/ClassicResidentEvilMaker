using System;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomPropertyDrawer(typeof(AngleDotProduct))]
public class AngleDotProductDrawer : VPropertyDrawer
{
	private SerializedProperty angle;
	private SerializedProperty dotProduct;

	/// <summary>Overrides OnGUI method.</summary>
	/// <param name="_position">Rectangle on the screen to use for the property GUI.</param>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The Label of this Property.</param>
	public override void OnGUI (Rect _position, SerializedProperty _property, GUIContent _label)
	{
		BeginPropertyDrawing(_position, _property, _label);
		float width = (labelWidth * 0.5f);
		positionRect.width = width * 0.5f;
		positionRect.x += width + SPACE_HORIZONTAL;
		//angle.floatValue = EditorGUI.Slider(positionRect, "Angle: ", angle.floatValue, 0.0f, 180.0f);
		EditorGUI.PropertyField(positionRect, angle, new GUIContent());
		positionRect.width = width * 1.5f;
		positionRect.x += width + SPACE_HORIZONTAL;
		EditorGUI.LabelField(positionRect, "Dot Product: " + dotProduct.floatValue.ToString("0.00"));
		EndPropertyDrawing(_property);
	}

	/// <summary>Override this method to specify how tall the GUI for this field is in pixels.</summary>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The label of this property.</param>
	public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
	{
		angle = _property.FindPropertyRelative("_angle");
		dotProduct = _property.FindPropertyRelative("_dotProduct");
		return SPACE_VERTICAL;
	}
}
}