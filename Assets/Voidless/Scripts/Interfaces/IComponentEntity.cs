using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public interface IComponentEntity<T> where T : Component
{
	T component { get; set; } /// <summary>T's Component.</summary>
}
}