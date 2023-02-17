using System;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomPropertyDrawer(typeof(NormalizedVector3))]
[CustomPropertyDrawer(typeof(NormalizedVector2))]
public class NormalizedVectorDrawer : VPropertyDrawer
{
	private const float WIDTH_FIELD_COMPONENT_MIN = 130.0f;
	private const float WIDTH_FIELD_COMPONENT_MAX = 250.0f;
	private const float WIDTH_SLIDER_RANGE = 150.0f;
	private const float WIDTH_FIELD_LABEL = 50.0f;

	private SerializedProperty x;
	private SerializedProperty y;
	private SerializedProperty z;

	/// <summary>Overrides OnGUI method.</summary>
	/// <param name="_position">Rectangle on the screen to use for the property GUI.</param>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The Label of this Property.</param>
	public override void OnGUI (Rect _position, SerializedProperty _property, GUIContent _label)
	{
		BeginPropertyDrawing(_position, _property, _label);

		Rect fieldRect = positionRect;
		Rect labelRect = positionRect;

		fieldRect.height = EditorGUIUtility.singleLineHeight;
		labelRect.height = EditorGUIUtility.singleLineHeight;
		fieldRect.width = Mathf.Min(Mathf.Max(positionRect.width, WIDTH_FIELD_COMPONENT_MIN), Mathf.Abs(positionRect.width - WIDTH_FIELD_LABEL));
		labelRect.width = WIDTH_FIELD_LABEL;
		fieldRect.x += labelRect.width;
		AddVerticalSpace();
		fieldRect.y += SPACE_VERTICAL;
		labelRect.y += SPACE_VERTICAL;
		EditorGUI.LabelField(labelRect, "X: ");
		EditorGUI.PropertyField(fieldRect, x, new GUIContent());
		AddVerticalSpace();
		fieldRect.y += SPACE_VERTICAL;
		labelRect.y += SPACE_VERTICAL;
		EditorGUI.LabelField(labelRect, "Y: ");
		EditorGUI.PropertyField(fieldRect, y, new GUIContent());
		if(z != null)
		{
			AddVerticalSpace();
			fieldRect.y += SPACE_VERTICAL;
			labelRect.y += SPACE_VERTICAL;
			EditorGUI.LabelField(labelRect, "Z: ");
			EditorGUI.PropertyField(fieldRect, z, new GUIContent());
		}

		EndPropertyDrawing(_property);
	}

	public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
	{
		x = _property.FindPropertyRelative("_x");
		y = _property.FindPropertyRelative("_y");
		z = _property.FindPropertyRelative("_z");

		return SPACE_VERTICAL * (z != null ? 4.0f : 3.0f);
	}
}
}