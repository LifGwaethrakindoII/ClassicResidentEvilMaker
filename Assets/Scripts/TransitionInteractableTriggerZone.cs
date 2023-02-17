using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Voidless
{
public class TransitionInteractableTriggerZone : InteractableTriggerZone
{
	[SerializeField] private string _sceneName; 	/// <summary>Scene that this interactable leads to.</summary>

	/// <summary>Gets sceneName property.</summary>
	public string sceneName { get { return _sceneName; } }

	/// <summary>Callback invoked when an action is performed.</summary>
	/// <param name="ID">Action's ID.</param>
	/// <param name="state">Input's State.</param>
	protected override void OnInputAction(int ID, InputState state)
	{
		PlayerPrefs.SetString("SceneToLoad", sceneName);
		SceneManager.LoadScene("Transition");
	}
}
}