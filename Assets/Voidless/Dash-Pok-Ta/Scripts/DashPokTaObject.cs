using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.DashPokTa
{
[RequireComponent(typeof(VCameraTarget))]
public class DashPokTaObject : PoolGameObject
{
    private VCameraTarget _cameraTarget;

    /// <summary>Gets cameraTarget Component.</summary>
    public VCameraTarget cameraTarget
    { 
        get
        {
            if(_cameraTarget == null) _cameraTarget = GetComponent<VCameraTarget>();
            return _cameraTarget;
        }
    }
}
}