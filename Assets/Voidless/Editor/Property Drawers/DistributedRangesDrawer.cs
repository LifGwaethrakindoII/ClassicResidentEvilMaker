using System;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomPropertyDrawer(typeof(DistributedRangesDrawer))]
public class DistributedRangesDrawer : PropertyDrawer
{
	private SerializedProperty limits;
	private SerializedProperty ranges;
	private SerializedProperty range;
	private int rangeSize;

	/// <summary>Overrides OnGUI method.</summary>
	/// <param name="_position">Rectangle on the screen to use for the property GUI.</param>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The Label of this Property.</param>
	public override void OnGUI (Rect _position, SerializedProperty _property, GUIContent _label)
	{
		limits = _property.FindPropertyRelative("limits");
		ranges = _property.FindPropertyRelative("ranges");

		_label = EditorGUI.BeginProperty(_position, _label, _property);
		int indent = EditorGUI.indentLevel;

		

		EditorGUI.indentLevel = indent;
		EditorGUI.EndProperty();
	}

	/// <summary>Override this method to specify how tall the GUI for this field is in pixels.</summary>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The label of this property.</param>
	public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
	{
		float height = base.GetPropertyHeight(_property, _label);

		return height * (rangeSize + 2.0f);
	}
}
}