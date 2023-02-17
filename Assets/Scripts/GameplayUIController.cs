using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Voidless
{
public class GameplayUIController : Singleton<GameplayUIController>
{
	[SerializeField] private Text _mainText; 	/// <summary>Main Text's Component.</summary>

	/// <summary>Gets mainText property.</summary>
	public static Text mainText { get { return Instance._mainText; } }
}
}