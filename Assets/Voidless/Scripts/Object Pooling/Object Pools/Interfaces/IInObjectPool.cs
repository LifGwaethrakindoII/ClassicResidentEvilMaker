using System.Collections;
using System.Collections.Generic;

namespace Voidless
{
public interface IInObjectPool<T> where T : IPoolObject
{
	T referenceObject { set; } 	/// <summary>Sets referenceObject property.</summary>

	/// <summary>Deactivates provided Pool's Object, liberating it for a possible recycling.</summary>
	/// <param name="_poolObject">Pool Object to deactivate.</param>
	void Deactivate(T _poolObject);
}
}