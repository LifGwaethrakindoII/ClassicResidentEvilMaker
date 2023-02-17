using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public struct EulerRotation : ISerializationCallbackReceiver
{
	[SerializeField] private Vector3 _eulerAngles; 	/// <summary>Euler's Space Rotation.</summary>
	[SerializeField] private Quaternion _rotation; 	/// <summary>Rotation as Quaternion.</summary>	

	/// <summary>Gets and Sets eulerAngles property.</summary>
	public Vector3 eulerAngles
	{
		get { return _eulerAngles; }
		set
		{
			_eulerAngles = value;
			Quaternion quaternion = rotation;
			quaternion.eulerAngles = eulerAngles;
			rotation = quaternion;
		}
	}

	/// <summary>Gets and Sets rotation property.</summary>
	public Quaternion rotation
	{
		get { return _rotation; }
		private set { _rotation = value; }
	}

	/// <summary>Implicit EulerRotation to Quaternion conversion.</summary>
	public static implicit operator Quaternion(EulerRotation _eulerRotation)
	{
		Quaternion newRotation = Quaternion.identity;
		newRotation.eulerAngles = _eulerRotation.eulerAngles;
		return newRotation;
	}

	/// <summary>Implicit Quaternion to EulerRotation conversion.</summary>
	public static implicit operator EulerRotation(Quaternion _rotation) { return new EulerRotation(_rotation); }

	/// <summary>Implicit Vector3 to EulerRotation conversion.</summary>
	public static implicit operator EulerRotation(Vector3 _eulerAngles) { return new EulerRotation(_eulerAngles); }

	/// <summary>Implicit Quaternion times Vector operator.</summary>
	public static Vector3 operator * (EulerRotation _eulerRotation, Vector3 _v) { return _eulerRotation.rotation * _v; }

	/// <summary>Implicit Quaternion times Scalar operator.</summary>
	public static EulerRotation operator * (EulerRotation _eulerRotation, float _scalar) { return new EulerRotation(_eulerRotation.eulerAngles * _scalar); }

	/// <summary>EulerRotation's Constructor.</summary>
	/// <param name="_rotation">Rotation in Quaternion.</param>
	public EulerRotation(Quaternion _rotation) : this()
	{
		eulerAngles = _rotation.eulerAngles;
	}

	/// <summary>EulerRotation's Constructor.</summary>
	/// <param name="_eulerAngles">Rotation in Euler's Space.</param>
	public EulerRotation(Vector3 _eulerAngles) : this()
	{
		eulerAngles = _eulerAngles;
	}

	/// <summary>EulerRotation's Constructor.</summary>
	/// <param name="_x">X Axis' Rotation.</param>
	/// <param name="_y">Y Axis' Rotation.</param>
	/// <param name="_z">Z Axis' Rotation.</param>
	public EulerRotation(float _x, float _y, float _z) : this()
	{
		eulerAngles = new Vector3(_x, _y, _z);
	}

	/// <summary>Implement this method to receive a callback before Unity serializes your object.</summary>
	public void OnBeforeSerialize()
    {
    	Quaternion quaternion = rotation;
		quaternion.eulerAngles = eulerAngles;
		rotation = quaternion;
    }

    /// <summary>Implement this method to receive a callback after Unity deserializes your object.</summary>
    public void OnAfterDeserialize()
    {
    	Quaternion quaternion = rotation;
		quaternion.eulerAngles = eulerAngles;
		rotation = quaternion;
    }

	/// <returns>String Representing Euler's Rotation.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append("[{ Euler Angles: ");
		builder.Append(eulerAngles.ToString());
		builder.Append(" }, { Quaternion: ");
		builder.Append(rotation.ToString());
		builder.Append(" }]");

		return builder.ToString();
	}
}
}