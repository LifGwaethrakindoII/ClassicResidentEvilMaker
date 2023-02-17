using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class SkyDome : Singleton<SkyDome>
{
	[SerializeField] private SkyDomeData _data; 	/// <summary>Sky Dome's Data.</summary>

	/// <summary>Gets and Sets data property.</summary>
	public SkyDomeData data
	{
		get { return _data; }
		set { _data = value; }
	}

#region UnityMethods:
	/// <summary>SkyDome's' instance initialization.</summary>
	void Awake()
	{
		if(Instance != this) Destroy(gameObject);
		else DontDestroyOnLoad(gameObject);
	}

	/// <summary>SkyDome's starting actions before 1st Update frame.</summary>
	void Start ()
	{
		
	}
	
	/// <summary>SkyDome's tick at each frame.</summary>
	void Update ()
	{
		transform.Rotate(transform.TransformDirection(data.rotationAxis), data.speed * Time.deltaTime);
	}
#endregion
}
}