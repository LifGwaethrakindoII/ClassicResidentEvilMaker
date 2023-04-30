using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Voidless.REMaker
{
public class HumanoidCharacter : Character
{
	[Space(5f)]
	[Header("Humaniod's Attributes:")]
	[SerializeField] private Transform _hip;

	/// <summary>Gets hip property.</summary>
	public Transform hip { get { return _hip; } }
}
}