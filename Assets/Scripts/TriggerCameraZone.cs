using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Voidless
{
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class TriggerCameraZone : MonoBehaviour
{
	[SerializeField] private TransformData transformData; 	/// <summary>Camera's Transform Data.</summary>
	private BoxCollider _boxCollider; 						/// <summary>BoxCollider's Component.</summary>
	private Rigidbody _rigidbody; 							/// <summary>Rigidbody's Component.</summary>

	/// <summary>Gets boxCollider Component.</summary>
	public BoxCollider boxCollider
	{ 
		get
		{
			if(_boxCollider == null) _boxCollider = GetComponent<BoxCollider>();
			return _boxCollider;
		}
	}

	/// <summary>Gets rigidbody Component.</summary>
	public Rigidbody rigidbody
	{ 
		get
		{
			if(_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
			return _rigidbody;
		}
	}

	/// <summary>Resets TriggerCameraZone's instance to its default values.</summary>
	public void Reset()
	{
		boxCollider.isTrigger = true;
		rigidbody.isKinematic = true;
		rigidbody.useGravity = false;
		transformData = new TransformData(Vector3.zero, Quaternion.identity, Vector3.one);
	}

	/// <summary>Draws Gizmos on Editor mode.</summary>
	private void OnDrawGizmos()
	{
		VGizmos.DrawTransformData(transformData);
	}

	[Button("Set TransformData To Camera's Transform")]
	/// <summary>Sets TransformData equal to Camera's Transform.</summary>
	private void SetTransformDataToCameraTransform()
	{
		Transform cameraTransform = Camera.main.transform;

		transformData.position = cameraTransform.position;
		transformData.rotation = cameraTransform.rotation;
	}

	[Button("Set Camera's Transform to TransformData")]
	/// <summary>Sets TransformData equal to Camera's Transform.</summary>
	private void SetCameraTransformToTransformData()
	{
		Transform cameraTransform = Camera.main.transform;

		cameraTransform.position = transformData.position;
		cameraTransform.rotation = transformData.rotation;
	}

	/// <summary>Event triggered when this Collider enters another Collider trigger.</summary>
	/// <param name="col">The other Collider involved in this Event.</param>
	private void OnTriggerEnter(Collider col)
	{
		GameObject obj = col.gameObject;
	
		switch(obj.tag)
		{
			case "Player":
			SetCameraTransformToTransformData();
			break;
	
			default:
			break;
		}
	}
}
}