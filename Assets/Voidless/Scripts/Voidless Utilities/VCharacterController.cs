using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
}