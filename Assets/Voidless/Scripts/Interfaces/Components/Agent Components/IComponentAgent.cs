using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public interface IComponentAgent<T, R> where T : IComponentAgent<T, R>
{
	IAgentComponent<T, R> rootComponent { get; set; } 	/// <summary>Root's Component.</summary>
	R currentComponentIterator { get; set; } 			/// <summary>Current Component's iterator.</summary>
}
}