using UnityEngine;
using UnityEngine.UI;

namespace Voidless
{
public static class VRectTransform
{
	/// <summary>Parents RectTransform with another RectTransform.</summary>
	/// <param name="_rectTransform">RectTransform to parent.</param>
	/// <param name="_newParent">New RectTransform parent.</param>
	public static void ParentWith(this RectTransform _rectTransform, RectTransform _newParent)
	{
		_rectTransform.parent = _newParent;
		_rectTransform.localScale = Vector3.one;
	}

	/// <summary>Parents RectTransform with another RectTransform.</summary>
	/// <param name="_rectTransform">RectTransform to parent.</param>
	/// <param name="_newParent">New Transform parent.</param>
	public static void ParentWith(this RectTransform _rectTransform, Transform _newParent)
	{
		_rectTransform.parent = _newParent;
		_rectTransform.localScale = Vector3.one;
	}
}
}