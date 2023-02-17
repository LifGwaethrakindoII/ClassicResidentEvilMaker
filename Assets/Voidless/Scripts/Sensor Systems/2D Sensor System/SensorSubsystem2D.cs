using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public class SensorSubsystem2D : IEnumerable<SensorData2D>
{
	[SerializeField] private RelativeTo _relativeTo; 				/// <summary>Relative To Which?.</summary>
	[SerializeField] private LayerMask _layerMask; 					/// <summary>Layers of interest for sensors.</summary>
	[SerializeField] private NormalizedVector2 _relativeOrigin; 	/// <summary>Sensor's Origin Relative to either Transform or Bounds.</summary>
	[SerializeField] private float _originDistance; 				/// <summary>Sensor's Origin distance.</summary>
	[SerializeField] private SensorData2D[] _sensorsData; 			/// <summary>Sensors' Data.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Gizmos' Attributes:")]
	[SerializeField] private Color _color; 							/// <summary>Debug color for the sensors.</summary>
#endif

	/// <summary>Gets and Sets relativeTo property.</summary>
	public RelativeTo relativeTo
	{
		get { return _relativeTo; }
		set { _relativeTo = value; }
	}

	/// <summary>Gets and Sets layerMask property.</summary>
	public LayerMask layerMask
	{
		get { return _layerMask; }
		set { _layerMask = value; }
	}

	/// <summary>Gets and Sets relativeOrigin property.</summary>
	public NormalizedVector2 relativeOrigin
	{
		get { return _relativeOrigin; }
		set { _relativeOrigin = value; }
	}

	/// <summary>Gets and Sets originDistance property.</summary>
	public float originDistance
	{
		get { return _originDistance; }
		set { _originDistance = value; }
	}

	/// <summary>Gets and Sets sensorsData property.</summary>
	public SensorData2D[] sensorsData
	{
		get { return _sensorsData; }
		set { _sensorsData = value; }
	}

#if UNITY_EDITOR
	/// <summary>Gets and Sets color property.</summary>
	public Color color
	{
		get { return _color; }
		set { _color = value; }
	}
#endif

	/// <returns>Returns an enumerator that iterates through the sensors' data.</returns>
	public IEnumerator<SensorData2D> GetEnumerator()
	{
		foreach(SensorData2D sensorData in sensorsData)
		{
			yield return sensorData;
		}
	}

	/// <returns>Returns an enumerator that iterates through the sensors' data.</returns>
	IEnumerator IEnumerable.GetEnumerator()
	{
		yield return GetEnumerator();
	}
}
}