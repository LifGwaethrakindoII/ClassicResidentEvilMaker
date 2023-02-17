using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Voidless
{
/// <summary>Event invoked when the CoroutineBehavior ends.</summary>
public delegate void OnCoroutineEnds();

public abstract class CoroutineBehavior<T> : MonoBehaviour
{
	public event OnCoroutineEnds onCoroutineEnds; 	/// <summary>OnCoroutineEnds event delegate.</summary>

//#if UNITY_EDITOR
	[Space(5f)]
	[Header("Gizmos' Attributes:")]
	[TabGroup("Gizmos Group", "Gizmos")][SerializeField] public bool drawGizmos; 		/// <summary>Draw Gizmos?.</summary>
	[TabGroup("Gizmos Group", "Gizmos")][SerializeField] public Color gizmosColor; 		/// <summary>Gizmos' Color.</summary>
	[TabGroup("Gizmos Group", "Gizmos")][SerializeField] public float gizmosRadius; 	/// <summary>Gizmos' Radius.</summary>	
//#endif

	/// <summary>Draws Gizmos [if drawGizmos' flag is turned on].</summary>
	protected virtual void DrawGizmos() { /*...*/ }

	/// <summary>Draws Gizmos on Editor mode when CoroutineBehavior's instance is selected.</summary>
	private void OnDrawGizmosSelected()
	{
//#if UNITY_EDITOR
		if(drawGizmos) DrawGizmos();
//#endif
	}

	/// <summary>Coroutine's IEnumerator.</summary>
	/// <param name="obj">Object of type T's argument.</param>
	public virtual IEnumerator Routine(T obj) { yield return null; }

	/// <summary>Invokes OnCoroutineEnds' delegate.</summary>
	public void InvokeCoroutineEnd() { if(onCoroutineEnds != null) onCoroutineEnds(); }
}
}