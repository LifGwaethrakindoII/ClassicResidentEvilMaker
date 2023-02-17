using System.Collections;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless.EditorNodes
{
[System.Serializable]
public abstract class LevelFlowLeaf : BaseLevelFlowNode
{
	protected const string GAME_OBJECT = "Game Object:";
	protected const string SCRIPT_LABEL = "Script:";
	protected const string METHOD_LABEL = "Method:";
	protected const string DISPLAY_NO_SCRIPT_TITLE = "No MonoBehaviour attached!";
	protected const string DISPLAY_NO_SCRIPT_MESSAGE = "This GameObject has no MonoBehaviour attached, be sure to attach it a MonoBehaviour script.";
	protected const string DISPLAY_ACCEPT_ANSWER = "Ok";

	[SerializeField] private GameObject _gameObject;
	[SerializeField] private MonoBehaviour[] _monos;
	[SerializeField] private List<MethodInfo> _methodsInfo;
    [SerializeField] private ParameterInfo[] _parametersInfo;
    [SerializeField] private object[] _arguments;   /// <summary>Arguments Array, where the first dimension belongs to the index of the methods, and the second dimension the array of parameters of the current method.</summary>
    [SerializeField] private string[] _monosNames;
    [SerializeField] private string[] _methodsNames;
    [SerializeField] private int _monoIndex;
    [SerializeField] private int _index;
    [SerializeField] private int _argumentIndex;

#region Getters/Setters:
	/// <summary>Gets and Sets nodeName property.</summary>
	public override string nodeName { get { return "Leaf"; } }

	/// <summary>Gets and Sets gameObject property.</summary>
	public GameObject gameObject
	{
		get { return _gameObject; }
		set { _gameObject = value; }
	}
	/// <summary>Gets and Sets monos property.</summary>
	public MonoBehaviour[] monos
	{
		get { return _monos; }
		set { _monos = value; }
	}

	 /// <summary>Gets and Sets methodsInfo property.</summary>
    public List<MethodInfo> methodsInfo
    {
        get { return _methodsInfo; }
        set { _methodsInfo = value; }
    }

    /// <summary>Gets and Sets parametersInfo property.</summary>
    public ParameterInfo[] parametersInfo
    {
        get { return _parametersInfo; }
        set { _parametersInfo = value; }
    }

    /// <summary>Gets and Sets arguments property.</summary>
    public object[] arguments
    {
        get { return _arguments; }
        set { _arguments = value; }
    }

    /// <summary>Gets and Sets monosNames property.</summary>
    public string[] monosNames
    {
    	get { return _monosNames; }
    	set { _monosNames = value; }
    }

    /// <summary>Gets and Sets methodsNames property.</summary>
    public string[] methodsNames
    {
        get { return _methodsNames; }
        set { _methodsNames = value; }
    }

    /// <summary>Gets and Sets monoIndex property.</summary>
    public int monoIndex
    {
    	get { return _monoIndex; }
    	set { _monoIndex = value; }
    }

    /// <summary>Gets and Sets index property.</summary>
    public int index
    {
        get { return _index; }
        set { _index = value; }
    }

    /// <summary>Gets and Sets argumentIndex property.</summary>
    public int argumentIndex
    {
    	get { return _argumentIndex; }
    	set { _argumentIndex = value; }
    }
#endregion

	public LevelFlowLeaf()
	{
		//...
	}

	/// <summary>LevelFlowLeaf's constructor.</summary>
	/// <param name="_position">LevelFlowLeaf's Starting Position.</param>
	/// <param name="_width">LevelFlowLeaf's rect width.</param>
	/// <param name="_height">LevelFlowLeaf's rect height.</param>
	/// <param name="_fieldHorizontalOffset">LevelFlowLeaf's Field horizontal offset.</param>
    /// <param name="_fieldHeight">LevelFlowLeaf's Field height.</param>
	/// <param name="_nodeStyle">LevelFlowLeaf's GUIStyle.</param>
    /// <param name="_selectedNodeStyle">Selected GUIStyle.</param>
    /// <param name="onRemoveNode">Action called when the LevelFlowLeaf has to be removed.</param>
    /// <param name="onCopyNode">Action called when the LevelFlowLeaf has to be copied.</param>
	public LevelFlowLeaf(Vector2 _position, float _width, float _height, float _fieldHorizontalOffset, float _fieldHeight, GUIStyle _nodeStyle, GUIStyle _selectedNodeStyle, Action<Node> onRemoveNode, Action<Node, Vector2> onCopyNode)
	{
		rect = new Rect(_position.x, _position.y, _width, _height);
		fieldHorizontalOffset = _fieldHorizontalOffset;
		fieldHeight = _fieldHeight;
		originalHeight = _height;
		nodeStyle = _nodeStyle;
		defaultNodeStyle = _nodeStyle;
		selectedNodeStyle = _selectedNodeStyle;
		OnRemoveNode = onRemoveNode;
        OnCopyNode = onCopyNode;
	}
}
}