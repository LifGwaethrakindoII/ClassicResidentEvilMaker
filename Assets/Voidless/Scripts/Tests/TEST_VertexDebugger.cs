using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless; /// My personal library

public class TEST_VertexDebugger : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;

    /// <summary>Draws Gizmos on Editor mode when TEST_VertexDebugger's instance is selected.</summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        float r = 0.015f;

        foreach(Vector3 worldSpaceVertex in meshFilter.WorldSpaceVertices())
        {
            Gizmos.DrawSphere(worldSpaceVertex, r);
        }
    }
}
