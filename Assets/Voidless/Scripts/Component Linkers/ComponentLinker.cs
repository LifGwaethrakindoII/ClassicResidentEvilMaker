using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public abstract class ComponentLinker<T> : MonoBehaviour where T : Component
{
	[SerializeField] private T _component; 	/// <summary>Component.</summary>

	/// <summary>Gets and Sets component property.</summary>
	public T component
	{
		get { return _component; }
		set { _component = value; }
	}
}
}