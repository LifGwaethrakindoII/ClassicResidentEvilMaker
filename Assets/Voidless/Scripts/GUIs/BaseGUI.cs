using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Voidless
{
public abstract class BaseGUI<T> : MonoBehaviour
{
	private T _data; 						/// <summary>Data that the BaseGUI is concerned with.</summary>
	private RectTransform _rectTransform; 	/// <summary>RectTransform Component.</summary>

	/// <summary>Gets rectTransform property.</summary>
	public RectTransform rectTransform
	{
		get
		{
			if(_rectTransform == null)
			{
				_rectTransform = GetComponent<RectTransform>();
			}

			return _rectTransform;
		}
	}

	/// <summary>Gests and Sets Data property.</summary>
	public T Data
	{
		get { return _data; }
		set
		{
			_data = value;
			OnDataAssigned();
		}
	}

	/// <summary>Method invoked when GUI's Data is assigned.</summary>
	protected virtual void OnDataAssigned(){}

	/// <summary>Updates the GUI's Feedback to the User.</summary>
	/// <param name="_updatedData">Data that the GUI will recieve.</param>
	public abstract void UpdateGUI(T _updatedData);

	/// <summary>Shows (Activates) GUI.</summary>
	/// <param name="_show">Will the GUI be shown?.</param>
	public virtual void ShowGUI(bool _show)
	{
		gameObject.SetActive(_show);
	}
}

}
