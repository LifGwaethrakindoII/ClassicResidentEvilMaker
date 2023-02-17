using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[RequireComponent(typeof(Rigidbody))]
public abstract class Sensor : MonoBehaviour
{
	[Header("Sensor's Attributes:")]
	[SerializeField] private float _updateRate; 			/// <summary>Update's Rate.</summary>
	[SerializeField] private LayerMask _detectableMask; 	/// <summary>Detectable's LayerMask.</summary>
	private float _currentWait; 							/// <summary>current Wait.</summary>
#if UNITY_EDITOR
	[Space(5f)]
	[Header("Gizmos' Attributes:")]
	[SerializeField] private Color _color; 				/// <summary>Gizmos' Color.</summary>

	/// <summary>Gets color property.</summary>
	public Color color { get { return _color; } }
#endif

	/// <summary>Gets and Sets updateRate property.</summary>
	public float updateRate
	{
		get { return _updateRate; }
		set { _updateRate = value; }
	}

	/// <summary>Gets and Sets currentWait property.</summary>
	public float currentWait
	{
		get { return _currentWait; }
		set { _currentWait = value; }
	}

	/// <summary>Gets and Sets detectableMask property.</summary>
	public LayerMask detectableMask
	{
		get { return _detectableMask; }
		set { _detectableMask = value; }
	}

	/// <summary>Resets Sensor's instance to its default values.</summary>
	public virtual void Reset()
	{
		currentWait = 0.0f;
	}

	/// <summary>Sensor's instance initialization.</summary>
	private void Awake()
	{
		Reset();	
	}
	
	/// <summary>Sensor's tick at each frame.</summary>
	private void Update ()
	{
		currentWait += Time.deltaTime;
		if(currentWait >= updateRate)
		{
			UpdateSensor();
			currentWait = 0.0f;
		}
	}

	/// <summary>Updates Sensor.</summary>
	protected virtual void UpdateSensor(){}
}
}