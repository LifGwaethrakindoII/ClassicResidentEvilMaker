using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public struct TransformData : ISerializationCallbackReceiver
{
	[SerializeField] private Transform _parent; 							/// <summary>Transform's Data Parent Reference.</summary>
	[SerializeField] private Vector3 _position; 							/// <summary>Transform's Data Position.</summary>
	[SerializeField] private Quaternion _rotation; 							/// <summary>Transform's Data Rotation.</summary>
	[SerializeField] private Vector3 _eulerAngles; 							/// <summary>Rotation representaiton in angles.</summary>
	[SerializeField] private Vector3 _scale; 								/// <summary>Transform's Data Scale.</summary>
	private Vector3 _right; 												/// <summary>Transform's Data Local Right Vector.</summary>
	private Vector3 _up; 													/// <summary>Transform's Data Local Up Vector.</summary>
	private Vector3 _forward; 												/// <summary>Transform's Data Local Forward Vector.</summary>
#if UNITY_EDITOR
	public bool hideScale; 													/// <summary>Hide Scale?.</summary>
	public bool showForLinePath; 											/// <summary>Show for Line Path?.</summary>
	public bool showHandles; 												/// <summary>Show Handles on Editor Mode?.</summary>
#endif

#region Getters/Setters:
	/// <summary>Gets and Sets parent property.</summary>
	public Transform parent
	{
		get { return _parent; }
		set { _parent = value; }
	}

	/// <summary>Gets and Sets position property.</summary>
	public Vector3 position
	{
		get { return _position; }
		set { _position = value; }
	}

	/// <summary>Gets and Sets rotation.eulerAngles.</summary>
	public Vector3 eulerAngles
	{
		get { return rotation.eulerAngles; }
		set
		{
			Quaternion rotationQuaternion = rotation;
	    	rotationQuaternion.eulerAngles = _eulerAngles = value;
	    	rotation = rotationQuaternion;
		}
	}

	/// <summary>Gets and Sets rotation property.</summary>
	public Quaternion rotation
	{
		get { return _rotation; }
		set
		{
			_rotation = value;
			_eulerAngles = rotation.eulerAngles;
			_right = _rotation * Vector3.right;
			_up = _rotation * Vector3.up;
			_forward = Vector3.Cross(_right, _up);
		}
	}

	/// <summary>Gets and Sets scale property.</summary>
	public Vector3 scale
	{
		get { return _scale; }
		set { _scale = value; }
	}

	/// <summary>Gets localPosition property.</summary>
	public Vector3 localPosition { get { return parent != null ? parent.position + (parent.rotation * Vector3.Scale(parent.localScale, position)) : position; } }

	/// <summary>Gets localRotation property.</summary>
	public Quaternion localRotation { get { return parent != null ? parent.rotation * Quaternion.Inverse(rotation) : rotation; } }

	/// <summary>Gets localEulerAngles property.</summary>
	public Vector3 localEulerAngles { get { return parent != null ? localRotation.eulerAngles : eulerAngles; } }

	/// <summary>Gets localScale property.</summary>
	public Vector3 localScale { get { return parent != null ? Vector3.Scale(parent.localScale, scale) : scale; } }

	/// <summary>Gets right property.</summary>
	public Vector3 right { get { return _right; } }

	/// <summary>Gets up property.</summary>
	public Vector3 up { get { return _up; } }

	/// <summary>Gets forward property.</summary>
	public Vector3 forward { get { return _forward; } }

	/// <summary>Implicit Transform to TransformData operator.</summary>
	public static implicit operator TransformData(Transform _transform) { return new TransformData(_transform); }
#endregion
	
	/// <summary>TransformData's constructor.</summary>
	/// <param name="_transform">Transform to retrieve data from.</param>
	public TransformData(Transform _transform) : this()
	{
		position = _transform.position;
		rotation = _transform.rotation;
		scale = _transform.localScale;
	}

	/// <summary>TransformData's constructor.</summary>
	/// <param name="_transform">Transform to retrieve data from.</param>
	/// <param name="_parent">Transform's Parent.</param>
	public TransformData(Transform _transform, Transform _parent) : this(_transform)
	{
		parent = _parent;
	}

	/// <summary>TransformData's constructor.</summary>
	/// <param name="_position">Position.</param>
	/// <param name="_rotation">Rotation.</param>
	/// <param name="_scale">Scale.</param>
	public TransformData(Vector3 _position, Quaternion _rotation, Vector3 _scale) : this()
	{
		position = _position;
		rotation = _rotation;
		scale = _scale;
	}

	/// <summary>TransformData's constructor.</summary>
	/// <param name="_position">Position.</param>
	/// <param name="_rotation">Rotation.</param>
	public TransformData(Vector3 _position, Quaternion _rotation) : this()
	{
		position = _position;
		rotation = _rotation;
		scale = Vector3.one;
	}

	/// <summary>TransformData's constructor.</summary>
	/// <param name="_position">Position.</param>
	/// <param name="_eulerAngles">Rotation in Euler.</param>
	/// <param name="_scale">Scale.</param>
	public TransformData(Vector3 _position, Vector3 _eulerAngles, Vector3 _scale) : this()
	{
		position = _position;
		eulerAngles = _eulerAngles;
		scale = _scale;
	}

	/// <summary>TransformData's constructor.</summary>
	/// <param name="_position">Position.</param>
	/// <param name="_eulerAngles">Rotation in Euler.</param>
	public TransformData(Vector3 _position, Vector3 _eulerAngles) : this()
	{
		position = _position;
		eulerAngles = _eulerAngles;
		scale = Vector3.one;
	}

	/// <summary>Implement this method to receive a callback before Unity serializes your object.</summary>
	public void OnBeforeSerialize()
    {
    	Quaternion rotationQuaternion = rotation;
    	rotationQuaternion.eulerAngles = _eulerAngles;
    	rotation = rotationQuaternion;
    }

    /// <summary>Implement this method to receive a callback after Unity deserializes your object.</summary>
    public void OnAfterDeserialize()
    {
    	Quaternion rotationQuaternion = rotation;
    	rotationQuaternion.eulerAngles = _eulerAngles;
    	rotation = rotationQuaternion;
    }

    /// <returns>Default's TransformData.</returns>
    public static TransformData Default()
    {
    	return new TransformData(Vector3.zero, Quaternion.identity, Vector3.one);
    }

    /// <summary>Linearly Interpolates TransformData A & B.</summary>
    /// <param name="a">TransformData A.</param>
    /// <param name="b">TransformData B.</param>
    /// <param name="t">Time parameter.</param>
    /// <returns>Interpolated Transform's Data.</returns>
    public static TransformData Lerp(TransformData a, TransformData b, float t)
    {
    	return new TransformData
    	(
    		Vector3.Lerp(a.position, b.position, t),
    		Quaternion.Lerp(a.rotation, b.rotation, t),
    		Vector3.Lerp(a.scale, b.scale, t)
    	);
    }

    /// <returns>String Representing Transform's Data.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("Transform's Data: ");
		builder.Append("\n");
		builder.Append("{");
		builder.Append("\n\t");
		builder.Append("Position: ");
		builder.Append(position.ToString());
		builder.Append("\n\t");
		builder.Append("Quaternion Rotation: ");
		builder.Append(rotation.ToString());
		builder.Append("\n\t");
		builder.Append("Euler Rotation: ");
		builder.Append(eulerAngles.ToString());
		builder.Append("\n\t");
		builder.Append("Scale: ");
		builder.Append(scale.ToString());
		builder.Append("\n");
		builder.Append("}");

		return builder.ToString();
	}
}
}