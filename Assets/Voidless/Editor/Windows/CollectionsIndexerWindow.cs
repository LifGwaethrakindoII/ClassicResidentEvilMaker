using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Object = UnityEngine.Object;

namespace Voidless
{
public class CollectionsIndexerWindow : EditorWindow
{
	protected const string COLLECTIONSINDEXERWINDOW_PATH = "Voidless Tools/ Collections' Indexer Window"; 	/// <summary>CollectionsIndexerWindow's path.</summary>

	public static CollectionsIndexerWindow collectionsIndexerWindow; 										/// <summary>CollectionsIndexerWindow's static reference</summary>
	public static Vector2 scrollPosition;

	/// <summary>Creates a new CollectionsIndexerWindow window.</summary>
	/// <returns>Created CollectionsIndexerWindow window.</summary>
	[MenuItem(COLLECTIONSINDEXERWINDOW_PATH)]
	public static CollectionsIndexerWindow CreateCollectionsIndexerWindow()
	{
		collectionsIndexerWindow = GetWindow<CollectionsIndexerWindow>("Collections' Indexer");
		
		scrollPosition = Vector2.zero;
		collectionsIndexerWindow.Load();

		return collectionsIndexerWindow;
	}

	/// <summary>Use OnGUI to draw all the controls of your window.</summary>
	private void OnGUI()
	{
		scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

		EditorGUILayout.Space();
		DrawObjectsField();
		if(GUILayout.Button("Save")) Save();

		EditorGUILayout.EndScrollView();
	}

	/// <summary>Draws Objects' Fields.</summary>
	private void DrawObjectsField()
	{
		GUILayout.Label("Persistent Objects: ");
		VEditorGUILayout.ShowListSizeConfiguration(ref CollectionsIndexer.objects);

		for(int i = 0; i < CollectionsIndexer.objects.Count; i++)
		{
			int collectionIndex = 0;
			int itemIndex = 0;
			IEnumerator<VTuple<string, IEnumerable<Object>>> iterator = CollectionsIndexer.GetEnumerator(i);

			CollectionsIndexer.objects[i] = EditorGUILayout.ObjectField("Object " + i + ": ", CollectionsIndexer.objects[i], typeof(Object), false) as Object;
			
			while(iterator.MoveNext())
			{
				VTuple<string, IEnumerable<Object>> tuple = iterator.Current;
				itemIndex = 0;
				
				GUILayout.Label("Collection " + collectionIndex + " - " + tuple.Item1.ToInspectorFormat() + ": ");

				EditorGUILayout.Space();

				foreach(Object obj in tuple.Item2)
				{
					if(obj != null)
					{
						EditorGUILayout.BeginHorizontal();
						GUILayout.Label("Element " + itemIndex + ": ");
						GUILayout.Label(obj.name);
						EditorGUILayout.EndHorizontal();
					}

					itemIndex++;
				}

				collectionIndex++;
				EditorGUILayout.Space();
			}
		}

		CollectionsIndexer.Update();

		EditorGUILayout.Space();
	}

	/// <summary>Saves Data into VoidlessEditor's Data.</summary>
	private void Save()
	{
		int index = 0;

		DeletePreviousData();

		for(int i = 0; i < CollectionsIndexer.objects.Count; i++)
		{
			Object obj = CollectionsIndexer.objects[i];

			if(obj != null)
			{
				VEditorData.SaveString(CollectionsIndexer.PATH_OBJECT + index, AssetDatabase.GetAssetPath(obj));
				index++;
			}
		}

		VEditorData.SaveInt(CollectionsIndexer.PATH_COUNT_OBJECTS, index);
	}

	/// <summary>Loads Assets from VoidlessEditor's Data.</summary>
	private void Load()
	{
		//CollectionsIndexer.Initialize();
		CollectionsIndexer.Load();
	}

	/// <summary>Cleans Previous VoidlessEditor's Data.</summary>
	private void DeletePreviousData()
	{
		int count = VEditorData.LoadInt(CollectionsIndexer.PATH_COUNT_OBJECTS);

		for(int i = 0; i < count; i++)
		{
			VEditorData.DeleteString(CollectionsIndexer.PATH_OBJECT + i);
		}
	}
}
}