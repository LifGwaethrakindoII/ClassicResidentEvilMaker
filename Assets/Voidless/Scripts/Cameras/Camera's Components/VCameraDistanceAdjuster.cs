using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class VCameraDistanceAdjuster : VCameraComponent
{
	[SerializeField] private FloatRange _distanceRange; 						/// <summary>Distance's Range.</summary>
	[SerializeField] private bool _limitDistanceChangeSpeed; 					/// <summary>Limit Distance's Change Speed?.</summary>
	[SerializeField]
	[Range(0.1f, 1.0f)] private float _distanceChangeDuration; 					/// <summary>Distance Change's Duration.</summary>
	[SerializeField] private float _maxDistanceChangeSpeed; 					/// <summary>Maximum Distance's Change Speed.</summary>
	private float _distance; 													/// <summary>Distance between the VCamera and the Target.</summary>
	private Dictionary<int, VCameraDistanceCalculator> _distanceCalculators; 	/// <summary>Distance Calculators.</summary>
	protected float currentDistance; 											/// <summary>Current Distance.</summary>

	/// <summary>Gets and Sets distanceRange property.</summary>
	public FloatRange distanceRange
	{
		get { return _distanceRange; }
		set { _distanceRange = value; }
	}

	/// <summary>Gets and Sets limitDistanceChangeSpeed property.</summary>
	public bool limitDistanceChangeSpeed
	{
		get { return _limitDistanceChangeSpeed; }
		set { _limitDistanceChangeSpeed = value; }
	}

	/// <summary>Gets and Sets distanceChangeDuration property.</summary>
	public float distanceChangeDuration
	{
		get { return _distanceChangeDuration; }
		set { _distanceChangeDuration = Mathf.Clamp(value, 0.0f, 1.0f); }
	}

	/// <summary>Gets and Sets maxDistanceChangeSpeed property.</summary>
	public float maxDistanceChangeSpeed
	{
		get { return _maxDistanceChangeSpeed; }
		set { _maxDistanceChangeSpeed = value; }
	}

	/// <summary>Gets and Sets distance property.</summary>
	public float distance
	{
		get { return _distance; }
		set { _distance = Mathf.Clamp(value, distanceRange.Min(), distanceRange.Max()); }
	}

	/// <summary>Gets and Sets distanceCalculators property.</summary>
	public Dictionary<int, VCameraDistanceCalculator> distanceCalculators
	{
		get { return _distanceCalculators; }
		protected set { _distanceCalculators = value; }
	}

	/// <summary>Method called when this instance is created.</summary>
	protected override void Awake()
	{
		base.Awake();
		if(distanceCalculators == null) distanceCalculators = new Dictionary<int, VCameraDistanceCalculator>();
	}

	/// <summary>Adds VCameraDistanceCalculator's reference.</summary>
	/// <param name="_calculator">Calculator to add to dictionary.</param>
	public void AddDistanceCalculator(VCameraDistanceCalculator _calculator)
	{
		if(_calculator == null) return;

		int ID = _calculator.GetInstanceID();

		if(distanceCalculators == null) distanceCalculators = new Dictionary<int, VCameraDistanceCalculator>();
		
		if(!distanceCalculators.ContainsKey(ID)) distanceCalculators.Add(ID, _calculator);
		else distanceCalculators[ID] = _calculator;
	}

	/// <summary>Removes VCameraDistanceCalculator's reference.</summary>
	/// <param name="_calculator">Calculator to add to dictionary.</param>
	public void RemoveDistanceCalculator(VCameraDistanceCalculator _calculator)
	{
		if(_calculator == null || distanceCalculators == null) return;

		int ID = _calculator.GetInstanceID();

		if(distanceCalculators.ContainsKey(ID)) distanceCalculators.Remove(ID);
	}

	/// <summary>Updates Distance defined by occlusion's handling.</summary>
	public virtual void UpdateDistance()
	{
		Vector3 target = vCamera.targetRetriever.GetTargetPosition();
		float bestDistance = distanceRange.Min();

		if(distanceCalculators.Count > 0)
		{
			float min = Mathf.Infinity;

			foreach(VCameraDistanceCalculator calculator in distanceCalculators.Values)
			{
				float d = calculator.GetCalculatedDistance(target);
				if(d < min) min = d;
			}

			bestDistance = min;
		}

		distance = Mathf.SmoothDamp(
			distance,
			Mathf.Clamp(bestDistance, distanceRange.Min(), distanceRange.Max()),
			ref currentDistance,
			distanceChangeDuration,
			limitDistanceChangeSpeed ? maxDistanceChangeSpeed : Mathf.Infinity,
			vCamera.GetDeltaTime()
		);
	}
}
}