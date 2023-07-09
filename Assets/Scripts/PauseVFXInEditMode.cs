using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Voidless
{
[RequireComponent(typeof(VisualEffect))]
[ExecuteInEditMode]
public class PauseVFXInEditMode : MonoBehaviour
{
    /// <summary>PauseVFXInEditMode's instance initialization when loaded [Before scene loads].</summary>
    private void Awake()
    {
        if(!Application.isPlaying)
        {
            VisualEffect VFX = GetComponent<VisualEffect>();
            VFX.Stop();
        }
    }
}
}