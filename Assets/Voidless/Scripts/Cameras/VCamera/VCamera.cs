using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/*===========================================================================
**
** Class:  VCamera
**
** Purpose: Base Camera class from Voidless. It works with separate modules
** that you can dinamically change, enable/disable.
**
** 	- Target Retriever: Gets a target.
** 	- Displacement and Rotation Follow: Dictates how the camera should
** 	and rotate given a target.
** 	- Distance Adjuster: Adjust distance with one or many
** 	DistanceCalculators.
** 	- Occlusion Evaluator: Evaluates target occlusion.
**
** Author: Lîf Gwaethrakindo
**
===========================================================================*/
namespace Voidless
{
	public enum LoopType
	{
		Update,
		LateUpdate,
		FixedUpdate
	}

	[Flags]
	public enum CameraModules
	{
		None = 0,
		TargetRetriever = 1,
		DisplacementFollow = 2,
		RotationFollow = 4,
		DistanceAdjuster = 8,
		OcclusionEvaluator = 16,
	}

	[Flags]
	public enum CameraStates
	{
		None = 0,
		ReachedTargetPosition = 1,
		ReachedTargetRotation = 2,
		TargetOccluded = 4,

	}

	[RequireComponent(typeof(Camera))]
	[RequireComponent(typeof(VCameraViewportHandler))]
	[RequireComponent(typeof(OrientationNormalAdjuster))]
	[RequireComponent(typeof(SphereCollider))]
	public class VCamera : MonoBehaviour
	{
		[SerializeField] private VCameraTarget _defaultTarget;
		[SerializeField] private LoopType _updateCameraAt;
		[SerializeField] protected Axes3D _ignoreFocusAxes;
		[Space(5f)]
		[TabGroup("ModulesGroup", "Modules")][SerializeField] private CameraModules _enabledModules;
		[TabGroup("ModulesGroup", "Modules")][SerializeField] private VCameraTargetRetrieverModule _targetRetrieverModule;
		[TabGroup("ModulesGroup", "Modules")][SerializeField] private VCameraDisplacementFollowModule _displacementFollowModule;
		[TabGroup("ModulesGroup", "Modules")][SerializeField] private VCameraRotationFollowModule _rotationFollowModule;
		[TabGroup("ModulesGroup", "Modules")][SerializeField] private VCameraOcclusionEvaluatorModule _occlusionEvaluatorModule;
		[Space(5f)]
		[Header("Distance Modules:")]
		[TabGroup("ModulesGroup", "Modules")][SerializeField] private VCameraDistanceAdjusterModule _distanceAdjusterModule;
		[TabGroup("ModulesGroup", "Modules")][SerializeField] private List<VCameraDistanceCalculatorModule> _distanceCalculatorModules;
		private VCameraViewportHandler _viewportHandler;
		private OrientationNormalAdjuster _normalAdjuster;
		private Camera _camera;
		private SphereCollider _sphereCollider;
		private CameraStates _states;
		private Vector3 _centerFocusDirection;
		private Ray _viewportRay;
		private Ray _centerRay;
		protected CameraViewportPlane boundariesPlane;
		protected Vector3 targetPosition;
		protected Vector3 targetOrientation;
		protected Quaternion targetRotation;
		protected Bounds targetBounds;
		protected float targetDistance;
		protected Vector3 desiredDisplacement;
		protected Vector3 desiredOrientation;
		protected Quaternion desiredRotation;
	#if UNITY_EDITOR
		[Space(5f)]
		[SerializeField] protected bool updateInEditor;
		[SerializeField] protected Color gizmosColor;
	#endif

	#region Getters/Setters:
		/// <summary>Gets defaultTarget property.</summary>
		public VCameraTarget defaultTarget { get { return _defaultTarget; } }

		/// <summary>Gets and Sets updateCameraAt property.</summary>
		public LoopType updateCameraAt
		{
			get { return _updateCameraAt; }
			set { _updateCameraAt = value; }
		}

		/// <summary>Gets and Sets ignoreFocusAxes property.</summary>
		public Axes3D ignoreFocusAxes
		{
			get { return _ignoreFocusAxes; }
			set { _ignoreFocusAxes = value; }
		}

		/// <summary>Gets and Sets states property.</summary>
		public CameraStates states
		{
			get { return _states; }
			protected set { _states = value; }
		}

		/// <summary>Gets and Sets enabledModules property.</summary>
		public CameraModules enabledModules
		{
			get { return _enabledModules; }
			set { _enabledModules = value; }
		}
		
		/// <summary>Gets and Sets centerFocusDirection property.</summary>
		public Vector3 centerFocusDirection
		{
			get { return _centerFocusDirection; }
			protected set { _centerFocusDirection = value; }
		}

		/// <summary>Gets and Sets viewportRay property.</summary>
		public Ray viewportRay
		{
			get { return _viewportRay; }
			protected set { _viewportRay = value; }
		}

		/// <summary>Gets and Sets centerRay property.</summary>
		public Ray centerRay
		{
			get { return _centerRay; }
			protected set { _centerRay = value; }
		}

		/// <summary>Gets and Sets rotationFollowModule property.</summary>
		public VCameraRotationFollowModule rotationFollowModule
		{
			get { return _rotationFollowModule; }
			set { _rotationFollowModule = value; }
		}

		/// <summary>Gets and Sets displacementFollowModule property.</summary>
		public VCameraDisplacementFollowModule displacementFollowModule
		{
			get { return _displacementFollowModule; }
			set { _displacementFollowModule = value; }
		}

		/// <summary>Gets and Sets targetRetrieverModule property.</summary>
		public VCameraTargetRetrieverModule targetRetrieverModule
		{
			get { return _targetRetrieverModule; }
			set { _targetRetrieverModule = value; }
		}

		/// <summary>Gets and Sets distanceAdjusterModule property.</summary>
		public VCameraDistanceAdjusterModule distanceAdjusterModule
		{
			get { return _distanceAdjusterModule; }
			set { _distanceAdjusterModule = value; }
		}

		/// <summary>Gets and Sets occlusionEvaluatorModule property.</summary>
		public VCameraOcclusionEvaluatorModule occlusionEvaluatorModule
		{
			get { return _occlusionEvaluatorModule; }
			set { _occlusionEvaluatorModule = value; }
		}

		/// <summary>Gets and Sets distanceCalculatorModules property.</summary>
		public List<VCameraDistanceCalculatorModule> distanceCalculatorModules
		{
			get { return _distanceCalculatorModules; }
			set { _distanceCalculatorModules = value; }
		}

		/// <summary>Gets and Sets camera Component.</summary>
		public new Camera camera
		{ 
			get
			{
				if(_camera == null) _camera = GetComponent<Camera>();
				return _camera;
			}
		}

		/// <summary>Gets and Sets viewportHandler Component.</summary>
		public VCameraViewportHandler viewportHandler
		{ 
			get
			{
				if(_viewportHandler == null) _viewportHandler = GetComponent<VCameraViewportHandler>();
				return _viewportHandler;
			}
		}

		/// <summary>Gets and Sets normalAdjuster Component.</summary>
		public OrientationNormalAdjuster normalAdjuster
		{ 
			get
			{
				if(_normalAdjuster == null) _normalAdjuster = GetComponent<OrientationNormalAdjuster>();
				return _normalAdjuster;
			}
		}

		/// <summary>Gets sphereCollider Component.</summary>
		public SphereCollider sphereCollider
		{ 
			get
			{
				if(_sphereCollider == null) _sphereCollider = GetComponent<SphereCollider>();
				return _sphereCollider;
			}
		}

		/// <summary>Implicit VCamera to Camera operator.</summary>
		public static implicit operator Camera(VCamera _baseCamera) { return _baseCamera.camera; }
	#endregion

	#region UnityCallbacks:
	/*===================================================================================================================================================
	| 	Unity's Callbacks:																																|
	===================================================================================================================================================*/
		/// <summary>Draws Gizmos on Editor mode.</summary>
		protected virtual void OnDrawGizmos()
		{
	#if UNITY_EDITOR
			Gizmos.color = gizmosColor;

			Gizmos.DrawRay(transform.position, centerFocusDirection);
			Vector3 direction = centerRay.origin - viewportRay.origin;

			Gizmos.color = VColor.orange;
			Gizmos.DrawRay(viewportRay.origin, direction);

			if(!Application.isPlaying) return;
			else
			{
				UpdateFocusDisplacement();
			}

			boundariesPlane.DrawGizmos();

			if(targetRetrieverModule != null) targetRetrieverModule.DrawGizmos(this);
			if(displacementFollowModule != null) displacementFollowModule.DrawGizmos(this);
			if(rotationFollowModule != null) rotationFollowModule.DrawGizmos(this);
			if(distanceAdjusterModule != null) distanceAdjusterModule.DrawGizmos(this);
			if(occlusionEvaluatorModule != null) occlusionEvaluatorModule.DrawGizmos(this);
	#endif
		}

		/// <summary>Resets Component.</summary>
		protected virtual void Reset()
		{
			ignoreFocusAxes = Axes3D.None;
			sphereCollider.radius = 0.25f;
			sphereCollider.isTrigger = true;
	#if UNITY_EDITOR
			gizmosColor = Color.white;
	#endif
		}

		/// <summary>VCamera's instance initialization when loaded [Before scene loads].</summary>
		protected virtual void Awake()
		{
			if(defaultTarget != null && targetRetrieverModule != null) targetRetrieverModule.AddTarget(defaultTarget);
		}
		
		/// <summary>VCamera's tick at the end of each frame.</summary>
		private void LateUpdate()
		{
			switch(updateCameraAt)
			{
				case LoopType.Update:
				case LoopType.LateUpdate:
					OnUpdate();
					CameraUpdate();
				break;
			}
		}

		/// <summary>Updates VCamera's instance at each Physics Thread's frame.</summary>
		private void FixedUpdate()
		{
			switch(updateCameraAt)
			{
				case LoopType.FixedUpdate:
					OnUpdate();
					CameraFixedUpdate();
				break;
			}
		}
	#endregion

	#region InternalCallbacks:
	/*===================================================================================================================================================
	| 	VCamera's Internal Callbacks:																													|
	===================================================================================================================================================*/
		/// <summary>Updates Camera.</summary>
		protected virtual void CameraUpdate() { /*...*/ }

		/// <summary>Updates Camera on Physics' Thread.</summary>
		protected virtual void CameraFixedUpdate() { /*...*/ }

		/// <summary>Updates Camera in Editor Mode [called inside OnDrawGizmos].</summary>
		protected virtual void EditorUpdate()
		{
			//if()
		}
	#endregion

	#region Functions:
	/*===================================================================================================================================================
	| 	VCamera's Functions:																															|
	===================================================================================================================================================*/
		/// <summary>Calculates an adjusted direction given direction's axes, taking into account the Camera's local X and Z axes [Z axis being reorientated].</summary>
		/// <param name="_x">Axis X.</param>
		/// <param name="_y">Axis Y.</param>
		/// <returns>Adjusted Direction.</returns>
		public virtual Vector3 GetAdjustedDirectionXZ(float _x, float _y)
		{
			return (transform.right * _x) + (normalAdjuster.forward * _y);
		}

		/// <summary>Calculates an adjusted direction given direction's axes, taking into account the Camera's local X and Y axes [Y axis being reorientated].</summary>
		/// <param name="_x">Axis X.</param>
		/// <param name="_y">Axis Y.</param>
		/// <returns>Adjusted Direction.</returns>
		public virtual Vector3 GetAdjustedDirectionXY(float _x, float _y)
		{
			return (transform.right * _x) + (normalAdjuster.up * _y);
		}

		/// <returns>Gets Delta Time according to the Loop Type.</returns>
		public float GetDeltaTime()
		{
			switch(updateCameraAt)
			{
				case LoopType.Update:
				case LoopType.LateUpdate: 	return Time.deltaTime;
				case LoopType.FixedUpdate: 	return Time.fixedDeltaTime;
				default: 					return Time.smoothDeltaTime;
			}
		}

		/// <returns>Target's Position.</returns>
		public Vector3 GetTargetPosition() { return targetPosition; }

		/// <returns>Target's Rotation.</returns>
		public Quaternion GetTargetRotation() { return targetRotation; }

		/// <returns>Target's Bounds.</returns>
		public Bounds GetTargetBounds() { return targetBounds; }

		/// <returns>Target's Distance.</returns>
		public float GetTargetDistance() { return targetDistance; }
	#endregion

	#region Grid&ViewportMethods&Functions:
	/*===================================================================================================================================================
	| 	Grid & Viewport Methods & Functions:																											|
	===================================================================================================================================================*/
		/// <summery>Sets Direction between Focus Center and Target's Position.</summery>
		protected virtual void UpdateFocusDisplacement()
		{
			viewportRay = camera.ViewportPointToRay(new Vector3(viewportHandler.gridAttributes.centerX, viewportHandler.gridAttributes.centerY, 0.0f));
			centerRay = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
			Vector3 scaledViewportDirection = viewportRay.direction * /*distanceAdjuster.distance*/10f;
			Vector3 centerProjection = VVector3.VectorProjection(scaledViewportDirection, centerRay.direction);
			centerFocusDirection = (centerProjection - (scaledViewportDirection));
			
			if(camera.orthographic)
			{
				Vector3 shift = transform.InverseTransformDirection(viewportRay.direction);
				shift.z = 0.0f;
				centerFocusDirection = transform.TransformVector(shift);
			}

	#if UNITY_EDITOR
			Debug.DrawRay(viewportRay.origin, scaledViewportDirection, gizmosColor);
			Debug.DrawRay(centerRay.origin, centerProjection, gizmosColor);
	#endif
		}

		/// <returns>Focus' Direction, ignoring the flagged axes.</returns>
		public Vector3 GetFocusDisplacement()
		{
			return transform.IgnoreVectorAxes(centerFocusDirection, ignoreFocusAxes, true);
		}
		
		/// <returns>Boundaries' Viewport Plane.</returns>
		public CameraViewportPlane GetBoundariesPlane()
		{
			return boundariesPlane;
		}

		/// <returns>Axes where the target is currently within.</returns>
		/// <param name="_point">Point to evaluate.</param>
		public Axes3D GetAxesWhereTargetIsWithin(Vector3 _point)
		{
			return viewportHandler.Axes3DWithinGridFocusArea(_point);
		}
	#endregion

		/// <summary>Callback called on either LateUpdate or FixedUpdate.</summary>
		protected virtual void OnUpdate()
		{
			//distanceAdjuster.UpdateDistance();
			UpdateFocusDisplacement();
			VCameraViewportHandler.UpdateViewportPlane(camera, GetTargetDistance(), ref boundariesPlane);

			if(enabledModules == CameraModules.None) return;

			UpdateModules();
			ApplyModuleUpdates();
		}

		/// <summary>Applies all data updated from Modules into this Camera.</summary>
		protected virtual void ApplyModuleUpdates()
		{
			//if((enabledModules | CameraModules.DisplacementFollow) == enabledModules) transform.position = targetPosition;
			//if((enabledModules | CameraModules.RotationFollow) == enabledModules) transform.rotation = targetRotation;
		}

	#region ModuleMethods:
	/*===================================================================================================================================================
	| 	Modules Methods & Functions:																													|
	===================================================================================================================================================*/
		/// <summary>Tries updating all Camera's Modules.</summary>
		protected void UpdateModules()
		{
			TryUpdatingTargetRetrieverModule();
			TryUpdatingDisplacementFollowModule();
			TryUpdatingRotationFollowModule();
			TryUpdatingDistanceAdjusterModule();
			TryUpdatingOcclusionEvaluatorModule();
		}

		/// <param name="_modules">Modules to evaluate.</param>
		/// <returns>True if provided Modules are enabled.</returns>
		public bool AreModulesEnabled(CameraModules _modules) { return (enabledModules | _modules) == enabledModules; }

		/// <summary>Sets Target-Retriever's Module.</summary>
		/// <param name="_module">Module to assign.</param>
		/// <param name="_enable">Enable module? true by default. If you pass a null module, it will ignore this boolean and disable the module anyway.</param>
		public void SetTargetRetrieverModule(VCameraTargetRetrieverModule _module, bool _enable = true)
		{
			targetRetrieverModule = _module;
			EnableTargetRetrieverModule(targetRetrieverModule != null ? _enable : false);
		}

		/// <summary>Sets Displacement-Follow's Module.</summary>
		/// <param name="_module">Module to assign.</param>
		/// <param name="_enable">Enable module? true by default. If you pass a null module, it will ignore this boolean and disable the module anyway.</param>
		public void SetDisplacementFollowModule(VCameraDisplacementFollowModule _module, bool _enable = true)
		{
			displacementFollowModule = _module;
			EnableDisplacementFollowModule(displacementFollowModule != null ? _enable : false);
		}

		/// <summary>Sets Rotation-Follow's Module.</summary>
		/// <param name="_module">Module to assign.</param>
		/// <param name="_enable">Enable module? true by default. If you pass a null module, it will ignore this boolean and disable the module anyway.</param>
		public void SetRotationFollowModule(VCameraRotationFollowModule _module, bool _enable = true)
		{
			rotationFollowModule = _module;
			EnableDisplacementFollowModule(rotationFollowModule != null ? _enable : false);
		}

		/// <summary>Sets Distance-Adjuster's Module.</summary>
		/// <param name="_module">Module to assign.</param>
		/// <param name="_enable">Enable module? true by default. If you pass a null module, it will ignore this boolean and disable the module anyway.</param>
		public void SetDistanceAdjusterModule(VCameraDistanceAdjusterModule _module, bool _enable = true)
		{
			distanceAdjusterModule = _module;
			EnableDistanceAdjusterModule(distanceAdjusterModule != null ? _enable : false);
		}

		/// <summary>Sets Occlusion-Evaluator's Module.</summary>
		/// <param name="_module">Module to assign.</param>
		/// <param name="_enable">Enable module? true by default. If you pass a null module, it will ignore this boolean and disable the module anyway.</param>
		public void SetOcclusionEvaluatorModule(VCameraOcclusionEvaluatorModule _module, bool _enable = true)
		{
			occlusionEvaluatorModule = _module;
			EnableOcclusionEvaluatorModule(occlusionEvaluatorModule != null ? _enable : false);
		}

		/// <summary>Tries to update the Target-Retriever Module (if the Camera has one reference and the module is enabled).</summary>
		protected virtual void TryUpdatingTargetRetrieverModule()
		{
			if((enabledModules | CameraModules.TargetRetriever) != enabledModules || targetRetrieverModule == null)
			{
				EnableTargetRetrieverModule(false);
				return;
			}

			targetRetrieverModule.GetTargetData(this, out targetPosition, out targetRotation, out targetBounds);
			targetRetrieverModule.UpdateModule(this);
		}

		/// <summary>Tries to update the Displacement-Follow Module (if the Camera has one reference and the module is enabled).</summary>
		protected virtual void TryUpdatingDisplacementFollowModule()
		{
			if((enabledModules | CameraModules.DisplacementFollow) != enabledModules || displacementFollowModule == null)
			{
				EnableDisplacementFollowModule(false);
				return;
			}

			desiredDisplacement = displacementFollowModule.GetTargetPosition(this, targetPosition);
			displacementFollowModule.UpdateModule(this);
			transform.position = desiredDisplacement;
		}

		/// <summary>Tries to update the Rotation-Follow Module (if the Camera has one reference and the module is enabled).</summary>
		protected virtual void TryUpdatingRotationFollowModule()
		{
			if((enabledModules | CameraModules.RotationFollow) != enabledModules || rotationFollowModule == null)
			{
				EnableRotationFollowModule(false);
				return;
			}

			rotationFollowModule.UpdateModule(this);
			//targetOrientation = rotationFollowModule.GetFollowingDirection(this, targetPosition);
			desiredRotation = rotationFollowModule.GetTargetRotation(this, targetPosition, out targetOrientation);
			transform.rotation = desiredRotation;
		}

		/// <summary>Tries to update the Distance-Adjuster Module (if the Camera has one reference and the module is enabled).</summary>
		protected virtual void TryUpdatingDistanceAdjusterModule()
		{
			if((enabledModules | CameraModules.DistanceAdjuster) != enabledModules || distanceAdjusterModule == null)
			{
				EnableDistanceAdjusterModule(false);
				return;
			}

			distanceAdjusterModule.UpdateModule(this);
			targetDistance = distanceAdjusterModule.UpdateDistance(this, targetPosition);
		}

		/// <summary>Tries to update the Occlusion-Evaluator Module (if the Camera has one reference and the module is enabled).</summary>
		protected virtual void TryUpdatingOcclusionEvaluatorModule()
		{
			if((enabledModules | CameraModules.OcclusionEvaluator) != enabledModules || _occlusionEvaluatorModule == null)
			{
				EnableOcclusionEvaluatorModule(false);
				return;
			}

			_occlusionEvaluatorModule.UpdateModule(this);
		}

		/// <summary>Enables Target-Retrievers Module.</summary>
		/// <param name="_enable">Enable module? true by default.</param>
		public void EnableTargetRetrieverModule(bool _enable = true)
		{
			switch(_enable)
			{
				case true:
					enabledModules |= CameraModules.TargetRetriever;
				break;

				case false:
					enabledModules &= ~CameraModules.TargetRetriever;
				break;
			}
		}

		/// <summary>Enables Displacement-Follow's Module.</summary>
		/// <param name="_enable">Enable module? true by default.</param>
		public void EnableDisplacementFollowModule(bool _enable = true)
		{
			switch(_enable)
			{
				case true:
					enabledModules |= CameraModules.DisplacementFollow;
				break;

				case false:
					enabledModules &= ~CameraModules.DisplacementFollow;
				break;
			}
		}

		/// <summary>Enables Rotation-Follow's Module.</summary>
		/// <param name="_enable">Enable module? true by default.</param>
		public void EnableRotationFollowModule(bool _enable = true)
		{
			switch(_enable)
			{
				case true:
					enabledModules |= CameraModules.RotationFollow;
				break;

				case false:
					enabledModules &= ~CameraModules.RotationFollow;
				break;
			}
		}

		/// <summary>Enables Distance-Adjuster's Module.</summary>
		/// <param name="_enable">Enable module? true by default.</param>
		public void EnableDistanceAdjusterModule(bool _enable = true)
		{
			switch(_enable)
			{
				case true:
					enabledModules |= CameraModules.DistanceAdjuster;
				break;

				case false:
					enabledModules &= ~CameraModules.DistanceAdjuster;
				break;
			}
		}

		/// <summary>Enables Occlusion-Evaluator's Module.</summary>
		/// <param name="_enable">Enable module? true by default.</param>
		public void EnableOcclusionEvaluatorModule(bool _enable = true)
		{
			switch(_enable)
			{
				case true:
					enabledModules |= CameraModules.OcclusionEvaluator;
				break;

				case false:
					enabledModules &= ~CameraModules.OcclusionEvaluator;
				break;
			}
		}
	#endregion

		/// <returns>String representing this VCamera's intance.</returns>
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();

			builder.Append("Enabled Modules: ");
			builder.AppendLine(enabledModules.ToString());

			return builder.ToString();
		}
	}
}