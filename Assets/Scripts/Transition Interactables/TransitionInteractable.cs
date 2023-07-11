using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.REMaker
{
public class TransitionInteractable : MonoBehaviour
{
    [SerializeField] private Transform _cameraPivot;
    [SerializeField] private string[] _keyRequirements;

    /// <summary>Gets cameraPivot property.</summary>
    public Transform cameraPivot { get { return _cameraPivot; } }

    /// <summary>Gets and Sets keyRequirements property.</summary>
    public string[] keyRequirements
    {
        get { return _keyRequirements; }
        set { _keyRequirements = value; }
    }
}
}