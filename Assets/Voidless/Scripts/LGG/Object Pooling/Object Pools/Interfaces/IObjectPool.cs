using System.Collections;
using System.Collections.Generic;

/*===========================================================================
**
** Interface:  IObjectPool
**
** Purpose: Object-Pool Interface. Maybe it is an innecessary interface, the
** base reference object should be BaseObjectPool<T> without this interface.
**
**
** Author: Lîf Gwaethrakindo
**
===========================================================================*/
namespace LGG
{
	public enum LimitHandling { None, RecycleRandom }

	public interface IObjectPool<out T> : IEnumerable<T> where T : IPoolObject
	{
		/// <summary>Gets referenceObject property.</summary>
		T referenceObject { get; }
		/// <summary>Gets and Sets limit property.</summary>
		int limit { get; set; }
		/// <summary>Gets occupiedSlotsCount property.</summary>
		int occupiedSlotsCount { get; }
		/// <summary>Gets vacantSlotsCount property.</summary>
		int vacantSlotsCount { get; }
		/// <summary>Gets And Sets limitHandling.</summary>
		LimitHandling limitHandling { get; set; }

		/// <summary>Adds Pool Object.</summary>
		/// <returns>Added Pool Object.</returns>
		T Add();

		/// <summary>Recycles Pool Object from queue [dequeues], then it enqueues is again.</summary>
		/// <returns>Recycled Pool Object.</returns>
		T Recycle();

		/// <summary>Dispatched Pool Object and returns it.</summary>
		/// <returns>Dispatched Pool Object.</returns>
		T Dispatch();

		/// <summary>Deactivates provided Pool's Object, liberating it for a possible recycling.</summary>
		/// <param name="_poolObject">Pool Object to deactivate.</param>
		void Deactivate(IPoolObject _poolObject);

		/// <summary>Callback when evaluation for Pool-Objects' destruction should occur.</summary>
		void OnObjectsToDestroyEvaluation();
	}

///Example of contravariant and covariant interface, by implementing in and out interfaces:
/*public interface In<in T>{ T x { set; } }
public interface Out<out T>{ T x { get; } }
public interface InAndOut<T> : In<T>, Out<T> {  }*/
}