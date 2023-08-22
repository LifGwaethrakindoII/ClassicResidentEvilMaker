using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VMesh
{
    /// <summary>Returns fittest Mesh depending whether the request is made on Editor or Play Mode.</summary>
    /// <param name="_meshFilter">MeshFilter's reference.</param>
    public static Mesh GetMesh(this MeshFilter _meshFilter)
    {
        return Application.isPlaying ? _meshFilter.mesh : _meshFilter.sharedMesh;
    }

    /// <summary>Gets Mesh's vertex in world space position.</summary>
    /// <param name="_meshFilter">MeshFilter's reference.</param>
    /// <param name="index">Vertex's index.</param>
    /// <returns>Vertex in world space position [relative to the MeshFilter's transform].</returns>
    public static Vector3 GetWorldSpaceVertex(this MeshFilter _meshFilter, int index)
    {
        Vector3 v = _meshFilter.GetMesh().vertices[index];
        return _meshFilter.transform.TransformPoint(v);
    }

    /// <summary>Gets mesh's vertices converted into world space [relative to the MeshFilter's Transform].</summary>
    /// <param name="_meshFilter">MeshFilter's reference.</param>
    public static IEnumerable<Vector3> WorldSpaceVertices(this MeshFilter _meshFilter)
    {
        Vector3 p = _meshFilter.transform.position;
        Vector3 s = _meshFilter.transform.lossyScale;
        Quaternion r = _meshFilter.transform.rotation;

        foreach(Vector3 vertex in _meshFilter.GetMesh().vertices)
        {
            /// This is the same as _meshFilter.transform.TransformPoint(vertex);
            yield return p + (r * Vector3.Scale(vertex, s));
        }
    }
}
}