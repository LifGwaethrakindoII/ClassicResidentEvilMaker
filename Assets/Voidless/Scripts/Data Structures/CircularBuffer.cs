using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Modulo rules: if(x1 < x2) return x1; else return modulo(x1, x2);

*/

namespace Voidless
{
public class CircularBuffer<T> : IEnumerable<T>, ICollection
{
	private List<T> list; 			/// <summary>Under-the-hood List.</summary>
	private int	_size; 				/// <summary>Circular Buffer's Size.</summary>
	private int _index; 			/// <summary>Current Index.</summary>
	private int _nextIndex; 		/// <summary>Next Index.</summary>
	private int _occupiedSlots; 	/// <summary>Slots occupied.</summary>

#region Getters/Setters:
	/// <summary>CircularBuffer's Indexer.</summary>
	public T this[int index]
	{
		get { return list[index]; }
		set { list[index] = value; }
	}

	/// <summary>Gets and Sets size property.</summary>
	public int size
	{
		get { return _size; }
		set { _size = value; }
	}

	/// <summary>Gets and Sets nextIndex property.</summary>
	public int nextIndex
	{
		get { return _nextIndex; }
		private set { _nextIndex = value; }
	}

	/// <summary>Gets and Sets index property.</summary>
	public int index
	{
		get { return _index; }
		private set { _index = value; }
	}

	/// <summary>Gets and Sets occupiedSlots property.</summary>
	public int occupiedSlots
	{
		get { return _occupiedSlots; }
		private set { _occupiedSlots = Mathf.Clamp(value, 0, size); }
	}

	/// <summary>Gets Count.</summary>
	public int Count { get { return list.Count; } }

	/// <summary>Gets tailIndex property.</summary>
	public int tailIndex { get { return RegressIndex(index, occupiedSlots - 1, occupiedSlots); } }

	/// <summary>Gets IsSynchronized property.</summary>
	public bool IsSynchronized
	{
		get
		{
			ICollection collection = list;
			return collection.IsSynchronized;
		}
	}

	/// <summary>Gets SyncRoot property.</summary>
	public object SyncRoot
	{
		get
		{
			ICollection collection = list;
			return collection.SyncRoot;
		}
	}
#endregion

	/// <summary>CircularBuffer default constructor.</summary>
	/// <param name="_size">CircularBuffer's Size.</param>
	/// <param name="_defaultValue">Default value for each item on the CircularBuffer.</param>
	public CircularBuffer(int _size, T _defaultValue = default(T))
	{
		Clear();
		size = Mathf.Max(_size, 1);
		list = new List<T>(size);

		for(int i = 0; i < size; i++)
		{
			list.Add(_defaultValue);
		}
	}

	/// <summary>Inserts item at current index.</summary>
	/// <param name="_item">Item to insert.</param>
	public void Insert(T _item)
	{
		list[nextIndex] = _item;
		index = nextIndex;
		nextIndex = IncreaseIndex(nextIndex, size);
		occupiedSlots++;
	}

	/// <summary>Removes item at current index.</summary>
	public void Remove()
	{
		list[index] = default(T);
		nextIndex = index;
		index = DecreaseIndex(index, size);
		occupiedSlots--;
	}

	/// <summary>Advances the Circular Buffer without making an insertion. The method is useful if you are explicitely modifying the CircularBuffer's item at the index.</summary>
	public void Advance()
	{
		index = nextIndex;
		nextIndex = IncreaseIndex(nextIndex, size);
		occupiedSlots++;
	}

	/// <summary>Regresses the Circular Buffer without making an removal. The method is useful if you are explicitely modifying the CircularBuffer's item at the index.</summary>
	public void Regress()
	{
		nextIndex = index;
		index = DecreaseIndex(index, size);
		occupiedSlots--;
	}

	/// <summary>Updates Index Forward.</summary>
	/// <param name="_index">Index to Increase.</param>
	/// <param name="_limit">Limit's Reference.</param>
	/// <returns>Increased Index.</returns>
	public static int IncreaseIndex(int _index, int _limit)
	{
		return _index < _limit - 1 ? _index + 1 : 0;
	}

	/// <summary>Updates Index Forward x amount of steps.</summary>
	/// <param name="_index">Index to Increase.</param>
	/// <param name="_amount">Amount of steps.</param>
	/// <param name="_limit">Limit's Reference.</param>
	/// <returns>Increased Index.</returns>
	public static int AdvanceIndex(int _index, int _amount, int _limit)
	{
		return (_index + _amount) % _limit;
	}

	/// <summary>Updates Index Backwards.</summary>
	/// <param name="_index">Index to Decrease.</param>
	/// <param name="_limit">Limit's Reference.</param>
	/// <returns>Decreased Index.</returns>
	public static int DecreaseIndex(int _index, int _limit)
	{
		return _index > 0 ? _index - 1 : _limit - 1;
	}

	/// <summary>Updates Index Backwar x amount of steps.</summary>
	/// <param name="_index">Index to Decrease.</param>
	/// <param name="_amount">Amount of steps.</param>
	/// <param name="_limit">Limit's Reference.</param>
	/// <returns>Decreased Index.</returns>
	public static int RegressIndex(int _index, int _amount, int _limit)
	{
		//return _limit % Mathf.Abs(_index - _amount);
		return _limit > 0 ? _index - (_amount % _limit) : 0;
	}

	/// <summary>Clears all Circular Buffer's Data.</summary>
	public void Clear()
	{
		index = 0;
		nextIndex = 0;
		occupiedSlots = 0;
	}

	/// <summary>Iterates through the buffer progressively [Clockwise].</summary>
	/// <param name="_index">Index to begin with [-1 by default that, if default, sets it to the current index].</param>
	public IEnumerator<T> IterateForward(int _index = -1)
	{
		if(_index < 0 || _index >= size) _index = tailIndex;
		
		T current = list[_index];
		int currentIndex = _index;
		int iterations = 0;

		if(occupiedSlots > 0)
		while(iterations < occupiedSlots)
		{
			yield return current;
			currentIndex = IncreaseIndex(currentIndex, occupiedSlots);
			current = list[currentIndex];
			iterations++;

		}
	}

	/// <summary>Iterates through the buffer regressively [Counter-Clockwise].</summary>
	/// <param name="_index">Index to begin with [-1 by default that, if default, sets it to the current index].</param>
	public IEnumerator<T> IterateBackward(int _index = -1)
	{
		if(_index < 0 || _index >= size) _index = index;
		
		T current = list[_index];
		int currentIndex = _index;
		int iterations = 0;

		if(occupiedSlots > 0)
		while(iterations < occupiedSlots)
		{
			yield return current;
			currentIndex = DecreaseIndex(currentIndex, occupiedSlots);
			current = list[currentIndex];
			iterations++;

		}
	}

	/// <returns>Returns an enumerator that iterates through the List's items.</returns>
	public IEnumerator<T> GetEnumerator()
	{
		IEnumerator<T> iterator = IterateForward();
		T current = default(T);

		while(iterator.MoveNext())
		{
			current = iterator.Current;
			yield return current;
		}
	}

	/// <returns>Returns an enumerator that iterates through the List's items.</returns>
	IEnumerator IEnumerable.GetEnumerator()
	{
		yield return GetEnumerator();
	}

	/// <summary>Copies the elements of the ICollection to an Array, starting at a particular Array index.</summary>
    public void CopyTo(Array _array, int _index)
    {
    	ICollection collection = list;
    	collection.CopyTo(_array, _index);
    }

    /// <returns>String representing this CircularBuffer.</returns>
    public override string ToString()
    {
    	StringBuilder builder = new StringBuilder();

    	builder.Append("Size: ");
    	builder.Append(size.ToString());
    	builder.Append("\nTail Index: ");
    	builder.Append(tailIndex.ToString());
    	builder.Append("\nCurrent Index: ");
    	builder.Append(index.ToString());
    	builder.Append("\nNext Index: ");
    	builder.Append(nextIndex.ToString());
    	builder.Append("\nOccupied Slots: ");
    	builder.Append(occupiedSlots.ToString());
    	builder.Append("\nContents:");

    	foreach(T item in list)
    	{
    		builder.Append("\n");
    		builder.Append(item != null ? item.ToString() : "NULL");
    	}

    	return builder.ToString();
    }
}
}