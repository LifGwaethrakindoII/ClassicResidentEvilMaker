using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public abstract class BaseNodeData<N> : ScriptableObject
{
	[SerializeField] private N _root; 	/// <summary>Node Data's Root.</summary>

#region Getters:
	/// <summary>Gets and Sets root property.</summary>
	public N root
	{
		get { return _root; }
		set { _root = value; }
	}
#endregion

	void OnEnable()
	{
		//hideFlags = HideFlags.HideAndDontSave;
	}

	public abstract N GetNodeFlow();
}
}