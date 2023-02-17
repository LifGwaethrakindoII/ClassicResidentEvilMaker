using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

namespace Voidless
{
public static class VList
{
	/// <returns>Random element from given List.</returns>
	public static T Random<T>(this List<T> _list)
	{
		return _list != null ? _list[UnityEngine.Random.Range(0, _list.Count)] : default(T);
	}

	public static int GetListsMaxLength<T>(params List<T>[] _lists)
	{
		int maxSize = 0;

		foreach(List<T> list in _lists)
		{
			if(list.Count > maxSize) maxSize = list.Count;
		}

		return maxSize;
	}

	/// <summary>Adds array to List.</summary>
	/// <param name="_list">List that extends and request an augmentation from the given array.</param>
	/// <param name="_set">Set of elements that will be added to the list.</param>
	public static void AddSet<T>(this List<T> _list, params T[] _set)
	{
		if(_list == null) _list = new List<T>(_set.Length);
		foreach(T element in _set)
		{
			_list.Add(element);
		}
	}

	/// <summary>Gets list of T Components from Object List.</summary>
	/// <param name="_list">The Object List.</param>
	/// <returns>List of Object List's T Component.</returns>
	public static List<T> GetComponentsFromGameObjects<T>(this List<GameObject> _list) where T : UnityEngine.Object
	{
		List<T> newList =  new List<T>();

		foreach(GameObject _unityObject in _list)
		{
			if(_unityObject.Has<T>()) newList.Add(_unityObject.GetComponent<T>());
		}

		return newList;
	}

	/// <summary>Destroys all GameObjects contained on a list of UnityEngine's Components.</summary>
	/// <param name="_list">The List containing the GameObjects.</param>
	public static void DestroyAllGameObjects<T>(this List<T> _list) where T : UnityEngine.Component
	{
		foreach(T _component in _list)
		{
			if(Application.isPlaying) UnityEngine.Object.Destroy(_component.gameObject);
			else if(Application.isEditor) UnityEngine.Object.DestroyImmediate(_component.gameObject);
		}
	}

	/// <summary>Destroys all Object elements on List.</summary>
	/// <param name="_list">List containing the Object elements.</param>
	public static void DestroyAllElements<T>(this List<T> _list) where T : UnityEngine.Object
	{
		foreach(T UnityObject in _list)
		{
			if(Application.isPlaying) UnityEngine.Object.Destroy(UnityObject);
			else if(Application.isEditor) UnityEngine.Object.DestroyImmediate(UnityObject);
		}
	}

	/// <summary>Adds List Elements to List.</summary>
	/// <param name="_list">The List that will have its elements added.</param>
	/// <param name="_elementsList">The List that contains the new Elements that will be added.</param>
	/// <returns>List with newly addedElements.</returns>
	public static List<T> AddElements<T>(this List<T> _list, List<T> _elementsList)
	{
		foreach(T element in _elementsList)
		{
			_list.Add(element);
		}

		return _list;
	}

	/// <summary>Finds index that accomplishes given predicate.</summary>
	/// <param name="_list">List that is going to look for the successful index.</param>
	/// <param name="_condition">Condition that will be evaluated on each element to determine the successful index.</param>
	/// <returns>Successful index that accomplishes the evaluated condition.</returns>
	public static int GetIndexThatAccomplishes<T>(this List<T> _list, System.Predicate<T> _condition)
	{
		for(int i = 0; i < _list.Count; i++)
		{
			if(_condition(_list[i])) return i;	
		}

		return 0;
	}

	/// <summary>Gets if list has something on all indexes.</summary>
	/// <param name="_list">List containing the T elements.</param>
	/// <returns>True if all elements on list are different from null.</returns>
	public static bool ListFull<T>(this List<T> _list)
	{
		foreach(T element in _list)
		{
			if(element == null) return false;
		}

	return true;
	}

	/// <summary>Checks if all elements on list accomplishes all conditions.</summary>
	/// <param name="_list">List that will have all its elements evaluated.</param>
	/// <param name="_condition">Condition that all elements have to accomplish for the method to return true.</param>
	/// <returns>If all elements on list accomplish the condition.</returns>
	public static bool AllAccomplish<T>(this List<T> _list, System.Predicate<T> _condition)
	{
		foreach(T element in _list)
		{
			if(!_condition(element)) return false;
		}

		return true;
	}

	/// <summary>Gets a list with the elements that accomplish a certain condition.</summary>
	/// <param name="_list">List that willhave its elements evaluated.</param>
	/// <param name="_condition">Condition that each element of the List has to accomplish.</param>
	/// <returns>List with all the elemets that accomplished the condition.</returns>
	public static List<T> GetAllThatAccomplish<T>(this List<T> _list, System.Predicate<T> _condition)
	{
		List<T> accomplishedList = new List<T>();

		foreach(T element in _list)
		{
			if(_condition(element)) accomplishedList.Add(element);
		}

		return accomplishedList;
	}

	/// <summary>Takes a derived class List and passes it to base class List.</summary>
	/// <param name="_fromList">Base class List.</param>
	/// <param name="_toList">Derived class List.</param>
	/// <returns>Converted base class list with derived class elements.</returns>
	public static List<A> ConvertTo<A, B>(this List<A> _fromList, List<B> _toList) where B : A
	{
		_fromList = new List<A>();

		for(int i = 0; i < _toList.Count; i++)
		{
			_fromList.Add(_toList[i]);	
		}

		return _fromList;
	}
}
}