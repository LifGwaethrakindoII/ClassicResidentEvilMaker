using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
public static class VHandles
{
	/// <summary>Draws a Position Handle for a Serialized Property.</summary>
	/// <param name="_property">SerializedProperty's reference.</param>
	/// <returns>Handles' Position.</returns>
	public static Vector3 PropertyPositionHandle(ref SerializedProperty _property, Quaternion _rotation = default(Quaternion))
	{
		EditorGUI.BeginChangeCheck();
		
		if(_rotation == default(Quaternion)) _rotation = Quaternion.identity;

		Vector3 newPosition = Handles.PositionHandle(_property.vector3Value, _rotation);
		
		if(EditorGUI.EndChangeCheck())
		{
			_property.vector3Value = newPosition;
			_property.serializedObject.ApplyModifiedProperties();
		}

		return newPosition;
	}
}
}