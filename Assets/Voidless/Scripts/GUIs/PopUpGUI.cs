using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Voidless
{
public enum AlertModes 									///< <summary>Pop-Up's Alert Modes.</summary>
{
	None, 												///< <summary>Unassigned Alert Mode.</summary>
	Warning, 											///< <summary>Warning Alert Mode.</summary>
	Error 												///< <summary>Error Alert Mode.</summary>
}

public class PopUpGUI : BaseGUI<PopUpContent>
{
	[Header("Visual Feedback:")]
	[SerializeField] private Image _alertImage; 		/// <summary>Pop-Up's Alert Image.</summary>
	[SerializeField] private Sprite _warningIcon; 		/// <summary>Pop-Up's [optional] warning alert icon.</summary>
	[SerializeField] private Sprite _errorIcon; 		/// <summary>Pop-Up's [optional] error alert icon.</summary>
	[Header("Labels:")]
	[SerializeField] private Text _title; 				/// <summary>Pop-Up's title.</summary>
	[SerializeField] private Text _message; 			/// <summary>Pop-Up's message.</summary>
	[Space(5f)]
	[Header("Buttons:")]
	[SerializeField] private Button _cancelButton; 		/// <summary>Pop-Up's cancel button.</summary>
	[SerializeField] private Button _confirmButton; 	/// <summary>Pop-Up's confirm button.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets alertImage property.</summary>
	public Image alertImage { get { return _alertImage; } }

	/// <summary>Gets and Sets warningIcon property.</summary>
	public Sprite warningIcon
	{
		get { return _warningIcon; }
		set { _warningIcon = value; }
	}

	/// <summary>Gets and Sets errorIcon property.</summary>
	public Sprite errorIcon
	{
		get { return _errorIcon; }
		set { _errorIcon = value; }
	}

	/// <summary>Gets and Sets title property.</summary>
	public Text title
	{
		get { return _title; }
		set { _title = value; }
	}

	/// <summary>Gets and Sets message property.</summary>
	public Text message
	{
		get { return _message; }
		set { _message = value; }
	}

	/// <summary>Gets and Sets cancelButton property.</summary>
	public Button cancelButton
	{
		get { return _cancelButton; }
		set { _cancelButton = value; }
	}

	/// <summary>Gets and Sets confirmButton property.</summary>
	public Button confirmButton
	{
		get { return _confirmButton; }
		set { _confirmButton = value; }
	}
#endregion

	/// <summary>Updates the GUI's Feedback to the User.</summary>
	/// <param name="_content">Pop-Up's GUI content.</param>
	public override void UpdateGUI(PopUpContent _content)
	{
		if(alertImage != null)
		switch(_content.alertMode)
		{
			case AlertModes.None:
			alertImage.sprite = null;
			break;

			case AlertModes.Warning:
			alertImage.sprite = warningIcon;
			break;

			case AlertModes.Error:
			alertImage.sprite = errorIcon;
			break;
		}

		title.text = _content.title;
		message.text = _content.message;
		/// Cancel Button calls assignment:
		cancelButton.onClick.RemoveAllListeners();
		cancelButton.onClick.AddListener(_content.OnCancel);
		if(_content.OnCancelEvent != null) cancelButton.onClick.AddListener(delegate { _content.OnCancelEvent.Invoke(); });
		cancelButton.gameObject.SetActive(_content.OnCancel != null || _content.OnCancelEvent != null);
		/// Confirm Button calls assingment:
		confirmButton.onClick.RemoveAllListeners();
		confirmButton.onClick.AddListener(_content.OnConfirm);
		if(_content.OnConfirmEvent != null) confirmButton.onClick.AddListener(delegate { _content.OnConfirmEvent.Invoke(); });
		confirmButton.gameObject.SetActive(_content.OnConfirm != null || _content.OnConfirmEvent != null);

		/// \TODO Make an Extendable Button class [with custom PointerData]
		if(cancelButton.transform.childCount > 0) cancelButton.transform.GetChild(0).GetComponent<Text>().text = _content.cancelButtonLabel;
		if(confirmButton.transform.childCount > 0) confirmButton.transform.GetChild(0).GetComponent<Text>().text = _content.confirmButtonLabel;
	}

	/// <summary>Shows Pop-Up and Updates it.</summary>
	/// <param name="_content">Pop-Up's GUI content.</param>
	public void UpdateAndShowPopUpGUI(PopUpContent _content)
	{
		ShowGUI(true);
		UpdateGUI(_content);
	}
}
}