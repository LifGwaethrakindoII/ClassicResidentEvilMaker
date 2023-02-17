using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public enum IdentityVector 										/// <summary>Identity Vectors { i, j, k }.</summary>
{
	Right, 														/// <summary>Right Identity Vector [i].</summary>
	Up, 														/// <summary>Up Identity Vectror [j].</summary>
	Forward 													/// <summary>Forward Identity Vector [k].</summary>
}

public enum RightRelativeIdentityVector 						/// <summary>Relative Identity Vector to calculate Right normal.</summary>
{
	Up = IdentityVector.Up, 									/// <summary>Up.</summary>
	Forward = IdentityVector.Forward 							/// <summary>Forward.</summary>
}

public enum UpRelativeIdentityVector 							/// <summary>Relative Identity Vector to calculate Up normal.</summary>
{
	Right = IdentityVector.Right, 								/// <summary>Right.</summary>
	Forward = IdentityVector.Forward 							/// <summary>Forward.</summary>
}

public enum ForwardRelativeIdentityVector 						/// <summary>Relative Identity Vector to calculate Forward normal.</summary>
{
	Right = IdentityVector.Right, 								/// <summary>Right.</summary>
	Up = IdentityVector.Up 										/// <summary>Up.</summary>
}

public class OrientationNormalAdjuster : MonoBehaviour
{
	[SerializeField] private bool _canChangeRelativeNormal; 	/// <summary>Can the relative normal be changed by outside classes?.</summary>
	[SerializeField] private IdentityVector _normalToAdjust; 	/// <summary>Normal To Adjust.</summary>
	[SerializeField] private IdentityVector _relativeNormal; 	/// <summary>Relative Normal's reference to calculate all relative normals from.</summary>
	[SerializeField] private Vector3 _right; 					/// <summary>Relative Right's Normal.</summary>
	[SerializeField] private Vector3 _up; 						/// <summary>Relative Up's Normal.</summary>
	[SerializeField] private Vector3 _forward; 					/// <summary>Relative Forward's Normal.</summary>
	[SerializeField] private Vector3 _normal; 					/// <summary>Resultant Relative's Normal.</summary>
#if UNITY_EDITOR
	[SerializeField] private float _normalLength; 				/// <summary>Normal's Length.</summary>

	/// <summary>Gets and Sets normalLength property.</summary>
	public float normalLength
	{
		get { return _normalLength; }
		set { _normalLength = value; }
	}
#endif

#region Getters/Setters:
	/// <summary>Gets and Sets canChangeRelativeNormal property.</summary>
	public bool canChangeRelativeNormal
	{
		get { return _canChangeRelativeNormal; }
		set { _canChangeRelativeNormal = value; }
	}

	/// <summary>Gets and Sets normalToAdjust property.</summary>
	public IdentityVector normalToAdjust
	{
		get { return _normalToAdjust; }
		set { _normalToAdjust = value; }
	}

	/// <summary>Gets and Sets relativeNormal property.</summary>
	public IdentityVector relativeNormal
	{
		get { return _relativeNormal; }
		set { _relativeNormal = value; }
	}

	/// <summary>Gets and Sets right property.</summary>
	public Vector3 right
	{
		get { return _right; }
		set { _right = value; }
	}

	/// <summary>Gets and Sets up property.</summary>
	public Vector3 up
	{
		get { return _up; }
		set { _up = value; }
	}

	/// <summary>Gets and Sets forward property.</summary>
	public Vector3 forward
	{
		get { return _forward; }
		set { _forward = value; }
	}

	/// <summary>Gets and Sets normal property.</summary>
	public Vector3 normal
	{
		get { return _normal; }
		private set { _normal = value; }
	}
#endregion

	/// <summary>Draws Gizmos.</summary>
	private void OnDrawGizmos()
	{
		DrawRelativeNormals();
	}
	
	/// <summary>OrientationNormalAdjuster's tick at each frame.</summary>
	private void Update ()
	{
		CalculateRelativeNormals();
	}

	/// <summary>Resets Component.</summary>
	private void Reset()
	{
		right = Vector3.right;
		up = Vector3.up;
		forward = Vector3.forward;
		normalToAdjust = IdentityVector.Forward;
		relativeNormal = IdentityVector.Up;

#if UNITY_EDITOR
		normalLength = 1.0f;
#endif
	}

	/// <summary>Calculates Relative Normals.</summary>
	private void CalculateRelativeNormals()
	{
		switch(normalToAdjust)
		{
			case IdentityVector.Right:
			if(relativeNormal == IdentityVector.Forward) up = transform.up;
			else if(!canChangeRelativeNormal) forward = transform.forward;
			right = Vector3.Cross(up, forward);
			break;

			case IdentityVector.Up:
			if(relativeNormal == IdentityVector.Forward) right = transform.right;
			else if(!canChangeRelativeNormal) forward = transform.forward;
			up = Vector3.Cross(right, forward);
			break;

			case IdentityVector.Forward:
			if(relativeNormal == IdentityVector.Up) right = transform.right;
			else if(!canChangeRelativeNormal) up = transform.up;
			forward = Vector3.Cross(right, up);
			break;
		}

		normal = (right + up + forward);
	}

	/// <summary>Draws Relative Normals.</summary>
	private void DrawRelativeNormals()
	{
#if UNITY_EDITOR
		if(!Application.isPlaying) CalculateRelativeNormals();
		VGizmos.DrawGizmoRay(transform.position, right, normalLength, Color.red);
		VGizmos.DrawGizmoRay(transform.position, up, normalLength, Color.green);
		VGizmos.DrawGizmoRay(transform.position, forward, normalLength, Color.blue);
#endif
	}
}
}