using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Voidless
{
public class ScriptableLightMapDataCreatorWindow : EditorWindow
{
	protected const string SCRIPTABLELIGHTMAPDATACREATORWINDOW_PATH = "Voidless / Lightning / Lightmap Data Creator"; 	/// <summary>ScriptableLightMapDataCreatorWindow's path.</summary>
	protected const string NAME_LIGHTMAP_COLOR = "Lightmap-0_comp_light";
	protected const string NAME_LIGHTMAP_DIR = "Lightmap-0_comp_dir";
	protected const string PATH_LASTPATH = "Path_ScriptableLightMapDataCreator";
	protected const string PATH_LASTASSET = "Path_LastAsset";
	protected const string FILEEXTENSION_PNG = ".png";
	protected const string FILEEXTENSION_EXR = ".exr";

	public static ScriptableLightMapDataCreatorWindow scriptableLightMapDataCreatorWindow; 										/// <summary>ScriptableLightMapDataCreatorWindow's static reference</summary>

	private string lightMapPath; 																								/// <summary>File's Path where the Lightmap Data is [supposedly] located.</summary>
	private Texture2D lightmapColor;
	private Texture2D lightmapDir;
	private Texture2D shadowMask;
	public ScriptableLightMapData lightmapData;

	/// <summary>Creates a new ScriptableLightMapDataCreatorWindow window.</summary>
	/// <returns>Created ScriptableLightMapDataCreatorWindow window.</summary>
	[MenuItem(SCRIPTABLELIGHTMAPDATACREATORWINDOW_PATH)]
	public static ScriptableLightMapDataCreatorWindow CreateScriptableLightMapDataCreatorWindow()
	{
		scriptableLightMapDataCreatorWindow = GetWindow<ScriptableLightMapDataCreatorWindow>("Lightmap Data Creator");
		
		scriptableLightMapDataCreatorWindow.lightmapData = AssetDatabase.LoadAssetAtPath(VEditorData.LoadString(PATH_LASTASSET), typeof(ScriptableLightMapData)) as ScriptableLightMapData;
		if(scriptableLightMapDataCreatorWindow.lightmapData != null) scriptableLightMapDataCreatorWindow.LoadData(scriptableLightMapDataCreatorWindow.lightmapData);

		return scriptableLightMapDataCreatorWindow;
	}

	/// <summary>Use OnGUI to draw all the controls of your window.</summary>
	private void OnGUI()
	{
		lightmapColor = EditorGUILayout.ObjectField(lightmapColor, typeof(Texture2D)) as Texture2D;
		lightmapDir = EditorGUILayout.ObjectField(lightmapDir, typeof(Texture2D)) as Texture2D;
		shadowMask = EditorGUILayout.ObjectField(shadowMask, typeof(Texture2D)) as Texture2D;
		
		lightmapData = EditorGUILayout.ObjectField(lightmapData, typeof(ScriptableLightMapData)) as ScriptableLightMapData;

		if(GUILayout.Button("Create New Lightmap's Data Asset"))
		{
			string path = EditorUtility.SaveFilePanelInProject("New LightMap Data", "LightmapData_New", "asset", string.Empty);
			
			if(path != string.Empty)
			{
				ScriptableLightMapData newData = ScriptableObject.CreateInstance<ScriptableLightMapData>();
				//string.Substring(path, path.Length - 5 - 1, path.Length - 5);
				AssetDatabase.CreateAsset(newData, path);
				lightmapData = newData;
				SaveData(newData);
			}
		}
		if(lightmapData != null)
		{
			if(GUILayout.Button("Overwrite Lightmap's Data Asset") && EditorUtility.DisplayDialog("Saving Lightmap Data", "Are you sure you wanna overwrite this Lightmap's Data?", "Yes", "No")) SaveData(lightmapData);
			if(GUILayout.Button("Load Lightmap's Data Asset")) LoadData(lightmapData);
		}

		string p = AssetDatabase.GetAssetPath(lightmapData);
		GUILayout.Label(p);
		GUILayout.Label(VAssetDatabase.GetAssetFolderPath(lightmapData));
	}

	/// <summary>Saves LightmapData.</summary>
	/// <param name="_lightmapData">Lightmap Data to save.</param>
	private void SaveData(ScriptableLightMapData _lightmapData)
	{
		string path = AssetDatabase.GetAssetPath(_lightmapData);
		string folderPath = VAssetDatabase.GetAssetFolderPath(_lightmapData);
		string lightmapDataName = _lightmapData.name;

		if(lightmapColor != null) lightmapColor.MarkTextureAsReadable();

		_lightmapData.lightmapColor = lightmapColor.Duplicated();
		_lightmapData.lightmapDir = lightmapDir.Duplicated();
		_lightmapData.shadowMask = shadowMask.Duplicated();

		Directory.CreateDirectory(folderPath + "/" + lightmapDataName);

		if(lightmapColor != null) ApplyTexture(_lightmapData.lightmapColor, folderPath, lightmapDataName, NAME_LIGHTMAP_COLOR, FILEEXTENSION_EXR);
		if(lightmapDir != null) ApplyTexture(_lightmapData.lightmapDir, folderPath, lightmapDataName, NAME_LIGHTMAP_DIR, FILEEXTENSION_PNG);

		EditorUtility.SetDirty(_lightmapData);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        VEditorData.SaveString(PATH_LASTASSET, path);
	}


	private void ApplyTexture(Texture2D _texture, string _folderPath, string _lightmapDataName, string _textureName, string _extension)
	{
		switch(_extension)
		{
			case FILEEXTENSION_EXR:
			_texture.EncodeToEXR(Texture2D.EXRFlags.CompressZIP);
			break;

			case FILEEXTENSION_PNG:
			_texture.EncodeToPNG();
			break;
		}

		if(AssetDatabase.GetAssetPath(_texture) == string.Empty)
		{
			string texturePath = _folderPath + "/" + _lightmapDataName + "/" + _textureName + _extension;
			AssetDatabase.CreateAsset(_texture, texturePath);
		}

		_texture.MarkTextureAsReadable();
	}

	/// <summary>Loads Data.</summary>
	/// <param name="_lightmapData">Lightmap Data to load.</param>
	private void LoadData(ScriptableLightMapData _lightmapData)
	{
		lightmapColor = _lightmapData.lightmapColor;
		lightmapDir = _lightmapData.lightmapDir;
		shadowMask = _lightmapData.shadowMask;
	}

/*#region DEPRECATED:
	private void DrawDataPathFields()
	{
		lightMapPath = GUILayout.TextField(lightMapPath);
		if(GUILayout.Button("Get Path"))
		{
			string path = EditorUtility.OpenFolderPanel("LightMap Data's Folder", VEditorData.LoadString(PATH_LASTPATH, Application.dataPath), "");
			
			if(path != string.Empty)
			{
				VEditorData.SaveString(PATH_LASTPATH, path);
				lightMapPath = path;
			}
		}
	}

	private void DrawDataLoading()
	{
		if(GUILayout.Button("Load Lightmap's Data"))
		{
			string[] files = Directory.GetFiles(lightMapPath);

			foreach(string file in files)
			{
				Debug.Log("[ScriptableLightMapDataCreatorWindow] File: " + file);
			}

			lightmapColor = AssetDatabase.LoadAssetAtPath(lightMapPath + "/" + NAME_LIGHTMAP_COLOR + ".png", typeof(Texture2D)) as Texture2D;
		}
	}
#endregion*/
}
}