using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Voidless
{
[RequireComponent(typeof(Rigidbody))]
public class CameraTriggerZone : TriggerZone<CameraTriggerZone>
{
	[SerializeField] private TransformData _transformData; 	/// <summary>Camera's Transform Data.</summary>

	/// <summary>Gets and Sets transformData property.</summary>
	public TransformData transformData
	{
		get { return _transformData; }
		set { _transformData = value; }
	}

	/// <summary>Resets CameraTriggerZone's instance to its default values.</summary>
	protected override void Reset()
	{
		base.Reset();
		transformData = new TransformData(Vector3.zero, Quaternion.identity, Vector3.one);
	}

	/// <summary>Draws Gizmos on Editor mode.</summary>
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		VGizmos.DrawTransformData(transformData);
	}

	[Button("Set TransformData To Camera's Transform")]
	/// <summary>Sets TransformData equal to Camera's Transform.</summary>
	private void SetTransformDataToCameraTransform()
	{
		Transform cameraTransform = Camera.main.transform;

		transformData = cameraTransform;
	}

	[Button("Set Camera's Transform to TransformData")]
	/// <summary>Sets TransformData equal to Camera's Transform.</summary>
	private void SetCameraTransformToTransformData()
	{
		Transform cameraTransform = Camera.main.transform;

		cameraTransform.position = transformData.position;
		cameraTransform.rotation = transformData.rotation;
	}

	/// <summary>Callback internally invoked when a GameObject's Collider enters the TriggerZone.</summary>
	/// <param name="_collider">Collider that Enters.</param>
	protected override void OnEnter(Collider _collider)
	{
		base.OnEnter(_collider);

		SetCameraTransformToTransformData();
	}
}
}