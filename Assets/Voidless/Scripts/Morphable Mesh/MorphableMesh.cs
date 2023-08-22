using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Voidless
{
[ExecuteInEditMode]
public class MorphableMesh : MonoBehaviour
{
    public const float SIZE_VERTEX = 0.0025f;

    [SerializeField] private MorphableMeshData[] _morphableAnchors;
    [SerializeField] private MorphableMeshData test;
    [SerializeField] private MeshFilter meshFilter;
#if UNITY_EDITOR
    [SerializeField] private bool drawHandles;
#endif

    /// <summary>Gets morphableAnchors property.</summary>
    public MorphableMeshData[] morphableAnchors { get { return _morphableAnchors; } }

    /// <summary>Draws Gizmos on Editor mode when MorphableMesh's instance is selected.</summary>
    private void OnDrawGizmosSelected()
    {
        Color a = Color.green;
        Color b = Color.yellow;
        Color c = Color.red;

        Gizmos.color = Color.cyan.WithAlpha(0.25f);
        Gizmos.DrawSphere(test.anchor.position, test.minRadius);
        Gizmos.color = VColor.purple.WithAlpha(0.25f);
        Gizmos.DrawSphere(test.anchor.position, test.maxRadius);
        Vector3[] vertices = meshFilter.GetMesh().vertices;

        Gizmos.color = Color.magenta;
        for(int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = vertices[i];
            Vector3 wv = meshFilter.transform.TransformPoint(vertex);
            Gizmos.DrawSphere(wv, SIZE_VERTEX);
        }

        if(test.indices == null) return;

        foreach(IndexStrengthTuple tuple in test.indices)
        {
            float t = tuple.strength;
            Vector3 wv = meshFilter.transform.TransformPoint(vertices[tuple.index]);

            Gizmos.color = VColor.CuadraticBeizer(a, b, c, t);
            Gizmos.DrawSphere(wv, SIZE_VERTEX);
        }
    }

    /// <summary>Callback invoked when GameObjectOrganizer's instance is enabled.</summary>
    private void OnEnable()
    {
#if UNITY_EDITOR
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
        SceneView.onSceneGUIDelegate += OnSceneGUI;
#endif
    }

    /// <summary>Callback invoked when GameObjectOrganizer's instance is disabled.</summary>
    private void OnDisable()
    {
#if UNITY_EDITOR
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
#endif
    }

    [Button("TEST")]
    private void TEST()
    {
        test.SetVerticesIndices(meshFilter);
        foreach(MorphableMeshData anchorData in morphableAnchors)
        {
            anchorData.SetVerticesIndices(meshFilter);
        }
    }

#if UNITY_EDITOR
    /// <summary>Enables the Editor to handle an event in the Scene view.</summary>
    /// <param name="_view">Scene's View.</param>
    private void OnSceneGUI(SceneView _view)
    {
        if(!drawHandles) return;

        Transform meshFilterTransform = meshFilter.transform;
        Vector3 p = meshFilterTransform.position;
        Vector3 s = meshFilterTransform.localScale;
        Quaternion r = meshFilterTransform.rotation;

        foreach(MorphableMeshData anchorData in morphableAnchors)
        {
            Vector3 a = anchorData.anchorPosition;
            Vector3 b = p + (r * a);
            Vector3 c = Handles.PositionHandle(b, r);

            /// The equivalent to meshFilterTransform.InverseTransformPoint(c);
            anchorData.anchorPosition = Quaternion.Inverse(r) * VVector3.Division((c - p), meshFilterTransform.localScale);
        }

        if(EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(this, "Position Handle Change");
        }
    }
#endif
}
}