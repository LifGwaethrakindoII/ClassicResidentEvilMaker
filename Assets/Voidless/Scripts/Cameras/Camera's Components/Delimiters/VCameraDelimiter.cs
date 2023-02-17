using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public abstract class VCameraDelimiter : VCameraComponent
{
	/// <summary>Method called when this instance is created.</summary>
	protected override void Awake()
	{
		vCamera.AddDelimiter(this);
	}

	/// <summary>Callback invoked when VCameraDelimiter's instance is going to be destroyed and passed to the Garbage Collector.</summary>
	private void OnDestroy()
	{
		if(vCamera != null)
		vCamera.RemoveDelimiter(this);
	}

	/// <summary>Gives target delimited by this component's implementation.</summary>
	/// <param name="_target">Target to delimit.</param>
	/// <returns>Target delimited by this component's implementation.</returns>
	public virtual Vector3 Delimited(Vector3 _target) { return _target; }
}
}