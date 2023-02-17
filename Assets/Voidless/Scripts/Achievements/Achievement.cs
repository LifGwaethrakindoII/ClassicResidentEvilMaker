using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Voidless
{
public class Achievement : IAchievement
{
	[SerializeField] private bool _completed; 				/// <summary>Has the Achievement been completed? set to true when percentCompleted is 100.0.</summary>
	[SerializeField] private bool _hidden; 					/// <summary>Is this Achievement currently hidden from the user?.</summary>
	[SerializeField] private string _id; 					/// <summary>Unique identifier of this Achievement.</summary>
	[SerializeField] private DateTime _lastReportedDate; 	/// <summary>Date when the Achivement was completed [set by the server when percentCompleted is updated].</summary>
	[SerializeField] private double _percentCompleted; 		/// <summary>Progress of this Achievement.</summary>

	/// <summary>Gets and Sets completed property.</summary>
	public bool completed
	{
		get { return _completed; }
		set { _completed = value; }
	}

	/// <summary>Gets and Sets hidden property.</summary>
	public bool hidden
	{
		get { return _hidden; }
		set { _hidden = value; }
	}

	/// <summary>Gets and Sets id property.</summary>
	public string id
	{
		get { return _id; }
		set { _id = value; }
	}

	/// <summary>Gets and Sets lastReportedDate property.</summary>
	public DateTime lastReportedDate
	{
		get { return _lastReportedDate; }
		set { _lastReportedDate = value; }
	}

	/// <summary>Gets and Sets percentCompleted property.</summary>
	public double percentCompleted
	{
		get { return _percentCompleted; }
		set { _percentCompleted = value; }
	}

	/// <summary>Send notification about progress on this achievement.</summary>
	public void ReportProgress(Action<bool> callback)
	{
		/// \TODO Implement....
	}
}
}