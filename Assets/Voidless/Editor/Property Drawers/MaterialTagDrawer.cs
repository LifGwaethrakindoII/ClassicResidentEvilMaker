using System;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomPropertyDrawer(typeof(MaterialTag))]
[CustomPropertyDrawer(typeof(AnimatorCredential))]
public class MaterialTagDrawer : VPropertyDrawer
{
	private SerializedProperty tag;
	private SerializedProperty ID;

	/// <summary>Overrides OnGUI method.</summary>
	/// <param name="_position">Rectangle on the screen to use for the property GUI.</param>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The Label of this Property.</param>
	public override void OnGUI (Rect _position, SerializedProperty _property, GUIContent _label)
	{
		BeginPropertyDrawing(_position, _property, _label);
		float width = (labelWidth * 0.5f);
		positionRect.width = width;
		positionRect.x += labelWidth + SPACE_HORIZONTAL;
		EditorGUI.PropertyField(positionRect, tag, new GUIContent());
		positionRect.x += width + SPACE_HORIZONTAL;
		EditorGUI.LabelField(positionRect, "ID: " + ID.intValue.ToString());
		EndPropertyDrawing(_property);
	}

	/// <summary>Override this method to specify how tall the GUI for this field is in pixels.</summary>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The label of this property.</param>
	public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
	{
		tag = _property.FindPropertyRelative("_tag");
		ID = _property.FindPropertyRelative("_ID");
		return SPACE_VERTICAL;
	}
}
}