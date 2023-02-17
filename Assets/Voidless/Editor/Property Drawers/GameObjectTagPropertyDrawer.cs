using System;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomPropertyDrawer(typeof(GameObjectTag))]
public class GameObjectTagPropertyDrawer : VPropertyDrawer
{
	private SerializedProperty tagProperty; 	/// <summary>Tag's Serialized Property.</summary>
	private SerializedProperty indexProperty; 	/// <summary>Index's Serialized Property.</summary>

	/// <summary>Overrides OnGUI method.</summary>
	/// <param name="_position">Rectangle on the screen to use for the property GUI.</param>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The Label of this Property.</param>
	public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
	{
		BeginPropertyDrawing(_position, _property, _label);

		string[] tags = UnityEditorInternal.InternalEditorUtility.tags;
		
		positionRect.x += labelWidth;
		positionRect.width = labelWidth;
		indexProperty.intValue = EditorGUI.Popup(positionRect, indexProperty.intValue, tags);
		tagProperty.stringValue = tags[indexProperty.intValue];
		EndPropertyDrawing(_property);
	}

	/// <summary>Override this method to specify how tall the GUI for this field is in pixels.</summary>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The label of this property.</param>
	public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
	{
		tagProperty = _property.FindPropertyRelative("_tag");
		indexProperty = _property.FindPropertyRelative("index");

		return SPACE_VERTICAL;
		return base.GetPropertyHeight(_property, _label);
	}
}
}