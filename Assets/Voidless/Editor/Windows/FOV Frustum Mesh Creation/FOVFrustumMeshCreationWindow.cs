using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
public class FOVFrustumMeshCreationWindow : EditorWindow
{
	private const string EDITOR_DATA_KEY_CONFIGURATIONS = "Path_FOVFrustumMeshCreation_Configurations"; 
	private const int COUNT_VERTICES = 8;
	private const int COUNT_TRIANGLES = 36;
	private const int INDEX_TOP_LEFT_NEAR = 0;
	private const int INDEX_TOP_RIGHT_NEAR = 1;
	private const int INDEX_BOTTOM_RIGHT_NEAR = 2;
	private const int INDEX_BOTTOM_LEFT_NEAR = 3;
	private const int INDEX_TOP_LEFT_FAR = 4;
	private const int INDEX_TOP_RIGHT_FAR = 5;
	private const int INDEX_BOTTOM_RIGHT_FAR = 6;
	private const int INDEX_BOTTOM_LEFT_FAR = 7;

	protected const string FRUSTUMMESHCREATIONWINDOW_PATH = "/FOV's Frustum Mesh Creation Tool"; 	/// <summary>FOVFrustumMeshCreationWindow's path.</summary>

	public static FOVFrustumMeshCreationWindow frustumMeshCreationWindow; 							/// <summary>FOVFrustumMeshCreationWindow's static reference</summary>
	private static FOVFrustumMeshCreationWindowConfigurations configurations; 						/// <summary>Window's Configurations.</summary>
	private static Vector3[] vertices; 																/// <summary>Mesh's Vertices.</summary>
	private static int[] triangles; 																/// <summary>Mesh's Triangles.</summary>
	private static Vector2 verticalScrollPosition; 													/// <summary>Vertical Scroll's Position.</summary>

	private void OnEnable()
	{
		SceneView.onSceneGUIDelegate += OnSceneGUI;
	}

	private void OnDisable()
	{
		SceneView.onSceneGUIDelegate -= OnSceneGUI;
	}

	/// <summary>Creates a new FOVFrustumMeshCreationWindow window.</summary>
	/// <returns>Created FOVFrustumMeshCreationWindow window.</summary>
	[MenuItem(VString.PATH_ROOT_VOIDLESS_TOOLS + FRUSTUMMESHCREATIONWINDOW_PATH)]
	public static FOVFrustumMeshCreationWindow CreateFOVFrustumMeshCreationWindow()
	{
		frustumMeshCreationWindow = GetWindow<FOVFrustumMeshCreationWindow>("Frustum Mesh Creation");
		vertices = new Vector3[COUNT_VERTICES];
		triangles = new int[COUNT_TRIANGLES];
		verticalScrollPosition = Vector2.zero;
		LoadConfigurations();

		return frustumMeshCreationWindow;
	}

	/// <summary>Use OnGUI to draw all the controls of your window.</summary>
	private void OnGUI()
	{
		verticalScrollPosition = EditorGUILayout.BeginScrollView(verticalScrollPosition);
		DrawFrustumSettings();
		SetVertices();
		DrawEditorConfigurations();
		ShowSerializationButtons();
		EditorGUILayout.EndScrollView();
	}

	/// <summary>Draws Frustum's Settings.</summary>
	private void DrawFrustumSettings()
	{
		EditorGUILayout.Space();
		GUILayout.Label("Field of View's Data:");
		VEditorGUILayout.Spaces(2);
		FOVData data = configurations.FOVData;

		data.angle = EditorGUILayout.FloatField("Angle: ", data.angle);
		data.nearPlane = EditorGUILayout.FloatField("Near Plane: ", data.nearPlane);
		data.farPlane = EditorGUILayout.FloatField("Far Plane: ", data.farPlane);
		data.height = EditorGUILayout.FloatField("Height: ", data.height);
		data.aspect = EditorGUILayout.FloatField("Aspect: ", data.aspect);
		//data.width = EditorGUILayout.FloatField("Width: ", data.width);
		data.width = data.height * data.aspect;
		data.planeDelta = (data.farPlane - data.nearPlane);
		data.inverseZ = (1.0f / data.planeDelta);
		
		configurations.FOVData = data;
		EditorGUILayout.Space();
	}

	/// <summary>Draws Editor's Configurations.</summary>
	private void DrawEditorConfigurations()
	{
		EditorGUILayout.Space();
		GUILayout.Label("Editor's Configurations:");
		VEditorGUILayout.Spaces(2);
		configurations.color = EditorGUILayout.ColorField("Color: ", configurations.color);
		configurations.name = EditorGUILayout.TextField("Mesh's Name: ", configurations.name);
		EditorGUILayout.Space();
	}

	/// <summary>Sets Mesh's Vertices.</summary>
	private void SetVertices()
	{
		float ratio = Mathf.Tan(configurations.FOVData.angle * Mathf.Deg2Rad * 0.5f);
		float nearY = (ratio * configurations.FOVData.nearPlane * configurations.FOVData.height);
		float farY = (ratio * configurations.FOVData.farPlane * configurations.FOVData.height);
		float nearX = /*(ratio * configurations.FOVData.nearPlane * configurations.FOVData.width)*/nearY * configurations.FOVData.aspect;
		float farX = /*(ratio * configurations.FOVData.farPlane * configurations.FOVData.width)*/ farY * configurations.FOVData.aspect;

		vertices[INDEX_TOP_LEFT_NEAR] = new Vector3(-nearX, nearY, configurations.FOVData.nearPlane);
		vertices[INDEX_TOP_RIGHT_NEAR] = new Vector3(nearX, nearY, configurations.FOVData.nearPlane);
		vertices[INDEX_BOTTOM_RIGHT_NEAR] = new Vector3(nearX, -nearY, configurations.FOVData.nearPlane);
		vertices[INDEX_BOTTOM_LEFT_NEAR] = new Vector3(-nearX, -nearY, configurations.FOVData.nearPlane);
		vertices[INDEX_TOP_LEFT_FAR] = new Vector3(-farX, farY, configurations.FOVData.farPlane);
		vertices[INDEX_TOP_RIGHT_FAR] = new Vector3(farX, farY, configurations.FOVData.farPlane);
		vertices[INDEX_BOTTOM_RIGHT_FAR] = new Vector3(farX, -farY, configurations.FOVData.farPlane);
		vertices[INDEX_BOTTOM_LEFT_FAR] = new Vector3(-farX, -farY, configurations.FOVData.farPlane);
	}

	/// <summary>Sets Mesh's Triangles.</summary>
	private void SetTriangles()
	{
		triangles[0] = 0;
		triangles[1] = 1;
		triangles[2] = 3;
		triangles[3] = 1;
		triangles[4] = 2;
		triangles[5] = 3;
		triangles[6] = 4;
		triangles[7] = 0;
		triangles[8] = 7;
		triangles[9] = 0;
		triangles[10] = 3;
		triangles[11] = 7;
		triangles[12] = 1;
		triangles[13] = 5;
		triangles[14] = 6;
		triangles[15] = 1;
		triangles[16] = 6;
		triangles[17] = 2;
		triangles[18] = 3;
		triangles[19] = 2;
		triangles[20] = 7;
		triangles[21] = 2;
		triangles[22] = 6;
		triangles[23] = 7;
		triangles[24] = 4;
		triangles[25] = 1;
		triangles[26] = 0;
		triangles[27] = 4;
		triangles[28] = 5;
		triangles[29] = 1;
		triangles[30] = 7;
		triangles[31] = 5;
		triangles[32] = 4;
		triangles[33] = 7;
		triangles[34] = 6;
		triangles[35] = 5;
	}

	/// <summary>Shows Buttons.</summary>
	private void ShowSerializationButtons()
	{
		EditorGUILayout.Space();
		if(GUILayout.Button("Save")) Save();
	}

	/// <summary>Saves Configurations' Data and Creates Frustum's Mesh.</summary>
	private void Save()
	{
		string json = JsonUtility.ToJson(configurations);
		VEditorData.SaveString(EDITOR_DATA_KEY_CONFIGURATIONS, json);
		CreateFOVFrustumColliderData();
	}

	/// <summary>Creates Frustum's Mesh Scriptale Object.</summary>
	private void CreateFOVFrustumColliderData()
	{
		string filePath = EditorUtility.SaveFilePanelInProject("Save FOV's Frustum Mesh", configurations.name, "asset", string.Empty);

		if(!string.IsNullOrEmpty(filePath))
		{
			FOVFrustumColliderData loadedData = AssetDatabase.LoadAssetAtPath(filePath, typeof(FOVFrustumColliderData)) as FOVFrustumColliderData;
			SetTriangles();

			if(loadedData != null)
			{
				if(EditorUtility.DisplayDialog
				(
					"Warning, you are about to overwrite previous Data",
					"Path already contains a FOV Frustum's Data, are you sure you want to overwrite the file?",
					"Yes",
					"No"
				))
				{
					loadedData.FOVData = configurations.FOVData;
					if(loadedData.mesh == null) loadedData.mesh = CreateMesh();
					loadedData.name = configurations.name;
					loadedData.mesh.name = "Mesh_" + configurations.name;
					loadedData.mesh.vertices = vertices;
					loadedData.mesh.triangles = triangles;
				}
			}
			else
			{
				Mesh mesh = CreateMesh();

				FOVFrustumColliderData frustumColliderData = FOVFrustumColliderData.Create();
				frustumColliderData.mesh = mesh;
				frustumColliderData.FOVData = configurations.FOVData;

				AssetDatabase.CreateAsset(frustumColliderData, filePath);
				AssetDatabase.AddObjectToAsset(mesh, frustumColliderData);
				EditorUtility.SetDirty(mesh);
				EditorUtility.SetDirty(frustumColliderData);
			}
			
			AssetDatabase.SaveAssets();
		}
	}

	/// <returns>Mesh from Window's Data.</returns>
	private Mesh CreateMesh()
	{
		Mesh mesh = new Mesh();
		mesh.name = "Mesh_" + configurations.name;
		mesh.vertices = vertices;
		mesh.triangles = triangles;

		return mesh;
	}

	private void OnSceneGUI(SceneView _view)
	{
		DrawVertices();
		//DrawTriangles();
	}

	/// <summary>Draws Vertices.</summary>
	private void DrawVertices()
	{
		Handles.color = configurations.color;

		Handles.DrawLine(vertices[0], vertices[1]);
		Handles.DrawLine(vertices[1], vertices[2]);
		Handles.DrawLine(vertices[2], vertices[3]);
		Handles.DrawLine(vertices[3], vertices[0]);

		Handles.DrawLine(vertices[4], vertices[5]);
		Handles.DrawLine(vertices[5], vertices[6]);
		Handles.DrawLine(vertices[6], vertices[7]);
		Handles.DrawLine(vertices[7], vertices[4]);

		Handles.DrawLine(vertices[4], vertices[0]);
		Handles.DrawLine(vertices[0], vertices[3]);
		Handles.DrawLine(vertices[3], vertices[7]);
		Handles.DrawLine(vertices[7], vertices[4]);

		Handles.DrawLine(vertices[1], vertices[5]);
		Handles.DrawLine(vertices[5], vertices[6]);
		Handles.DrawLine(vertices[6], vertices[2]);
		Handles.DrawLine(vertices[2], vertices[1]);
	}

	/// <summary>Draws Triangles.</summary>
	private void DrawTriangles()
	{
		Handles.DrawLine(vertices[1], vertices[3]);
		Handles.DrawLine(vertices[1], vertices[4]);
		Handles.DrawLine(vertices[2], vertices[7]);
		Handles.DrawLine(vertices[5], vertices[7]);
		Handles.DrawLine(vertices[0], vertices[7]);
		Handles.DrawLine(vertices[1], vertices[6]);
	}

	/// <summary>Loads FOV's Data.</summary>
	/// <param name="_data">Data to load.</param>
	public static void LoadFOVData(FOVFrustumColliderData _data)
	{
		configurations.FOVData = _data.FOVData;
		configurations.name = _data.name;
	}

	/// <summary>Loads Configurations.</summary>
	private static void LoadConfigurations()
	{
		FOVFrustumMeshCreationWindowConfigurations loadedConfigurations
		= JsonUtility.FromJson<FOVFrustumMeshCreationWindowConfigurations>(VEditorData.LoadString(EDITOR_DATA_KEY_CONFIGURATIONS));

		configurations = loadedConfigurations;
	}
}
}