using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Serializable]
public struct IndexStrengthTuple
{
    public int index;
    public float strength;

    public IndexStrengthTuple(int _index, float _strength)
    {
        index = _index;
        strength = _strength;
    }
}

[Serializable]
public class MorphableMeshData
{
    [SerializeField] private Vector3 _anchorPosition;
    [SerializeField] private Transform _anchor;
    [SerializeField] private float _minRadius;
    [SerializeField] private float _maxRadius;
    private List<IndexStrengthTuple> _indices;

    /// <summary>Gets and Sets anchorPosition property.</summary>
    public Vector3 anchorPosition
    {
        get { return _anchorPosition; }
        set { _anchorPosition = value; }
    }

    /// <summary>Gets anchor property.</summary>
    public Transform anchor { get { return _anchor; } }

    /// <summary>Gets minRadius property.</summary>
    public float minRadius { get { return _minRadius; } }

    /// <summary>Gets maxRadius property.</summary>
    public float maxRadius { get { return _maxRadius; } }

    /// <summary>Gets and Sets indices property.</summary>
    public List<IndexStrengthTuple> indices
    {
        get { return _indices; }
        set { _indices = value; }
    }

    /// <summary>Sets Indices of influenced vertices.</summary>
    public void SetVerticesIndices(MeshFilter _meshFilter)
    {
        indices = new List<IndexStrengthTuple>();
    
        if(_meshFilter == null || anchor == null) return;

        Mesh mesh = Application.isPlaying ? _meshFilter.mesh : _meshFilter.sharedMesh;
        Transform meshTransform = _meshFilter.transform;
        Vector3 localPosition = meshTransform.InverseTransformPoint(anchor.position);
        float range = maxRadius - minRadius;
        float sMinRadius = minRadius * minRadius;
        float sMaxRadius = maxRadius * maxRadius;
        float sRange = range * range;

        for(int i = 0; i < mesh.vertices.Length; i++)
        {
            Vector3 v = mesh.vertices[i];
            Vector3 wv = meshTransform.TransformPoint(v);
            float sd = (wv - localPosition).sqrMagnitude;

            if(sd > sMaxRadius) continue;

            sd = (anchor.position - wv).sqrMagnitude - sMinRadius;
            float strength = sd < 0.0f ? 1.0f : 1.0f - (sd / sRange);
            IndexStrengthTuple tuple = new IndexStrengthTuple(i, strength);

            indices.Add(tuple);
        }
    }
}
}