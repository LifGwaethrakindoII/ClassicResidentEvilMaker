using System;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
#if UNITY_EDITOR
public static class VEditorWindow
{
	/// \TODO XML Documentation of this method...
	/*public static Rect GetVerticalLayoutDisplacement(this Rect _rect, ref float _currentLayoutY, float _verticalDisplacement, Vector2 _rectOffset)
	{
		return new Rect
		{
			(_rect.x + _rectOffset.x),
			((_rect.y + _rectOffset.y) + (_currentLayoutY += _verticalDisplacement)),
			(_rect.width - (_rectOffset.x * 2f)),
			_rectOffset.y;
		}
	}*/

	/// <summary>Creates most adequate object Field on EditorGUI Layout.</summary>
	public static object CreateEditorGUIField(this Rect _fieldRect, string _label, Type _objectType, object _methodArgument)
	{
		//for(int i = 0; i < _methodParameter.Length; i++)
		{
			//Type t = _methodParameter.ParameterType;

			if(_objectType == typeof(UnityEngine.Object))
			{
				_methodArgument = EditorGUI.ObjectField(_fieldRect, _label, (UnityEngine.Object)(object)_methodArgument, _objectType, true) as UnityEngine.Object;
			}
			else if(_objectType == typeof(UnityEngine.GameObject))
			{
				return _methodArgument = EditorGUI.ObjectField(_fieldRect, _label, (UnityEngine.GameObject)(object)_methodArgument, _objectType, true) as UnityEngine.GameObject;
			}
			else if(_objectType == typeof(System.Enum))
			{
				//Type enumType = System.Enum.GetUnderlyingType(_objectType);
				//_methodArgument = EditorGUI.EnumPopup(_fieldRect, _label, (enumType)(object)_objectType);
				//_methodArgument = _methodArgument.CreateEnumPopUp(_objectType, _fieldRect, _label);
			}
			else if(_objectType == typeof(string))
			{

			}
			else if(_objectType == typeof(int))
			{

			}

			return _methodArgument;
		}
	}

	/// <summary>Abstract Layer that creates EditorGUI object Field relative to the data Type needed.</summary>
	/// <param name="_object">Object variable to evaluate.</param>
	/// <param name="_type">Type of the new EditorGUI Field.</param>
	/// <param name="_layoutPosition">Field's Layout position.</param>
	/// <param name="_label">Field's Label.</param>
	/// <returns>Most proper EditorGUI's Field [null if there is none].</returns>
	public static object CreatePropertyField<T>(this object _object, Type _type, Rect _layoutPosition, string _label) where T : System.Type
	{
		if(_type == typeof(UnityEngine.Object))
		{
			//return _object.CreateObjectField<>(_type.GetType(), _layoutPosition, _label);

			///Should run CreateObjectField.
		}
		else if(_type == typeof(System.Enum))
		{
			
		}

		return _object;
	}

	/// <summary>Creates EditorGUI object Field.</summary>
	/// <param name="_object">Object variable to evaluate.</param>
	/// <param name="_type">Type of the new EditorGUI Field.</param>
	/// <param name="_layoutPosition">Field's Layout position.</param>
	/// <param name="_label">Field's Label.</param>
	/// <returns>Most proper EditorGUI's Field [null if there is none].</returns>
	public static object CreateObjectField<T>(this object _object, T _type, Rect _layoutPosition, string _label) where T : UnityEngine.Object
	{
		return _object = EditorGUI.ObjectField(_layoutPosition, _label, (T)_object, typeof(T), true) as T;
	}

	/// <summary>Creates EditorGUI Enum Pop-Up Field.</summary>
	/// <param name="_object">Object variable to evaluate.</param>
	/// <param name="_type">Type of the new EditorGUI Field.</param>
	/// <param name="_layoutPosition">Field's Layout position.</param>
	/// <param name="_label">Field's Label.</param>
	/// <returns>Most proper EditorGUI's Field [null if there is none].</returns>
	public static object CreateEnumPopUp(this object _object, System.Enum _type, Rect _layoutPosition, string _label)
	{
		return _object = EditorGUI.EnumPopup(_layoutPosition, _label, _type);			
	}

	/// <summary>Creates EditorGUI Pop-Up Field.</summary>
	/// <param name="_object">Object variable to evaluate.</param>
	/// <param name="_layoutPosition">Field's Layout position.</param>
	/// <param name="_label">Field's Label.</param>
	/// <param name="_displayedOptions">Options to display [Either by a direct array or a converted one by params].</param>
	/// <returns>Index of the current selected Pop-Up's array element.</returns>
	public static int CreatePopUp(this object _object, Rect _layoutPosition, string _label, params string[] _displayedOptions)
	{
		return EditorGUI.Popup(_layoutPosition, _label, (int)_object, _displayedOptions);
	}

}
#endif
}