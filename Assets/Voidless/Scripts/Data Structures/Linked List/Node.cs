using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public class Node<T> : IEnumerable<Node<T>>
{
	[SerializeField] private T _data;
	[SerializeField] private Node<T> _next;

	/// <summary>Gets and Sets data property.</summary>
	public T data
	{
		get { return _data; }
		set { _data = value; }
	}

	/// <summary>Gets and Sets next property.</summary>
	public Node<T> next
	{
		get { return _next; }
		set { _next = value; }
	}

	/// <summary>Parameterless' Constructor.</summary>
	public Node() { /*...*/ }

	/// <summary>Node default constructor.</summary>
	/// <param name="_data">Node's Data.</param>
	public Node(T _data)
	{
		data = _data;
	}

	/// <summary>Node overload constructor.</summary>
	/// <param name="_data">Node's Data.</param>
	/// <param name="_next">Next's Node.</param>
	public Node(T _data, Node<T> _next) : this(_data)
	{
		next = _next;
	}

	///<summary> Implementing IEnumerable<T> interface.</summary>
    public virtual IEnumerator<Node<T>> GetEnumerator()
    {
        yield return next;
    }

    ///<summary> Implementing IEnumerable interface (non-generic version).</summary>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

	/// <summary>Gets updated data from this Node.</summary>
	public virtual T Update() { return default(T); }
}
}