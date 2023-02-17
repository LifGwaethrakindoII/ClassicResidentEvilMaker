using System.Collections;
using System.Collections.Generic;

namespace Voidless
{
public interface IObjectPool<out T> where T : IPoolObject
{
	T referenceObject { get; } 				/// <summary>Gets referenceObject property.</summary>
	int limit { get; set; } 				/// <summary>Gets and Sets limit property.</summary>
	int occupiedSlotsCount { get; set; } 	/// <summary>Gets and Sets occupiedSlotsCount property.</summary>
	int vacantSlotsCount { get; set; } 		/// <summary>Gets and Sets vacantSlotsCount property.</summary>

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

	/// <summary>Evaluates which objects to destroy.</summary>
	void EvaluateObjectsToDestroy();
}

///Example of contravariant and covariant interface, by implementing in and out interfaces:
/*public interface In<in T>{ T x { set; } }
public interface Out<out T>{ T x { get; } }
public interface InAndOut<T> : In<T>, Out<T> {  }*/
}