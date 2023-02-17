using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
public class VEditorDataWindow : EditorWindow
{
	protected const string VOIDLESSEDITORDATAWINDOW_PATH = "Voidless Tools/Editor's Data"; 	/// <summary>VEditorDataWindow's path.</summary>

	public static VEditorDataWindow voidlessEditorDataWindow; 								/// <summary>VEditorDataWindow's static reference</summary>
	private static SerializedProperty editorDictionary;

	/// <summary>Creates a new VEditorDataWindow window.</summary>
	/// <returns>Created VEditorDataWindow window.</summary>
	[MenuItem(VOIDLESSEDITORDATAWINDOW_PATH)]
	public static VEditorDataWindow CreateVEditorDataWindow()
	{
		voidlessEditorDataWindow = GetWindow<VEditorDataWindow>("Editor's Data");
		return voidlessEditorDataWindow;
	}

	/// <summary>Use OnGUI to draw all the controls of your window.</summary>
	private void OnGUI()
	{
		VEditorData.ShowDictionary();
	}
}
}