using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[System.Serializable]
public class Blackboard<T, N>
{
	/// <summary>Action called when a value has changed.</summary>
	/// <param name="_key">Key of the value.</param>
	/// <param name="_value">New changed value.</param>
	public delegate void ValueChanged(T _key, N _value);
	public static event ValueChanged onValueChanged;

	/// <summary>Action called when a value is going to change.</summary>
	/// <param name="_key">Key of the value.</param>
	/// <param name="_value">New upcoming value.</param>
	public delegate void ValueChanging(T _key, N _value);
	public static event ValueChanging onValueChanging;

	private static Blackboard<T, N> _instance; 	/// <summary>Blackboard's Instance reference.</summary>
	private Dictionary<T, N> _dictionary; 		/// <summary>Blackboard's Dictionary.</summary>
	private bool _hasPriority; 					/// <summary>Does this Blackboard have priority?.</summary>
	private T _priorityKey; 					/// <summary>Blackboard's current priority key.</summary>
	private Queue<T> _priorityKeys; 			/// <summary>Priority Key Queue.</summary>

#region Getters/Setters:
	/// <summary>Gets Instance property.</summary>
	public static Blackboard<T, N> Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = new Blackboard<T, N>();
			}

			return _instance;
		}
	}

	/// <summary>Gets dictionary indexed property.</summary>
	public N this[T _index] { get { return dictionary[_index]; } }

	/// <summary>Gets and Sets dictionary property.</summary>
	public Dictionary<T, N> dictionary
	{
		get { return _dictionary; }
		protected set { _dictionary = value; }
	}

	/// <summary>Gets and Sets hasPriority property.</summary>
	public bool hasPriority
	{
		get { return _hasPriority; }
		protected set { _hasPriority = value; }
	}

	/// <summary>Gets hasPriorities property.</summary>
	public bool hasPriorities { get { return priorityKeys.Count > 1; } }

	/// <summary>Gets and Sets priorityKey property.</summary>
	public T priorityKey
	{
		get { return _priorityKey; }
		set
		{
			_priorityKey = value;
			hasPriority = true;
		}
	}

	/// <summary>Gets and Sets priorityKeys property.</summary>
	public Queue<T> priorityKeys
	{
		get { return _priorityKeys; }
		protected set { _priorityKeys = value; }
	}	
#endregion

	/// <summary>Default Blackboard constructor.</summary>
	public Blackboard()
	{
		hasPriority = false;
		priorityKeys = new Queue<T>();
		dictionary = new Dictionary<T, N>();
	}

	/// <summary>Overload Blackboard constructor.</summary>
	/// <param name="_initialCapacity">Internal Dictionary's initial capacity.</param>
	public Blackboard(int _initialCapacity)
	{
		hasPriority = false;
		priorityKeys = new Queue<T>();
		dictionary = new Dictionary<T, N>(_initialCapacity);
	}

#region PublicMethods:
	/// <returns>True if internal Dictionary contains given key.</returns>
	public bool ContainsKey(T _key) { return dictionary != null ? dictionary.ContainsKey(_key) : false; }

	/// <summary>Gets entry registered on T key.</summary>
	/// <param name="_key">Key where the Entry is registered.</param>
	/// <returns>Entry registered on T key.</returns>
	public N GetEntry(T _key)
	{
		if(dictionary.ContainsKey(_key)) return dictionary[_key];
		else
		{
			Debug.Log("[Blackboard] There is no key " + _key.ToString() +  " registered on Blackboard");
			return default(N);
		}
	}

	/// <summary>Adds new entry to Blackboard.</summary>
	/// <param name="_key">Key of the new entry.</param>
	/// <param name="_value">New entry's value.</param>
	public void AddEntry(T _key, N _value)
	{
		if(!dictionary.ContainsKey(_key)) dictionary.Add(_key, _value);
		else
		{
			//Debug.Log("[Blackboard] Blackboard already contains key " + _key.ToString());
			UpdateEntry(_key, _value);
		}
	}

	/// <summary>Updates value stored on key.</summary>
	/// <param name="_value">Value to update.</param>
	public void UpdateEntry(T _key, N _value)
	{
		if(dictionary.ContainsKey(_key))
		{
			OnValueChanging(_key, _value);
			dictionary[_key] = _value;
			OnValueChanged(_key, _value);
		}
	}

	/// <summary>Sets Blackboard's main priority.</summary>
	/// <param name="_priorityKey">Key of the main priority.</param>
	public void SetPriority(T _priorityKey)
	{
		priorityKey = _priorityKey;
	}

	/// <summary>Adds a priority to the priority's collection.</summary>
	/// <param name="_priorityKey">Priority to add to the collection of priorities.</param>
	public void AddPriority(T _priorityKey)
	{
		if(dictionary.ContainsKey(_priorityKey)) priorityKeys.Enqueue(_priorityKey);
		else Debug.LogError("[Blackboard] Blackboard dictionary has no key " + _priorityKey);
	}

	/// <summary>Adds priorities to collection of priorities.</summary>
	/// <param name="_priorityKeys">Priorities to add to collection of priorities.</param>
	public void AddPriorities(params T[] _priorityKeys)
	{
		for(int i = 0; i < _priorityKeys.Length; i++)
		{
			AddPriority(_priorityKeys[i]);
		}
	}

	/// <summary>Queries priority key.</summary>
	/// <returns>Proprity Key value.</returns>
	public T PeekPriorityKey()
	{
		return priorityKey;
	}

	/// <summary>Fetches proprity key, then removes priority.</summary>
	/// <returns>Priority Key value.</returns>
	public T FetchPriorityKey()
	{
		hasPriority = false;
		return priorityKey;
	}

	/// <summary>Removes proprities's collection and unchecks hasPriority property.</summary>
	public void RemovePriorities()
	{
		priorityKeys.Clear();
		priorityKeys = new Queue<T>();
		hasPriority = false;
	}
#endregion

#region PrivateMethods:
	/// <summary>Calls onValueChanged event delegate.</summary>
	/// <param name="_key">Key of the value.</param>
	/// <param name="_value">New upcoming value.</param>
	private void OnValueChanging(T _key, N _value)
	{
		if(onValueChanging != null) onValueChanging(_key, _value);
	}

	/// <summary>Calls onValueChanging event delegate.</summary>
	/// <param name="_key">Key of the value.</param>
	/// <param name="_value">New changed value.</param>
	private void OnValueChanged(T _key, N _value)
	{
		if(onValueChanged != null) onValueChanged(_key, _value);
	}
#endregion
}
}