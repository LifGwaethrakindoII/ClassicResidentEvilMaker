using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

namespace Voidless
{
public static class VArray
{
	/// <summary>Bubble-sorts given array.</summary>
	/// <param name="_array">Array's reference.</param>
	public static T[] BubbleSort<T>(this T[] _array, Func<T, T, bool> condition)
	{
		if(_array == null) return null;

		int length = _array.Length;

		if(length == 0) return null;

/*
		int n = arr.Length;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (arr[j] > arr[j + 1]) {
                    // swap temp and arr[i]
                    int temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;
                }
            }
        }
*/

		for(int i = 0; i < length - 1; i++)
		{
			for(int j = 0; j < length - i - 1; j++)
			{
				T a = _array[j];
				T b  = _array[j + 1];

				if(condition(a, b))
				{
					T temp = a;
					_array[j] = b;
					_array[j + 1] = temp;
				}
			}
		}

		return _array;
	}

	/// <summary>Balances 2 arrays, so they are of the same size.</summary>
	/// <param name="a">Array A.</param>
	/// <param name="b">Array B.</param>
	public static void Balance<T>(ref T[] a, ref T[] b)
	{
		if(a == null || b == null) return;

		int difference = a.Length - b.Length;
		int min = Mathf.Min(a.Length, b.Length);

		if(difference == 0) return;

		else if(difference > 0)
		{ /// A > B
			Array.Resize(ref a, min);

		} else if(difference < 0)
		{ /// B > A
			Array.Resize(ref b, min);
		}
	}

	/// <returns>Randomized Array.</returns>
	public static T[] Randomized<T>(this T[] _array)
	{
		return RandomSet<T>(_array);
	}

	/// <summary>Resizes Array, initializes it if it is null.</summary>
	/// <param name="_array">Array to resize.</param>
	public static void Resize<T>(ref T[] _array, int _size)
	{
		if(_array == null) _array = new T[_size];
		else Array.Resize(ref _array, _size);
	}

	/// <returns>Array from given array with shuffled elements.</returns>
	public static T[] RandomSet<T>(params T[] _elements)
	{
		if(_elements == null) return null;

		int size = _elements.Length;
		int[] indices = VMath.GetUniqueRandomSet(size);
		T[] array = new T[size];

		for(int i = 0; i < size; i++)
		{
			array[i] = _elements[indices[i]];
		}

		return array;
	}

	/// <returns>Max length of matrix.</returns>
	public static int GetArrayMaxLength<T>(params T[][] _arrays)
	{
		int maxSize = 0;

		foreach(T[] array in _arrays)
		{
			if(array.Length > maxSize) maxSize = array.Length;
		}

		return maxSize;
	}

	/// <summary>Initializes array of T elements to their default constructors.</summary>
	/// <param name="_array">Array to initialize.</param>
	/// <param name="_length">Length of the array.</param>
	/// <returns>Initialized Array of T elements.</returns>
	public static T[] InitializeArray<T>(this T[] _array, int _length) where T : new()
	{
	    T[] array = new T[_length];

	    for (int i = 0; i < _length; ++i)
	    {
	        array[i] = new T();
	    }

	    return array;
	}

	/// <summary>Checks if all elements on list accomplishes all conditions.</summary>
	/// <param name="_array">Array that will have all its elements evaluated.</param>
	/// <param name="_condition">Condition that all elements have to accomplish for the method to return true.</param>
	/// <returns>If all elements on list accomplish the condition.</returns>
	public static bool AllAccomplish<T>(this T[] _array, System.Predicate<T> _condition)
	{
		foreach(T element in _array)
		{
			if(!_condition(element)) return false;
		}

		return true;
	}

	/// <summary>Gets last element on given array.</summary>
	/// <param name="_array">Array to get last element from.</param>
	/// <returns>Array's last element.</returns>
	public static T GetLast<T>(this T[] _array)
	{
		return _array[_array.Length - 1];
	}

	/// <summary>Gets a random element from given array.</summary>
	/// <param name="_array">Array to retreive random element from.</param>
	/// <returns>Random element from array.</returns>
	public static T Random<T>(this T[] _array)
	{
		return _array != null ? _array[UnityEngine.Random.Range(0, _array.Length)] : default(T);
	}

#region Matrices:
	/// <summary>Gets length of the rows in a matrix.</summary>
	/// <param name="_matrix">Matrix to extend.</param>
	/// <returns>Length of rows.</returns>
	public static int GetRows<T>(this T[,] _matrix) { return _matrix.GetLength(0); }

	/// <summary>Gets length of the columns in a matrix.</summary>
	/// <param name="_matrix">Matrix to extend.</param>
	/// <returns>Length of columns.</returns>
	public static int GetColumns<T>(this T[,] _matrix) { return _matrix.GetLength(1); }

	/// <summary>Transposes matrix.</summary>
	/// <param name="_matrix">Matrix to transpose.</param>
	public static void Transpose<T>(this T[,] _matrix)
	{
		T[,] transposedMatrix = new T[_matrix.GetColumns(), _matrix.GetRows()];

		for(int i = 0; i < _matrix.GetRows(); i++)
		{
			for(int j = 0; j < _matrix.GetColumns(); j++)
			{
				transposedMatrix[j, i] = _matrix[i, j];
			}
		}

		_matrix = transposedMatrix;
	}

	/// <summary>Applies a function to each element of the matrix.</summary>
	/// <param name="_matrix">Matrix to apply formula to each one of its elements.</param>
	public static void Map<T>(this T[,] _matrix, Func<T, T> f)
	{
		for(int i = 0; i < _matrix.GetRows(); i++)
		{
			for(int j = 0; j < _matrix.GetColumns(); j++)
			{
				T element = _matrix[i, j];
				_matrix[i, j] = f(element);
			}
		}
	}

	public static float[,] DotProduct(float[,] a, float[,] b)
	{
		if(a.GetRows() == b.GetColumns() && a.GetColumns() == b.GetRows())
		{
			float[,] product = new float[2, 2];
			return product;
		}
		else return null;
	}
#endregion
}
}