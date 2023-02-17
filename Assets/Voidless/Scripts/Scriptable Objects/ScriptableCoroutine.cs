using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public abstract class ScriptableCoroutine<T> : ScriptableObject
{
	public event OnCoroutineEnds onCoroutineEnds; 	/// <summary>OnCoroutineEnds event delegate.</summary>

#if UNITY_EDITOR
	[SerializeField] public bool drawGizmos; 		/// <summary>Draw Gizmos?.</summary>
#endif

	/// <summary>Draws Gizmos [if drawGizmos' flag is turned on].</summary>
	public void DrawGizmos()
	{
#if UNITY_EDITOR
		if(drawGizmos) OnDrawGizmos();
#endif
	}
	
	/// <summary>Draws Gizmos [only called internally if drawGizmos' flag is turned on].</summary>
	protected virtual void OnDrawGizmos() {/*...*/}

	/// <summary>Coroutine's IEnumerator.</summary>
	/// <param name="obj">Object of type T's argument.</param>
	public virtual IEnumerator Routine(T obj) { yield return null; }

	/// <summary>Invokes OnCoroutineEnds' delegate.</summary>
	public void InvokeCoroutineEnd() { if(onCoroutineEnds != null) onCoroutineEnds(); }
}
}