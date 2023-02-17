using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[ExecuteInEditMode]
public class Collider2DAlligner : MonoBehaviour
{
	[SerializeField] private Transform _relativeTo; 		/// <summary>Transform's Relativeness.</summary>
	[SerializeField] private Transform _secondRelativeTo; 	/// <summary>Optional Transform's Relativeness for Vector B.</summary>
	[SerializeField] private Vector3 _a; 					/// <summary>Vector A [relative to transform].</summary>
	[SerializeField] private Vector3 _b; 					/// <summary>Vector B [relative to transform].</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Gizmos' Attributes:")]
	[SerializeField] private Color gizmosColor; 			/// <summary>Gizmos' Color.</summary>
	[SerializeField] private float gizmosRadius; 			/// <summary>Gizmos' Radius.</summary>
#endif

	/// <summary>Gets and Sets relativeTo property.</summary>
	public Transform relativeTo
	{
		get { return _relativeTo; }
		set { _relativeTo = value; }
	}

	/// <summary>Gets and Sets secondRelativeTo property.</summary>
	public Transform secondRelativeTo
	{
		get { return _secondRelativeTo; }
		set { _secondRelativeTo = value; }
	}

	/// <summary>Gets and Sets a property.</summary>
	public Vector3 a
	{
		get { return _a; }
		set { _a = value; }
	}

	/// <summary>Gets and Sets b property.</summary>
	public Vector3 b
	{
		get { return _b; }
		set { _b = value; }
	}

#if UNITY_EDITOR
	/// <summary>Draws Gizmos on Editor mode when Collider2DAlligner's instance is selected.</summary>
	private void OnDrawGizmosSelected()
	{
		if(relativeTo == null) return;

		Gizmos.color = gizmosColor;
		Gizmos.DrawSphere(relativeTo.TransformPoint(a), gizmosRadius);
		Gizmos.DrawSphere(secondRelativeTo != null ? secondRelativeTo.TransformPoint(b) : relativeTo.TransformPoint(b), gizmosRadius);

		//if(!Application.isPlaying) UpdateCollider();
	}

	/// <summary>Resets Collider2DAlligner's instance to its default values.</summary>
	private void Reset()
	{
		gizmosColor = Color.white.WithAlpha(0.5f);
		gizmosRadius = 0.1f;
	}
#endif
	
	/// <summary>Collider2DAlligner's tick at each frame.</summary>
	private void Update ()
	{
		UpdateCollider();
	}

	/// <summary>Updates Collider2D.</summary>
	protected virtual void UpdateCollider() { /*...*/ }
}
}