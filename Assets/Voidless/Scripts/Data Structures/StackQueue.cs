using System;
using System.Collections;
using System.Collections.Generic;

namespace Voidless
{
public class StackQueue<T> : IEnumerable<T>/*, IReadOnlyCollection<T>*/, ICollection
{
	private LinkedList<T> linkedList; 	/// <summary>Under the hood's Linked List.</summary>

	/// <summary>Gets Count.</summary>
	public int Count { get { return linkedList.Count; } }

	/// <summary>Gets IsSynchronized property.</summary>
	public bool IsSynchronized
	{
		get
		{
			ICollection collection = linkedList;
			return collection.IsSynchronized;
		}
	}

	/// <summary>Gets SyncRoot property.</summary>
	public object SyncRoot
	{
		get
		{
			ICollection collection = linkedList;
			return collection.SyncRoot;
		}
	}

	/// <summary>StackQueue default constructor.</summary>
	public StackQueue()
	{
		linkedList = new LinkedList<T>();
	}

	/// <summary>StackQueue constructor.</summary>
	/// <param name="_iterator">Elements to copy from iterator.</param>
	public StackQueue(IEnumerable<T> _iterator)
	{
		linkedList = new LinkedList<T>(_iterator);
	}

	/// <summary>StackQueue destructor.</summary>
	~StackQueue()
	{
		
	}

	/// <summary>Pushes an item like a stack.</summary>
	/// <param name="_item">Item to push.</param>
    public void Push(T _item)
    {
        linkedList.AddFirst(_item);
    }

    /// <summary>Enqueues an item like a queue.</summary>
    /// <param name="_item">Item to enqueue.</param>
    public void Enqueue(T _item)
    {
        linkedList.AddLast(_item);
    }

    /// <summary>Pops an item like a stack.</summary>
    /// <returns>Popped item.</returns>
    public T Pop()
    {
        T item = linkedList.First.Value;
        linkedList.RemoveFirst();
        return item;
    }

    /// <summary>Dequeues item like a queue.</summary>
    /// <returns>Dequeued item.</returns>
    public T Dequeue()
    {
        T item = linkedList.Last.Value;
        linkedList.RemoveLast();
        return item;
    }

    /// <summary>Peeks last item, like a stack.</summary>
    /// <returns>Peeked item.</returns>
    public T PeekStack()
    {
        return linkedList.First.Value;
    }

    /// <summary>Peeks first item, like a queue.</summary>
    /// <returns>Peeked item.</returns>
    public T PeekQueue()
    {
        return linkedList.Last.Value;
    }

    /// <summary>Removes all nodes from the LinkedList.</summary>
    public void Clear()
    {
    	linkedList.Clear();
    }

    /// <returns>Returns an enumerator that iterates through the LinkedList's items.</returns>
	public IEnumerator<T> GetEnumerator()
	{
		foreach(T item in linkedList)
		{
			yield return item;
		}
	}

	/// <returns>Returns an enumerator that iterates through the LinkedList's items.</returns>
	IEnumerator IEnumerable.GetEnumerator()
	{
		yield return GetEnumerator();
	}

    /// <summary>Copies the elements of the ICollection to an Array, starting at a particular Array index.</summary>
    public void CopyTo(Array _array, int _index)
    {
    	ICollection collection = linkedList;
    	collection.CopyTo(_array, _index);
    }

    /// <summary>Iterates the StackQueue as if it was a Stack.</summary>
    public IEnumerator<T> IterateAsStack()
    {
    	LinkedListNode<T> current = linkedList.Last;

    	while(current != null)
    	{
    		yield return current.Value;
    		current = current.Previous;
    	}
    }

    /// <summary>Iterates the StackQueue as if it was a Queue.</summary>
    public IEnumerator<T> IterateAsQueue()
    {
    	LinkedListNode<T> current = linkedList.First;

    	while(current != null)
    	{
    		yield return current.Value;
    		current = current.Next;
    	}
    }

    /// <summary>Iterates the StackQueue as if it was a Queue AND a Stack [oscillating between them].</summary>
    public IEnumerator<T> IterateAsQueueAndStack()
    {
        LinkedListNode<T> current = linkedList.First;
        LinkedListNode<T> head = current;
        LinkedListNode<T> tail = linkedList.Last;
        bool headsTurn = true;
        bool keepIterating = true;

        while(current != null)
        {
            current = headsTurn ? head : tail;

            switch(headsTurn)
            {
                case true:
                head = head.Next;
                break;

                case false:
                tail = tail.Previous;
                break;
            }

            headsTurn = !headsTurn;
            
            yield return current.Value;
            
            if(!keepIterating) current = null;
            if(head == tail) keepIterating = false;
        }
    }
}
}