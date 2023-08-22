using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  Voidless;

public class TEST_UnambiguousColors : MonoBehaviour
{
    [SerializeField] private Mesh mesh;
    [SerializeField] private Transform orangeAnchor;
    [SerializeField] private Transform purpleAnchor;
    [SerializeField] private Transform pinkAnchor;
    [SerializeField] private Transform brownAnchor;
    [SerializeField] private float scale;

    /// <summary>Draws Gizmos on Editor mode when TEST_UnambiguousColors's instance is selected.</summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = VColor.orange;
        Gizmos.DrawMesh(mesh, orangeAnchor.position, orangeAnchor.rotation, Vector3.one * scale);
        Gizmos.color = VColor.purple;
        Gizmos.DrawMesh(mesh, purpleAnchor.position, purpleAnchor.rotation, Vector3.one * scale);
        Gizmos.color = VColor.pink;
        Gizmos.DrawMesh(mesh, pinkAnchor.position, pinkAnchor.rotation, Vector3.one * scale);
        Gizmos.color = VColor.brown;
        Gizmos.DrawMesh(mesh, brownAnchor.position, brownAnchor.rotation, Vector3.one * scale);
    }
}
