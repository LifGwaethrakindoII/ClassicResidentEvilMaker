using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class VCameraOrbitedDisplacementFollow : VCameraDisplacementFollow
{
	[Space(5f)]
	[Header("Orbit Following's Attributes:")]
	[SerializeField] private Axes2D _ignoreOrbitAxes; 	/// <summary>Axes to ignore when orbiting.</summary>
	[SerializeField] private Axes2D _invertAxes; 		/// <summary>Axes to invert when orbiting.</summary>
	[SerializeField] private Vector2 _orbitSpeed; 		/// <summary>Orbiting's Speed.</summary>
	[Space(5f)]
	[Header("Limits' Attributes:")]
	[SerializeField] private Axes2D _limitAxes; 		/// <summary>Axes to limit when orbiting.</summary>
	[SerializeField] private FloatRange _axisXLimits; 	/// <summary>X's Axis Limits.</summary>
	[SerializeField] private FloatRange _axisYLimits; 	/// <summary>Y's Axis Limits.</summary>
	[Space(5f)]
	[Header("Distance Adjuster's Attributes:")]
	[SerializeField] private Axes2D _adjustScalarOn; 	/// <summary>Orbit's Axes to adjust distance's scalar to.</summary>
	[SerializeField] private FloatRange _xScalarRange; 	/// <summary>Range for X's Scalar.</summary>
	[SerializeField] private FloatRange _yScalarRange; 	/// <summary>Range for Y's Scalar.</summary>
	private EulerRotation _orbitRotation; 				/// <summary>Orbit Rotation's State.</summary>
	private Vector2 distanceScalar; 					/// <summary>Distance's Scalar.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets ignoreOrbitAxes property.</summary>
	public Axes2D ignoreOrbitAxes
	{
		get { return _ignoreOrbitAxes; }
		set { _ignoreOrbitAxes = value; }
	}

	/// <summary>Gets and Sets invertAxes property.</summary>
	public Axes2D invertAxes
	{
		get { return _invertAxes; }
		set { _invertAxes = value; }
	}
	/// <summary>Gets and Sets limitAxes property.</summary>
	public Axes2D limitAxes
	{
		get { return _limitAxes; }
		set { _limitAxes = value; }
	}

	/// <summary>Gets and Sets adjustScalarOn property.</summary>
	public Axes2D adjustScalarOn
	{
		get { return _adjustScalarOn; }
		set { _adjustScalarOn = value; }
	}

	/// <summary>Gets and Sets orbitSpeed property.</summary>
	public Vector2 orbitSpeed
	{
		get { return _orbitSpeed; }
		set { _orbitSpeed = value; }
	}

	/// <summary>Gets and Sets axisXLimits property.</summary>
	public FloatRange axisXLimits
	{
		get { return _axisXLimits; }
		set { _axisXLimits = value; }
	}

	/// <summary>Gets and Sets axisYLimits property.</summary>
	public FloatRange axisYLimits
	{
		get { return _axisYLimits; }
		set { _axisYLimits = value; }
	}

	/// <summary>Gets and Sets xScalarRange property.</summary>
	public FloatRange xScalarRange
	{
		get { return _xScalarRange; }
		set { _xScalarRange = value; }
	}

	/// <summary>Gets and Sets yScalarRange property.</summary>
	public FloatRange yScalarRange
	{
		get { return _yScalarRange; }
		set { _yScalarRange = value; }
	}

	/// <summary>Gets and Sets orbitRotation property.</summary>
	public EulerRotation orbitRotation
	{
		get { return _orbitRotation; }
		set { _orbitRotation = value; }
	}
#endregion

	/// <summary>Resets Component.</summary>
	protected override void Reset()
	{
		base.Reset();
	}

	/// <summary>Resets Rotation.</summary>
	public void ResetRotation()
	{
		orbitRotation = new EulerRotation(Vector3.zero);
	}

	/// <summary>Orbits camera on both axes. The axes passed must be normalized, and not scaled by any time's delta.</summary>
	/// <param name="x">X's Axis.</param>
	/// <param name="y">Y's Axis.</param>
	/// <param name="_speedX">Speed on the X's Axis.</param>
	/// <param name="_speedY">Speed on the Y's Axis.</param>
	/// <param name="_ignoreAxisInversion">Ignore Axis Inversion's Flags?.</param>
	public virtual void OrbitInAxes(float x, float y, float _speedX, float _speedY, bool _ignoreAxisInversion = false)
	{
		if(!_ignoreAxisInversion)
		{
			if(invertAxes.HasFlag(Axes2D.Y)) x *= -1.0f;
			if(invertAxes.HasFlag(Axes2D.X)) y *= -1.0f;
		}

		Vector3 eulerAngles = orbitRotation.eulerAngles;
		float xRotation = (x * _speedX * Time.deltaTime);
		float yRotation = (y * _speedY * Time.deltaTime);

		eulerAngles.x = (limitAxes.HasFlag(Axes2D.Y) ? Mathf.Clamp(eulerAngles.x - yRotation, axisYLimits.Min(), axisYLimits.Max()) : eulerAngles.x - yRotation);
		eulerAngles.y = (limitAxes.HasFlag(Axes2D.X) ? Mathf.Clamp(eulerAngles.y + xRotation, axisXLimits.Min(), axisXLimits.Max()) : eulerAngles.y + xRotation);

		_orbitRotation.eulerAngles = eulerAngles;
	}

	/// <summary>Orbits camera on both axes. The axes passed must be normalized, and not scaled by any time's delta.</summary>
	/// <param name="x">X's Axis.</param>
	/// <param name="y">Y's Axis.</param>
	/// <param name="_ignoreAxisInversion">Ignore Axis Inversion's Flags?.</param>
	public virtual void OrbitInAxes(float x, float y, bool _ignoreAxisInversion = false)
	{
		OrbitInAxes(x, y, orbitSpeed.x, orbitSpeed.y, _ignoreAxisInversion);
	}

	/// <param name="_target">Target's Vector.</param>
	/// <returns>Offseted position from target's position.</returns>
	protected override Vector3 GetOffsetPositionRelativeToTarget(Vector3 _target)
	{
		AdjustScalar();

		Vector3 scaledOffset = (displacementOffset.normalized * vCamera.distanceAdjuster.distance * distanceScalar.y);
		Vector3 point = _target + (orbitRotation.rotation * (relativeToTarget ? (vCamera.targetRetriever.GetTargetRotation() * scaledOffset) : scaledOffset));

		return point;
	}

	/// <summary>Adjusts Distance's Scalars.</summary>
	protected virtual void AdjustScalar()
	{
		if((adjustScalarOn | Axes2D.X) == adjustScalarOn)
		{
			float xAngle = orbitRotation.eulerAngles.y;
			float x = 1.0f;
			float t = VMath.T(xAngle, axisXLimits.Min(), axisXLimits.Max());

			if(xAngle < 0.0f)
			{
				x = Mathf.Lerp(x, xScalarRange.Min(), 1.0f - t);
			
			} else if(x > 0.0f)
			{
				x = Mathf.Lerp(x, xScalarRange.Max(), t);
			}

			distanceScalar.x = x;
		}
		else distanceScalar.x = 1.0f;

		if((adjustScalarOn | Axes2D.Y) == adjustScalarOn)
		{
			float yAngle = orbitRotation.eulerAngles.x;
			float y = 1.0f;
			float t = VMath.T(yAngle, axisYLimits.Min(), axisYLimits.Max());

			if(yAngle < 0.0f)
			{
				y = Mathf.Lerp(y, yScalarRange.Min(), 1.0f - t);
			
			} else if(y > 0.0f)
			{
				y = Mathf.Lerp(y, yScalarRange.Max(), t);
			}
			
			distanceScalar.y = y;
		}
		else distanceScalar.y = 1.0f;
	}
}
}