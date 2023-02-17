using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class FOVFrustumColliderData : ScriptableObject, IEnumerable<Vector3>
{
	[SerializeField] private FOVData _FOVData; 			/// <summary>FOV's Data.</summary>
	[SerializeField] private Mesh _mesh; 				/// <summary>Mesh's Collider.</summary>
	[SerializeField] private Vector3 _leftNormal; 		/// <summary>Left Face's Normal.</summary>
	[SerializeField] private Vector3 _rightNormal; 		/// <summary>Right Face's Normal.</summary>
	[SerializeField] private Vector3 _bottomNormal; 	/// <summary>Bottom Face's Normal.</summary>
	[SerializeField] private Vector3 _topNormal; 		/// <summary>Top Face's Normal.</summary>
	[SerializeField] private Vector3 _backNormal; 		/// <summary>Back Face's Normal.</summary>
	[SerializeField] private Vector3 _forwardNormal; 	/// <summary>Forward Face's Normal.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets FOVData property.</summary>
	public FOVData FOVData
	{
		get { return _FOVData; }
		set { _FOVData = value; }
	}

	/// <summary>Gets and Sets mesh property.</summary>
	public Mesh mesh
	{
		get { return _mesh; }
		set { _mesh = value; }
	}

	/// <summary>Gets and Sets leftNormal property.</summary>
	public Vector3 leftNormal
	{
		get { return _leftNormal; }
		set { _leftNormal = value; }
	}

	/// <summary>Gets and Sets rightNormal property.</summary>
	public Vector3 rightNormal
	{
		get { return _rightNormal; }
		set { _rightNormal = value; }
	}

	/// <summary>Gets and Sets bottomNormal property.</summary>
	public Vector3 bottomNormal
	{
		get { return _bottomNormal; }
		set { _bottomNormal = value; }
	}

	/// <summary>Gets and Sets topNormal property.</summary>
	public Vector3 topNormal
	{
		get { return _topNormal; }
		set { _topNormal = value; }
	}

	/// <summary>Gets and Sets backNormal property.</summary>
	public Vector3 backNormal
	{
		get { return _backNormal; }
		set { _backNormal = value; }
	}

	/// <summary>Gets and Sets forwardNormal property.</summary>
	public Vector3 forwardNormal
	{
		get { return _forwardNormal; }
		set { _forwardNormal = value; }
	}
#endregion

	/// <returns>New FOVFrustumColliderData's Instance.</returns>
	public static FOVFrustumColliderData Create()
	{
		FOVFrustumColliderData frustumColliderData = CreateInstance<FOVFrustumColliderData>();
		return frustumColliderData;
	}

	/// <returns>Returns an enumerator that iterates through the Frustum Mesh's normals.</returns>
	public IEnumerator<Vector3> GetEnumerator()
	{
		yield return leftNormal;
		yield return rightNormal;
		yield return bottomNormal;
		yield return topNormal;
		yield return backNormal;
		yield return forwardNormal;
	}

	/// <returns>Returns an enumerator that iterates through the Frustum Mesh's normals.</returns>
	IEnumerator IEnumerable.GetEnumerator()
	{
		yield return GetEnumerator();
	}

	/// <summary>String representing this FOV's Data.</summary>
	public override string ToString()
	{
		return FOVData.ToString();
	}
}
}