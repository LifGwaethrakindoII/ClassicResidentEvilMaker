using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public struct DistributedRanges
{
	[SerializeField] private FloatRange _limits; 	/// <summary>Range's Limits.</summary>
	[SerializeField] private FloatRange[] _ranges; 	/// <summary>Ranges.</summary>

	/// <summary>Gets and Sets limits property.</summary>
	public FloatRange limits
	{
		get { return _limits; }
		set { _limits = value; }
	}

	/// <summary>Gets and Sets ranges property.</summary>
	public FloatRange[] ranges
	{
		get { return _ranges; }
		set { _ranges = value; }
	}
}
}