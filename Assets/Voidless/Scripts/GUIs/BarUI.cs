using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Voidless
{
public class BarUI : UIElement
{
	[SerializeField] private RectTransform _mainBar; 			/// <summary>Main Bar.</summary>
	[SerializeField] private float _positiveDisplacementSpeed; 	/// <summary>Positive Displacement's Speed.</summary>
	[SerializeField] private float _negativeDisplacementSpeed; 	/// <summary>Negative Displacement's Speed.</summary>
	[SerializeField] private Axes3D _scaleAxes; 				/// <summary>Axes to Scale.</summary>
	private float n; 											/// <summary>Current's Normalized Value.</summary>
	private Coroutine coroutine; 								/// <summary>Coroutine's reference.</summary>

	/// <summary>Gets mainBar property.</summary>
	public RectTransform mainBar { get { return _mainBar; } }

	/// <summary>Gets positiveDisplacementSpeed property.</summary>
	public float positiveDisplacementSpeed { get { return _positiveDisplacementSpeed; } }

	/// <summary>Gets negativeDisplacementSpeed property.</summary>
	public float negativeDisplacementSpeed { get { return _negativeDisplacementSpeed; } }

	/// <summary>Gets scaleAxes property.</summary>
	public Axes3D scaleAxes { get { return _scaleAxes; } }

	/// <summary>Callback invoked when BarUI's instance is enabled.</summary>
	private void OnEnable()
	{
		if((scaleAxes | Axes3D.X) == scaleAxes)
		{
			n = mainBar.localScale.x;

		} else if((scaleAxes | Axes3D.Y) == scaleAxes)
		{
			n = mainBar.localScale.y;

		} else if((scaleAxes | Axes3D.Z) == scaleAxes)
		{
			n = mainBar.localScale.z;
		}
	}

	/// <summary>Updates Bar.</summary>
	/// <param name="t">Normalized t value [internally clamped].</param>
	/// <param name="_scale">Activate Scale Coroutine? True by default, otherwise it directly updates bar without interpolation.</param>
	public void UpdateBar(float t, bool _scale = true)
	{
		t = Mathf.Clamp(t, 0.0f, 1.0f);

		if(_scale) this.StartCoroutine(UpdateBarRoutine(t), ref coroutine);
		else ScaleBar(t);
	}

	/// <summary>Scales Bar to given normalized t value.</summary>
	/// <param name="t">Normalized t value.</param>
	private void ScaleBar(float t)
	{
		Vector3 scale = mainBar.localScale;

		if((scaleAxes | Axes3D.X) == scaleAxes) scale.x *= t;
		if((scaleAxes | Axes3D.Y) == scaleAxes) scale.y *= t;
		if((scaleAxes | Axes3D.Z) == scaleAxes) scale.z *= t;

		n = t;
		mainBar.localScale = scale;
	}

	/// <summary>Updates Bar's Coroutine.</summary>
	private IEnumerator UpdateBarRoutine(float t)
	{
		float s = Mathf.Sign(t - n);
		float speed = s == 1.0f ? positiveDisplacementSpeed : negativeDisplacementSpeed;

		while(s == 1.0f ? n < t : n > t)
		{
			ScaleBar(n);
			n += (s * speed * Time.deltaTime);

			yield return null;
		}

		ScaleBar(t);
	}
}
}