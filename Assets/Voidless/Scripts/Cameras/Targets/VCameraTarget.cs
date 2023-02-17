using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class VCameraTarget : MonoBehaviour
{
	[SerializeField]
	[Range(0.0f, 1.0f)] private float _weight; 			/// <summary>Target's Weight.</summary>
	[SerializeField] private Space _space; 				/// <summary>Space's Reletiveness of the Bounds.</summary>
	[SerializeField] private Vector3 _size; 			/// <summary>Bounds' Size.</summary>
	[SerializeField] private Vector3 _centerOffset; 	/// <summary>Bounds' Center's Offset.</summary>
	[SerializeField] private bool _scaleSize; 			/// <summary>Scale the Bounds' size with the Transform's Scale?.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[SerializeField] private Color gizmosColor; 		/// <summary>Gizmos' Color.</summary>
#endif

#region Getters/Setters:
	/// <summary>Gets and Sets weight property.</summary>
	public float weight
	{
		get { return _weight; }
		set { _weight = Mathf.Clamp(value, 0.0f, 1.0f); }
	}

	/// <summary>Gets and Sets space property.</summary>
	public Space space
	{
		get { return _space; }
		set { _space = value; }
	}

	/// <summary>Gets and Sets size property.</summary>
	public Vector3 size
	{
		get { return _size; }
		set { _size = value; }
	}

	/// <summary>Gets and Sets centerOffset property.</summary>
	public Vector3 centerOffset
	{
		get { return _centerOffset; }
		set { _centerOffset = value; }
	}

	/// <summary>Gets and Sets scaleSize property.</summary>
	public bool scaleSize
	{
		get { return _scaleSize; }
		set { _scaleSize = value; }
	}
#endregion

#if UNITY_EDITOR
	/// <summary>Draws Gizmos on Editor mode when VCameraTarget's instance is selected.</summary>
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = gizmosColor;
		Vector3 scale = scaleSize ? Vector3.Scale(size, transform.localScale) : size;

		switch(space)
		{
			case Space.World:
			Gizmos.DrawWireCube(GetPosition() + (GetRotation() * centerOffset), scale);
			break;

			case Space.Self:
			VGizmos.DrawWireBox(GetPosition() + (GetRotation() * centerOffset), scale * 0.5f, transform.rotation);
			break;
		}
	}
#endif

	/// <summary>Resets VCameraTarget's instance to its default values.</summary>
	protected virtual void Reset()
	{
#if UNITY_EDITOR
		gizmosColor = Color.white;
#endif
		weight = 1.0f;
	}

	/// <returns>Bounds' Center.</returns>
	public Vector3 GetBoundsCenter()
	{
		return GetPosition() + (GetRotation() * centerOffset);
	}

	/// <returns>Camera's Target.</returns>
	public virtual Vector3 GetPosition()
	{
		return transform.position;
	}

	/// <returns>Target's Rotation.</returns>
	public virtual Quaternion GetRotation()
	{
		return transform.rotation;
	}

	/// <returns>Target's Bounds.</returns>
	public virtual Bounds GetBounds()
	{
		Vector3 scale = scaleSize ? Vector3.Scale(size, transform.localScale) : size;
		return new Bounds(GetBoundsCenter(), scale);
	}
}
}