using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/*===========================================================================
**
** Class:  LGGCamera
**
** Purpose: Extension methods & functions for Camera.
**
**
** Author: Lîf Gwaethrakindo
**
===========================================================================*/

public enum AABBResult { Outside, Inside, Intersecting }

namespace LGG
{
    public static class LGGCamera
    {
        /// <summary>Evaluates if current camera is the Editor Camera [useful for OnBecameVisible and OnBecameInvisible's callbacks].</summary>
        /// <returns>True if the current camera is from the scene view.</returns>
        public static bool SeenByEditorSceneCamera()
        {
#if UNITY_EDITOR
            foreach(Camera camera in SceneView.GetAllSceneCameras())
            {
                if(Camera.current == camera) return true;
            }
#endif
                return false;
        }

        /// <summary>Evaluates if point is inside Camera's Viewport.</summary>
        /// <param name="_camera">Camera that will convert the world position to viewport position.</param>
        /// <param name="p">World position to evaluate.</param>
        /// <returns>True if point is inside Viewport's area (NOTE: does not evaluate occlusion).</returns>
        public static bool PointInsideViewport(this Camera _camera, Vector3 p)
        {
            if(_camera == null) return false;
            
            /// Get Normalized Viewport Point:
            Vector3 v = _camera.WorldToViewportPoint(p);

            /// Evaluate that x and y normalized points are between 0.0f and 1.0f and that z is above 0.0f (meaning not behind camera).
            return (v.x >= 0.0f && v.x <= 1.0f) && (v.y >= 0.0f && v.y <= 1.0f) && v.z > 0.0f;
        }

        /// <summary>Evaluates if point is inside Camera's frustum area.</summary>
        /// <param name="_camera">Camera that will evaluate the point.</param>
        /// <param name="p">Point in world space to evaluate.</param>
        /// <param name="b">Axis-Align Bounding-Boxes around the point.</param>
        /// <returns>AABB result of the pount and the camera's frustum.</returns>
        public static bool PointInsideFrustum(this Camera _camera, Vector3 p, Vector3 b)
        {
            if(_camera == null) return false;
            
            Bounds bounds = new Bounds(p, b);
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_camera);
            bool inside = GeometryUtility.TestPlanesAABB(planes, bounds);

            if(inside) return true;
            return inside;

            /// Ugh...
            /*Vector3 zero = Vector3.zero;

            if(b != zero) foreach(Vector3 corner in bounds.GetCornerVertices())
            {
                Debug.DrawLine(_camera.transform.position, corner);
                if(GeometryUtility.TestPlanesAABB(planes, new Bounds(corner, zero)))
                //if(_camera.PointInsideViewport(corner))
                return true;
            }

            return AABBResult.Outside;*/
        }

        /// <summary>Evaluates if point is inside Camera's frustum area and optionally evaluates for occlusion.</summary>
        /// <param name="_camera">Camera that will evaluate the point.</param>
        /// <param name="p">Point in world space to evaluate.</param>
        /// <param name="b">Axis-Align Bounding-Boxes around the point.</param>
        /// <param name="_evaluateOcclusion">Evaluate occlusion? true by default.</param>
        /// <param name="_mask">LayerMask to pass into the Raycast function [All Layers by default].</param>
        /// <returns>True if the point is inside Camera's frustum area and there is no occlusion.</returns>
        public static bool PointInsideFrustum(this Camera _camera, Vector3 p, Vector3 b, bool _evaluateOcclusion = true, int _mask = Physics.AllLayers)
        {
            if(_camera == null) return false;
            
            bool evaluation = _camera.PointInsideFrustum(p, b);

            if(!evaluation) return false;

            if(!_evaluateOcclusion) return true;

            /// Finally, evaluate for occlusion:
            Bounds bounds = new Bounds(p, b);
            Vector3 cameraPosition = _camera.transform.position;
            Vector3 closestPoint = bounds.ClosestPoint(cameraPosition);
            Vector3 direction = closestPoint - cameraPosition;
            float cameraLength = _camera.farClipPlane - _camera.nearClipPlane;
            RaycastHit depthCheck;
            bool objectBlockingPoint = false;

            direction = direction.normalized;

            return !Physics.Raycast(cameraPosition, direction, out depthCheck, Mathf.Max(cameraLength - 0.05f, 0.0f), _mask);
        }
    }
}