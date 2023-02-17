using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

public class SinusoidalDisplacer : MonoBehaviour
{
    [SerializeField] private Transform child;
    [SerializeField] private float speed;
    [SerializeField] private float magnitude;

    /// <summary>Draws Gizmos on Editor mode when SelfMotionDisplacer's instance is selected.</summary>
    private void OnDrawGizmosSelected()
    {
        if(child == null) return;
        
        float r = 0.25f;
        Vector3 p = child.transform.position;
        Vector3 a = p + (Vector3.up * magnitude);
        Vector3 b = p + (Vector3.down * magnitude);

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(p, a);
        Gizmos.DrawLine(p, b);
        Gizmos.DrawSphere(p + a, r);
        Gizmos.DrawSphere(p + b, r);
    }

    /// <summary>Updates SinusoidalDisplacer's instance at each frame.</summary>
    private void Update()
    {
        if(child == null) return;

        Vector3 position = Vector3.zero;
        position.y = Mathf.Sin(Time.time * speed) * magnitude;
        child.localPosition = position;
    }
}
