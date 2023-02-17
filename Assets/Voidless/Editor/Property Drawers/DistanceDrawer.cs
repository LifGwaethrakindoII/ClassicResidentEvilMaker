using System;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomPropertyDrawer(typeof(Distance))]
public class DistanceDrawer : VPropertyDrawer
{
	private SerializedProperty distance;
	private SerializedProperty squareDistance;

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
		EditorGUI.PropertyField(positionRect, distance, new GUIContent());
		positionRect.x += width + SPACE_HORIZONTAL;
		EditorGUI.LabelField(positionRect, "Square Distance: " + squareDistance.floatValue.ToString());
		EndPropertyDrawing(_property);
	}

	/// <summary>Override this method to specify how tall the GUI for this field is in pixels.</summary>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The label of this property.</param>
	public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
	{
		distance = _property.FindPropertyRelative("_distance");
		squareDistance = _property.FindPropertyRelative("_squareDistance");
		return SPACE_VERTICAL;
	}
}
}