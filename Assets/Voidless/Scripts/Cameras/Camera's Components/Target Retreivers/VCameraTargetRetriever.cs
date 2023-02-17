using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public abstract class VCameraTargetRetriever : VCameraComponent
{
	[SerializeField] protected VCameraTarget _target; 	/// <summary>Main VCamera's Target.</summary>

	/// <summary>Gets and Sets target property.</summary>
	public virtual VCameraTarget target
	{
		get { return _target; }
		set { _target = value; }
	}

	/// <summary>VCameraTargetRetriever's instance initialization when loaded [Before scene loads].</summary>
	protected virtual void Awake() { /*...*/ }

	/// <returns>Camera's Target.</returns>
	public virtual Vector3 GetTargetPosition()
	{
		return target != null ? target.GetPosition() : Vector3.zero;
	}

	/// <returns>Target's Rotation.</returns>
	public virtual Quaternion GetTargetRotation()
	{
		return target != null ? target.GetRotation() : Quaternion.identity;
	}

	/// <returns>Target's Bounds.</returns>
	public virtual Bounds GetTargetBounds()
	{
		return target != null ? target.GetBounds() : default(Bounds);
	}
}
}