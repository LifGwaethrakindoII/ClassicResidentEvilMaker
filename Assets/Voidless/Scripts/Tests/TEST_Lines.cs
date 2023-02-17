using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

public class TEST_Lines : MonoBehaviour
{
    [SerializeField] private Vector3 a;             /// <summary>Point A.</summary>
    [SerializeField] private Vector3 b;             /// <summary>Point B.</summary>
    [SerializeField] private float seed;            /// <summary>Time Seed [for custom random function].</summary>
    [SerializeField] private float maxWidth;        /// <summary>Max Width.</summary>

    /// <summary>Draws Gizmos on Editor mode.</summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawLine(a, b);

        Gizmos.color = Color.magenta;

        foreach(ValueVTuple<float, Vector3> tuple in GetLateralSegments())
        {
            Gizmos.DrawRay(Vector3.Lerp(a, b, tuple.Item1), tuple.Item2);
        }
    }

    private ValueVTuple<float, Vector3>[] GetLateralSegments()
    {
        seed = Mathf.Abs(seed);
        Vector3 u = Vector3.back;
        Vector3 d = b - a;
        float m = d.magnitude;
        float min = Mathf.Min(m, 2.0f);
        float max = Mathf.Max(m * m, 2.0f);
        float s = Mathf.Ceil(VMath.Rand(min, max, seed));
        ValueVTuple<float, Vector3>[] tuples = new ValueVTuple<float, Vector3>[(int)s];
        Vector3 p = Vector3.ProjectOnPlane(d, u);
        Quaternion r = Quaternion.LookRotation(p, u);
        Vector3 l = Quaternion.Inverse(r) * p;
        Vector2 c = new Vector2(l.x, l.z);
        Vector3 k = Vector3.Cross(u, d).normalized;

        for(float i = 0.0f; i < s; i++)
        {
            float t = (i / s);
            float x = ((t * c.x * maxWidth) / m ) * maxWidth;
            float y = ((t * c.y * maxWidth) / m ) * maxWidth;
            float n = Mathf.PerlinNoise(x, y) * maxWidth;
            float o = VMath.Rand(-1.0f, 1.0f, t * seed);
            o = Mathf.Sign(o);

            tuples[(int)i] = new ValueVTuple<float, Vector3>(t, k * n * o);
        }

        return tuples;
    }
}
