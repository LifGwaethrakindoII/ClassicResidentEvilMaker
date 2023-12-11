using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[CreateAssetMenu(menuName = PATH_TARGETRETRIEVER + "Default")]
public class VCameraTargetRetrieverModule : VCameraModule
{
    public const string PATH_TARGETRETRIEVER = PATH_ROOT + "Target Retrievers / ";

    [SerializeField] protected VCameraTarget _target;

    /// <summary>Gets and Sets target property.</summary>
    public virtual VCameraTarget target
    {
        get { return _target; }
        set { _target = value; }
    }

    /// <summary>Gets Target's Position.</summary>
    /// <param name="_camera">VCamera's requesting Target's Position.</param>
    /// <returns>Camera's Target.</returns>
    public virtual Vector3 GetTargetPosition(VCamera _camera)
    {
        return target != null ? target.GetPosition() : _camera.transform.position;
    }

    /// <summary>Gets Target's Rotation.</summary>
    /// <param name="_camera">VCamera's requesting Target's Rotation.</param>
    /// <returns>Target's Rotation.</returns>
    public virtual Quaternion GetTargetRotation(VCamera _camera)
    {
        return target != null ? target.GetRotation() : _camera.transform.rotation;
    }

    /// <summary>Gets Target's Bounds.</summary>
    /// <param name="_camera">VCamera's requesting Target's Bounds.</param>
    /// <returns>Target's Bounds.</returns>
    public virtual Bounds GetTargetBounds(VCamera _camera)
    {
        return target != null ? target.GetBounds() : default(Bounds);
    }

    /// <summary>Retrieves target data and stores it into the provided variables.</summary>
    /// <param name="position">Position reference.</param>
    /// <param name="rotation">Rotation reference.</param>
    /// <param name="bounds">Bounds reference.</param>
    public virtual void GetTargetData(VCamera _camera, out Vector3 position, out Quaternion rotation, out Bounds bounds)
    {
        position = GetTargetPosition(_camera);
        rotation = GetTargetRotation(_camera);
        bounds = GetTargetBounds(_camera);
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
