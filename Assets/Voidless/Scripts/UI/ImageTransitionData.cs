using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Voidless
{
[CreateAssetMenu(menuName = VString.PATH_SCRIPTABLE_OBJECTS + " / Image Transition's Data")]
public class ImageTransitionData : ScriptableObject
{

#region Properties:
	[SerializeField] private Sprite _sprite; 								/// <summary>Data's Sprite.</summary>
	[SerializeField] private Vector2 _minSize; 								/// <summary>Sprite's Minimum Size.</summary>
	[SerializeField] private Vector2 _maxSize; 								/// <summary>Sprite's Maximum Size.</summary>
	[SerializeField] private float _fadeInDuration; 						/// <summary>Fade In's Duration.</summary>
	[SerializeField] private float _fadeOutDuration; 						/// <summary>Fade Out's Duration.</summary>
	[SerializeField] private NormalizedPropertyFunction _fadeInFunction; 	/// <summary>Fade In's Function.</summary>
	[SerializeField] private NormalizedPropertyFunction _fadeOutFunction; 	/// <summary>Fade Out's Function.</summary>
#endregion

#region Getters:
	/// <summary>Gets sprite property.</summary>
	public Sprite sprite { get { return _sprite; } }

	/// <summary>Gets minSize property.</summary>
	public Vector2 minSize { get { return _minSize; } }

	/// <summary>Gets maxSize property.</summary>
	public Vector2 maxSize { get { return _maxSize; } }

	/// <summary>Gets fadeInDuration property.</summary>
	public float fadeInDuration { get { return _fadeInDuration; } }

	/// <summary>Gets fadeOutDuration property.</summary>
	public float fadeOutDuration { get { return _fadeOutDuration; } }

	/// <summary>Gets fadeInFunction property.</summary>
	public NormalizedPropertyFunction fadeInFunction { get { return _fadeInFunction; } }

	/// <summary>Gets fadeOutFunction property.</summary>
	public NormalizedPropertyFunction fadeOutFunction { get { return _fadeOutFunction; } }
#endregion

}
}