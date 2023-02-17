using UnityEngine;

namespace Voidless
{
public static class VTransform
{
	/// <summary>Gets Top Parent.</summary>
	/// <param name="_transform">Parent's Reference.</param>
	/// <returns>Top Parent.</returns>
	public static Transform GetTopParent(this Transform _transform)
	{
		Transform parent = _transform;

		while(parent.parent != null)
		{
			parent = parent.parent;
		}

		return parent;
	}

	/// <summary>Sets Transform's Position.</summary>
	/// <param name="_transform">Transform to modify.</param>
	/// <param name="_position">New Position.</param>
	public static void SetPosition(this Transform _transform, Vector3 _position)
	{
		_transform.position = _position;
	}

	/// <summary>Sets Transform's Local Position.</summary>
	/// <param name="_transform">Transform to modify.</param>
	/// <param name="_localPosition">New Local Position.</param>
	public static void SetLocalPosition(this Transform _transform, Vector3 _localPosition)
	{
		_transform.localPosition = _localPosition;
	}

	/// <summary>Sets Transform's Rotation.</summary>
	/// <param name="_transform">Transform to modify.</param>
	/// <param name="_rotation">New Rotation.</param>
	public static void SetRotation(this Transform _transform, Quaternion _rotation)
	{
		_transform.rotation = _rotation;
	}

	/// <summary>Sets Transform's Local Rotation.</summary>
	/// <param name="_transform">Transform to modify.</param>
	/// <param name="_localRotation">New Local Rotation.</param>
	public static void SetLocalRotation(this Transform _transform, Quaternion _localRotation)
	{
		_transform.localRotation = _localRotation;
	}

	/// <summary>Makes the Transform's right vector orientated towards given target.</summary>
	/// <param name="_transform">Transform that will be orientated.</param>
	/// <param name="_target">Target's Position.</param>
	/// <param name="_up">Up vector reference [Vector3.up by default].</param>
	public static void RightLookAt(this Transform _transform, Vector3 _target, Vector3 _up)
	{
		Vector3 d = _target - _transform.position;
		Quaternion rotation = VQuaternion.RightLookRotation(d, _up);
		
		_transform.rotation = rotation;
	}

	/// <summary>Makes the Transform's right vector orientated towards given target.</summary>
	/// <param name="_transform">Transform that will be orientated.</param>
	/// <param name="_target">Target's Position.</param>
	public static void RightLookAt(this Transform _transform, Vector3 _target)
	{
		_transform.RightLookAt(_target, Vector3.up);
	}

	/// <summary>Makes the Transform's up vector orientated towards given target.</summary>
	/// <param name="_transform">Transform that will be orientated.</param>
	/// <param name="_target">Target's Position.</param>
	/// <param name="_up">Up vector reference [Vector3.back by default].</param>
	public static void UpLookAt(this Transform _transform, Vector3 _target, Vector3 _up)
	{
		Vector3 d = _target - _transform.position;
		Quaternion rotation = VQuaternion.UpLookRotation(d, _up);
		
		_transform.rotation = rotation;
	}

	/// <summary>Makes the Transform's up vector orientated towards given target.</summary>
	/// <param name="_transform">Transform that will be orientated.</param>
	/// <param name="_target">Target's Position.</param>
	public static void UpLookAt(this Transform _transform, Vector3 _target)
	{
		_transform.UpLookAt(_target, Vector3.back);
	}

	/// <summary>Returns a Vector3 relative to the space given.</summary>
	/// <param name="_transform">Transform that requests the relative to space Vector.</param>
	/// <param name="_vector">Vector to evaluate.</param>
	/// <param name="_space">Space relativeness.</param>
	/// <returns>Vector relative to the given space, local space ig given 'Self', world space if 'World' given.</returns>
	public static Vector3 RelativeTo(this Transform _transform, Vector3 _vector, Space _space)
	{
		return _space == Space.World ? _vector : _transform.rotation * _vector;
	}

	/// <summary>Gets Vector with Axis ignored.</summary>
	/// <param name="_transform">Transoform that extends the method.</param>
	/// <param name="v">Vector to return.</param>
	/// <param name="_axes">Axes to ignore [Axes3D.None by default].</param>
	/// <param name="_zeroIgnoredAxes">Set to 0.0f all ignored axes? [if false, it will set the vector equal to the transform's respective component].</param>
	/// <returns>Vector with ignored axes.</returns>
	public static Vector3 IgnoreVectorAxes(this Transform _transform, Vector3 v, Axes3D _axes = Axes3D.None, bool _zeroIgnoredAxes = false)
	{
		if(_axes != Axes3D.None)
		{
			if((_axes | Axes3D.X) == _axes) v.x = _zeroIgnoredAxes ? 0.0f : _transform.position.x;
			if((_axes | Axes3D.Y) == _axes) v.y = _zeroIgnoredAxes ? 0.0f : _transform.position.y;
			if((_axes | Axes3D.Z) == _axes) v.z = _zeroIgnoredAxes ? 0.0f : _transform.position.z;
		}

		return v;
	}

	/// <summary>Gets Vector with Axis considered.</summary>
	/// <param name="_transform">Transoform that extends the method.</param>
	/// <param name="v">Vector to return.</param>
	/// <param name="_axes">Axes to consider [Axes3D.All by default].</param>
	/// <param name="_zeroIgnoredAxes">Set to 0.0f all ignored axes? [if false, it will set the vector equal to the transform's respective component].</param>
	/// <returns>Vector with with considered axes.</returns>
	public static Vector3 WithVectorAxes(this Transform _transform, Vector3 v, Axes3D _axes = Axes3D.All, bool _zeroIgnoredAxes = false)
	{
		if(_axes != Axes3D.All)
		{
			if((_axes & Axes3D.X) != _axes) v.x = _zeroIgnoredAxes ? 0.0f : _transform.position.x;
			if((_axes & Axes3D.Y) != _axes) v.y = _zeroIgnoredAxes ? 0.0f : _transform.position.y;
			if((_axes & Axes3D.Z) != _axes) v.z = _zeroIgnoredAxes ? 0.0f : _transform.position.z;
		}

		return v;
	}


	/// <summary>Sets transform's properties equal to given TransformData.</summary>
	/// <param name="_transform">Transform's reference.</param>
	/// <param name="_data">TransformData to set to transform.</param>
	/// <param name="_properties">Transform's Properties to modify [All by default].</param>
	public static void Set(this Transform _transform, TransformData _data, TransformProperties _properties = TransformProperties.All)
	{
		if((_properties | TransformProperties.Position) == _properties) _transform.position = _data.position;
		if((_properties | TransformProperties.Rotation) == _properties) _transform.rotation = _data.rotation;
		if((_properties | TransformProperties.Scale) == _properties) _transform.localScale = _data.scale;
	}

	/// <summary>Activates/Deactivates children beneath given transform.</summary>
	/// <param name="_transform">Parent's Transform.</param>
	/// <param name="_activate">Activate Children? True by default.</param>
	public static void ActivateChildren(this Transform _transform, bool _activate = true)
	{
		foreach(Transform child in _transform)
		{
			child.gameObject.SetActive(_activate);
		}
	}

	/// <summary>Sets Parent for Transform.</summary>
	/// <param name="_transform">Transform to set Parent to.</param>
	/// <param name="_parent">New Parent [null by default].</param>
	public static void SetParent(this Transform _transform, Transform _parent = null)
	{
		Vector3 scale = _transform.localScale;

		_transform.parent = _parent;
		_transform.localScale = scale;
	}

	/// <summary>Sets ReorientedTransform as parent of given Transform.</summary>
	/// <param name="_transform">Transform that will have a new Parent.</param>
	/// <param name="_reorientedParent">ReorientedTransform that will become the new Parent.</param>
	/// <param name="_worldPositionStays">If true, the parent-relative position, scale and rotation are modified such that the object keeps the same world space position, rotation and scale as before.</param>
	public static void SetReorientedParent(this Transform _transform, ReorientedTransform _reorientedParent, bool _worldPositionStays = true)
	{
		if(_reorientedParent != null)
		_transform.SetParent(_reorientedParent.transform, _worldPositionStays);
	}

	/// <summary>Sets ReorientedTransform as parent of given Transform.</summary>
	/// <param name="_transform">Transform that will have a new Parent.</param>
	/// <param name="_reorientedParent">ReorientedTransform that will become the new Parent.</param>
	/// <param name="_setParentProperties">ReorientedParent's properties to give to new child.</param>
	public static void SetReorientedParent(this Transform _transform, ReorientedTransform _reorientedParent, TransformProperties _setParentProperties = TransformProperties.None)
	{
		if(_reorientedParent == null) return;

		_transform.SetParent(_reorientedParent.transform);

		if((_setParentProperties | TransformProperties.Position) == _setParentProperties) _transform.position = _reorientedParent.transform.position;
		if((_setParentProperties | TransformProperties.Rotation) == _setParentProperties) _transform.rotation = _reorientedParent.rotation;
		if((_setParentProperties | TransformProperties.Scale) == _setParentProperties) _transform.localScale = _reorientedParent.transform.localScale;
	}

	/// <summary>Rotates Transform around given point, axis and angle.</summary>
	/// <param name="_transform">Transform to rotate.</param>
	/// <param name="o">Orbit point.</param>
	/// <param name="axis">Rotation's Axis.</param>
	/// <param name="r">Separation's Radius.</param>
	public static void RotateAround(this Transform _transform, Vector3 o, Vector3 axis, float a, float r)
	{
		_transform.position = _transform.position.RotatedAround(o, axis, a, r);
	}
}
}