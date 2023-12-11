using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*===========================================================================
**
** Class:  VCharacterController
**
** Purpose: Extension methods & functions for CharacterController's API.
**
**
** Author: Lîf Gwaethrakindo
**
===========================================================================*/
namespace Voidless
{
public static class VCharacterController
{
	/// <summary>Sets CharacterController's Position.</summary>
	/// <param name="_characterController">CharacterController's reference.</param>
	/// <param name="p">New position for CharacterController.</param>
	public static void SetPosition(this CharacterController _characterController, Vector3 p)
	{
		_characterController.enabled = false;
		_characterController.transform.position = p;
		_characterController.enabled = true;
	}

	/// <summary>Sets Capsule Collider's attributes equal to that of this CharacterController.</summary>
	/// <param name="_charatcerController">CharacterController's reference.</param>
	/// <param name="_capsule">CapsuleCollider's reference.</param>
	public static void SetAttributesToCapsuleCollider(this CharacterController _characterController, CapsuleCollider _capsule)
	{
		_capsule.center = _characterController.center;
		_capsule.radius = _characterController.radius;
		_capsule.height = _characterController.height;
	}

	/// <summary>Sets attributes from CapsuleCollider to CharacterController.</summary>
	/// <param name="_charatcerController">CharacterController's reference.</param>
	/// <param name="_capsule">CapsuleCollider's reference.</param>
	public static void GetAttributesFromCapsuleCollider(this CharacterController _characterController, CapsuleCollider _capsule)
	{
		_characterController.center = _capsule.center;
		_characterController.radius = _capsule.radius;
		_characterController.height = _capsule.height;
	}
}
}