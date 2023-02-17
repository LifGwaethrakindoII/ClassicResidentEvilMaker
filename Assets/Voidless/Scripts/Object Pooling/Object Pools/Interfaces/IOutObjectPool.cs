using System.Collections;
using System.Collections.Generic;

namespace Voidless
{
public interface IOutObjectPool<out T> where T : IPoolObject
{
	T referenceObject { get; } 	/// <summary>Gets referenceObject property.</summary>
	
	/// <summary>Adds Pool Object.</summary>
	/// <returns>Added Pool Object.</returns>
	T Add();

	/// <summary>Recycles Pool Object from queue [dequeues], then it enqueues is again.</summary>
	/// <returns>Recycled Pool Object.</returns>
	T Recycle();

	/// <summary>Dispatched Pool Object and returns it.</summary>
	/// <returns>Dispatched Pool Object.</returns>
	T Dispatch();	
}
}