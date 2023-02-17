using System;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomPropertyDrawer(typeof(Line))]
public class LineDrawer : VPropertyDrawer
{
	private const float HEIGHT_TRANSFORMDATA = SPACE_VERTICAL * 6.0f;
	public const float TIME_ITERATIONS = 30.0f;
	public const float TIME_FRACTION = 1.0f / TIME_ITERATIONS;

	private SerializedProperty type;
	private SerializedProperty a;
	private SerializedProperty b;
	private SerializedProperty point1;
	private SerializedProperty point2;
	private SerializedProperty showHandles;
	private SerializedProperty showForLinePath;
	private LineSegmentType typeEnum;

	/// <summary>Overrides OnGUI method.</summary>
	/// <param name="_position">Rectangle on the screen to use for the property GUI.</param>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The Label of this Property.</param>
	public override void OnGUI (Rect _position, SerializedProperty _property, GUIContent _label)
	{
		BeginPropertyDrawing(_position, _property, _label);

		type = _property.FindPropertyRelative("type");
		a = _property.FindPropertyRelative("a");
		b = _property.FindPropertyRelative("b");
		point1 = _property.FindPropertyRelative("point1");
		point2 = _property.FindPropertyRelative("point2");
		showHandles = _property.FindPropertyRelative("showHandles");
		showForLinePath = _property.FindPropertyRelative("showForLinePath");

		SceneView.onSceneGUIDelegate -= OnSceneGUI;
		if(!showForLinePath.boolValue)
		{
			SceneView.onSceneGUIDelegate += OnSceneGUI;

			AddVerticalSpace();
			EditorGUI.PropertyField(positionRect, showHandles);
		}

		AddVerticalSpace();
		EditorGUI.PropertyField(positionRect, type);
		AddVerticalSpace();
		EditorGUI.PropertyField(positionRect, a);
		AddVerticalSpace();
		EditorGUI.PropertyField(positionRect, b);
		AddVerticalSpace();

		typeEnum = (LineSegmentType)type.enumValueIndex;

		switch(typeEnum)
		{
			case LineSegmentType.CuadraticBeizer:
			AddVerticalSpace();
			EditorGUI.PropertyField(positionRect, point1);
			break;

			case LineSegmentType.CubicBeizer:
			AddVerticalSpace();
			EditorGUI.PropertyField(positionRect, point1);
			AddVerticalSpace();
			EditorGUI.PropertyField(positionRect, point2);
			break;
		}

		if(typeEnum != LineSegmentType.Linear)
		{
			AddVerticalSpace();
			AddVerticalSpace();
			positionRect.width = layoutWidth;

			if(GUI.Button(positionRect, "Center Middle-Points"))
			{
				switch(typeEnum)
				{
					case LineSegmentType.CuadraticBeizer:
					point1.vector3Value = Vector3.Lerp(a.vector3Value, b.vector3Value, 0.5f);
					break;

					case LineSegmentType.CubicBeizer:
					point1.vector3Value = Vector3.Lerp(a.vector3Value, b.vector3Value, 0.33f);
					point2.vector3Value = Vector3.Lerp(a.vector3Value, b.vector3Value, 0.66f);
					break;
				}
			}
		}

		EndPropertyDrawing(_property);
	}

	/// <summary>Enables the Editor to handle an event in the Scene view.</summary>
	/// <param name="_view">Scene's View.</param>
	private void OnSceneGUI(SceneView _view)
	{
		try
		{
			if((a == null || b == null) || (showHandles == null || !showHandles.boolValue)) return;

			Vector3 A = Vector3.zero;
			Vector3 B = Vector3.zero;
			Quaternion rotation = Quaternion.LookRotation(b.vector3Value - a.vector3Value);
			Color color = Handles.color;

			switch(Tools.current)
			{
				case Tool.Move:
				VHandles.PropertyPositionHandle(ref a, Quaternion.identity);
				VHandles.PropertyPositionHandle(ref b, Quaternion.identity);

				switch(typeEnum)
				{
					case LineSegmentType.Linear:
					Handles.DrawLine(a.vector3Value, b.vector3Value);
					break;

					case LineSegmentType.CuadraticBeizer:
					VHandles.PropertyPositionHandle(ref point1, rotation);

					A = VMath.CuadraticBeizer(a.vector3Value, b.vector3Value, point1.vector3Value, 0.0f);

					Handles.color = Handles.color.WithAlpha(color.a * 0.35f);
					Handles.DrawLine(a.vector3Value, point1.vector3Value);
					Handles.DrawLine(point1.vector3Value, b.vector3Value);
					Handles.color = color;

					for(float t = TIME_FRACTION; t < TIME_ITERATIONS + TIME_FRACTION; t += TIME_FRACTION)
					{
						B = VMath.CuadraticBeizer(a.vector3Value, b.vector3Value, point1.vector3Value, t);
						Handles.DrawLine(A, B);
						A = B;
					}
					break;

					case LineSegmentType.CubicBeizer:
					VHandles.PropertyPositionHandle(ref point1, rotation);
					VHandles.PropertyPositionHandle(ref point2, rotation);

					A = VMath.CubicBeizer(a.vector3Value, b.vector3Value, point1.vector3Value, point2.vector3Value, 0.0f);

					Handles.color = Handles.color.WithAlpha(color.a * 0.35f);
					Handles.DrawLine(a.vector3Value, point1.vector3Value);
					Handles.DrawLine(point1.vector3Value, point2.vector3Value);
					Handles.DrawLine(point2.vector3Value, b.vector3Value);
					Handles.color = color;

					for(float t = TIME_FRACTION; t < TIME_ITERATIONS + TIME_FRACTION; t += TIME_FRACTION)
					{
						B = VMath.CubicBeizer(a.vector3Value, b.vector3Value, point1.vector3Value, point2.vector3Value, t);
						Handles.DrawLine(A, B);
						A = B;
					}
					break;
				}	
				break;
			}
		}
		catch(Exception exception)
		{
			//Debug.Log("[LineDrawer] Catched Exception when trying to reload SceneGUI: " + exception.Message);
		}
	}

	/// <summary>Override this method to specify how tall the GUI for this field is in pixels.</summary>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The label of this property.</param>
	public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
	{
		float height = 0.0f;

		height += SPACE_VERTICAL * 6.0f;

		switch(typeEnum)
		{
			case LineSegmentType.CuadraticBeizer:
			height += SPACE_VERTICAL * 4.0f;
			break;

			case LineSegmentType.CubicBeizer:
			height += SPACE_VERTICAL * 6.0f;
			break;
		}

		return height;
		return base.GetPropertyHeight(_property, _label);
	}
}
}