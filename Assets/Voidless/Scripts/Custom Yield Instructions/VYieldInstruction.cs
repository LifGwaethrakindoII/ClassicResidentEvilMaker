using System;
using System.Collections;

namespace Voidless
{
public abstract class VYieldInstruction : IEnumerator
{
	/// <summary>Gets Current property.</summary>
	public virtual Object Current { get { return  null; } }

	/// <summary>VYieldInstruction's constructor.</summary>
	public VYieldInstruction()
	{

	}

	/// <summary>Advances the enumerator to the next element of the collection.</summary>
	public abstract bool MoveNext();

	/// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
	public virtual void Reset(){ /*...*/ }
}
}