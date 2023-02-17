using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Voidless.LevelFlowControl;

namespace Voidless.EditorNodes
{
public enum LevelFlowNodeTypes
{
	CreateRootNode,
	CreateSelectorNode,
	CreateSequenceNode,
	CreateConditionNode,
	CreateActionNode,
	CreateRepeatNTimesNode,
	CreateInverseNode
}

[System.Serializable]
public class LevelFlowNodeEditorWindow : BaseNodeEditorWindow<BaseLevelFlowNode, LevelFlowNodeEditorData, LevelFlowNodeEditorAttributes, LevelFlowNodeData, ILevelFlowNode>
{
	private const string WINDOW_NAME = "Level Flow Editor";
	private const string WINDOW_PATH = WINDOW_ROOT_PATH + WINDOW_NAME;
	private const string WINDOW_ROOT_EXISTS_TITLE = "There is already a Root Node on window!";
	private const string WINDOW_ROOT_EXISTS_MESSAGE = "There is already a Root Node on the window. There can only be one Root per level flow tree";

	private LevelFlowNodeTypes _selectedLevelFlowType;
	private LevelFlowRoot _root;

	public static LevelFlowNodeEditorWindow levelFlowNodeEditorWindow; 										/// <summary>LevelFlowNodeEditorWindow's static reference</summary>

	/// <summary>Gets and Sets selectedLevelFlowType property.</summary>
	public LevelFlowNodeTypes selectedLevelFlowType
	{
		get { return _selectedLevelFlowType; }
		protected set { _selectedLevelFlowType = value; }
	}

	/// <summary>Gets and Sets root property.</summary>
	public LevelFlowRoot root
	{
		get { return _root; }
		set { _root = value; }
	}

	[MenuItem(WINDOW_PATH)]
	public static new BaseNodeEditorWindow<BaseLevelFlowNode, LevelFlowNodeEditorData, LevelFlowNodeEditorAttributes, LevelFlowNodeData, ILevelFlowNode> CreateNodeEditorWindow()
	{
		levelFlowNodeEditorWindow = GetWindow<LevelFlowNodeEditorWindow>(false, WINDOW_NAME);
		levelFlowNodeEditorWindow.titleContent = new GUIContent(WINDOW_NAME);
		levelFlowNodeEditorWindow.Focus();
		levelFlowNodeEditorWindow.Show();

		return levelFlowNodeEditorWindow;
	}

    /// <summary>Opens Node Editor Window with its respective required Data.</summary>
    /// <param name="_data">Required Node Editor Data.</param>
    public static new void OpenNodeEditorWindowWithData(LevelFlowNodeEditorData _data)
    {
        LevelFlowNodeEditorWindow levelFlowNodeEditorWindow = CreateNodeEditorWindow() as LevelFlowNodeEditorWindow;
        levelFlowNodeEditorWindow.data = _data;
        levelFlowNodeEditorWindow.LoadData(null);
    }

	/// <summary>Processes Context Menu.</summary>
	/// <param name="_mousePosition">Mouse Position when the Context Menu invocation took place.</param>
	public override void ProcessContextMenu(Vector2 _mousePosition)
	{
		nodeContextMenu = new GenericMenu();

	    for(int i = 0; i < LevelFlowNodeTypes.GetValues(typeof(LevelFlowNodeTypes)).Length; i++)
	    {
	    	LevelFlowNodeTypes currentLevelFLowType = (LevelFlowNodeTypes)(i);
	    	nodeContextMenu.AddItem
	    	(
	    		new GUIContent(currentLevelFLowType.ToString()),
	    		false,
	    		delegate
	    		{
	    			selectedLevelFlowType = currentLevelFLowType;
	    			CreateNode(_mousePosition);
	    		}
	    	);  
	    }

	    nodeContextMenu.ShowAsContext();
	}

	/// <summary>Creates Node.</summary>
	/// <param name="_mousePosition">Event's Mouse Position.</param>
	public override void CreateNode(Vector2 _mousePosition)
	{
		if(nodes == null)
		{
			nodes = new List<BaseLevelFlowNode>();
		}
		if(root == null)
		{
			for(int i = 0; i < nodes.Count; i++)
			{
				if(nodes[i] is LevelFlowRoot)
				{
					if(root == null) root = nodes[i] as LevelFlowRoot;
					else OnClickRemoveNode(nodes[i]);
				}	
			}
		}

		BaseLevelFlowNode newNode = null;

		switch(selectedLevelFlowType)
		{
			case LevelFlowNodeTypes.CreateRootNode:
			//newNode = CreateNodeOfType<Node>(_mousePosition);
			//newNode = CreateNewNode(_mousePosition) as BaseLevelFlowNode;
			if(root == null) newNode = root = CreateNodeOfType<LevelFlowRoot>(_mousePosition, false, true) as LevelFlowRoot;
			else
			{
				EditorUtility.DisplayDialog
				(
					WINDOW_ROOT_EXISTS_TITLE,
					WINDOW_ROOT_EXISTS_MESSAGE,
					WINDOW_ACCEPT_ANSWER
				);
				return;
			}
				
			break;

			case LevelFlowNodeTypes.CreateSelectorNode:
			newNode = CreateNodeOfType<LevelFlowSelector>(_mousePosition, true, true);
			break;

			case LevelFlowNodeTypes.CreateSequenceNode:
			newNode = CreateNodeOfType<LevelFlowSequence>(_mousePosition, true, true);
			break;

			case LevelFlowNodeTypes.CreateConditionNode:
			newNode = CreateNodeOfType<LevelFlowCondition>(_mousePosition, true, false);
			break;

			case LevelFlowNodeTypes.CreateActionNode:
			newNode = CreateNodeOfType<LevelFlowAction>(_mousePosition, true, false);
			break;

			case LevelFlowNodeTypes.CreateInverseNode:
			break;

			case LevelFlowNodeTypes.CreateRepeatNTimesNode:
			newNode = CreateNodeOfType<LevelFlowRepeatNTimes>(_mousePosition, true, true);
			break;
		}	

		nodes.Add(newNode);
		//SaveData(null);
	}

	public override void SaveData(Action onSaveEnds)
	{
		if(data != null) data.root = root;
		base.SaveData(onSaveEnds);
	}

	public override void LoadData(Action onLoadEnds)
	{
		if(data != null) root = data.root;
		base.LoadData(onLoadEnds);
	}

	public LevelFlowRoot CreateLevelFlowTree()
	{
		return (LevelFlowRoot)root.ConvertToLevelFlowNodeOfType();
	}

	/// <summary>Converts Editor Nodes to a readable format on Play mode.</summary>
    /// <returns>Readable format Node [preferibaly inheriting INode].</returns>
    public override ILevelFlowNode GetConvertedNodes()
    {
    	return null;
    }
}
}