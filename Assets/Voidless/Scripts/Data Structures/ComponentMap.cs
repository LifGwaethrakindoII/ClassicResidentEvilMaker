using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class ComponentMap<T> : IEnumerable<T>, ICollection<T>
{
	private Dictionary<int, T> dictionary; 	/// <summary>Internal dictionary that maps all components.</summary>

	/// <summary>Map's indexer. Hides dictionary's implementation</summary>
	/// <param name="_object">Object to retreive ID from.</param>
	public T this[GameObject _object]
	{
		get
		{
			int ID = _object.GetInstanceID();
			return dictionary.ContainsKey(ID) ? dictionary[ID] : default(T);
		}
	}

	/// <summary>Gets Count property.</summary>
	public int Count { get { return dictionary.Count; } }

	/// <summary>Gets IsReadOnly property.</summary>
	public bool IsReadOnly { get { return false; } }

	/// <summary>ComponentMap default constructor.</summary>
	/// <param name="_size">Internal Dictionary's size [0 by default].</param>
	public ComponentMap(int _size = 0)
	{
		dictionary = new Dictionary<int, T>(_size);
	}

	/// <summary>Checks if mapping contains an ID associated with Object.</summary>
	/// <param name="_object">Object to retrieve ID from.</param>
	/// <returns>True if the mapping contains an ID associated with given Object.</returns>
	public bool Contains(GameObject _object)
	{
		return dictionary.ContainsKey(_object.GetInstanceID());
	}

	/// <summary>Checks if mapping contains GameObject.</summary>
	/// <param name="_component">GameObject to check.</param>
	/// <returns>True if mapping contains GameObject.</returns>
	public bool Contains(T _component)
	{
		return dictionary.ContainsValue(_component);
	}

	/// <summary>Adds GameObject from Object.</summary>
	/// <param name="_object">GameObject's Entity.</param>
	/// <param name="_component">GameObject to add.</param>
	public void Add(GameObject _object, T _component)
	{
		if(!Contains(_object))
		dictionary.Add(_object.GetInstanceID(), _component);
	}

	/// <summary>Adds T component from Object.</summary>
	/// <param name="_object">Object to retrieve GameObject from.</param>
	public bool AddFrom(GameObject _object)
	{
		if(!Contains(_object))
		{
			T component = _object.GetComponent<T>();
			if(component != null)
			{
				dictionary.Add(_object.GetInstanceID(), component);
				return true;
			}
			else return false;
		}
		else return true;
	}

	/// <summary>Removes entry on ID associated with Object.</summary>
	/// <param name="_object">Object to retrieve ID from.</param>
	public void Remove(GameObject _component)
	{
		if(dictionary.ContainsKey(_component.GetInstanceID()))
		dictionary.Remove(_component.GetInstanceID());
	}

	/// <summary>Removes GameObject from mappin.</summary>
	/// <param name="_component">GameObject to remove.</param>
	/// <returns>True if the remove was successful.</returns>
	public bool Remove(T _component)
	{
		return false;
	}

	/// <summary>Adds component to mapping.</summary>
	/// <param name="_component">GameObject to add.</param>
	public void Add(T _component) { /*...*/ }

	/// <summary>Clears dictionary.</summary>
	public void Clear()
	{
		dictionary.Clear();
	}

	/// <summary>Copies the elements of the ICollection T to an Array, starting at a particular Array index.</summary>
	public void CopyTo(T[] _array, int _index) { /*...*/ }

	/// <returns>Returns an enumerator that iterates through the mapping's components.</returns>
	public IEnumerator<T> GetEnumerator()
	{
		foreach(T component in dictionary.Values)
		{
			yield return component;
		}
	}

	/// <returns>Returns an enumerator that iterates through the mapping's components.</returns>
	IEnumerator IEnumerable.GetEnumerator()
	{
		yield return GetEnumerator();
	}
}
}