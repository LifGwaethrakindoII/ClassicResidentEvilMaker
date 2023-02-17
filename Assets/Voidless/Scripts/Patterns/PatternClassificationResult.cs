using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Voidless
{
public class PatternClassificationResult : IEnumerator<UserPatternWaypoints>
{
	private const int DEFAULT_VALUE = -1;

	private PatternClassification _classification;
	private int _currentIndex;
	private float _waitTolerance;
	private SecondsDelayWait wait;

	/// <summary>Gets and Sets classification property.</summary>
	public PatternClassification classification
	{
		get { return _classification; }
		set { _classification = value; }
	}

	/// <summary>Gets and Sets currentIndex property.</summary>
	public int currentIndex
	{
		get { return _currentIndex; }
		private set { _currentIndex = value; }
	}

	/// <summary>Gets and Sets waitTolerance property.</summary>
	public float waitTolerance
	{
		get { return _waitTolerance; }
		set { _waitTolerance = value; }
	}

	/// <summary>Gets current iterator's value.</summary>
	public UserPatternWaypoints Current
	{
		get
		{
			UserPatternWaypoints waypoint = default(UserPatternWaypoints);

			try { waypoint = classification.waypoints[currentIndex]; }
			catch(Exception exception) { Debug.LogError("[PatternClassificationResult] Catched Exception (Returning default UserPatternWaypoints structure): " + exception.Message); }
			
			return waypoint;
		}
	}

	/// <summary>Gets previous iterator value.</summary>
	public UserPatternWaypoints Previous
	{
		get { return currentIndex > 0 ? classification.waypoints[currentIndex - 1] : Current; }
	}

	/// <summary>Gets current iterator's value as an Object.</summary>
	object IEnumerator.Current { get { return Current; } }

	/// <returns>True if the iterator has started [Index different than 0].</returns>
	public bool started { get { return (currentIndex > DEFAULT_VALUE); } }

	/// <returns>True of the iterator is at the end [Index at last possible value].</returns>
	public bool finished { get { return currentIndex == (classification.waypoints.Length - 1); } }

	/// <summary>PatternClassificationResult constructor.</summary>
	/// <param name="_classification">Classification to evaluate through iteration.</param>
	public PatternClassificationResult(PatternClassification _classification, float _waitTolerance)
	{
		classification = _classification;
		currentIndex = DEFAULT_VALUE;
		wait = new SecondsDelayWait(waitTolerance = _waitTolerance);
	}

	/// <summary>Moves to the next iterator.</summary>
	/// <returns>True if it was able to move to next iterator.</returns>
	public bool MoveNext()
	{
		currentIndex++;
		if(currentIndex >= classification.waypoints.Length)
		{
			currentIndex = DEFAULT_VALUE;
			return false;
		}
		else return true;
	}

	/// <summary>Has the Wait toleration ended?.</summary>
	public bool TolerationEnded()
	{
		return !wait.MoveNext();
	}

	/// <summary>Resets Iterator.</summary>
	public void Reset()
	{
		currentIndex = DEFAULT_VALUE;
		wait.Reset();
	}

	/// <summary>Resets Toleration Wait.</summary>
	public void ResetToleration()
	{
		wait.Reset();
	}

	/// <summary>Disposes Iterator.</summary>
	public void Dispose() {  }
}
}