using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public enum LineSegmentType
{
	Linear,
	CuadraticBeizer,
	CubicBeizer,
	Spline
}

[Serializable]
public class LinePath
{
	[SerializeField] private Line[] _lines; 	/// <summary>Path's Vector3s.</summary>
	[SerializeField] private bool _closed; 		/// <summary>Does the last Vector3 close with the first?.</summary>
#if UNITY_EDITOR
	public bool showHandles; 					/// <summary>Show Handles? Editor Mode Only.</summary>
#endif

	/// <summary>Gets and Sets lines property.</summary>
	public Line[] lines
	{
		get { return _lines; }
		set { _lines = value; }
	}

	/// <summary>Gets and Sets closed property.</summary>
	public bool closed
	{
		get { return _closed; }
		set { _closed = value; }
	}

	/// <summary>LinePath default constructor.</summary>
	public LinePath(bool _closed = false, params Line[] _lines)
	{
		closed = _closed;
		lines = _lines;	
	}

	/// <summary>Gets point in line determined by t.</summary>
	/// <param name="_index">Line's Index.</param>
	/// <param name="t">Normalized Time t.</param>
	/// <returns>Point in Line.</returns>
	public Vector3 GetLinePoint(int _index, float t)
	{
		if(lines == null || lines.Length <= 0) return Vector3.zero;

		_index = Mathf.Clamp(_index, 0, lines.Length);

		return lines[_index].Lerp(t);
	}
}
}