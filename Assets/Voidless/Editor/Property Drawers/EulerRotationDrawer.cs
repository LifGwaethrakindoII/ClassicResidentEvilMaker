using System;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomPropertyDrawer(typeof(EulerRotation))]
public class EulerRotationDrawer : VPropertyDrawer
{
	private const float RATIO_WIDTH_LABEL = 0.2f;
	private const float RATIO_WIDTH_FIELD = 0.4f;

	private SerializedProperty eulerAngles;
	private SerializedProperty rotation;

	/// <summary>Overrides OnGUI method.</summary>
	/// <param name="_position">Rectangle on the screen to use for the property GUI.</param>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The Label of this Property.</param>
	public override void OnGUI (Rect _position, SerializedProperty _property, GUIContent _label)
	{
		BeginPropertyDrawing(_position, _property, _label);
		
		float fieldWidth = ((layoutWidth - (SPACE_HORIZONTAL * 2.0f)) * RATIO_WIDTH_FIELD);
		float textWidth = ((layoutWidth - (SPACE_HORIZONTAL * 2.0f)) * RATIO_WIDTH_LABEL);
		float horizontalDisplacement = fieldWidth + SPACE_HORIZONTAL;

		positionRect.width = textWidth;
		AddVerticalSpace();
		EditorGUI.LabelField(positionRect, "Euler Angles: ");
		positionRect.x += textWidth + SPACE_HORIZONTAL;
		positionRect.width = fieldWidth;
		EditorGUI.PropertyField(positionRect, eulerAngles, new GUIContent());
		positionRect.x += horizontalDisplacement;
		EditorGUI.LabelField(positionRect, "Quaternion: " + rotation.quaternionValue.ToString());
		EndPropertyDrawing(_property);
	}

	/// <summary>Override this method to specify how tall the GUI for this field is in pixels.</summary>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The label of this property.</param>
	public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
	{
		eulerAngles = _property.FindPropertyRelative("_eulerAngles");
		rotation = _property.FindPropertyRelative("_rotation");
		return SPACE_VERTICAL * 2.0f;
	}
}
}