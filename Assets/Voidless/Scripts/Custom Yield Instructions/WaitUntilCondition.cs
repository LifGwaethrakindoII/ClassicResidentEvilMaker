using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class WaitUntilCondition : VYieldInstruction
{
	private Func<bool> condition;

	/// <summary>Gets and Sets Condition property.</summary>
	public Func<bool> Condition
	{
		get { return condition; }
		set { condition = value; }
	}

	/// <summary>WaitUntilCondition's Constructor.</summary>
	/// <param name="_condition">Condition.</param>
	public WaitUntilCondition(Func<bool> _condition)
	{
		Condition = _condition;
	}

	/// <summary>Advances the enumerator to the next element of the collection.</summary>
	public override bool MoveNext()
	{
		return Condition != null ? !Condition() : false;
	}
}
}