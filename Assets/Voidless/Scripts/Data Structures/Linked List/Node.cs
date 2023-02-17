using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class Node<T>
{
	private T _data; 		/// <summary>Node's Data.</summary>
	private Node<T> _next; 	/// <summary>Next's Node.</summary>

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
}
}