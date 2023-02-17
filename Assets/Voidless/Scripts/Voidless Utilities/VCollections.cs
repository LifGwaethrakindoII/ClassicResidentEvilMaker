using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VCollections
{
	/// <summary>Turns array into a List of the same generic type.</summary>
	/// <param name="_array">Array to convert to List.</param>
	/// <returns>List created from array elements.</returns>
	public static List<T> ToList<T>(this T[] _array)
	{
		if(_array == null) return null;

		int length = _array.Length;
		List<T> list = new List<T>(length);

		for(int i = 0; i < length; i++)
		{
			list.Add(_array[i]);
		}

		return list;
	}

	/// <summary>Moves index to next position.</summary>
	/// <param name="_collection">Reference collection.</param>
	/// <param name="_index">Index to move.</param>
	/// <returns>Index after being moved.</returns>
	public static int AddIndex<T>(this ICollection<T> _collection, ref int _index)
	{
		return ((_index + 1 < (_collection.Count - 1)) ? _index + 1 : _index);
	}

	/// <summary>Moves index to previous position.</summary>
	/// <param name="_collection">Reference collection.</param>
	/// <param name="_index">Index to move.</param>
	/// <returns>Index after being moved.</returns>
	public static int SubtractIndex<T>(this ICollection<T> _collection, ref int _index)
	{
		return ((_index - 1 > -1) ? _index - 1 : _index);
	}

	/// <summary>Evaluates if provided index is between Collection's bounds.</summary>
	/// <param name="_collection">Collection that will define the bounds.</param>
	/// <param name="_index">Index to evaluate.</param>
	/// <returns>True if the index provided is bettween Collection's bounds, false otherwise.</returns>
	public static bool CheckIfIndexBetweenBounds<T>(this ICollection<T> _collection, int _index)
	{
		return (_index >= 0 && _index <= (_collection.Count - 1));
	}

	/// <summary>Returns an index constrained by the Collection's boundaries.</summary>
	/// <param name="_collection">Collection that will define the boundaries.</param>
	/// <param name="_index">Index to evaluate.</param>
	/// <returns>constrained Index.</returns>
	public static int ConstrainedIndex<T>(this ICollection<T> _collection, int _index)
	{
		return Mathf.Clamp(_index, 0, _collection.Count - 1);
	}

	/// <summary>Adds Elements to LinkedList.</summary>
	/// <param name="_list">LinkedList to add elements to.</param>
	/// <param name="_elements">Set of elements to add to the LinkedList.</param>
	public static void Add<T>(this LinkedList<T> _list, params T[] _elements)
	{
		if(_list == null || _elements == null) return;

		foreach(T element in _elements)
		{
			_list.AddFirst(element);
		}
	}

	/// <summary>Clears LinkedList and adds Elements to it.</summary>
	/// <param name="_list">LinkedList to clear and add elements to.</param>
	/// <param name="_elements">Set of elements to add to the LinkedList.</param>
	public static void ClearAndAdd<T>(this LinkedList<T> _list, params T[] _elements)
	{
		if(_list == null || _elements == null) return;

		_list.Clear();

		foreach(T element in _elements)
		{
			_list.AddFirst(element);
		}
	}

	/// <summary>Does a foreach statement on a generic IEnumerator.</summary>
	/// <param name="_iterator">Iterator.</param>
	/// <param name="action">Action to do on each iterated object.</param>
	public static void ForEach<T>(this IEnumerator<T> _iterator, Action<T> action)
	{
		while(_iterator.MoveNext())
		{
			action(_iterator.Current);
		}
	}

	/// <summary>Resizes List.</summary>
	/// <param name="list">List to resize.</param>
	/// <param name="size">New size.</param>
	/// <param name="element">Default element [default by default. Damn, so redundant...].</param>
	public static void Resize<T>(this List<T> list, int size, T element = default(T))
    {
        int count = list.Count;

        if (size < count)
        {
            list.RemoveRange(size, count - size);
        }
        else if (size > count)
        {
            if (size > list.Capacity)   // Optimization
                list.Capacity = size;

            list.AddRange(Enumerable.Repeat(element, size - count));
        }
    }
}
}