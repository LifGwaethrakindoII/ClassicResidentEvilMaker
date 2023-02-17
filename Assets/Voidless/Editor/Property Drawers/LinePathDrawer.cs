using System;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomPropertyDrawer(typeof(LinePath))]
public class LinePathDrawer : VPropertyDrawer
{
	private SerializedProperty lines;
	private SerializedProperty closed;
	private SerializedProperty showHandles;

	/// <summary>Overrides OnGUI method.</summary>
	/// <param name="_position">Rectangle on the screen to use for the property GUI.</param>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The Label of this Property.</param>
	public override void OnGUI (Rect _position, SerializedProperty _property, GUIContent _label)
	{
		BeginPropertyDrawing(_position, _property, _label);

		lines = _property.FindPropertyRelative("_lines");
		showHandles = _property.FindPropertyRelative("showHandles");

		SceneView.onSceneGUIDelegate -= OnSceneGUI;
		SceneView.onSceneGUIDelegate += OnSceneGUI;

		int length = lines.arraySize;

		AddVerticalSpace();
		EditorGUI.PropertyField(positionRect, showHandles);
		AddVerticalSpace();
		VEditorGUI.ShowSerializedPropertySizeConfiguration(ref lines, positionRect);
		//EditorGUI.PropertyField(positionRect, lines);

		if(length > 0)
		{
			Vector3 lastPoint = Vector3.zero;

			for(int i = 0; i < length; i++)
			{
				SerializedProperty property = lines.GetArrayElementAtIndex(i);
				SerializedProperty a = property.FindPropertyRelative("a");
				SerializedProperty b = property.FindPropertyRelative("b");
				SerializedProperty showHandles = property.FindPropertyRelative("showHandles");
				SerializedProperty showForLinePath = property.FindPropertyRelative("showForLinePath");

				AddVerticalSpace();
				EditorGUI.PropertyField(positionRect, property);
				positionRect.y += EditorGUI.GetPropertyHeight(property);

				showHandles.boolValue = false;
				showForLinePath.boolValue = true;

				if(i > 0)
				{
					a.vector3Value = lastPoint;
				}

				lastPoint = b.vector3Value;
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
			if(lines == null) return;

			SerializedProperty property = null;
			SerializedProperty a = null;
			SerializedProperty b = null;
			SerializedProperty point1 = null;
			SerializedProperty point2 = null;
			LineSegmentType typeEnum = default(LineSegmentType);
			Vector3 A = Vector3.zero;
			Vector3 B = Vector3.zero;
			Quaternion rotation = default(Quaternion);
			Color color = Handles.color;
			int length = lines.arraySize;

			if(length > 0) for(int i = 0; i < length; i++)
			{
				property = lines.GetArrayElementAtIndex(i);

				typeEnum = (LineSegmentType)property.FindPropertyRelative("type").enumValueIndex;
				a = property.FindPropertyRelative("a");
				b = property.FindPropertyRelative("b");
				point1 = property.FindPropertyRelative("point1");
				point2 = property.FindPropertyRelative("point2");

				if(a == null || b == null || point1 == null || point2 == null) return;

				rotation = Quaternion.LookRotation(b.vector3Value - a.vector3Value);

				if(showHandles != null && showHandles.boolValue) switch(Tools.current)
				{
					case Tool.Move:
					if(i == 0) VHandles.PropertyPositionHandle(ref a, Quaternion.identity);
					VHandles.PropertyPositionHandle(ref b, Quaternion.identity);

					switch(typeEnum)
					{
						case LineSegmentType.Linear:
						/// Do Nothing....
						break;

						case LineSegmentType.CuadraticBeizer:
						VHandles.PropertyPositionHandle(ref point1, rotation);
						break;

						case LineSegmentType.CubicBeizer:
						VHandles.PropertyPositionHandle(ref point1, rotation);
						VHandles.PropertyPositionHandle(ref point2, rotation);
						break;
					}
					break;
				}

				switch(typeEnum)
				{
					case LineSegmentType.Linear:
					Handles.DrawLine(a.vector3Value, b.vector3Value);
					break;

					case LineSegmentType.CuadraticBeizer:
					A = VMath.CuadraticBeizer(a.vector3Value, b.vector3Value, point1.vector3Value, 0.0f);

					Handles.color = Handles.color.WithAlpha(color.a * 0.35f);
					Handles.DrawLine(a.vector3Value, point1.vector3Value);
					Handles.DrawLine(point1.vector3Value, b.vector3Value);
					Handles.color = color;

					for(float t = LineDrawer.TIME_FRACTION; t < LineDrawer.TIME_ITERATIONS + LineDrawer.TIME_FRACTION; t += LineDrawer.TIME_FRACTION)
					{
						B = VMath.CuadraticBeizer(a.vector3Value, b.vector3Value, point1.vector3Value, t);
						Handles.DrawLine(A, B);
						A = B;
					}
					break;

					case LineSegmentType.CubicBeizer:
					A = VMath.CubicBeizer(a.vector3Value, b.vector3Value, point1.vector3Value, point2.vector3Value, 0.0f);

					Handles.color = Handles.color.WithAlpha(color.a * 0.35f);
					Handles.DrawLine(a.vector3Value, point1.vector3Value);
					Handles.DrawLine(point1.vector3Value, point2.vector3Value);
					Handles.DrawLine(point2.vector3Value, b.vector3Value);
					Handles.color = color;

					for(float t = LineDrawer.TIME_FRACTION; t < LineDrawer.TIME_ITERATIONS + LineDrawer.TIME_FRACTION; t += LineDrawer.TIME_FRACTION)
					{
						B = VMath.CubicBeizer(a.vector3Value, b.vector3Value, point1.vector3Value, point2.vector3Value, t);
						Handles.DrawLine(A, B);
						A = B;
					}
					break;
				}
			}
		}
		catch(Exception exception)
		{
			Debug.Log("[LineDrawer] Catched Exception when trying to reload SceneGUI: " + exception.Message);
		}
	}

	/// <summary>Override this method to specify how tall the GUI for this field is in pixels.</summary>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The label of this property.</param>
	public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
	{
		lines = _property.FindPropertyRelative("_lines");
		float height = 0.0f;

		height += SPACE_VERTICAL * 5.0f;

		foreach(SerializedProperty property in lines)
		{
			height += EditorGUI.GetPropertyHeight(property);
			height += SPACE_VERTICAL * 2.0f;
		}

		return height;
		return base.GetPropertyHeight(_property, _label);
	}
}
}