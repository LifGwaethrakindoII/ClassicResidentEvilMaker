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

	/// <summary>Rotates Quaternion A towards Quaternion B with different speeds per axis.</summary>
	/// <param name="a">Quaternion A [From].</param>
	/// <param name="b">Quaternion B [Target].</param>
	/// <param name="pitchSpeed">Speed on the X-Axis.</param>
	/// <param name="yawSpeed">Speed on the Y-Axis.</param>
	/// <param name="rollSpeed">Speed on the Z-Axis.</param>
	/// <param name="dt">Delta Time.</param>
	/// <param name="up">Up Vector.</param>
	public static Quaternion RotateTowards(Quaternion a, Quaternion b, float pitchSpeed, float yawSpeed, float rollSpeed, float dt, Vector3 up)
    {
        float pitchAngle, yawAngle, rollAngle;
        Vector3 pitchAxis, yawAxis, rollAxis;

        // Calculate the rotation from "from" to "to"
        Quaternion deltaRotation = b * Quaternion.Inverse(a);

        // Convert the delta rotation to angles and axes
        deltaRotation.ToAngleAxis(out float deltaPitch, out Vector3 deltaPitchAxis);
        deltaRotation.ToAngleAxis(out float deltaYaw, out Vector3 deltaYawAxis);
        deltaRotation.ToAngleAxis(out float deltaRoll, out Vector3 deltaRollAxis);

        // Ensure that the angles are within the range (-180, 180]
        deltaPitch = Mathf.DeltaAngle(0, deltaPitch);
        deltaYaw = Mathf.DeltaAngle(0, deltaYaw);
        deltaRoll = Mathf.DeltaAngle(0, deltaRoll);

        // Calculate the new angles and axes based on the desired speeds
        pitchAngle = Mathf.MoveTowardsAngle(0, deltaPitch, pitchSpeed * dt);
        yawAngle = Mathf.MoveTowardsAngle(0, deltaYaw, yawSpeed * dt);
        rollAngle = Mathf.MoveTowardsAngle(0, deltaRoll, rollSpeed * dt);

        // Calculate the new axes based on the updated angles
        pitchAxis = Quaternion.AngleAxis(pitchAngle, up) * deltaPitchAxis;
        yawAxis = Quaternion.AngleAxis(yawAngle, up) * deltaYawAxis;
        rollAxis = Quaternion.AngleAxis(rollAngle, up) * deltaRollAxis;

        // Combine the new rotations for pitch, yaw, and roll
        Quaternion pitchRotation = Quaternion.AngleAxis(pitchAngle, pitchAxis);
        Quaternion yawRotation = Quaternion.AngleAxis(yawAngle, yawAxis);
        Quaternion rollRotation = Quaternion.AngleAxis(rollAngle, rollAxis);

        // Combine all the rotations and return the result
        return a * pitchRotation * yawRotation * rollRotation;
    }

	/// <summary>Calculates the average of a set of Quaternions.</summary>
	/// <param name="quaternions">Set of Quaternions.</param>
	/// <returns>Quaternions' Average.</returns>
	public static Quaternion Average(params Quaternion[] quaternions)
    {
        if (quaternions == null || quaternions.Length == 0)
        {
            // Handle the case of an empty array or null input.
            return Quaternion.identity;
        }

        // Initialize the accumulated quaternion to the first quaternion in the set.
        Quaternion averageQuaternion = quaternions[0];

        // Start the loop from the second quaternion.
        for (int i = 1; i < quaternions.Length; i++)
        {
            // Calculate the weighting factor based on the current position in the loop.
            float weight = 1.0f / (float)(i + 1.0f);

            // Perform slerp between the accumulated quaternion and the current quaternion.
            averageQuaternion = Quaternion.Slerp(averageQuaternion, quaternions[i], weight);
        }

        // Normalize the accumulated quaternion to obtain the average quaternion.
        averageQuaternion.Normalize();

        return averageQuaternion;
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