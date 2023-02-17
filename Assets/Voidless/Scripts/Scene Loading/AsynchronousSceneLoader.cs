using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Voidless
{
public static class AsynchronousSceneLoader
{
	public const float PROGRESS_LIMIT = 0.9f; 	/// <summary>Asynchronous Operation's progress limit.</summary>

	private static AsyncOperation _operation; 	/// <summary>Asynchronous Loading Scene.</summary>

	/// <summary>Gets and Sets operation property.</summary>
	public static AsyncOperation operation
	{
		get { return _operation; }
		private set { _operation = value; }
	}

	/// <summary>Gets progress property.</summary>
	public static float progress { get { return operation != null ? operation.progress : 0.0f; } }

	/// <summary>Gets operationLoading property.</summary>
	public static bool operationLoading { get { return operation != null; } }

	/// <summary>Loads scene Asynchronously and stores the static operation reference.</summary>
	/// <param name="_scene">Scene's Name.</param>
	/// <param name="_mode">Load Scene's Mode.</param>
	/// <param name="_allowSceneActivation">Allow Scene Activation? false as default.</param>
	public static void LoadScene(string _scene, LoadSceneMode _mode = LoadSceneMode.Single, bool _allowSceneActivation = false)
	{
		operation = SceneManager.LoadSceneAsync(_scene, _mode);
		operation.allowSceneActivation = _allowSceneActivation;
	}

	/// <summary>Loads scene Asynchronously and stores the static operation reference.</summary>
	/// <param name="_scene">Scene's Index in Build Settings.</param>
	/// <param name="_mode">Load Scene's Mode.</param>
	/// <param name="_allowSceneActivation">Allow Scene Activation? false as default.</param>
	public static void LoadScene(int _scene, LoadSceneMode _mode = LoadSceneMode.Single, bool _allowSceneActivation = false)
	{
		operation = SceneManager.LoadSceneAsync(_scene, _mode);
		operation.allowSceneActivation = _allowSceneActivation;
	}

	/// <summary>Allows scene animation to loading operation.</summary>
	/// <param name="_allowSceneActivation">Allow Scene Activation? true as default.</param>
	public static void AllowSceneActivation(bool _allow = true)
	{
		if(operationLoading) operation.allowSceneActivation = _allow;
	}

	/// <summary>Loads Scene Asunchronously and executes an action taking the loading progress as parameter.</summary>
	/// <param name="_scene">Scene's Name.</param>
	/// <param name="action">Action to invoke, taking the loading progress.</param>
	/// <param name="onLoadEnds">Optional callback invoked when the loading ends.</param>
	/// <param name="_mode">Load Scene's Mode.</param>
	/// <param name="_additionalWait">Additional Wait [0 by default].</param>
	public static IEnumerator LoadSceneAndDoWhileWaiting(string _scene, Action<float> action, Action onLoadEnds = null, LoadSceneMode _mode = LoadSceneMode.Single, float _additionalWait = 0.0f)
	{
		LoadScene(_scene, _mode, false);
		float wait = 0.0f;
		float inverseDuration = PROGRESS_LIMIT / _additionalWait;
		float min = Mathf.Min(wait, progress);

		while(min < PROGRESS_LIMIT)
		{
			action(min);
			wait += (Time.deltaTime * inverseDuration);
			min = Mathf.Min(wait, progress);
			yield return null;
		}

		action(1.0f);
		AllowSceneActivation();
		if(onLoadEnds != null) onLoadEnds();
	}

	/// <summary>Loads Scene Asunchronously and executes an action taking the loading progress as parameter.</summary>
	/// <param name="_scene">Scene's Index in Build Settings.</param>
	/// <param name="action">Action to invoke, taking the loading progress.</param>
	/// <param name="onLoadEnds">Optional callback invoked when the loading ends.</param>
	/// <param name="_mode">Load Scene's Mode.</param>
	/// <param name="_additionalWait">Additional Wait [0 by default].</param>
	public static IEnumerator LoadSceneAndDoWhileWaiting(int _scene, Action<float> action, Action onLoadEnds = null, LoadSceneMode _mode = LoadSceneMode.Single, float _additionalWait = 0.0f)
	{
		LoadScene(_scene, _mode, false);
		float wait = 0.0f;
		float inverseDuration = PROGRESS_LIMIT / _additionalWait;
		float min = Mathf.Min(wait, progress);

		while(min < PROGRESS_LIMIT)
		{
			action(min);
			wait += (Time.deltaTime * inverseDuration);
			min = Mathf.Min(wait, progress);
			yield return null;
		}

		action(1.0f);
		if(onLoadEnds != null) onLoadEnds();
		AllowSceneActivation();
	}
}
}