using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class ReorientedTransform : MonoBehaviour
{
	[SerializeField] private IdentityVector _forwardReference; 	/// <summary>Forward Vector's Reference [in local space].</summary>
	[SerializeField] private Axes3D _invertAxes; 				/// <summary>Axes to invert.</summary>
	[SerializeField] private EulerRotation _rotationOffset; 	/// <summary>Rotation's Offset.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[SerializeField] private float rayLength; 					/// <summary>Ray's Length.</summary>
#endif
	private Quaternion _deltaRotation; 							/// <summary>Difference of rotation between the original forward vector and the reference vector.</summary>
	private Vector3 _referenceForward; 							/// <summary>Reference forward vector.</summary>
	private Vector3 _right; 									/// <summary>Reoriented Right's Vector.</summary>
	private Vector3 _up; 										/// <summary>Reoriented Up's Vector.</summary>
	private Vector3 _forward; 									/// <summary>Reoriented Forward's Vector.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets forwardReference property.</summary>
	public IdentityVector forwardReference
	{
		get { return _forwardReference; }
		set
		{
			_forwardReference = value;
			UpdateReferenceForward();
		}
	}

	/// <summary>Gets and Sets invertAxes property.</summary>
	public Axes3D invertAxes
	{
		get { return _invertAxes; }
		set { _invertAxes = value; }
	}

	/// <summary>Gets and Sets rotationOffset property.</summary>
	public EulerRotation rotationOffset
	{
		get { return _rotationOffset; }
		set { _rotationOffset = value; }
	}

	/// <summary>Gets and Sets deltaRotation property.</summary>
	public Quaternion deltaRotation
	{
		get { return _deltaRotation; }
		private set { _deltaRotation = value; }
	}

	/// <summary>Gets rotation property.</summary>
	public Quaternion rotation { get { return transform.rotation * rotationOffset * deltaRotation; } }

	/// <summary>Gets and Sets referenceForward property.</summary>
	public Vector3 referenceForward
	{
		get { return _referenceForward; }
		private set { _referenceForward = value; }
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
#endregion

#if UNITY_EDITOR
	/// <summary>Draws Gizmos on Editor mode when ReorientedTransform's instance is selected.</summary>
	private void OnDrawGizmosSelected()
	{
		if(!Application.isPlaying)
		{
			UpdateReferenceForward();
			UpdateReorientedNormals();
		}

		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, right * rayLength);
		Gizmos.color = Color.green;
		Gizmos.DrawRay(transform.position, up * rayLength);
		Gizmos.color = Color.blue;
		Gizmos.DrawRay(transform.position, forward * rayLength);

		Gizmos.color = Color.cyan;
		Gizmos.DrawRay(transform.position, rotation * Vector3.forward * rayLength);
	}
#endif

	/// <summary>Resets ReorientedTransform's instance to its default values.</summary>
	public void Reset()
	{
#if UNITY_EDITOR
		rayLength = 1.0f;
#endif
		forwardReference = IdentityVector.Right;
		invertAxes = Axes3D.None;
	}

	/// <summary>ReorientedTransform's instance initialization.</summary>
	private void Awake()
	{
		UpdateReferenceForward();
	}
	
	/// <summary>ReorientedTransform's tick at each frame.</summary>
	private void Update ()
	{
		UpdateReorientedNormals();
	}

	/// <summary>Rotates towards given direction.</summary>
	/// <param name="d">Direction.</param>
	public void RotateTowards(Vector3 d, Vector3 _up = new Vector3())
	{
		if(_up.sqrMagnitude == 0.0f) _up = ((invertAxes | Axes3D.X) == invertAxes || (invertAxes | Axes3D.Y) == invertAxes) ? Vector3.down : Vector3.up;
		RotateTowards(Quaternion.LookRotation(d, _up));
	}

	/// <summary>Rotates towars given look rotations
	/// <param name="_lookRotation">Look's Rotation.</param>.</summary>
	public void RotateTowards(Quaternion _lookRotation)
	{
		transform.rotation = _lookRotation * Quaternion.Inverse(deltaRotation) * Quaternion.Inverse(rotationOffset);
	}

	/// <summary>Updates Reference Forward.</summary>
	private void UpdateReferenceForward()
	{
		switch(forwardReference)
		{
			case IdentityVector.Right:
			referenceForward = Vector3.right;
			break;

			case IdentityVector.Up:
			referenceForward = Vector3.up;
			break;

			case IdentityVector.Forward:
			referenceForward = Vector3.forward;
			break;
		}

		deltaRotation = Quaternion.FromToRotation(Vector3.forward, referenceForward);
	}

	/// <summary>Updates Reoriented Normals.</summary>
	private void UpdateReorientedNormals()
	{
		Quaternion rotation = transform.rotation * rotationOffset;

		forward = rotation * referenceForward;
		
		switch(forwardReference)
		{
			case IdentityVector.Right:
			up = rotation * Vector3.up;
			right = Vector3.Cross(up, forward);
			break;

			case IdentityVector.Up:
			right = rotation * -Vector3.right;
			up = Vector3.Cross(forward, right);
			break;

			case IdentityVector.Forward:
			right = rotation * Vector3.right;
			up = rotation * Vector3.up;
			break;
		}

		if((invertAxes | Axes3D.X) == invertAxes) right *= -1.0f;
		if((invertAxes | Axes3D.Y) == invertAxes) up *= -1.0f;
		if((invertAxes | Axes3D.Z) == invertAxes) forward *= -1.0f;
	}
}
}

/*
You may know that a rotation can be represented by a quaternion of the following form:

cos (phi / 2)
sin (phi / 2) * axis.x
sin (phi / 2) * axis.y
sin (phi / 2) * axis.z
axis is the rotation axis and phi is the rotation angle. These are the two measures you need to define your quaternion.

There are multiple rotations that map a vector from to another vector to. The shortest rotation is the one where the axis is perpendicular to both of the vectors. Hence, the axis is:

axis = normalize(from x to)
x denotes the cross product.

And the angle is the angle between the two vectors:

phi = acos(dot(from, to) / (norm(from) * norm(to))
norm is the vector norm or vector length.

With these values, you can then calculate the quaternion.
*/