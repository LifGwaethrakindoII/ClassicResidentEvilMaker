using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public struct CameraGrid
{
	public const float RANGE_MIN = 0.0f; 											/// <summary>Minimum range.</summary>
	public const float RANGE_MID = 0.5f; 											/// <summary>Medium range.</summary>
	public const float RANGE_MAX = 1.0f; 											/// <summary>Maximum range.</summary>

	[SerializeField] [Range(RANGE_MIN, RANGE_MAX)] private float _centerX; 			/// <summary>Center's X normalized position.</summary>
	[SerializeField] [Range(RANGE_MIN, RANGE_MAX)] private float _centerY; 			/// <summary>Center's Y normalized position.</summary>
	[SerializeField] [Range(RANGE_MIN, RANGE_MAX)] private float _horizontalMin; 	/// <summary>Horizontal-up line normalized position.</summary>
	[SerializeField] [Range(RANGE_MIN, RANGE_MAX)] private float _horizontalMax;	/// <summary>Horizontal-down line normalized position.</summary>
	[SerializeField] [Range(RANGE_MIN, RANGE_MAX)] private float _verticalMin;		/// <summary>Vertical-left line normalized position.</summary>
	[SerializeField] [Range(RANGE_MIN, RANGE_MAX)] private float _verticalMax; 		/// <summary>Vertical-right line normalized position.</summary>
	private Vector3 _center; 														/// <summary>Grid's Center.</summary>

	/// <summary>Gets and Sets centerX property.</summary>
	public float centerX
	{
		get { return _centerX; }
		set { _centerX = Mathf.Clamp(value, RANGE_MIN, RANGE_MAX); }
	}

	/// <summary>Gets and Sets centerY property.</summary>
	public float centerY
	{
		get { return _centerY; }
		set { _centerY = Mathf.Clamp(value, RANGE_MIN, RANGE_MAX); }
	}

	/// <summary>Gets and Sets horizontalMin property.</summary>
	public float horizontalMin
	{
		get { return _horizontalMin; }
		set { _horizontalMin = Mathf.Clamp(value, RANGE_MIN, RANGE_MAX); }
	}

	/// <summary>Gets and Sets horizontalMax property.</summary>
	public float horizontalMax
	{
		get { return _horizontalMax; }
		set { _horizontalMax = Mathf.Clamp(value, RANGE_MIN, RANGE_MAX); }
	}

	/// <summary>Gets and Sets verticalMin property.</summary>
	public float verticalMin
	{
		get { return _verticalMin; }
		set { _verticalMin = Mathf.Clamp(value, RANGE_MIN, RANGE_MAX); }
	}

	/// <summary>Gets and Sets verticalMax property.</summary>
	public float verticalMax
	{
		get { return _verticalMax; }
		set { _verticalMax = Mathf.Clamp(value, RANGE_MIN, RANGE_MAX); }
	}

	/// <summary>Gets and Sets center property.</summary>
	public Vector3 center
	{
		get { return _center; }
		set { _center = value; }
	}

	/// <summary>Implicit CameraGrid plus CameraGrid operator.</summary>
	public static CameraGrid operator + (CameraGrid a, CameraGrid b)
	{
		return new CameraGrid
		(
			a.centerX + b.centerX,
			a.centerY + b.centerY,
			a.horizontalMin + b.horizontalMin,
			a.horizontalMax + b.horizontalMax,
			a.verticalMin + b.verticalMin,
			a.verticalMax + b.verticalMax
		);
	}

	/// <summary>Implicit CameraGrid minus CameraGrid operator.</summary>
	public static CameraGrid operator - (CameraGrid a, CameraGrid b)
	{
		return new CameraGrid
		(
			a.centerX - b.centerX,
			a.centerY - b.centerY,
			a.horizontalMin - b.horizontalMin,
			a.horizontalMax - b.horizontalMax,
			a.verticalMin - b.verticalMin,
			a.verticalMax - b.verticalMax
		);
	}

	/// <summary>Implicit CameraGrid times x operator.</summary>
	public static CameraGrid operator * (CameraGrid _grid, float x)
	{
		return new CameraGrid
		(
			_grid.centerX * x,
			_grid.centerY * x,
			_grid.horizontalMin * x,
			_grid.horizontalMax * x,
			_grid.verticalMin * x,
			_grid.verticalMax * x
		);
	}

	/// <summary>CameraGrid's Constructor.</summary>
	/// <param name="x">Value to be shared to all fields.</param>
	public CameraGrid(float x = RANGE_MID) : this(x, x, x, x, x, x) { /*...*/ }

	/// <summary>CameraGrid's constructor.</summary>
	/// <param name="_centerX">Center's X normalized position.</param>
	/// <param name="_centerY">Center's Y normalized position.</param>
	/// <param name="_horizontalMin">Horizontal-up normalized position.</param>
	/// <param name="_horizontalMax">Horizontal-down normalized position.</param>
	/// <param name="_verticalMin">Vertical-left normalized position.</param>
	/// <param name="_verticalMax">Vertical-right normalized position.</param>
	public CameraGrid(float _centerX, float _centerY, float _horizontalMin, float _horizontalMax, float _verticalMin, float _verticalMax) : this(_horizontalMin, _horizontalMax, _verticalMin, _verticalMax)
	{
		centerX = _centerX;
		centerY = _centerY;
	}

	/// <summary>CameraGrid's constructor.</summary>
	/// <param name="_horizontalMin">Horizontal-up normalized position.</param>
	/// <param name="_horizontalMax">Horizontal-down normalized position.</param>
	/// <param name="_verticalMin">Vertical-left normalized position.</param>
	/// <param name="_verticalMax">Vertical-right normalized position.</param>
	public CameraGrid(float _horizontalMin, float _horizontalMax, float _verticalMin, float _verticalMax) : this()
	{
		horizontalMin = _horizontalMin;
		horizontalMax = _horizontalMax;
		verticalMin = _verticalMin;
		verticalMax = _verticalMax;
	}

	/// <returns>Minimum value between Horizontal's Min and Max.</returns>
	public float HorizontalMin() { return Mathf.Min(horizontalMin, horizontalMax); }

	/// <returns>Maximum value between Horizontal's Min and Max.</returns>
	public float HorizontalMax() { return Mathf.Max(horizontalMin, horizontalMax); }

	/// <returns>Minimum value between Vertical's Min and Max.</returns>
	public float VerticalMin() { return Mathf.Min(verticalMin, verticalMax); }

	/// <returns>Maximum value between Vertical's Min and Max.</returns>
	public float VerticalMax() { return Mathf.Max(verticalMin, verticalMax); }

	/// <summary>Sets new Center.</summary>
	/// <param name="_center">new Center.</param>
	public void SetCenter(Vector3 _center)
	{
		center = _center;
	}

	/// <summary>Linearly interpolates CameraGrid a to CameraGrid b.</summary>
	/// <param name="a">CameraGrid a.</param>
	/// <param name="b">CameraGrid b.</param>
	/// <param name="t">Normalized Time t.</param>
	/// <returns>Interpolated CameraGrid.</returns>
	public static CameraGrid Lerp(CameraGrid a, CameraGrid b, float t)
	{
		return a + ((b - a) * t);
	}
}
}