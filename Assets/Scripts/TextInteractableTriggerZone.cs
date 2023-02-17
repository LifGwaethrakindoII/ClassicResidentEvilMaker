using System;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace Voidless
{
public class TextInteractableTriggerZone : InteractableTriggerZone
{
	[TextArea]
	[SerializeField] private string[] _lines; 				/// <summary>Text Lines.</summary>
	[SerializeField] private float _writingDuration; 		/// <summary>Writing's Duration.</summary>
	[Range(0.0f, 1.0f)]
	[SerializeField] private float _interruptThreshold; 	/// <summary>Threshold's percentage to be allowed to pass current line.</summary>
	[Space(5f)]
	[SerializeField] private bool _changeCameraTake; 		/// <summary>Change Camera Take?.</summary>
	[SerializeField] private TransformData _transformData; 	/// <summary>Camera's Transform Data.</summary>
	private Coroutine writeCoroutine; 						/// <summary>Coroutine's reference.</summary>
	private int _currentLineIndex; 							/// <summary>Current Line's Index.</summary>
	private float _currentLineProgress;  					/// <summary>Current Line's Normalized Progress.</summary>
	private TransformData _previousTransformData; 			/// <summary>Previous' TransformData.</summary>

	/// <summary>Gets and Sets lines property.</summary>
	public string[] lines
	{
		get { return _lines; }
		set { _lines = value; }
	}

	/// <summary>Gets writingDuration property.</summary>
	public float writingDuration { get { return _writingDuration; } }

	/// <summary>Gets interruptThreshold property.</summary>
	public float interruptThreshold { get { return _interruptThreshold; } }

	/// <summary>Gets changeCameraTake property.</summary>
	public bool changeCameraTake { get { return _changeCameraTake; } }

	/// <summary>Gets and Sets transformData property.</summary>
	public TransformData transformData
	{
		get { return _transformData; }
		set { _transformData = value; }
	}

	/// <summary>Gets and Sets previousTransformData property.</summary>
	public TransformData previousTransformData
	{
		get { return _previousTransformData; }
		set { _previousTransformData = value; }
	}

	/// <summary>Gets and Sets currentLineIndex property.</summary>
	public int currentLineIndex
	{
		get { return _currentLineIndex; }
		set { _currentLineIndex = value; }
	}

	/// <summary>Gets and Sets currentLineProgress property.</summary>
	public float currentLineProgress
	{
		get { return _currentLineProgress; }
		set { _currentLineProgress = value; }
	}

	/// <summary>Draws Gizmos on Editor mode.</summary>
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		
		if(changeCameraTake) VGizmos.DrawTransformData(transformData);
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

	/// <summary>Callback invoked when an action is performed.</summary>
	/// <param name="ID">Action's ID.</param>
	/// <param name="state">Input's State.</param>
	protected override void OnInputAction(int ID, InputState state)
	{
		Debug.Log("[TextInteractableTriggerZone] Input Action called. ID: " + ID + ", State: " + state.ToString());

		if(lines == null || lines.Length == 0) return;

		switch(state)
		{
			case InputState.Begins:
				switch(ID)
				{
					case 1:
						if(writeCoroutine == null)
						{
							if(changeCameraTake)
							{
								Transform cameraTransform = Camera.main.transform;

								previousTransformData = cameraTransform;
								cameraTransform.position = transformData.position;
								cameraTransform.rotation = transformData.rotation;
							}
							Character character = FindObjectOfType<Character>();
							if(character != null) character.EnableRenderers(false);
							this.StartCoroutine(WriteText(lines[currentLineIndex]), ref writeCoroutine);

						} else if(currentLineIndex + 1 < lines.Length && currentLineProgress >= interruptThreshold)
						{
							currentLineIndex++;
							this.StartCoroutine(WriteText(lines[currentLineIndex]), ref writeCoroutine);
						}
						else
						{
							this.DispatchCoroutine(ref writeCoroutine);
							currentLineIndex = 0;
							currentLineProgress = 0.0f;
							GameplayUIController.mainText.text = string.Empty;

							if(changeCameraTake)
							{
								Transform cameraTransform = Camera.main.transform;

								cameraTransform.position = previousTransformData.position;
								cameraTransform.rotation = previousTransformData.rotation;
							}
							Debug.Log("[TextInteractableTriggerZone] End of text...");
							Character character = FindObjectOfType<Character>();
							if(character != null) character.EnableRenderers(true);
						}
					break;
				}
			break;
		}
	}

	/// <summary>Write Text's Coroutine.</summary>
	private IEnumerator WriteText(string line)
	{
		Text text = GameplayUIController.mainText;
		StringBuilder builder = new StringBuilder();
		SecondsDelayWait wait = new SecondsDelayWait(writingDuration);
		float lineLength = (float)line.Length;
		
		text.text = string.Empty;
		currentLineProgress = 0.0f;

		for(int i = 0; i < line.Length; i++)
		{
			builder.Append(line[i]);
			currentLineProgress = (float)(i + 1.0f) / lineLength;
			text.text = builder.ToString();

			while(wait.MoveNext()) yield return null;
			wait.Reset();
		}

		currentLineProgress = 1.0f;
	}
}
}