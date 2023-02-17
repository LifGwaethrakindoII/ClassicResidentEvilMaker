using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefabUtilitysWindow : EditorWindow
{
	protected const string PREFABUTILITYSWINDOW_PATH = "Voidless Tools/Windows/ PrefabUtilitysWindow's"; 	/// <summary>PrefabUtilitysWindow's path.</summary>

	public static PrefabUtilitysWindow prefabUtilitysWindow;                                        /// <summary>PrefabUtilitysWindow's static reference</summary>


	[SerializeField] private GameObject _prefab;                                        /// <summary>Prefab to substitute the selected game objects</summary>
	private Vector2 _scrollPos;                                                         /// <summary>Position of the scroll view of selected gameObjects</summary>
	

	#region Geters/ Seters
	/// <summary>Gets and Sets prefab property.</summary>
	public GameObject prefab
	{
		get { return _prefab; }
		set { _prefab = value; }
	}

	#endregion

	/// <summary>Creates a new PrefabUtilitysWindow window.</summary>
	/// <returns>Created PrefabUtilitysWindow window.</summary>
	[MenuItem(PREFABUTILITYSWINDOW_PATH)]
	public static PrefabUtilitysWindow CreatePrefabUtilitysWindow()
	{
		prefabUtilitysWindow = GetWindow<PrefabUtilitysWindow>("Prefab's Utility");
		
		return prefabUtilitysWindow;
	}

	/// <summary>Use OnGUI to draw all the controls of your window.</summary>
	private void OnGUI()
	{
		//Debug.Log("tan 90: " + Mathf.Tan(90*Mathf.Deg2Rad));
		CreateLabel("Prefab Reference");
		EditorGUILayout.BeginHorizontal();

		prefab = EditorGUILayout.ObjectField(prefab,typeof(GameObject),false) as GameObject;

		if(prefab != null && Selection.gameObjects.Length >0)
        {
			if(GUILayout.Button("Link Objets to prefabs"))
            {
				
				if( EditorUtility.DisplayDialog("Warning","You are about to link: " + Selection.gameObjects.Length + " game objects with " + prefab.name + 
					". Are you sure you want to proceed ",
					"ok", 
					"cancel" ))
                {

					SubstituteGameObjectsForPrefabs();
					
                }
            }
        }
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();

		CreateLabel("Selected Game Objects");
		_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
		ShowSeletedGameObjects();
		
		EditorGUILayout.EndScrollView();


		
	}

	/// <summary>Creates a label with bold text.</summary>
	void CreateLabel(string _text)
	{
		GUILayout.Label(_text, EditorStyles.boldLabel);
		
	}
	/// <summary>Creates a label with regular tex.</summary>
	void CreateContentLabel(string _text)
	{
		GUILayout.Label(_text);
	}
	/// <summary>Show the current selected objects.</summary>
	void ShowSeletedGameObjects()
    {
		
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
			
			CreateContentLabel(Selection.gameObjects[i].name);
			
        }
		
    }

	/// <summary>Swaps the objects selected for the prefab assinged.</summary>
	/// /// /// <param name="i">Limit to the cycle.</param>
	/// /// <param name="initialObjectsAmount">Count of the amount of objects to change.</param>
	/// /// <param name="progressAmout">the count of times the cycle have repeat itself to show the progress bar.</param>
	void SubstituteGameObjectsForPrefabs()
    {

		int i = 0;
		int initialObjectsAmount = Selection.gameObjects.Length;
		float progressAmount =0;

		while (Selection.gameObjects.Length != i)
        {
			progressAmount += 1;
			EditorUtility.DisplayProgressBar("Prefabs Subtitution", "Working on it...", progressAmount/ initialObjectsAmount);
			Vector3 recycledPosition = Selection.gameObjects[i].transform.position;
			Quaternion recycledRotation = Selection.gameObjects[i].transform.rotation;

			GameObject obj = PrefabUtility.ConnectGameObjectToPrefab(Selection.gameObjects[i], prefab);
			obj.transform.position = recycledPosition;
			obj.transform.rotation = recycledRotation;
			
			
		}
		EditorUtility.ClearProgressBar();
	}

	
}