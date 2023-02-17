using System;
using UnityEditor;
using UnityEngine;

namespace Voidless
{
[CustomPropertyDrawer(typeof(Accelerable))]
public class AccelerablePropertyDrawer : VPropertyDrawer
{
	private const float RATIO_WIDTH = 0.25f;
	private const float WIDTH_LABEL_SPEED = 50.0f;

	private SerializedProperty deltaTimeCoefficient;
	private SerializedProperty minSpeed;
	private SerializedProperty maxSpeed;
	private SerializedProperty accelerationDuration;
	private SerializedProperty decelerationDuration;
	private SerializedProperty accelerationEquationType;
	private SerializedProperty decelerationEquationType;
	private SerializedProperty accelerationFunction;
	private SerializedProperty decelerationFunction;

	/// <summary>Overrides OnGUI method.</summary>
	/// <param name="_position">Rectangle on the screen to use for the property GUI.</param>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The Label of this Property.</param>
	public override void OnGUI (Rect _position, SerializedProperty _property, GUIContent _label)
	{
		BeginPropertyDrawing(_position, _property, _label);
		float width = (layoutWidth - (SPACE_HORIZONTAL * 3.0f)) * RATIO_WIDTH;
		float horizontalDisplacement = width + SPACE_HORIZONTAL;
		float initialX = positionRect.x;

		positionRect.width = width;
		AddVerticalSpace();
		EditorGUI.LabelField(positionRect, "Min. Speed: ");
		positionRect.x += horizontalDisplacement;
		EditorGUI.PropertyField(positionRect, minSpeed, new GUIContent());
		positionRect.x += horizontalDisplacement;
		EditorGUI.LabelField(positionRect, "Max. Speed: ");
		positionRect.x += horizontalDisplacement;
		EditorGUI.PropertyField(positionRect, maxSpeed, new GUIContent());
		positionRect.x = initialX;
		AddVerticalSpace();
		EditorGUI.LabelField(positionRect, "Acceleration: ");
		positionRect.x += horizontalDisplacement;
		EditorGUI.PropertyField(positionRect, accelerationDuration, new GUIContent());
		positionRect.x += horizontalDisplacement;
		EditorGUI.LabelField(positionRect, "Deceleration: ");
		positionRect.x += horizontalDisplacement;
		EditorGUI.PropertyField(positionRect, decelerationDuration, new GUIContent());
		positionRect.x = initialX;
		AddVerticalSpace();
		EditorGUI.LabelField(positionRect, "Acceleration Equation: ");
		positionRect.x += horizontalDisplacement;
		EditorGUI.PropertyField(positionRect, accelerationEquationType, new GUIContent());
		if((EquationType)accelerationEquationType.enumValueIndex == EquationType.NonLineal)
		{
			positionRect.width = labelWidth - SPACE_HORIZONTAL;
			positionRect.x += horizontalDisplacement;
			EditorGUI.PropertyField(positionRect, accelerationFunction, new GUIContent());
			positionRect.width = width;
		}
		positionRect.x = initialX;
		AddVerticalSpace();
		EditorGUI.LabelField(positionRect, "Deceleration Equation: ");
		positionRect.x += horizontalDisplacement;
		EditorGUI.PropertyField(positionRect, decelerationEquationType, new GUIContent());
		if((EquationType)decelerationEquationType.enumValueIndex == EquationType.NonLineal)
		{
			positionRect.width = labelWidth - SPACE_HORIZONTAL;
			positionRect.x += horizontalDisplacement;
			EditorGUI.PropertyField(positionRect, decelerationFunction, new GUIContent());
			positionRect.width = width;
		}
		AddVerticalSpace();
		positionRect.x = initialX;
		positionRect.width = labelWidth - SPACE_HORIZONTAL;
		EditorGUI.LabelField(positionRect, "Time Coefficient: ");
		positionRect.x += horizontalDisplacement;
		EditorGUI.PropertyField(positionRect, deltaTimeCoefficient, new GUIContent());

		EndPropertyDrawing(_property);
	}

	/// <summary>Override this method to specify how tall the GUI for this field is in pixels.</summary>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The label of this property.</param>
	public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
	{
		deltaTimeCoefficient = _property.FindPropertyRelative("_deltaTimeCoefficient");
		minSpeed = _property.FindPropertyRelative("_minSpeed");
		maxSpeed = _property.FindPropertyRelative("_maxSpeed");
		accelerationDuration = _property.FindPropertyRelative("_accelerationDuration");
		decelerationDuration = _property.FindPropertyRelative("_decelerationDuration");
		accelerationEquationType = _property.FindPropertyRelative("_accelerationEquationType");
		decelerationEquationType = _property.FindPropertyRelative("_decelerationEquationType");
		accelerationFunction = _property.FindPropertyRelative("_accelerationFunction");
		decelerationFunction = _property.FindPropertyRelative("_decelerationFunction");

		return SPACE_VERTICAL * 6.0f;
	}
}
}