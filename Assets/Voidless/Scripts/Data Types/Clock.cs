using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[Flags]
public enum TimeFormat
{
	None = 0,
	Hours = 1,
	Minutes = 2,
	Seconds = 4,

	HoursAndMinutes = Hours | Minutes,
	MinutesAndSeconds = Minutes | Seconds,
	All = Hours | Minutes | Seconds
}

public enum SecondsFractioning
{
	None,
	Deciseconds,
	Centiseconds,
	Miliseconds
}

[Serializable]
public class Clock
{
	public const float INVERSE_MINUTES = 1.0f / 60.0f; 						/// <summary>Seconds to Minutes converter.</summary>
	public const float INVERSE_HOURS = INVERSE_MINUTES * INVERSE_MINUTES; 	/// <summary>Seconds to Hours converter.</summary>

	[SerializeField] private TimeFormat _format; 							/// <summary>Time Format.</summary>
	[SerializeField] private SecondsFractioning _secondsFractioning; 		/// <summary>Type of seconds' fractioning.</summary>
	private float _ellapsedTime; 											/// <summary>Total Ellapsed Time.</summary>
	private float _hours; 													/// <summary>Hours.</summary>
	private float _minutes; 												/// <summary>Minutes.</summary>
	private float _seconds; 												/// <summary>Seconds.</summary>
	private float _deciseconds; 											/// <summary>Deciseconds.</summary>
	private float _centiseconds; 											/// <summary>Centiseconds.</summary>
	private float _miliseconds; 											/// <summary>Miliseconds.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets format property.</summary>
	public TimeFormat format
	{
		get { return _format; }
		set { _format = value; }
	}

	/// <summary>Gets and Sets secondsFractioning property.</summary>
	public SecondsFractioning secondsFractioning
	{
		get { return _secondsFractioning; }
		set { _secondsFractioning = value; }
	}

	/// <summary>Gets and Sets ellapsedTime property.</summary>
	public float ellapsedTime
	{
		get { return _ellapsedTime; }
		set
		{
			_ellapsedTime = Mathf.Max(value, 0.0f);
			UpdateUnits();
		}
	}

	/// <summary>Gets and Sets hours property.</summary>
	public float hours
	{
		get { return _hours; }
		private set { _hours = value; }
	}

	/// <summary>Gets and Sets minutes property.</summary>
	public float minutes
	{
		get { return _minutes; }
		private set { _minutes = value; }
	}

	/// <summary>Gets and Sets seconds property.</summary>
	public float seconds
	{
		get { return _seconds; }
		private set { _seconds = value; }
	}

	/// <summary>Gets and Sets deciseconds property.</summary>
	public float deciseconds
	{
		get { return _deciseconds; }
		private set { _deciseconds = value; }
	}

	/// <summary>Gets and Sets centiseconds property.</summary>
	public float centiseconds
	{
		get { return _centiseconds; }
		private set { _centiseconds = value; }
	}

	/// <summary>Gets and Sets miliseconds property.</summary>
	public float miliseconds
	{
		get { return _miliseconds; }
		private set { _miliseconds = value; }
	}
#endregion

	/// <summary>Clock's Constructor.</summary>
	/// <param name="_format">Time Format [All by default].</param>
	/// <param name="_secondsFractioning">Type of seconds' fractioning [None by default].</param>
	/// <param name="_initialTime">Initial Ellapsed time ['0' by default].</param>
	public Clock(TimeFormat _format = TimeFormat.All, SecondsFractioning _secondsFractioning = SecondsFractioning.None, float _initialTime = 0.0f)
	{
		format = _format;
		secondsFractioning = _secondsFractioning;
		ellapsedTime = _initialTime;
	}

	/// <summary>Ticks clock by given time step.</summary>
	/// <param name="_deltaTime">Delta Time as time step.</param>
	public void Update(float _deltaTime)
	{
		ellapsedTime += _deltaTime;
	}

	/// <summary>Resets Clock.</summary>
	/// <param name="_initialTime">Initial time [0.0f by default].</param>
	public void Reset(float _initialTime = 0.0f)
	{
		ellapsedTime = Mathf.Max(_initialTime, 0.0f);
	}

	/// <summary>Updates all clock units given the updated ellapsedTime.</summary>
	private void UpdateUnits()
	{
		hours = ellapsedTime >= 3600.0f ? Mathf.Floor(ellapsedTime * INVERSE_HOURS) : 0.0f;
		minutes = ellapsedTime >= 60.0f ? (format | TimeFormat.Hours) == format ? Mathf.Floor(ellapsedTime * INVERSE_MINUTES) % 60.0f : Mathf.Floor(ellapsedTime * INVERSE_MINUTES) : 0.0f;
		seconds = ellapsedTime % 60.0f;

		float flooredSeconds = Mathf.Floor(seconds);
		float secondsResidue = seconds - flooredSeconds;

		seconds = flooredSeconds;
		deciseconds = secondsResidue * 10.0f;
		centiseconds = secondsResidue * 100.0f;
		miliseconds = secondsResidue * 1000.0f;
	}

	/// <returns>String representing this clock's time.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();
		bool includesHours = (format | TimeFormat.Hours) == format;
		bool includesMinutes = (format | TimeFormat.Minutes) == format;
		bool includesSeconds = (format | TimeFormat.Seconds) == format;

		if(includesHours)
		{
			builder.Append(hours.ToString("00"));
		}
		if(includesMinutes)
		{
			if(includesHours) builder.Append(":");
			builder.Append(minutes.ToString("00"));
		}
		if(includesSeconds)
		{
			if(includesHours || includesMinutes) builder.Append(":");
			builder.Append(seconds.ToString("00"));
		}

		if(secondsFractioning != SecondsFractioning.None)
		{
			if(includesHours || includesMinutes || includesSeconds) builder.Append(":");

			switch(secondsFractioning)
			{
				case SecondsFractioning.Deciseconds:
				builder.Append(deciseconds.ToString("0"));
				break;

				case SecondsFractioning.Centiseconds:
				builder.Append(centiseconds.ToString("00"));
				break;

				case SecondsFractioning.Miliseconds:
				builder.Append(miliseconds.ToString("000"));
				break;
			}
		}

		return builder.ToString();
	}
}
}