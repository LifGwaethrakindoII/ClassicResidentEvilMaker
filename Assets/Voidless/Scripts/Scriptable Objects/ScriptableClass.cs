using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public abstract class ScriptableClass<T> : ScriptableObject where T : ScriptableObject
{
	/// <summary>ScriptableObject new() substitute.</summary>
	/// <returns>Instance of this ScriptableClass.</returns>
	public static T NewInstance()
	{
		CreateInstance<T>();
		return default(T);
	}
}
}