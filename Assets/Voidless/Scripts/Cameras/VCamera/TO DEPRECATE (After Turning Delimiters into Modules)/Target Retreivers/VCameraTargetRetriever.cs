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

	/// <summary>Retrieves target data and stores it into the provided variables.</summary>
	/// <param name="position">Position reference.</param>
	/// <param name="rotation">Rotation reference.</param>
	/// <param name="bounds">Bounds reference.</param>
	public virtual void GetTargetData(out Vector3 position, out Quaternion rotation, out Bounds bounds)
	{
		position = GetTargetPosition();
		rotation = GetTargetRotation();
		bounds = GetTargetBounds();
	}

	/// <summary>Clears all targets' references.</summary>
    public virtual void ClearTargets()
    {
        target = null;
    }

    /// <summary>Adds Target's Transform into the internal dictionary.</summary>
    /// <param name="_target">Target Transform to add.</param>
    public virtual void AddTarget(VCameraTarget _target)
    {
        target = _target;
    }

    /// <summary>Removes Target's Transform into the internal dictionary.</summary>
    /// <param name="_target">Target Transform to add.</param>
    public virtual void RemoveTarget(VCameraTarget _target)
    {
        if(target == _target) target = null;
    }

    /// <returns>True if has any active targets.</returns>
    public virtual bool HasActiveTargets()
    {
        return target != null && target.gameObject.activeSelf;
    }
}
}