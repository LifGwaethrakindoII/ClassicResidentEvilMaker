using System;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomPropertyDrawer(typeof(TransformData))]
public class TransformDataDrawer : VPropertyDrawer
{
	private const float LENGHT_SCALE_RAY = 50.0f;
	private const float RATIO_WIDTH_LABEL = 0.333333333f;
	private const float RATIO_WIDTH_FIELD = 0.666666666f;

	private SerializedProperty parent;
	private SerializedProperty position;
	private SerializedProperty rotation;
	private SerializedProperty eulerAngles;
	private SerializedProperty scale;
	private SerializedProperty hideScale;
	private SerializedProperty showForLinePath;
	private SerializedProperty showHandles;
	private Transform parentTransform;
	private bool subscribedToSceneGUI;

	/// <summary>Overrides OnGUI method.</summary>
	/// <param name="_position">Rectangle on the screen to use for the property GUI.</param>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The Label of this Property.</param>
	public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
	{
		position = _property.FindPropertyRelative("_position");
		rotation = _property.FindPropertyRelative("_rotation");
		eulerAngles = _property.FindPropertyRelative("_eulerAngles");
		scale = _property.FindPropertyRelative("_scale");
		hideScale = _property.FindPropertyRelative("hideScale");
		showForLinePath = _property.FindPropertyRelative("showForLinePath");
		showHandles = _property.FindPropertyRelative("showHandles");

		SceneView.onSceneGUIDelegate -= OnSceneGUI;
		SceneView.onSceneGUIDelegate += OnSceneGUI;

		float width = _position.width;
		float fieldDimension = width * RATIO_WIDTH_FIELD;
		float labelDimension = width * RATIO_WIDTH_LABEL;
		float horizontalDisplacement = labelDimension + SPACE_HORIZONTAL;

		BeginPropertyDrawing(_position, _property, _label);
		EditorGUIUtility.labelWidth = labelDimension;

		if(!showForLinePath.boolValue)
		{
			AddVerticalSpace();
			positionRect.width = labelDimension;
			EditorGUI.LabelField(positionRect, "Show Handles: ");
			positionRect.x += horizontalDisplacement;
			positionRect.width = fieldDimension;
			EditorGUI.PropertyField(positionRect, showHandles, new GUIContent());
			positionRect.x -= horizontalDisplacement;
		}
		AddVerticalSpace();
		positionRect.width = labelDimension;
		EditorGUI.LabelField(positionRect, "Position: ");
		positionRect.x += horizontalDisplacement;
		positionRect.width = fieldDimension;
		EditorGUI.PropertyField(positionRect, position, new GUIContent());
		positionRect.x -= horizontalDisplacement;
		AddVerticalSpace();
		positionRect.width = labelDimension;
		EditorGUI.LabelField(positionRect, "Euler Rotation: ");
		positionRect.x += horizontalDisplacement;
		positionRect.width = fieldDimension;
		EditorGUI.PropertyField(positionRect, eulerAngles, new GUIContent());
		positionRect.x -= horizontalDisplacement;
		AddVerticalSpace();
		EditorGUI.LabelField(positionRect, "Rotation: ");
		positionRect.x += horizontalDisplacement;
		EditorGUI.LabelField(positionRect, rotation.quaternionValue.ToString());
		positionRect.x -= horizontalDisplacement;
		
		if(!hideScale.boolValue)
		{
			AddVerticalSpace();
			positionRect.width = labelDimension;
			EditorGUI.LabelField(positionRect, "Scale: ");
			positionRect.x += horizontalDisplacement;
			positionRect.width = fieldDimension;
			EditorGUI.PropertyField(positionRect, scale, new GUIContent());
		}

		EndPropertyDrawing(_property);
	}

	/// <summary>Enables the Editor to handle an event in the Scene view.</summary>
	/// <param name="_view">Scene's View.</param>
	private void OnSceneGUI(SceneView _view)
	{
		try
		{
			if(showHandles == null || !showHandles.boolValue) return;

			switch(Tools.current)
			{
				case Tool.View:
				break;

				case Tool.Move:
				if(position == null) return;
				
				EditorGUI.BeginChangeCheck();
				Vector3 newPosition = Handles.PositionHandle(position.vector3Value, rotation.quaternionValue);
				if(EditorGUI.EndChangeCheck())
				{
					position.vector3Value = newPosition;
					position.serializedObject.ApplyModifiedProperties();
				}
				break;

				case Tool.Rotate:
				if(rotation == null) return;
				
				EditorGUI.BeginChangeCheck();
				Quaternion newRotation = Handles.RotationHandle(rotation.quaternionValue, position.vector3Value);
				if(EditorGUI.EndChangeCheck())
				{
					rotation.quaternionValue = newRotation;
					eulerAngles.vector3Value = newRotation.eulerAngles;
					eulerAngles.serializedObject.ApplyModifiedProperties();
					rotation.serializedObject.ApplyModifiedProperties();
				}
				break;

				case Tool.Scale:
				if(scale == null) return;
				
				EditorGUI.BeginChangeCheck();
				Vector3 newScale = Handles.ScaleHandle(scale.vector3Value, position.vector3Value, rotation.quaternionValue, scale.vector3Value.GetAverage() * LENGHT_SCALE_RAY);
				if(EditorGUI.EndChangeCheck())
				{
					scale.vector3Value = newScale;
					scale.serializedObject.ApplyModifiedProperties();
				}
				break;
			}
		}
		catch(Exception exception)
		{
			//Debug.Log("[TransformDataDrawer] Catched Exception when trying to reload SceneGUI: " + exception.Message);
		}	
	}

	/// <summary>Override this method to specify how tall the GUI for this field is in pixels.</summary>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The label of this property.</param>
	public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
	{
		hideScale = _property.FindPropertyRelative("hideScale");
		return SPACE_VERTICAL * (!hideScale.boolValue ? 6.0f : 5.0f);
	}
}
}