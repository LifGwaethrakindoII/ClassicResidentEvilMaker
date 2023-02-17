using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VQuaternion
{
	public static readonly EulerRotation ROTATION_OFFSET_RIGHT; 	/// <summary>Quaternion's Offset from Forward to Right orientation.</summary>
	public static readonly EulerRotation ROTATION_OFFSET_UP; 		/// <summary>Quaternion's Offset from Forward to Up orientation.</summary>

	/// <summary>Static VQuaternion's constructor.</summary>
	static VQuaternion()
	{
		ROTATION_OFFSET_RIGHT = new EulerRotation(0.0f, 90.0f, 0.0f);
		ROTATION_OFFSET_UP = new EulerRotation(270.0f, 0.0f, 0.0f);
	}

	/// <summary>Gets the rotations (Quaternion) from a list of Transforms.</summary>
	/// <param name="_list">The list of Transforms from where the Quaternion list will be created.</param>
	/// <returns>List of the Transform rotation (Quaternion).</returns>
	public static List<Quaternion> GetRotations(this List<Transform> _list)
	{
		List <Quaternion> newList = new List<Quaternion>();

		foreach(Transform _transform in _list)
		{
			newList.Add(_transform.rotation);
		}

		return newList;
	}

	/// <summary>Gets the rotations (Quaternion) from a list of GameObjects.</summary>
	/// <param name="_list">The list of GameObjects from where the Quaternion list will be created.</param>
	/// <returns>List of the Transform rotation (Quaternion).</returns>
	public static List<Quaternion> GetRotations(this List<GameObject> _list)
	{
		List <Quaternion> newList = new List<Quaternion>();

		foreach(GameObject _gameObject in _list)
		{
			if(_gameObject != null) newList.Add(_gameObject.transform.rotation);
		}

		return newList;
	}

	/// <summay>Sets Quaternion.Euler Y component.</summary>
	/// <param name="_quaternion">Queternion that will have its eulerAnglles.y modified.</param>
	/// <param name="_y">Ne Y component value.</param>
	/// <returns>Quaternion with eulerAngles.y modified.</returns>
	public static Quaternion SetY(this Quaternion _quaternion, float _y)
	{
		return Quaternion.Euler(_quaternion.eulerAngles.x, _y, _quaternion.eulerAngles.z);
	}

	/// <summary>Calculates the difference between two Quaternions.</summary>
	/// <param name="a">Quaternion A.</param>
	/// <param name="b">Quaternion B.</param>
	/// <returns>Difference between two given Quaternions.</returns>
	public static Quaternion Delta(Quaternion a, Quaternion b)
	{
		return a * Quaternion.Inverse(b);
	}

	/// <returns>Random Unit Quaternion.</returns>
	public static Quaternion Random()
	{
		return Quaternion.Euler(new Vector3(
			UnityEngine.Random.Range(0.0f, 360.0f),
			UnityEngine.Random.Range(0.0f, 360.0f),
			UnityEngine.Random.Range(0.0f, 360.0f)
		));
	}

	/// <summary>Gets a rotation where the forward vector would be the vector oriented towards the direction.</summary>
	/// <param name="d">Turn direction.</param>
	/// <param name="up">Upwards vector's reference.</param>
	/// <returns>Rotation for the forward vector to be oriented.</returns>
	public static Quaternion LookRotation(Vector3 d, Vector3 up)
	{
		return Quaternion.LookRotation(d, d.x >= 0.0f ? up : -up);
	}

	/// <summary>Gets a rotation where the forward vector would be the vector oriented towards the direction.</summary>
	/// <param name="d">Turn direction.</param>
	/// <returns>Rotation for the forward vector to be oriented.</returns>
	public static Quaternion LookRotation(Vector3 d)
	{
		return LookRotation(d, Vector3.up);
	}

	/// <summary>Gets a rotation where the right vector would be the vector oriented towards the direction.</summary>
	/// <param name="d">Turn direction.</param>
	/// <param name="up">Upwards vector's reference.</param>
	/// <returns>Rotation for the right vector to be oriented.</returns>
	public static Quaternion RightLookRotation(Vector3 d, Vector3 up)
	{
#region NO_BOSS
		/*
		// Attempt: https://www.gamedev.net/forums/topic/613595-quaternion-lookrotationlookat-up/
		Vector3 forward = d.normalized;
		Vector3 right = Vector3.zero;
		Quaternion r = default(Quaternion);
		float iW = 0.0f;

		Vector3.OrthoNormalize(ref up, ref forward);
		right = Vector3.Cross(forward, up);
		

		r.w = Mathf.Sqrt(1.0f + right.x + up.y + forward.z) * 0.5f;
		iW = 1.0f / (4.0f * r.w);
		r.x = (forward.y  - up.z) * iW;
		r.y = (right.z  - forward.x) * iW;
		r.z = (up.x  - right.y) * iW;

		return r;*/
#endregion
		
		return LookRotation(d, up) * Quaternion.Inverse(ROTATION_OFFSET_RIGHT);
	}

	/// <summary>Gets a rotation where the up vector would be the vector oriented towards the direction.</summary>
	/// <param name="d">Turn direction.</param>
	/// <param name="up">Upwards vector's reference.</param>
	/// <returns>Rotation for the up vector to be oriented.</returns>
	public static Quaternion UpLookRotation(Vector3 d, Vector3 up)
	{
		return Quaternion.LookRotation(d, up) * Quaternion.Inverse(ROTATION_OFFSET_UP);
	}

	/// <summary>Gets a rotation where the right vector would be the vector oriented towards the direction.</summary>
	/// <param name="d">Turn direction.</param>
	/// <returns>Rotation for the right vector to be oriented.</returns>
	public static Quaternion RightLookRotation(Vector3 d)
	{
		return RightLookRotation(d, Vector3.up);
	}

	/// <summary>Gets a rotation where the up vector would be the vector oriented towards the direction.</summary>
	/// <param name="d">Turn direction.</param>
	/// <returns>Rotation for the up vector to be oriented.</returns>
	public static Quaternion UpLookRotation(Vector3 d)
	{
		return UpLookRotation(d, Vector3.back);
	}
}
}