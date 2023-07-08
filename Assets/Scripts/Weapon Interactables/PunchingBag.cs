using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Sirenix.OdinInspector;

namespace Voidless.REMaker
{
public class PunchingBag : MonoBehaviour
{
    private Quaternion originalRotation;
    public float intensity = 1f;
    public float force;
    public float restitution;
    public float damping = 0.9f;
    public float drag = 0.9f;
    public float restitutionThreshold = 0.01f;
    public int bounceCount = 3;
    public float value;

    private void Start()
    {
        originalRotation = transform.rotation;
    }

    [Button("Hit")]
    public void Hit(Vector3 hitDirection)
    {
        StartCoroutine(Bounce(hitDirection.normalized));
    }

    [Button("TEST")]
    public void TEST()
    {
        StartCoroutine(TESTRoutine());
    }

    private IEnumerator TESTRoutine()
    {
        value = 0.0f;
        float acceleration = force;

        bool movingPositive = true;

        while (Mathf.Abs(value) > 0.01f || acceleration > 0.01f)
        {
            // Apply acceleration
            value += acceleration;

            // Change direction if necessary
            if (value > 0 && !movingPositive)
            {
                acceleration *= -restitution;
                movingPositive = true;
            }
            else if (value < 0 && movingPositive)
            {
                acceleration *= -restitution;
                movingPositive = false;
            }

            // Apply drag
            acceleration *= drag;

            yield return null;
        }

        // Ensure value reaches exactly 0.0f
        value = 0.0f;
    }

    private IEnumerator Bounce(Vector3 direction)
    {
        /*float i = intensity;
        float time = 0.7f;
        float inverseDuration = 1.0f / time;

        int bouncesRemaining = bounceCount * 2; // Each bounce consists of back and forth motion
        while (bouncesRemaining > 0)
        {
            Quaternion initialRotation = transform.rotation;
            Quaternion targetRotation = originalRotation * Quaternion.Euler(direction * i);
            Quaternion inverseRotation = originalRotation * Quaternion.Euler(-direction * i);
            float t = 0f;
            while (t < 1.0f)
            {
                transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, VMath.EaseOutBack(t));
                t += Time.deltaTime * inverseDuration;
                yield return null;
            }

            time *= damping;
            i *= damping;
            direction = -direction; // Reverse the direction for the next bounce
            bouncesRemaining--;
            inverseDuration = 1.0f / time;
        }

        // Reset the rotation after the bounce effect is finished
        transform.rotation = originalRotation;*/

        Vector3 rotation = Vector3.right * intensity;
        Vector3 torque = rotation;
        Vector3 angularVelocity = torque;

        while(angularVelocity.sqrMagnitude > 0.01f)
        {
            float dt = Time.deltaTime;
            transform.rotation *= Quaternion.Euler(angularVelocity * dt);
            torque += rotation * dt;
            angularVelocity += torque;
            yield return null;
        }
    }
}
}