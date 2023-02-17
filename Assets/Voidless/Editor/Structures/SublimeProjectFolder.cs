using UnityEngine;
using System;
using System.Text;

namespace Voidless
{
[Serializable]
public struct SublimeProjectFolder
{
	public string path; 					/// <summary>Folder's Path.</summary>
	public string[] file_exclude_patterns; 	/// <summary>File Exclude Patterns.</summary>

	/// <summary>SublimeProjectFolder's Constructor.</summary>
	/// <param name="_path">Path [must include Assets/...].</param>
	public SublimeProjectFolder(string _path)
	{
		path = _path;
		file_exclude_patterns = new string[] { "*.dll", "*.meta" };
	}
}
}