using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*============================================================
**
** Class:  Wrapper<T>
**
** Purpose: This generic class lets structs be used as reference types. It is very similar to a
** Node<T> class, except that it has less properties (therefore less memory) and implicit conversions.
**
**
** Author: Lîf Gwaethrakindo
**
==============================================================*/

namespace Voidless
{
[Serializable]
public class Wrapper<T> where T : struct
{
	public T value; 	/// <summary>Wrapper's Value.</summary>

	/// <summary>Implicit Wrapper<T> to T operator.</summary>
	public static implicit operator T(Wrapper<T> _wrapper) { return _wrapper.value; }

	/// <summary>Implicit T to Wrapper<T> operator.</summary>
	public static implicit operator Wrapper<T>(T _value) { return new Wrapper<T>(_value); }

	/// <summary>Wrapper default constructor.</summary>
	/// <param name="_value">Wrapper's Value [if you don't pass anything, value will be equal to 0 bits activated].</param>
	public Wrapper(T _value = default(T))
	{
		value = _value;
	}

	/// <returns>String representing the value contained within the Wrapper.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("Wrapper <");
		builder.Append(typeof(T).Name);
		builder.Append(">: ");
		builder.Append(value.ToString());

		return builder.ToString();
	}
}

[Serializable] public class FloatWrapper : Wrapper<float> {  public FloatWrapper(float _value) : base(_value) {} }
}