using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
public class InputMappingWindow : EditorWindow
{
	protected const string INPUTMAPPINGWINDOW_PATH = "/Voidless Tools/Settings/Input Mapping"; 																						/// <summary>InputMappingWindow's path.</summary>
	private const string EDITOR_DATA_KEY_MAPPING_PATH = "Path_InputMapping_File"; 																									/// <summary>Input Mapping's File path key for the EditorPrefs.</summary>
	private const string MESSAGE_NO_MAPPING = "You are going to save configurations into a text file that has no previous configurations data. Are you sure you wanna overwrite?"; 	/// <summary>Message when the user is about to save and there was no previous mapping on file.</summary>
	private const string MESSAGE_MAPPING = "You are going to overwrite previous data. Are you sure you want to overwrite the previous content?"; 									/// <summary>Message when the user is about to save and there was previous mapping on file.</summary>
	private const float WIDTH_ID = 50.0f; 																																			/// <summary>ID's Zone Width.</summary>
	private const float WIDTH_BUTTON = 100.0f; 																																		/// <summary>Button's Width.</summary>
	private const float WIDTH_LABEL = 120.0f; 																																		/// <summary>Label's Width.</summary>
	private const float WIDTH_TEXT_FIELD = 200.0f; 																																	/// <summary>Text Field's Width.</summary>
	private const float WIDTH_CONTROLLER_SETUP_CELL = 150.0f; 																														/// <summary>Input Layout's Cell Width.</summary>

	public static InputMappingWindow inputMappingWindow; 																															/// <summary>InputMappingWindow's static reference</summary>
	private static InputMapping inputMapping; 																																		/// <summary>Input's Mapping.</summary>
	private static TextAsset file; 																																					/// <summary>File to save and load the mappings from.</summary>
	private static Vector2 verticalScrollPosition; 																																	/// <summary>Vertical Scroll Bar's position.</summary>
	private static bool XBoxLeftAxisYKeyDefault; 																																	/// <summary>Is the Left Y's Axis set as default?.</summary>
	private static bool XBoxLeftAxisXKeyDefault; 																																	/// <summary>Is the Left X's Axis set as default?.</summary>
	private static bool XBoxRightAxisYKeyDefault; 																																	/// <summary>Is the Right Y's Axis set as default?.</summary>
	private static bool XBoxRightAxisXKeyDefault; 																																	/// <summary>Is the Right X's Axis set as default?.</summary>
	private static bool XBoxLeftTriggerKeyDefault; 																																	/// <summary>Is the Left Trigger's Axis set as default?.</summary>
	private static bool XBoxRightTriggerXKeyDefault; 																																/// <summary>Is the Right Trigger's Axis set as default?.</summary>
	private static bool XBoxDPadAxisYKeyDefault; 																																	/// <summary>Is the D-Pad Y's Axis set as default?.</summary>
	private static bool XBoxDPadAxisXKeyDefault; 																																	/// <summary>Is the D-Pad X's Axis set as default?.</summary>

	/// <summary>Creates a new InputMappingWindow window.</summary>
	/// <returns>Created InputMappingWindow window.</summary>
	[MenuItem(VString.PATH_ROOT_VOIDLESS_TOOLS + INPUTMAPPINGWINDOW_PATH)]
	public static InputMappingWindow CreateInputMappingWindow()
	{
		inputMappingWindow = GetWindow<InputMappingWindow>("Input Mapping");
		LoadInputMapping();
		SetDefaultKeyFlags();
		verticalScrollPosition = Vector2.zero;
		return inputMappingWindow;
	}

	/// <summary>Use OnGUI to draw all the controls of your window.</summary>
	private void OnGUI()
	{
		verticalScrollPosition = EditorGUILayout.BeginScrollView(verticalScrollPosition);
		DrawArray();
		DrawAdditionalSettings();
		ShowTextAssetSettings();
		EditorGUILayout.EndScrollView();
	}

	/// <summary>Draws array of all the controller's setups associated by ID.</summary>
	private void DrawArray()
	{
		EditorGUILayout.Space();
		ShowArraysSizeConfiguration();
		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("ID: ", GUILayout.Width(WIDTH_CONTROLLER_SETUP_CELL));
		GUILayout.Label("Name: ", GUILayout.Width(WIDTH_CONTROLLER_SETUP_CELL));
		GUILayout.Label("PC: ", GUILayout.Width(WIDTH_CONTROLLER_SETUP_CELL));
		GUILayout.Label("XBox: ", GUILayout.Width(WIDTH_CONTROLLER_SETUP_CELL));
#if UNITY_SWITCH
		GUILayout.Label("Nintendo Switch: ", GUILayout.Width(WIDTH_CONTROLLER_SETUP_CELL));
#elif UNITY_N3DS
		GUILayout.Label("N3DS: ", GUILayout.Width(WIDTH_CONTROLLER_SETUP_CELL));
#endif
		EditorGUILayout.EndHorizontal();

		if(inputMapping == null) return;

		for(int i = 0; i < inputMapping.PCControllerSetup.keyMapping.Length; i++)
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(i.ToString() + ": ", GUILayout.Width(WIDTH_ID));
			inputMapping.keyNames[i] = EditorGUILayout.TextField(inputMapping.keyNames[i], GUILayout.Width(WIDTH_TEXT_FIELD));
			inputMapping.PCControllerSetup.keyMapping[i] = (KeyCode)EditorGUILayout.EnumPopup(inputMapping.PCControllerSetup.keyMapping[i], GUILayout.Width(WIDTH_CONTROLLER_SETUP_CELL));
			inputMapping.XBoxControllerSetup.keyMapping[i] = (XBoxInputKey)EditorGUILayout.EnumPopup(inputMapping.XBoxControllerSetup.keyMapping[i], GUILayout.Width(WIDTH_CONTROLLER_SETUP_CELL));
#if UNITY_SWITCH
			inputMapping.NintendoSwitchControllerSetup.keyMapping[i] = (NintendoSwitchButton)EditorGUILayout.EnumPopup(inputMapping.NintendoSwitchControllerSetup.keyMapping[i], GUILayout.Width(WIDTH_CONTROLLER_SETUP_CELL));
#endif
#if UNITY_N3DS
			inputMapping.N3DSControllerSetup.keyMapping[i] = (N3dsButton)EditorGUILayout.EnumPopup(inputMapping.N3DSControllerSetup.keyMapping[i], GUILayout.Width(WIDTH_CONTROLLER_SETUP_CELL));
#endif
			EditorGUILayout.EndHorizontal();
		}
	}

	/// <summary>Draws Additional controller setup's configurations.</summary>
	private void DrawAdditionalSettings()
	{
		if(inputMapping == null) return;

		EditorGUILayout.Space();
		EditorGUILayout.Space();
		GUILayout.Label("Aditional XBox Controller Settings:");
		EditorGUILayout.Space();
		inputMapping.XBoxControllerSetup.leftAxisYKey = ShowAxisKeyConfiguration("Left Axis Y's Key: ", inputMapping.XBoxControllerSetup.leftAxisYKey, XBoxControllerSetup.DEFAULT_AXIS_LEFT_Y, ref XBoxLeftAxisYKeyDefault);
		inputMapping.XBoxControllerSetup.leftAxisXKey = ShowAxisKeyConfiguration("Left Axis X's Key: ", inputMapping.XBoxControllerSetup.leftAxisXKey, XBoxControllerSetup.DEFAULT_AXIS_LEFT_X, ref XBoxLeftAxisXKeyDefault);
		inputMapping.XBoxControllerSetup.rightAxisYKey = ShowAxisKeyConfiguration("Right Axis Y's Key: ", inputMapping.XBoxControllerSetup.rightAxisYKey, XBoxControllerSetup.DEFAULT_AXIS_RIGHT_Y, ref XBoxRightAxisYKeyDefault);
		inputMapping.XBoxControllerSetup.rightAxisXKey = ShowAxisKeyConfiguration("Right Axis X's Key: ", inputMapping.XBoxControllerSetup.rightAxisXKey, XBoxControllerSetup.DEFAULT_AXIS_RIGHT_X, ref XBoxRightAxisXKeyDefault);
		inputMapping.XBoxControllerSetup.leftTriggerKey = ShowAxisKeyConfiguration("Left Trigger's Key: ", inputMapping.XBoxControllerSetup.leftTriggerKey, XBoxControllerSetup.DEFAULT_AXIS_TRIGGER_LEFT, ref XBoxLeftTriggerKeyDefault);
		inputMapping.XBoxControllerSetup.rightTriggerKey = ShowAxisKeyConfiguration("Right Trigger's Key: ", inputMapping.XBoxControllerSetup.rightTriggerKey, XBoxControllerSetup.DEFAULT_AXIS_TRIGGER_RIGHT, ref XBoxRightTriggerXKeyDefault);
		inputMapping.XBoxControllerSetup.dPadAxisYKey = ShowAxisKeyConfiguration("D-Pad Axis Y's Key: ", inputMapping.XBoxControllerSetup.dPadAxisYKey, XBoxControllerSetup.DEFAULT_AXIS_DPAD_Y, ref XBoxDPadAxisYKeyDefault);
		inputMapping.XBoxControllerSetup.dPadAxisXKey = ShowAxisKeyConfiguration("D-Pad Axis X's Key: ", inputMapping.XBoxControllerSetup.dPadAxisXKey, XBoxControllerSetup.DEFAULT_AXIS_DPAD_X, ref XBoxDPadAxisXKeyDefault);
	}

	/// <summary>Sets all registered flags' values.</summary>
	private static void SetDefaultKeyFlags()
	{
		SetDefaultKeyFlag(inputMapping.XBoxControllerSetup.leftAxisYKey, XBoxControllerSetup.DEFAULT_AXIS_LEFT_Y, ref XBoxLeftAxisYKeyDefault);
		SetDefaultKeyFlag(inputMapping.XBoxControllerSetup.leftAxisXKey, XBoxControllerSetup.DEFAULT_AXIS_LEFT_X, ref XBoxLeftAxisXKeyDefault);
		SetDefaultKeyFlag(inputMapping.XBoxControllerSetup.rightAxisYKey, XBoxControllerSetup.DEFAULT_AXIS_RIGHT_Y, ref XBoxRightAxisYKeyDefault);
		SetDefaultKeyFlag(inputMapping.XBoxControllerSetup.rightAxisXKey, XBoxControllerSetup.DEFAULT_AXIS_RIGHT_X, ref XBoxRightAxisXKeyDefault);
		SetDefaultKeyFlag(inputMapping.XBoxControllerSetup.leftTriggerKey, XBoxControllerSetup.DEFAULT_AXIS_TRIGGER_LEFT, ref XBoxLeftTriggerKeyDefault);
		SetDefaultKeyFlag(inputMapping.XBoxControllerSetup.rightTriggerKey, XBoxControllerSetup.DEFAULT_AXIS_TRIGGER_RIGHT, ref XBoxRightTriggerXKeyDefault);
		SetDefaultKeyFlag(inputMapping.XBoxControllerSetup.dPadAxisYKey, XBoxControllerSetup.DEFAULT_AXIS_DPAD_Y, ref XBoxDPadAxisYKeyDefault);
		SetDefaultKeyFlag(inputMapping.XBoxControllerSetup.dPadAxisXKey, XBoxControllerSetup.DEFAULT_AXIS_DPAD_X, ref XBoxDPadAxisXKeyDefault);
	}

	/// <summary>Sets flags according to the comparision to provided key and expected default key value.</summary>
	/// <param name="_key">Key to compare.</param>
	/// <param name="_default">Key's expected default value.</param>
	/// <param name="_flag">Flag's reference to modify.</param>
	private static void SetDefaultKeyFlag(string _key, string _default, ref bool _flag)
	{
		_flag = (_key == _default);
	}

	/// <summary>Shows Axis Key's Configurations.</summary>
	private string ShowAxisKeyConfiguration(string _label, string _key, string _default, ref bool _flag)
	{
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label(_label, GUILayout.Width(WIDTH_LABEL));
		if(_flag)
		{
			GUILayout.Label(_default, GUILayout.Width(WIDTH_TEXT_FIELD));
			if(GUILayout.Button("Configure", GUILayout.Width(WIDTH_BUTTON))) _flag = false;
			EditorGUILayout.EndHorizontal();
			return _default;
		}
		else
		{
			_key = GUILayout.TextField(_key, GUILayout.Width(WIDTH_TEXT_FIELD));

			if(GUILayout.Button("Set as Default", GUILayout.Width(WIDTH_BUTTON)))
			{
				_key = _default;
				_flag = true;
			}
			EditorGUILayout.EndHorizontal();
			return _key;
		}
	}

	/// <summary>Shows TextAsset's Settings.</summary>
	private void ShowTextAssetSettings()
	{
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		GUILayout.Label("Serialization Settings: ");
		EditorGUILayout.Space();
		file = EditorGUILayout.ObjectField("File: ", file, typeof(TextAsset), true) as TextAsset;
		if(file != null)
		{
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Save")) Save();
			if(GUILayout.Button("Load")) Load();
			EditorGUILayout.EndHorizontal();
		}
	}

	/// <summary>Gets Max Length of the Keys of all possible controllers.</summary>
	private int GetMaxLength()
	{
		if(inputMapping == null) return 0;

		int maxSize = 0;

		if(inputMapping.PCControllerSetup.keyMapping.Length > maxSize) maxSize = inputMapping.PCControllerSetup.keyMapping.Length;
		if(inputMapping.XBoxControllerSetup.keyMapping.Length > maxSize) maxSize = inputMapping.XBoxControllerSetup.keyMapping.Length;
		if(inputMapping.NintendoSwitchControllerSetup.keyMapping.Length > maxSize) maxSize = inputMapping.NintendoSwitchControllerSetup.keyMapping.Length;
		if(inputMapping.N3DSControllerSetup.keyMapping.Length > maxSize) maxSize = inputMapping.N3DSControllerSetup.keyMapping.Length;

		return maxSize;
	}

	/// <summary>Shows Array's shared configurations for all controllers' keys.</summary>
	private void ShowArraysSizeConfiguration()
	{
		int maxSize = GetMaxLength();
		int newSize = Mathf.Max(0, EditorGUILayout.DelayedIntField("Size: ", maxSize));
		int difference = newSize - maxSize;

		if(difference != 0)
		{
			inputMapping.ResizeMapping(newSize);
			inputMapping.PCControllerSetup.ResizeMapping(newSize);
			inputMapping.XBoxControllerSetup.ResizeMapping(newSize);
			inputMapping.NintendoSwitchControllerSetup.ResizeMapping(newSize);
			inputMapping.N3DSControllerSetup.ResizeMapping(newSize);
		}
	}

	/// <summary>Saves configurations into stored TextAsset.</summary>
	private void Save()
	{
		InputMapping mapping = VJSONSerializer.DeserializeFromJSONFromTextAsset<InputMapping>(file);
		
		if(EditorUtility.DisplayDialog
		(
			"Warning!",
			mapping != null ? MESSAGE_MAPPING : MESSAGE_NO_MAPPING,
			"Yes",
			"No"
		))
		{			
			inputMapping.SerializeToJSONIntoTextAsset(file);
			VEditorData.SaveString(EDITOR_DATA_KEY_MAPPING_PATH, AssetDatabase.GetAssetPath(file));
			AssetDatabase.Refresh();
		}
	}

	/// <summary>Loads configurations from stored TextAsset.</summary>
	private void Load()
	{
		InputMapping mapping = VJSONSerializer.DeserializeFromJSONFromTextAsset<InputMapping>(file);
		if(mapping == null)
		{
			EditorUtility.DisplayDialog
			(
				"Input Mapping Not Found!",
				"Text File provided did not contain any Input Mapping's Information, data won't be loaded.",
				"Ok"
			);
		} else inputMapping = mapping;
	}

	/// <summary>Loads Input Mapping at the window's beginning.</summary>
	private static void LoadInputMapping()
	{
		string path = VEditorData.LoadString(EDITOR_DATA_KEY_MAPPING_PATH);

		InputMapping mapping = VJSONSerializer.DeserializeFromJSONFromPath<InputMapping>(path);
		inputMapping = mapping != null ? mapping : new InputMapping();
		if(mapping != null) file = AssetDatabase.LoadAssetAtPath(path, typeof(TextAsset)) as TextAsset;

		inputMapping.ResizeMappings();
	}
}
}