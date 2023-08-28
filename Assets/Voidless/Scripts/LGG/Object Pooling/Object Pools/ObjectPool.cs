using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*===========================================================================
**
** Class:  ObjectPool
**
** Purpose: Object-Pool class that receives as a reference a class that
** implementsthe the IPoolObject interface.
**
**
** Author: Lîf Gwaethrakindo
**
===========================================================================*/
namespace LGG
{
	[Serializable]
	public class ObjectPool<T> : BaseObjectPool<T> where T : class, IPoolObject, new()
	{
		/// <summary>ObjectPool's Constructor.</summary>
		/// <param name="_referenceObject">Pool's Reference Prefab.</param>
		/// <param name="_size">Pool's starting size.</param>
		/// <param name="_limit">Pool's Limit.</param>
		/// <param name="_limitHandling">How to handle when Pool's size surpasses the limit.</param>
		public ObjectPool(T _referenceObject, int _size = 0, int _limit = int.MaxValue, LimitHandling _limitHandling = LimitHandling.None) : base(_referenceObject, _size, _limit, _limitHandling)
		{ /*...*/ }

		/// <summary>Creates Pool-Object.</summary>
		/// <returns>Created Pool-Object.</returns>
		protected override T CreatePoolObject()
		{
			if(referenceObject == null) return new T();
			else
			{
				ICloneable cloneable = referenceObject as ICloneable;
				return cloneable != null ? cloneable.Clone() as T : new T();
			}
		}
	}
}