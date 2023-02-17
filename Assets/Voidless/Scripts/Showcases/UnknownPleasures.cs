using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Voidless
{
[RequireComponent(typeof(AudioSource))]
public class UnknownPleasures : MonoBehaviour
{
	[SerializeField] private AudioClip[] songs; 			/// <summary>Unknown Pleasure's Songs.</summary>
	[Space(5f)]
	[SerializeField] private LineRenderer[] lineRenderers; 	/// <summary>LineRenderers.</summary>
	[Space(5f)]
	[SerializeField] private int beizerSegmentsPerLine; 	/// <summary>Beizer Segments per-line.</summary>
	[SerializeField] private float beizerDivisions; 		/// <summary>Divisions (points per-segment) for Beizer Curve.</summary>
	[SerializeField] private float verticalSpace; 			/// <summary>Vertical Space between each LineRenderer.</summary>
	[SerializeField] private float width; 					/// <summary>Area's Width.</summary>
	[SerializeField] private float startingOffset; 			/// <summary>Starting's Offset.</summary>
	[SerializeField] private float speed; 					/// <summary>Sinusoidal Speed.</summary>
	[Space(5f)]
	[Header("UI:")]
	[SerializeField] private GameObject UIContainer; 		/// <summary>UI's Container.</summary>
	[SerializeField] private Button previousButton; 		/// <summary>Previous Song's Button.</summary>
	[SerializeField] private Button nextButton; 			/// <summary>Next Song's Button.</summary>
	[SerializeField] private Text currentSongText; 			/// <summary>Current Song's Text.</summary>
	private Vector3 startingPoint;
	private Vector3 verticalDisplacement;
	private float height;
	private Vector3[,] positions;
	private int songIndex;
	private AudioSource audioSource;
	private Coroutine coroutine;

	/// <summary>Draws Gizmos on Editor mode.</summary>
	private void OnDrawGizmos()
	{
		if(!Application.isPlaying) UpdateLineRenderers();
		
		Vector3 a = startingPoint + (Vector3.right * startingOffset);
		Vector3 b = a + (Vector3.down * height);
		Vector3 c = startingPoint + (Vector3.right * (width - startingOffset));
		Vector3 d = c + (Vector3.down * height);;

		Gizmos.DrawWireCube(Vector3.zero, new Vector3(width, height, 1.0f));
		Gizmos.DrawLine(a, b);
		Gizmos.DrawLine(c, d);
	}

	/// <summary>UnknownPleasures's instance initialization.</summary>
	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		UpdateLineRenderers();
		songIndex = 0;

		previousButton.onClick.AddListener(PreviousSong);
		nextButton.onClick.AddListener(NextSong);

		this.StartCoroutine(WaitForSongToEnd(songs[songIndex]), ref coroutine);
	}
	
	/// <summary>UnknownPleasures's tick at each frame.</summary>
	private void Update ()
	{
		if(Input.GetKeyDown(KeyCode.Space)) UIContainer.SetActive(!UIContainer.activeSelf);
		if(Input.GetKeyDown(KeyCode.LeftArrow)) PreviousSong();
		if(Input.GetKeyDown(KeyCode.RightArrow)) NextSong();

		if(lineRenderers == null) return;

		int j = 0;

		foreach(LineRenderer lineRenderer in lineRenderers)
		{
			int count = lineRenderer.positionCount;

			lineRenderer.SetPosition(0, lineRenderer.transform.position);
			
			for(int i = 1; i < (count - 1); i++)
			{
				float t = VMath.RemapValueToNormalizedRange(Mathf.Sin(Time.time * speed), -1.0f, 1.0f);
				float y = VMath.RemapValueToNormalizedRange(Mathf.Sin(((float)i / (float)(count - 1)) * speed), -1.0f, 1.0f);
				Vector3 a = positions[j, i];
				Vector3 b = Vector3.Lerp(positions[j, i], lineRenderer.GetPosition(count - 1), t);
				Vector3 p = Vector3.Lerp(a, b, t);
				p.y = y * 5f;

				lineRenderer.SetPosition(i, p);
			}

			lineRenderer.SetPosition(count - 1, lineRenderer.transform.position + (lineRenderer.transform.right * width));
			j++;
		}
	}

	/// <summary>Updates LineRenderers.</summary>
	public void UpdateLineRenderers()
	{
		if(lineRenderers == null) return;

		float length = (float)lineRenderers.Length;
		height = (Mathf.Max((length - 1.0f), 0.0f) * verticalSpace);
		width = height * 0.75f;
		startingPoint = new Vector3(
			(width * -0.5f),
			(height * 0.5f),
			0.0f
		);
		verticalDisplacement = new Vector3(0.0f, -verticalSpace, 0.0f);
		float i = 0.0f;

		positions = new Vector3[lineRenderers.Length, beizerSegmentsPerLine + 1];

		foreach(LineRenderer lineRenderer in lineRenderers)
		{
			lineRenderer.transform.position = startingPoint + (verticalDisplacement * i);
			lineRenderer.positionCount = beizerSegmentsPerLine + 1;

			for(int j = 0; j < lineRenderer.positionCount; j++)
			{
				positions[(int)i, j] = lineRenderer.GetPosition(j);
			}

			i++;
		}
	}

	/// <summary>Goes to next song.</summary>
	public void NextSong()
	{
		int next = songIndex + 1;
		songIndex = next < songs.Length - 1 ? next : 0;

		this.StartCoroutine(WaitForSongToEnd(songs[songIndex]), ref coroutine);
	}

	/// <summary>Goes to previous song.</summary>
	public void PreviousSong()
	{
		int previous = songIndex - 1;
		songIndex = previous > -1 ? previous : songs.Length - 1;

		this.StartCoroutine(WaitForSongToEnd(songs[songIndex]), ref coroutine);
	}

	/// <summary>Waits for song to end before going to next song.</summary>
	private IEnumerator WaitForSongToEnd(AudioClip clip)
	{
		audioSource.PlaySound(clip);
		
		StringBuilder builder = new StringBuilder();

		builder.Append((songIndex + 1).ToString());
		builder.Append(" - ");
		builder.Append(clip.name);

		currentSongText.text = builder.ToString();

		float t = 0.0f;
		float duration = clip.length;

		while(t < duration)
		{
			t += Time.deltaTime;
			yield return null;
		}

		NextSong();
	}
}
}