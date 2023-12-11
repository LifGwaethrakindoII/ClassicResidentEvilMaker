using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Text;

/*===========================================================================
**
** Class:  VCameraRaycastingOrigin
**
** Purpose: CameraComponent class responsible for handling sensors (triggers
** and raycasting).
**
**
** Author: Lîf Gwaethrakindo
**
===========================================================================*/
namespace Voidless
{
    public enum VCameraRaycastingOrigin { None, FromFocusPoint, FromAllViewportPoints }

    public class VCameraSensorSystem : VCameraComponent
    {
        public const int STATE_FLAG_CAMERACOLLIDING = 1 << 0;
        public const int STATE_FLAG_TARGETOCCLUDED = 1 << 1;

        public static readonly string[] stateNames;

        [InfoBox("@ToString()")]
        [Space(5f)]
        [SerializeField] private LayerMask _mask;
        [Space(5f)]
        [Header("Overlap Sphere's Attributes:")]
        [SerializeField] private float _radius;
        [Space(5f)]
        [Header("Occlusion Sensor's Attributes:")]
        [SerializeField] private VCameraRaycastingOrigin _raycastOrigin;
        [SerializeField] private float _pointRatio;
        protected Collider[] overlappingColliders;
        private int _states;
        protected RaycastHit raycastInfo;

        /// <summary>Gets and Sets mask property.</summary>
        public LayerMask mask
        {
            get { return _mask; }
            set { _mask = value; }
        }

        /// <summary>Gets and Sets radius property.</summary>
        public float radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        /// <summary>Gets and Sets raycastOrigin property.</summary>
        public VCameraRaycastingOrigin raycastOrigin
        {
            get { return _raycastOrigin; }
            set { _raycastOrigin = value; }
        }

        /// <summary>Gets and Sets pointRatio property.</summary>
        public float pointRatio
        {
            get { return _pointRatio; }
            set { _pointRatio = value; }
        }

        /// <summary>Gets and Sets states property.</summary>
        public int states
        {
            get { return _states; }
            protected set { _states = value; }
        }

        /// <summary>Static constructor.</summary>
        static VCameraSensorSystem()
        {
            stateNames = new string[]
            {
                "STATE_FLAG_CAMERACOLLIDING",
                "STATE_FLAG_TARGETOCCLUDED"
            };
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            Gizmos.color = (states | STATE_FLAG_CAMERACOLLIDING) == states ? VColor.transparentRed : VColor.transparentWhite;
            Gizmos.DrawSphere(vCamera.transform.position, radius);
            Gizmos.color = (states | STATE_FLAG_TARGETOCCLUDED) == states ? VColor.transparentRed : VColor.transparentWhite;
        }

        /// <summary>Method called when this instance is created.</summary>
        protected override void Awake()
        {
            base.Awake();
            overlappingColliders = new Collider[3];
        }

        protected override void Update()
        {
            base.Update();
            EvaluateOverlappingSphere();
        }

        /// <summary>Evaluates Sphere's Sensor.</summary>
        protected virtual void EvaluateOverlappingSphere()
        {
            int length = Physics.OverlapSphereNonAlloc(vCamera.transform.position, radius, overlappingColliders, mask);
            
            if(length == 0)
            {
                states &= ~STATE_FLAG_CAMERACOLLIDING;
                return;
            }

            states |= STATE_FLAG_CAMERACOLLIDING;
        }

        /// <summary>Evaluates raycast sensor(s).</summary>
        protected virtual void EvaluateRaycast(Vector3 _target)
        {
            if(raycastOrigin == VCameraRaycastingOrigin.None) return;

            Vector3 center = vCamera.viewportHandler.gridAttributes.center;
            Vector3 origin = Vector3.zero;
            Vector3 direction = Vector3.zero;
            float distance = 0.0f;
            Ray ray = default;

            switch(raycastOrigin)
            {
                case VCameraRaycastingOrigin.FromFocusPoint:
                    origin = center;
                    direction = _target - origin;
                    ray = new Ray(origin, direction);

                    if(Physics.Raycast(ray, out raycastInfo, distance, mask)) states |= STATE_FLAG_TARGETOCCLUDED;
                    else states &= ~STATE_FLAG_TARGETOCCLUDED;
                break;

                case VCameraRaycastingOrigin.FromAllViewportPoints:
                    foreach(Vector3 point in vCamera.viewportHandler)
                    {
                        origin = Vector3.LerpUnclamped(center, point, pointRatio);
                        direction = _target - origin;
                        ray = new Ray(origin, direction);
                        distance = direction.magnitude;

                        if(Physics.Raycast(ray, out raycastInfo, distance, mask))
                        {
                            states |= STATE_FLAG_TARGETOCCLUDED;
                            break;
                        }
                    }

                    states &= ~STATE_FLAG_TARGETOCCLUDED;
                    raycastInfo = default;
                break;
            }
        }

        /// <returns>String representing this Sensor System.</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("States: ");
            builder.AppendLine(VString.GetNamedBitChain(states, stateNames));

            return builder.ToString();
        }
    }
}