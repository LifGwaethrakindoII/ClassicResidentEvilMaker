using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
public enum EssentialFolders 																	/// <summary>Enumerator listing the considered essential Folders.</summary>
{
	Animations, 																				/// <summary>Animations' Folder.</summary>
	Animator_Controllers, 																		/// <summary>Animator Controller's Folder.</summary>
	Audios, 																					/// <summary>Audios' Folder.</summary>
	Editor, 																					/// <summary>Editor's Folder.</summary>
	Editor_Default_Resources, 																	/// <summary>Editor Default Resources' Folder.</summary>
	Images, 																					/// <summary>Images' Folder.</summary>
	Gizmos, 																					/// <summary>Gizmos' Folder.</summary>
	Materials, 																					/// <summary>Materials' Folder.</summary>
	Models, 																					/// <summary>Models' Folder.</summary>
	Music, 																						/// <summary>Music's Folder.</summary>
	Plugins, 																					/// <summary>Plugins' Folder.</summary>
	Resources, 																					/// <summary>Resources' Folder.</summary>
	Scenes, 																					/// <summary>Scenes' Folder.</summary>
	Scripts, 																					/// <summary>Scripts' Folder.</summary>
	Shaders, 																					/// <summary>Shaders' Folder.</summary>
	Sounds, 																					/// <summary>Sounds' Folder.</summary>
	StreamingAssets, 																			/// <summary>StreamingAssets' Folder.</summary>
	Textures 																					/// <summary>Textures' Folder.</summary>
}

public class FolderCreationWindow : EditorWindow
{
	private const string FOLDERCREATIONWINDOW_PATH = "Voidless Tools/Folder Creation Menu"; 	/// <summary>FolderCreationWindow's path.</summary>
	private const string ICON_FOLDER = "Folder Icon"; 											/// <summary>Folder's Icon.</summary>
	private const float SPACE = 20.0f; 															/// <summary>Default Space.</summary>
	private const float WIDTH_BUTTON = 50.0f; 													/// <summary>Button's Width.</summary>
	private const int MAX_STRING_SIZE = 24; 													/// <summary>Maximum's size of string.</summary>

	public static FolderCreationWindow folderCreationWindow; 									/// <summary>FolderCreationWindow's static reference</summary>
	private static List<string> unexistingFolders; 												/// <summary>Folders not yet created.</summary>
	private static List<string> existingFolders; 												/// <summary>Folders already created.</summary>
	private static Vector3 scrollPosition; 														/// <summary>Scroll's Position.</summary>
	private static string dataPath; 															/// <summary>Data Path (Assets).</summary>

	/// <summary>Creates a new FolderCreationWindow window.</summary>
	/// <returns>Created FolderCreationWindow window.</summary>
	[MenuItem(FOLDERCREATIONWINDOW_PATH)]
	public static FolderCreationWindow CreateFolderCreationWindow()
	{
		unexistingFolders = null;
		existingFolders = null;
		dataPath = Application.dataPath + "/";
		folderCreationWindow = GetWindow<FolderCreationWindow>("Folder Creation:");
		EvaluateDirectories();
		return folderCreationWindow;
	}

	private void OnGUI()
	{
		scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
		GUILayout.Space(SPACE);
		ShowExistingFolders();
		ShowUnexistingFolders();
		EditorGUILayout.EndScrollView();
	}

	/// <summary>Shows a List of the already-existing folders.</summary>
	private void ShowExistingFolders()
	{
		GUILayout.Label("Existing Folders");
		GUILayout.Space(SPACE);
		foreach(string folder in existingFolders)
		{
			GUILayout.Label(folder.SnakeCaseToSpacedText());
		}
		GUILayout.Space(SPACE);
	}

	/// <summary>Shows the essential folders that haven't yet been created.</summary>
	private void ShowUnexistingFolders()
	{
		GUILayout.Label("Non-Existing Folders");
		GUILayout.Space(SPACE);
		foreach(string folder in unexistingFolders)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(folder.SnakeCaseToSpacedText());
			if(GUILayout.Button("Create",  GUILayout.Width(WIDTH_BUTTON)))
			{
				CreateFolder(folder);
				break;
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.Space(SPACE);
	}

	/// <summary>Created a Folder.</summary>
	/// <param name="_folder">Folder's Name.</param>
	private static void CreateFolder(string _folder)
	{
		Directory.CreateDirectory(dataPath + _folder.SnakeCaseToSpacedText());
		unexistingFolders.Remove(_folder);
		existingFolders.Add(_folder);
		AssetDatabase.Refresh();
	}

	/// <summary>Evaluates all directories to update the existing and non-existing lists.</summary>
	private static void EvaluateDirectories()
	{
		int enumCount = Enum.GetNames(typeof(EssentialFolders)).Length;

		if(unexistingFolders == null || existingFolders == null)
		{
			unexistingFolders = new List<string>();
			existingFolders = new List<string>();

			for(int i = 0; i < enumCount; i++)
			{
				EssentialFolders folder = (EssentialFolders)(i);
				string currentFolder = folder.ToString();
				string spacedFolder = currentFolder.SnakeCaseToSpacedText();

				if(!Directory.Exists(dataPath + spacedFolder)) unexistingFolders.Add(spacedFolder);
				else existingFolders.Add(spacedFolder);
			}
		}
		else
		{
			foreach(string folder in unexistingFolders)
			{
				if(Directory.Exists(dataPath + folder))
				{
					unexistingFolders.Remove(folder);
					existingFolders.Add(folder);
					break;
				}
			}
		}
	}
}
}