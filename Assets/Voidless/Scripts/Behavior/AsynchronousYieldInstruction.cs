using System;
using System.Collections;
using System.Collections.Generic;

namespace Voidless
{
#pragma warning disable 67, 649
public class AsynchronousYieldInstruction<T> : IEnumerator<T>
{
	private IEnumerator _enumerator; 	/// <summary>Asynchronous Behavior's Yield Instruction.</summary>
	
	/// <summary>Gets and Sets enumerator property.</summary>
	public IEnumerator enumerator
	{
		get { return _enumerator; }
		private set { enumerator = value; }
	}

#region IEnumeratorTProperties:
	/// <summary>Gets Current property.</summary>
	T IEnumerator<T>.Current { get { return default(T); } }

	/// <summary>Gets Current property.</summary>
	Object IEnumerator.Current { get { return enumerator.Current; } }
#endregion

	/// <summary>AsynchronousYieldInstruction's constructor.</summary>
	/// <param name="_enumerator">Yield Instruction's IEnumerator.</param>
	public AsynchronousYieldInstruction(IEnumerator _enumerator)
	{
		enumerator = _enumerator;
	}

#region IEnumeratorTMethods:
	/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
	public void Dispose()
	{
		throw new NotImplementedException();
	}

	/// <summary>Advances the enumerator to the next element of the collection.</summary>
	public bool MoveNext()
	{
		return enumerator.MoveNext();
	}

	/// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
	public void Reset()
	{
		enumerator.Reset();
	}
#endregion

}
}