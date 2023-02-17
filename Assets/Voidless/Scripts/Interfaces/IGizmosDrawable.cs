using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public interface IGizmosDrawable
{
#if UNITY_EDITOR
	bool drawWhenSelected { get; set; }		/// <summary>Draw Gizmos when object is selected?.</summary>

	/// <summary>Draws Gizmos (Must be called by either OnDrawGizmos or OnDrawGizmosSelected).</summary>
	void DrawGizmos();
#endif
}
}