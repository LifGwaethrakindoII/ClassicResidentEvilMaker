using System;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomPropertyDrawer(typeof(PrimitiveUnion))]
public class PrimitiveUnionDrawer : VPropertyDrawer
{
	private SerializedProperty type;
	private SerializedProperty data;
	//private string dataLabel;

	/// <summary>Overrides OnGUI method.</summary>
	/// <param name="_position">Rectangle on the screen to use for the property GUI.</param>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The Label of this Property.</param>
	public override void OnGUI (Rect _position, SerializedProperty _property, GUIContent _label)
	{
		BeginPropertyDrawing(_position, _property, _label);

		float displacement = SPACE_HORIZONTAL + labelWidth;
		positionRect.width = labelWidth;
		positionRect.x += displacement;
		if(type != null) EditorGUI.PropertyField(positionRect, type, new GUIContent());
		positionRect.x -= displacement;
		positionRect.y += SPACE_VERTICAL;
		if(data != null) EditorGUI.PropertyField(positionRect, data, new GUIContent());

		EndPropertyDrawing(_property);
	}

	/// <summary>Override this method to specify how tall the GUI for this field is in pixels.</summary>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The label of this property.</param>
	public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
	{
		type = _property.FindPropertyRelative("type");
		data = _property.FindPropertyRelative("stringData");

		if(type != null)
		switch((DataType)type.enumValueIndex)
		{
			/*case DataType.String:
			dataLabel = "String Data";
			data = _property.FindPropertyRelative("stringData");
			break;

			case DataType.Character:
			dataLabel = "Character Data";
			data = _property.FindPropertyRelative("charData");
			break;

			case DataType.Integer:
			dataLabel = "Integer Data";
			data = _property.FindPropertyRelative("intData");
			break;

			case DataType.Float:
			data = _property.FindPropertyRelative("floatData");
			break;

			case DataType.Boolean:
			data = _property.FindPropertyRelative("boolData");
			break;*/

			default:
			data = null;
			break;
		}

		return SPACE_VERTICAL * 2.0f;
	}
}
}