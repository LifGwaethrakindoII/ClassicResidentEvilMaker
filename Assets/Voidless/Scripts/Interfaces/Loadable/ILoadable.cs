using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public interface ILoadable
{
	bool Loaded { get; set; } 	/// <summary>Has the Object been Loaded?.</summary>
}
}