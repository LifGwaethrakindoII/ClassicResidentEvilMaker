using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace Voidless.EditorNodes
{
//[CreateAssetMenu(menuName = VString.PATH_SCRIPTABLE_OBJECTS + PATH_NODE_EDITOR)]
public abstract class BaseNodeEditorAttributes : ScriptableObject
{
	protected const string PATH_NODE_EDITOR = " / Node Editor"; 		///< <summary>Node Editor's Path.</summary>

	[Header("Node Attributes:")]
	[SerializeField] private GUIStyle _nodeStyle; 						///< <summary>Node's GUIStyle.</summary>
	[SerializeField] private GUIStyle _selectedNodeStyle; 				///< <summary>Node's selected GUIStyle.</summary>
	[SerializeField] private float _nodeWidth; 							///< <summary>Node's width.</summary>
	[SerializeField] private float _nodeHeight; 						///< <summary>Node's height.</summary>
	[SerializeField] private float _nodeFieldHorizontalOffset; 			///< <summary>Node's field horizontal offset.</summary>
	[SerializeField] private float _nodeFieldHeight; 					///< <summary>Node's field height.</summary>
	[Space(5f)]
	[Header("Connection Attributes:")]
	[SerializeField] private Color _beizerLineColor; 					///< <summary>Connection's beizer line color.</summary>
	[SerializeField] private float _beizerLineTangent; 					///< <summary>Connection's beizer line tangent.</summary>
	[SerializeField] private float _beizerLineWidth; 					///< <summary>Connection's beizer line width.</summary>
	[SerializeField] private float _handlesButtonSize; 					///< <summary>Connection's handles button size.</summary>
	[SerializeField] private float _handlesButtonPickSize; 				///< <summary>Connection's handles button pick size.</summary>
	[Space(5f)]
	[Header("Connection Points Attributes:")]
	[SerializeField] PointAllignmentTypes _pointAllingmentType; 		///< <summary>Point allingment's type.</summary>
	[SerializeField] private GUIStyle _inPointStyle; 					///< <summary>Input Connection Point's GUIStyle.</summary>
	[SerializeField] private GUIStyle _outPointStyle; 					///< <summary>Output Connection Point's GUIStyle.</summary>
	[SerializeField] private float _pointWidth; 						///< <summary>Connection Point's width.</summary>
	[SerializeField] private float _pointHeight; 						///< <summary>Connection Point's height.</summary>
	[SerializeField] private float _rectWidthOffset; 					///< <summary>Connection Point's rect width offset.</summary>
	[SerializeField] private float _rectHeightOffset; 					///< <summary>Connection Point's rect height offset.</summary>
	[Space(5f)]
	[Header("Data Panel Atributes:")]
	[SerializeField] private GUIStyle _loadingBackgroundStyle; 			///< <summary>Loading background's GUIStyle.</summary>
	[SerializeField] private GUIStyle _dataPanelStyle; 					///< <summary>Data Panel's GUIStyle.</summary>
	[SerializeField] private GUIStyle _buttonStyle; 					///< <summary>Button's GUIStyle.</summary>
	[SerializeField] private float _dataPanelHeight = 150f; 			///< <summary>Data Panel's field height.</summary>
	[SerializeField] private float _dataFieldYOffset = 50f; 			///< <summary>Data Panel's field Y-Axis's offset.</summary>
	[SerializeField] private float _dataFieldHeight = 15f; 				///< <summary>Data Panel's field height.</summary>
	[SerializeField] private float _buttonYOffset = 300f; 				///< <summary>Data Panel's load button Y-Axis's offset.</summary>
	[SerializeField] private float _buttonWidth = 120f; 				///< <summary>Data Panel's load button width.</summary>
	[SerializeField] private float _buttonHeight = 75; 					///< <summary>Data Panel's load button height.</summary>
	[SerializeField] private float _buttonSpacing = 200f; 				///< <summary>Data Panel's spacing between buttons.</summary>
	[Space(5f)]
	[Header("Grid Attributes:")]
	[SerializeField] private Color _grossLineColor; 					///< <summary>Grid's gross line color.</summary>
	[SerializeField] private Color _thinLineColor; 						///< <summary>Grid's thin line color.</summary>
	[SerializeField] private float _grossLineSpacing; 					///< <summary>Grid's gross line spacing.</summary>
	[SerializeField] private float _thinLineSpacing; 					///< <summary>Grid's thin line spacing.</summary>
	[SerializeField] [Range(0f, 1f)] private float _grossLineAlpha; 	///< <summary>Grid's dross line alpha value.</summary>
	[SerializeField] [Range(0f, 1f)] private float _thinLineAlpha; 		///< <summary>Grid's thin line alpha value.</summary>

#region Getters:
	/// <summary>Gets pointAllingmentType property.</summary>
	public PointAllignmentTypes pointAllingmentType { get { return _pointAllingmentType; } }

	/// <summary>Gets grossLineColor property.</summary>
	public Color grossLineColor { get { return _grossLineColor; } }

	/// <summary>Gets thinLineColor property.</summary>
	public Color thinLineColor { get { return _thinLineColor; } }

	/// <summary>Gets beizerLineColor property.</summary>
	public Color beizerLineColor { get { return _beizerLineColor; } }

	/// <summary>Gets nodeStyle property.</summary>
	public GUIStyle nodeStyle { get { return _nodeStyle; } }

	/// <summary>Gets selectedNodeStyle property.</summary>
	public GUIStyle selectedNodeStyle { get { return _selectedNodeStyle; } }

	/// <summary>Gets inPointStyle property.</summary>
	public GUIStyle inPointStyle { get { return _inPointStyle; } }

	/// <summary>Gets outPointStyle property.</summary>
	public GUIStyle outPointStyle { get { return _outPointStyle; } }

	/// <summary>Gets loadingBackgroundStyle property.</summary>
	public GUIStyle loadingBackgroundStyle { get { return _loadingBackgroundStyle; } }

	/// <summary>Gets dataPanelStyle property.</summary>
	public GUIStyle dataPanelStyle { get { return _dataPanelStyle; } }

	/// <summary>Gets buttonStyle property.</summary>
	public GUIStyle buttonStyle { get { return _buttonStyle; } }

	/// <summary>Gets nodeWidth property.</summary>
	public float nodeWidth { get { return _nodeWidth; } }

	/// <summary>Gets nodeHeight property.</summary>
	public float nodeHeight { get { return _nodeHeight; } }

	/// <summary>Gets nodeFieldHorizontalOffset property.</summary>
	public float nodeFieldHorizontalOffset { get { return _nodeFieldHorizontalOffset; } }

	/// <summary>Gets nodeFieldHeight property.</summary>
	public float nodeFieldHeight { get { return _nodeFieldHeight; } }

	/// <summary>Gets beizerLineTangent property.</summary>
	public float beizerLineTangent { get { return _beizerLineTangent; } }

	/// <summary>Gets beizerLineWidth property.</summary>
	public float beizerLineWidth { get { return _beizerLineWidth; } }

	/// <summary>Gets handlesButtonSize property.</summary>
	public float handlesButtonSize { get { return _handlesButtonSize; } }

	/// <summary>Gets handlesButtonPickSize property.</summary>
	public float handlesButtonPickSize { get { return _handlesButtonPickSize; } }

	/// <summary>Gets pointWidth property.</summary>
	public float pointWidth { get { return _pointWidth; } }

	/// <summary>Gets pointHeight property.</summary>
	public float pointHeight { get { return _pointHeight; } }

	/// <summary>Gets rectWidthOffset property.</summary>
	public float rectWidthOffset { get { return _rectWidthOffset; } }

	/// <summary>Gets rectHeightOffset property.</summary>
	public float rectHeightOffset { get { return _rectHeightOffset; } }

	/// <summary>Gets dataPanelHeight property.</summary>
	public float dataPanelHeight { get { return _dataPanelHeight; } }

	/// <summary>Gets dataFieldYOffset property.</summary>
	public float dataFieldYOffset { get { return _dataFieldYOffset; } }

	/// <summary>Gets dataFieldHeight property.</summary>
	public float dataFieldHeight { get { return _dataFieldHeight; } }

	/// <summary>Gets buttonYOffset property.</summary>
	public float buttonYOffset { get { return _buttonYOffset; } }

	/// <summary>Gets buttonWidth property.</summary>
	public float buttonWidth { get { return _buttonWidth; } }

	/// <summary>Gets buttonHeight property.</summary>
	public float buttonHeight { get { return _buttonHeight; } }

	/// <summary>Gets buttonSpacing property.</summary>
	public float buttonSpacing { get { return _buttonSpacing; } }

	/// <summary>Gets grossLineSpacing property.</summary>
	public float grossLineSpacing { get { return _grossLineSpacing; } }

	/// <summary>Gets thinLineSpacing property.</summary>
	public float thinLineSpacing { get { return _thinLineSpacing; } }

	/// <summary>Gets grossLineAlpha property.</summary>
	public float grossLineAlpha { get { return _grossLineAlpha; } }

	/// <summary>Gets thinLineAlpha property.</summary>
	public float thinLineAlpha { get { return _thinLineAlpha; } }
#endregion
}
}