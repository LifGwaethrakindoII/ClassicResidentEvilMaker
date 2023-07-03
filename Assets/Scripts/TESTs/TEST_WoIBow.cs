using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

public class TEST_WoIBow : MonoBehaviour
{
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform bow;
    [SerializeField] private Transform bottomLimb;
    [SerializeField] private Transform topLimb;
    [SerializeField][Range(0.0f, 1.0f)] private float minLimit;
    [SerializeField][Range(0.0f, 1.0f)] private float maxLimit;

    /// <summary>TEST_WoIBow's instance initialization when loaded [Before scene loads].</summary>
    private void Awake()
    {
        bow.position = leftHand.position;
        bow.SetParent(leftHand);
    }

    /// <summary>Draws Gizmos on Editor mode.</summary>
    private void OnDrawGizmos()
    {
        Vector3 direction = leftHand.position - rightHand.position;

        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(rightHand.position, direction);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(leftHand.position, bow.forward * 2.5f);
    }

    /// <summary>Updates TEST_WoIBow's instance at each frame.</summary>
    private void Update()
    {
        /// Rotate the left hand towards the direction of the right hand pointing to the left hand:
        Vector3 up = topLimb.position - bottomLimb.position;
        Vector3 d = rightHand.position - bottomLimb.position;
        Vector3 direction = leftHand.position - rightHand.position;
        float segmentPosition = VVector3.GetScalarProjectionProgress(d, up);

        if(segmentPosition < minLimit || segmentPosition > maxLimit) return;

        //Quaternion rotation = Quaternion.LookRotation(direction, plane);
        Quaternion rotation = bow.rotation;
        Quaternion inverseRotation = Quaternion.Inverse(bow.rotation);
        Quaternion deltaRotation = inverseRotation * leftHand.transform.rotation;
        Vector3 localDirection = inverseRotation * direction;

        direction = rotation * localDirection;
        leftHand.transform.rotation = Quaternion.LookRotation(direction, up) * deltaRotation;
    }
}
