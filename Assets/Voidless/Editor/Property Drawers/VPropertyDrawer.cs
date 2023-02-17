using System;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
public abstract class VPropertyDrawer : PropertyDrawer
{
	protected const float SPACE_VERTICAL = 20.0f; 		/// <summary>Default Vertical Space.</summary>
	protected const float SPACE_HORIZONTAL = 10.0f; 	/// <summary>Default Horizontal Space.</summary>
	protected const float WIDTH_TEXT = 6.0f; 			/// <summary>Text's Width.</summary>

	protected Rect positionRectState; 					/// <summary>Posisiton's Rect State.</summary>
	protected Rect positionRect; 						/// <summary>Position's Rect.</summary>
	protected int indent; 								/// <summary>Indent's Reference.</summary>
	protected float labelWidth; 						/// <summary>Label's Width.</summary>
	protected float layoutWidth; 						/// <summary>Layout's Width.</summary>

	public virtual float space { get { return SPACE_VERTICAL; } }

	/// <summary>Overrides OnGUI method.</summary>
	/// <param name="_position">Rectangle on the screen to use for the property GUI.</param>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The Label of this Property.</param>
	public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
	{
		BeginPropertyDrawing(_position, _property, _label);
		EndPropertyDrawing(_property);
	}

	/// <summary>Begins Property Drawing.</summary>
	/// <param name="_position">Rectangle on the screen to use for the property GUI.</param>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The Label of this Property.</param>
	protected virtual void BeginPropertyDrawing(Rect _position, SerializedProperty _property, GUIContent _label)
	{
		layoutWidth = _position.width;
		positionRect = _position;
		positionRect.height = EditorGUIUtility.singleLineHeight;
		EditorGUIUtility.labelWidth = positionRect.width * 0.5f;
		_label = EditorGUI.BeginProperty(positionRect, _label, _property);
		labelWidth = EditorGUIUtility.labelWidth;
		EditorGUI.PrefixLabel(positionRect, GUIUtility.GetControlID(FocusType.Passive), _label);
		indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;
	}

	protected void AddIndent()
	{
		EditorGUI.indentLevel++;
	}

	protected void RemoveIndent()
	{
		EditorGUI.indentLevel = Mathf.Max(EditorGUI.indentLevel--, 0);
	}

	/// <summary>Ends Property Drawing. Reseting both position and indent values.</summary>
	protected virtual void EndPropertyDrawing(SerializedProperty _property, bool _applyModifiedProperties = true)
	{
		if(_applyModifiedProperties) _property.serializedObject.ApplyModifiedProperties();
		EditorGUI.indentLevel = indent;
		EditorGUI.EndProperty();
	}

	/// <summary>Adds Values to Position's Rect.</summary>
	/// <param name="x">X value to add.</param>
	/// <param name="y">Y value to add.</param>
	/// <param name="width">Width value to add.</param>
	/// <param name="height">Height value to add.</param>
	/// <returns>Modified Position's Rect.</returns>
	protected virtual Rect AddToPositionRect(float x = 0.0f, float y = 0.0f, float width = 0.0f, float height = 0.0f)
	{
		positionRect.x += x;
		positionRect.y += y;
		positionRect.width += width;
		positionRect.height += height;

		return positionRect;
	}

	/// <summary>Adds Vertical Space (adds y value).</summary>
	/// <param name="y">Y value to add.</param>
	/// <returns>Modified Position's Rect.</returns>
	protected virtual Rect AddVerticalSpace(float y = SPACE_VERTICAL)
	{
		positionRect.y += y;

		return positionRect;
	}

	/// <summary>Adds Vertical Space (adds x value).</summary>
	/// <param name="x">X value to add.</param>
	/// <returns>Modified Position's Rect.</returns>
	protected virtual Rect AddHorizontalSpace(float x = SPACE_HORIZONTAL)
	{
		positionRect.x += x;

		return positionRect;
	}

	protected virtual void BeginHorizontalLayout()
	{
		positionRectState = positionRect;
	}

	protected virtual void EndHorizontalLayout()
	{
		positionRect.x = positionRectState.x;
	}
}
}