using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public enum SensorType 										/// <summary>Sensor Type.</summary>
{
	Ray, 													/// <summary>Ray Sensor Type.</summary>
	Box, 													/// <summary>Box Sensor Type.</summary>
	Sphere, 												/// <summary>Sphere Sensor Type.</summary>
	Capsule 												/// <summary>Capsule Sensor Type.</summary>
}

[Serializable]
public struct SensorData3D : IRayConvertible
{
	[SerializeField] private SensorType _sensorType; 		/// <summary>Type of Sensor.</summary>
	[SerializeField] private Vector3 _dimensions; 			/// <summary>Sensor's Dimensions.</summary>
	[SerializeField] private Vector3 _origin; 				/// <summary>Ray convertible's origin reference.</summary>
	[SerializeField] private NormalizedVector3 _direction; 	/// <summary>Ray convertible's heading direction.</summary>
	[SerializeField] private float _distance; 				/// <summary>Sensor's Distance.</summary>
	
	/// <summary>Gets and Sets sensorType property.</summary>
	public SensorType sensorType
	{
		get { return _sensorType; }
		set { _sensorType = value; }
	}

	/// <summary>Gets and Sets dimensions property.</summary>
	public Vector3 dimensions
	{
		get { return _dimensions; }
		set { _dimensions = value; }
	}

	/// <summary>Gets and Sets origin property.</summary>
	public Vector3 origin
	{
		get { return _origin; }
		set { _origin = value; }
	}

	/// <summary>Gets and Sets direction property.</summary>
	public Vector3 direction
	{
		get { return _direction; }
		set { _direction = value; }
	}

	/// <summary>Gets and Sets distance property.</summary>
	public float distance
	{
		get { return _distance; }
		set { _distance = value; }
	}

	/// <summary>Gets radius property.</summary>
	public float radius { get { return dimensions.GetMaxVectorProperty(); } }

	/// <summary>Implicit SensorData3D to Ray conversion.</summary>
	public static implicit operator Ray(SensorData3D _sensor) { return new Ray(_sensor.origin, _sensor.direction); }		
}
}