using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Voidless
{
[Serializable]
public class PopUpContent
{
	public AlertModes alertMode; 		///< <summary>Pop-Up's Alert mode.</summary>
	[Space(5f)]
	[Header("Labels:")]
	public string title; 				///< <summary>Pop-Up's Title label.</summary>
	public string message; 				///< <summary>Pop-Up's Message.</summary>
	public string cancelButtonLabel; 	///< <summary>Pop-Up's cancel button label.</summary>
	public string confirmButtonLabel; 	///< <summary>Pop-Up's confirm button label.</summary>
	[Space(5f)]
	[Header("Button Events:")]
	public UnityEvent OnCancelEvent; 	///< <summary>Pop-Up's cancel button Unity Event. Useful if you want to assign button's actions on the inspector</summary>
	public UnityEvent OnConfirmEvent; 	///< <summary>Pop-Up's confirm button Unity Event. Useful if you want to assign button's actions on the inspector</summary>
	public UnityAction OnCancel; 		///< <summary>Pop-Up's cancel button Unity Action.</summary>
	public UnityAction OnConfirm; 		///< <summary>Pop-Up's confirm button Unity Action.</summary>

	/// <summary>PopUpContent's constructor.</summary>
	/// <param name="_alertMode">Pop-Up's Alert Mode.</param>
	/// <param name="_title">Pop-Up's title.</param>
	/// <param name="_message">Pop-Up's message.</param>
	/// <param name="_cancelButtonLabel">Cancel button's label.</param>
	/// <param name="_confirmButtonLabel">Confirm Button's label.</param>
	/// <param name="onCancel">Cancel button's action.</param>
	/// <param name="onConfirm">Confirm button's action.</param>
	public PopUpContent(AlertModes _alertMode, string _title, string _message, string _cancelButtonLabel, string _confirmButtonLabel, UnityAction onCancel, UnityAction onConfirm)
	{
		alertMode = _alertMode;
		title = _title;
		message = _message;
		cancelButtonLabel = _cancelButtonLabel;
		confirmButtonLabel = _confirmButtonLabel;
		OnCancelEvent = null;
		OnConfirmEvent = null;
		OnCancel = onCancel;
		OnConfirm = onConfirm;
	}

	/// <summary>PopUpContent's constructor.</summary>
	/// <param name="_alertMode">Pop-Up's Alert Mode.</param>
	/// <param name="_title">Pop-Up's title.</param>
	/// <param name="_message">Pop-Up's message.</param>
	/// <param name="onCancel">Cancel button's action.</param>
	/// <param name="onConfirm">Confirm button's action.</param>
	public PopUpContent(AlertModes _alertMode, string _title, string _message, UnityAction onCancel, UnityAction onConfirm)
	{
		alertMode = _alertMode;
		title = _title;
		message = _message;
		cancelButtonLabel = string.Empty;
		confirmButtonLabel = string.Empty;
		OnCancelEvent = null;
		OnConfirmEvent = null;
		OnCancel = onCancel;
		OnConfirm = onConfirm;
	}
}
}