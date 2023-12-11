using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Voidless.DashPokTa
{
//[RequireComponent(typeof(MiddlePointBetweenTransformsTargetRetriever))]
public class GameplayCamera : VCamera
{
    private MiddlePointBetweenTransformsTargetRetriever _middlePointTargetRetriever;

    /// <summary>Gets middlePointTargetRetriever Component.</summary>
    public MiddlePointBetweenTransformsTargetRetriever middlePointTargetRetriever
    { 
        get
        {
            if(_middlePointTargetRetriever == null) _middlePointTargetRetriever = GetComponent<MiddlePointBetweenTransformsTargetRetriever>();
            return _middlePointTargetRetriever;
        }
    }

    /// <summary>Updates Camera.</summary>
    protected override void CameraUpdate()
    {
        
    }

    /// <summary>Updates Camera on Physics' Thread.</summary>
    protected override void CameraFixedUpdate()
    {
        
    }
}
}