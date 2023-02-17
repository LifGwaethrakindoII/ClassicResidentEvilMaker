using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class ReorientedJoint : MonoBehaviour
{
	[SerializeField] private Transform _reference; 					/// <summary>Reference's Transform.</summary>
	[SerializeField] private EulerRotation _rotationOffset; 		/// <summary>Rotation's Offset.</summary>
	private Quaternion _rotation; 									/// <summary>Reoriented's Rotation.</summary>
	private Vector3 _right; 										/// <summary>Right's Reoriented Local Vector.</summary>
	private Vector3 _up; 											/// <summary>Up's Reoriented Local Vector.</summary>
	private Vector3 _forward; 										/// <summary>Forward's Reoriented Local Vector.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[SerializeField] private float rayLength; 						/// <summary>Ray's Length.</summary>
#endif

	/// <summary>Gets and Sets reference property.</summary>
	public Transform reference
	{
		get { return _reference; }
		set { _reference = value; }
	}

	/// <summary>Gets and Sets rotationOffset property.</summary>
	public EulerRotation rotationOffset
	{
		get { return _rotationOffset; }
		set { _rotationOffset = value; }
	}

	/// <summary>Gets referenceRotation property.</summary>
	public Quaternion referenceRotation { get { return reference != null ? reference.rotation : Quaternion.identity; } }

	/// <summary>Gets and Sets rotation property.</summary>
	public Quaternion rotation
	{
		get { return _rotation; }
		private set { _rotation = value; }
	}

	/// <summary>Gets and Sets right property.</summary>
	public Vector3 right
	{
		get { return _right; }
		private set { _right = value; }
	}

	/// <summary>Gets and Sets up property.</summary>
	public Vector3 up
	{
		get { return _up; }
		private set { _up = value; }
	}

	/// <summary>Gets and Sets forward property.</summary>
	public Vector3 forward
	{
		get { return _forward; }
		private set { _forward = value; }
	}

#if UNITY_EDITOR
	/// <summary>Draws Gizmos on Editor mode when ReorientedJoint's instance is selected.</summary>
	private void OnDrawGizmosSelected()
	{
		if(!Application.isPlaying) UpdateRotation();

		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, right * rayLength);
		Gizmos.color = Color.green;
		Gizmos.DrawRay(transform.position, up * rayLength);
		Gizmos.color = Color.blue;
		Gizmos.DrawRay(transform.position, forward * rayLength);
	}
#endif	

	/// <summary>Callback invoked when ReorientedJoint's instance is enabled.</summary>
	private void OnEnable()
	{
		UpdateRotation();
	}

	/// <summary>CorrectedJoint's tick at each frame.</summary>
	private void Update ()
	{
		UpdateRotation();
	}

	/// <summary>Updates Reoriented Rotation.</summary>
	private void UpdateRotation()
	{
		rotation = transform.rotation * rotationOffset;
		
		right = rotation * Vector3.right;
		up = rotation * Vector3.up;
		forward = rotation * Vector3.forward;
	}

	/// <summary>Updates Rotation Offset if it has a Reference Transform.</summary>
	public void UpdateRotationOffset()
	{
		if(reference == null) return;
		rotationOffset = VQuaternion.Delta(reference.rotation, transform.rotation);
	}
}
}