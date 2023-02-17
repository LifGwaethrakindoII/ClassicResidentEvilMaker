using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[CreateAssetMenu(menuName = VString.PATH_SCRIPTABLE_OBJECTS + " / Text File Asset")]
public class TextFile : ScriptableObject
{
	[SerializeField] private TextAsset _textAsset; 	/// <summary>TextAsset's file.</summary>
	private string _textAssetPath; 					/// <summary>TextAsset's Path.</summary>	

	/// <summary>Gets and Sets textAsset property.</summary>
	public TextAsset textAsset
	{
		get { return _textAsset; }
		set { _textAsset = value; }
	}

	/// <summary>Gets and Sets textAssetPath property.</summary>
	public string textAssetPath
	{
		get { return _textAssetPath; }
		set { _textAssetPath = value; }
	}

	/// <summary>Sets TextAsset.</summary>
	/// <param name="_textAsset">New TextAsset to assign.</param>
	/// <param name="_path">Optional path [null by default].</param>
	public void SetTextAsset(TextAsset _textAsset, string _path = null)
	{
		textAsset = _textAsset;

		if(string.IsNullOrEmpty(_path))
		{
#if UNITY_EDITOR
			textAssetPath = UnityEditor.AssetDatabase.GetAssetPath(textAsset);
#endif
		}
		else textAssetPath = _path;
	}
}
}