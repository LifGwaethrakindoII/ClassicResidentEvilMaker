using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace Voidless
{
public class SceneTransitionController : Singleton<SceneTransitionController>
{
	[SerializeField] private PlayableDirector _playableDirector; 	/// <summary>PlayableDirector's reference.</summary>

	/// <summary>Gets playableDirector property.</summary>
	public PlayableDirector playableDirector { get { return _playableDirector; } }

#region UnityMethods:
	/// <summary>SceneTransitionController's instance initialization.</summary>
	private void Awake()
	{
		
	}

	/// <summary>SceneTransitionController's starting actions before 1st Update frame.</summary>
	private void Start ()
	{
		this.StartCoroutine(PlaySequence());
	}
	
	/// <summary>SceneTransitionController's tick at each frame.</summary>
	private void Update ()
	{
		
	}
#endregion

	private IEnumerator PlaySequence()
	{
		playableDirector.Play();

		SecondsDelayWait wait = new SecondsDelayWait((float)playableDirector.playableAsset.duration);
	
		while(wait.MoveNext()) yield return null;

		Debug.Log("[SceneTransitionController] Stop");
		playableDirector.Pause();
		playableDirector.Stop();
		SceneManager.LoadScene(PlayerPrefs.GetString("SceneToLoad"));
	}
}
}