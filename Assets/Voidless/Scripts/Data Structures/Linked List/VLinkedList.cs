using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
/// \TODO Implement the following interfaces:
/*
System.Collections.Generic.ICollection<T>,
System.Collections.Generic.IEnumerable<T>,
System.Collections.Generic.IReadOnlyCollection<T>,
System.Collections.ICollection,
System.Runtime.Serialization.IDeserializationCallback,
System.Runtime.Serialization.ISerializable
*/
public class VLinkedList<T> : IEnumerable<T>, IEnumerable<Node<T>>
{
	private Node<T> _headNode; 	/// <summary>Head's Node.</summary>
	private Node<T> _tailNode; 	/// <summary>Tail's Node.</summary>

	/// <summary>Gets and Sets headNode property.</summary>
	public Node<T> headNode
	{
		get { return _headNode; }
		set { _headNode = value; }
	}

	/// <summary>Gets and Sets tailNode property.</summary>
	public Node<T> tailNode
	{
		get { return _tailNode; }
		set { _tailNode = value; }
	}

	/// <summary>Gets and Sets head property.</summary>
	public T head
	{
		get { return headNode != null ? headNode.data : default(T); }
		set
		{
			if(headNode == null) headNode = new Node<T>(value);
			else headNode.data = value;
		}
	}

	/// <summary>Gets and Sets tail property.</summary>
	public T tail
	{
		get { return tailNode != null ? tailNode.data : default(T); }
		set
		{
			if(tailNode == null) tailNode = new Node<T>(value);
			else tailNode.data = value;
		}
	}

	/// <summary>Gets isEmpty property.</summary>
	public bool isEmpty { get { return headNode == null && tailNode == null; } }

	/// <summary>VLinkedList default constructor.</summary>
	public VLinkedList()
	{

	}

	/// <summary>VLinkedList overload constructor.</summary>
	/// <param name="_enumerable">The IEnumerable whose elements are copied to the new LinkedList.</param>
	public VLinkedList(IEnumerable<T> _enumerable)
	{
		Initialize(_enumerable);
	}

	/// <summary>Initializes Linked List with given IEnumerable instance.</summary>
	/// <param name="_enumerable">The IEnumerable whose elements are copied to the new LinkedList.</param>
	public void Initialize(IEnumerable<T> _enumerable)
	{
		Clear();
		IEnumerator<T> iterator = _enumerable.GetEnumerator();
		
		iterator.MoveNext();
		T current = iterator.Current;

		if(current == null) return;
		
		Node<T> node = new Node<T>(current);
		headNode = node;

		while(iterator.MoveNext())
		{
			current = iterator.Current;
			node.next = new Node<T>(current);
			node = node.next;
		}

		tailNode = node != headNode ? node : null;
	}

	/// <summary>[Sort of] clears the LinkedList.</summary>
	public void Clear()
	{
		List<Node<T>> list = new List<Node<T>>();

		foreach(Node<T> element in this as IEnumerable<Node<T>>)
		{
			list.Add(element);
		}

		for(int i = 0; i < list.Count; i++)
		{
			if(list[i] != null)
			list[i].next = null;
		}

		if(headNode != null)
		{
			headNode.next = null;
			headNode = null;
		}
		if(tailNode != null)
		{
			tailNode.next = null;
			tailNode = null;
		}
	}

	/// <summary>Adds new Head to the LinkedList.</summary>
	/// <param name="_data">Data to add as Head.</param>
	public void AddHead(T _data)
	{
		Node<T> node = new Node<T>(_data, headNode);
		headNode = node;
	}

	/// <summary>Adds new Tail to the LinkedList.</summary>
	/// <param name="_data">Data to add as Tail.</param>
	public void AddTail(T _data)
	{
		Node<T> node = new Node<T>(_data);

		if(headNode == null)
		{
			headNode = node;
		
		} else if(tailNode == null)
		{
			headNode.next = node;
			tailNode = node;
		
		} else if(tailNode != null)
		{
			tailNode.next = node;
			tailNode = node;
		}
	}

#region IEnumerator:
    /// <returns>Iterator of all LinkedList's Nodes.</returns>
    IEnumerator<Node<T>> IEnumerable<Node<T>>.GetEnumerator()
    {
        Node<T> node = headNode;

        do
        {
        	yield return node;
        	node = node;
        }
        while(node != null);
    }

    /// <returns>Iterator of all LinkedList's Nodes as objects.</returns>
    public IEnumerator GetEnumerator()
    {
        //return GetEnumerator();
        return null;
    }

    /// <returns>Iterator of all LinkedList's Nodes.</returns>
    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        foreach(Node<T> node in this as IEnumerable<Node<T>>)
        {
        	yield return node.data;
        }
    }

    /*/// <returns>Iterator of all LinkedList's Nodes as objects.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }*/
#endregion

}
}