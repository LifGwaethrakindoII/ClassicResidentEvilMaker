using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  Voidless
{
public static class VTrailRenderer
{
    /// <summary>Activates TrailRenderer.</summary>
    /// <param name="_trailRenderer">TrailRenderer's reference.</param>
    /// <param name="_activate">Activate? true by default.</param>
    public static void Activate(this TrailRenderer _trailRenderer, bool _activate = true)
    {
        if(_activate) _trailRenderer.Clear();
        _trailRenderer.enabled = _activate;
    }
}   
}