using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Sirenix.OdinInspector;

namespace Voidless.REMaker
{
public class PunchingBag : MonoBehaviour
{
    [SerializeField] private HitInteractable hitInteractable;
    [SerializeField] private Transform punchingBagTransform;
    [SerializeField] private float maxForce;
    [SerializeField] private float minTorqueDuration;
    [SerializeField] private float maxTorqueDuration;
    [SerializeField] private float minRestitutionDuration;
    [SerializeField] private float maxRestitutionDuration;
    [SerializeField][Range(0.0f, 85.0f)] private float minRotationLimit;
    [SerializeField][Range(0.0f, 85.0f)] private float maxRotationLimit;
    private Coroutine hitRoutine;

    /// <summary>PunchingBag's instance initialization when loaded [Before scene loads].</summary>
    private void Awake()
    {
        hitInteractable.onHitEvent += OnHit;
    }

    [Button("Hit")]
    /// <summary>Performs Hit's Routine.</summary>
    /// <param name="direction">Global space direction of the hit.</param>
    /// <param name="force">Hit's force.</param>
    public void OnHit(GameObject _source, Vector3 direction, float force)
    {
        if(hitRoutine == null)
        this.StartCoroutine(HitRoutine(direction, force), ref hitRoutine);
    }

    /// <summary>Hit's Routine.</summary>
    /// <param name="direction">Global space direction of the hit.</param>
    /// <param name="force">Hit's force.</param>
    private IEnumerator HitRoutine(Vector3 direction, float force)
    {
        Vector3 rotationAxis = Vector3.Cross(punchingBagTransform.up, direction.normalized);
        rotationAxis.Normalize();

        float x = Mathf.Min(force / maxForce, 1.0f);
        x = VMath.EaseOutSine(x);
        float t = 0.0f;
        float torqueDuration = Mathf.Lerp(minTorqueDuration, maxTorqueDuration, x);
        float restitutionDuration = Mathf.Lerp(minRestitutionDuration, maxRestitutionDuration, x);
        float angularRotation = Mathf.Lerp(minRotationLimit, maxRotationLimit, x);
        float i = 1.0f / torqueDuration;
        Quaternion a = punchingBagTransform.rotation;
        Quaternion b = a * Quaternion.Euler(punchingBagTransform.localEulerAngles + (rotationAxis * angularRotation));

        while(t < 1.0f)
        {
            punchingBagTransform.rotation = Quaternion.Lerp(a, b, VMath.EaseOutCirc(t));
            t += Time.deltaTime * i;
            yield return null;
        }

        i = 1.0f / restitutionDuration;
        t = 0.0f;

        while(t < 1.0f)
        {
            punchingBagTransform.rotation = Quaternion.Lerp(b, a, VMath.EaseOutBounce(t));
            t += Time.deltaTime * i;
            yield return null;
        }

        punchingBagTransform.rotation = a;
        this.DispatchCoroutine(ref hitRoutine);
    }
}
}