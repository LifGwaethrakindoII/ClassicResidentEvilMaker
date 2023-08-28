using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*===========================================================================
**
** Class:  PoolObjectRequester<T>
**
** Purpose: This generic class requests a Pool-Object when it is within the
** view of the camera. It is a way to do occlusion culling with dynamic
** objects and to save memory allocation by recycling objects of the same 
** type.
**
**
** Author: Lîf Gwaethrakindo
**
===========================================================================*/
namespace LGG
{
	[Flags]
	public enum RequesterStates
	{
		None = 0,
		RequesterSeen = 1,
		PoolObjectSeen = 2,
	}

	public abstract class PoolObjectRequester<T> : PoolGameObject, IPoolObjectRequester<T> where T : MonoBehaviour, IPoolObject
	{
		[SerializeField] private Camera _camera;
		[Space(5f)]
		[SerializeField] private T _requestedPoolObject;
		[Space(5f)]
		[SerializeField] private Transform _spawnPoint;
		[SerializeField] private Bounds _bounds;
		[Space(5f)]
		[SerializeField] private bool _evaluateOcclusionOnRequester;
		[SerializeField] private bool _evaluateOcclusionOnPoolObject;
		[SerializeField] private LayerMask _requesterOcclusionMask;
		[SerializeField] private LayerMask _poolObjectOcclusionMask;
		[Space(5f)]
		[Header("Deactivation Criteria's Attributes:")]
		[SerializeField] private float _invisibleTolerance;
		[SerializeField] private float _maxDistance;
		private T _poolObject;
		private Quaternion _rotation;
		private Vector3 _position;
		private Vector3 _scale;
		private float _invisibleTime;
		private RequesterStates _states;

#region Getters/Setters:
        	/// <summary>Gets Requested Pool-Object [this should be the prefab or the template object].</summary>
                public T requestedPoolObject
                {
                	get { return _requestedPoolObject; }
                	protected set { _requestedPoolObject = value; }
                }

                /// <summary>Gets Pool-Object [this should be the reference the currently requested Pool-Object].</summary>
                public T poolObject
                {
                	get { return _poolObject; }
                	protected set { _poolObject = value; }
                }

                /// <summary>Gets and Sets spawnPoint property.</summary>
                public Transform spawnPoint
                {
                	get { return _spawnPoint; }
                	set { _spawnPoint = value; }
                }

                /// <summary>Gets and Sets bounds property.</summary>
                public Bounds bounds
                {
                	get { return _bounds; }
                	set { _bounds = value; }
                }

                /// <summary>Gets and Sets camera property.</summary>
                public new Camera camera
                {
                	get
                	{
                		if(_camera == null) _camera = Camera.main;
                		return _camera;
                	}
                	protected set { _camera = value; }
                }

                /// <summary>Gets and Sets states property.</summary>
                public RequesterStates states
                {
                	get { return _states; }
                	protected set { _states = value; }
                }

                /// <summary>Gets and Sets evaluateOcclusionOnRequester property.</summary>
                public bool evaluateOcclusionOnRequester
                {
                	get { return _evaluateOcclusionOnRequester; }
                	set { _evaluateOcclusionOnRequester = value; }
                }

                /// <summary>Gets and Sets evaluateOcclusionOnPoolObject property.</summary>
                public bool evaluateOcclusionOnPoolObject
                {
                	get { return _evaluateOcclusionOnPoolObject; }
                	set { _evaluateOcclusionOnPoolObject = value; }
                }

                /// <summary>Gets and Sets requesterOcclusionMask property.</summary>
                public LayerMask requesterOcclusionMask
                {
                	get { return _requesterOcclusionMask; }
                	set { _requesterOcclusionMask = value; }
                }

                /// <summary>Gets and Sets poolObjectOcclusionMask property.</summary>
                public LayerMask poolObjectOcclusionMask
                {
                	get { return _poolObjectOcclusionMask; }
                	set { _poolObjectOcclusionMask = value; }
                }

                /// <summary>Gets and Sets invisibleTolerance property.</summary>
                public float invisibleTolerance
                {
                	get { return _invisibleTolerance; }
                	set { _invisibleTolerance = value; }
                }

                /// <summary>Gets and Sets maxDistance property.</summary>
                public float maxDistance
                {
                	get { return _maxDistance; }
                	set { _maxDistance = value; }
                }

                /// <summary>Gets and Sets invisibleTime property.</summary>
                public float invisibleTime
                {
                	get { return _invisibleTime; }
                	protected set { _invisibleTime = value; }
                }
#endregion

                /// <summary>Draws Gizmos on Editor mode when PoolObjectRequester's instance is selected.</summary>
                protected virtual void OnDrawGizmosSelected()
                {
                	Color transparentWhite = new Color(1.0f, 1.0f, 1.0f, 0.35f);
                	Gizmos.color = transparentWhite;
                	Gizmos.DrawCube(transform.TransformPoint(bounds.center), bounds.size);

                        if(poolObject == null) return;

                        Gizmos.DrawCube(poolObject.referenceTransform.TransformPoint(bounds.center), bounds.size);
                }

                /// <summary>PoolObjectRequester's instance initialization when loaded [Before scene loads].</summary>
                protected virtual void Awake()
                {
                	states = RequesterStates.None;
                }

                /// <summary>Updates PoolObjectRequester's instance at each frame.</summary>
                protected virtual void Update()
                {
                	Vector3 boundsSize = Vector3.one;
                	bool requesterSeen = camera.PointInsideFrustum(transform.TransformPoint(bounds.center), bounds.size, evaluateOcclusionOnRequester, requesterOcclusionMask);

                	if(requesterSeen)
                	{
#if UNITY_EDITOR
                                Debug.DrawLine(camera.transform.position, transform.position);
#endif
                		if((states | RequesterStates.RequesterSeen) != states) OnRequesterSeen();
                	}
                	else
                	{
                		if((states | RequesterStates.RequesterSeen) == states) OnRequesterUnseen();
                	}

                	if(poolObject == null)
                	{
                		//OnRequesterUnseen();
                		OnPoolObjectUnseen();
                		return;
                	}

                	Vector3 poolObjectPosition = GetPoolObjectPosition();
                	bool poolObjectSeen = camera.PointInsideFrustum(poolObject.referenceTransform.TransformPoint(bounds.center), bounds.size, evaluateOcclusionOnPoolObject, poolObjectOcclusionMask);

                	if(poolObjectSeen)
                	{
                		/// If the Pool-Object is seen, reset the tolerance time to 0.0f.
                		if((states | RequesterStates.PoolObjectSeen) != states) OnPoolObjectSeen();
                		invisibleTime = 0.0f;
 #if UNITY_EDITOR                               
                                Debug.DrawLine(camera.transform.position, poolObjectPosition, Color.cyan);
#endif
                	}
                	else
                	{
                		if((states | RequesterStates.PoolObjectSeen) == states) OnPoolObjectUnseen();

                		Vector3 direction = poolObjectPosition - camera.transform.position;
                		float distance = maxDistance * maxDistance;
                		bool distanceEvaluation = maxDistance > 0.0f ? direction.sqrMagnitude > distance : true;
                		bool invisibleToleranceEvaluation = invisibleTolerance > 0.0f ? invisibleTime >= invisibleTolerance : true;

        				if(distanceEvaluation && invisibleToleranceEvaluation && !requesterSeen && DeactivationCondition())
        				{
        					/* Deactivate if:
        						- Far-away enough.
        						- Invisible to the camera beyond the tolerance wait.
        						- The requester is also ut of sight.
        						- The optional/additional deactivation condition is also met (true by default).
        					*/
        					RequestDeactivation();
        				}
        				invisibleTime += Time.deltaTime;
                	}
                }

                /// \TODO Shit's temporal
                /// <returns>Pool-Object's Position.</returns>
                public virtual Vector3 GetPoolObjectPosition(Vector3 _default = default(Vector3))
                {
                	return poolObject != null ? poolObject.referenceTransform.position : Vector3.zero;
                }

                /// <returns>Retrieved Pool-Object from pool.</returns>
                public abstract T RequestPoolObject();

        	/// <summary>Requests Deactivation.</summary>
        	protected virtual void RequestDeactivation()
        	{
        		if(poolObject != null)
        		{
        			OnBeforePoolObjectDeactivation();
        			poolObject.OnObjectDeactivation();
        		}
        		poolObject = null;
        		states = RequesterStates.None;
        	}

                /// <summary>Condition Evaluation, this must be true in order to be able to request.</summary>
                /// <returns>Condition's Evaluation.</returns>
                public virtual bool RequestCondition() { return true; }

                /// <summary>After target object is invisible to camera, this additional evaluation occurs to determine whether to deactivate the Pool-Object.</summary>
                /// <returns>Deactivation Condition.</returns>
                protected virtual bool DeactivationCondition() { return true; }

#region Callbacks:
                /// <summary>Callback internally invoked when the Requester is seen by the camera.</summary>
                protected virtual void OnRequesterSeen()
                {
                	if(poolObject == null && RequestCondition())
                        {
                                poolObject = RequestPoolObject();
                                if(poolObject != null)
                                {
                                        OnPoolObjectRetrieved();
                                        OnPoolObjectUnseen();
                                }
                        }
                	else return;

                	states |= RequesterStates.RequesterSeen;
                }

                /// <summary>Callback internally invoked when the Requester is seen by the camera.</summary>
                protected virtual void OnRequesterUnseen()
                {
                	states &= ~RequesterStates.RequesterSeen;
                }

                /// <summary>Callback internally invoked when the Pool-Object is seen by the camera.</summary>
                protected virtual void OnPoolObjectSeen()
                {
                	states |= RequesterStates.PoolObjectSeen;
                }

                /// <summary>Callback internally invoked when the Pool-Object is seen by the camera.</summary>
                protected virtual void OnPoolObjectUnseen()
                {
                	states &= ~RequesterStates.PoolObjectSeen;
                }

                /// <summary>Callback internally invoked after Pool-Object was successfully retrieved.</summary>
                /// \TODO In the future, add a result argument [if the retrieval was or not successful, additinal info, etc.]
                protected virtual void OnPoolObjectRetrieved() { /*...*/ }

                /// <summary>Callback internally invoked before deactivating Pool-Object.</summary>
                protected virtual void OnBeforePoolObjectDeactivation() { /*...*/ }
#endregion
	}
}