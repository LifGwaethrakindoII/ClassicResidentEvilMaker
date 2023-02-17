using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Voidless.LevelFlowControl;

namespace Voidless.EditorNodes
{
[System.Serializable]
public class LevelFlowRoot : BaseLevelFlowNode
{
	/// <summary>Gets and Sets nodeName property.</summary>
	public override string nodeName { get { return "Root"; } }

	public LevelFlowRoot()
	{
		//...
	}

	/// <summary>LevelFlowRoot's constructor.</summary>
	/// <param name="_position">LevelFlowRoot's Starting Position.</param>
	/// <param name="_width">LevelFlowRoot's rect width.</param>
	/// <param name="_height">LevelFlowRoot's rect height.</param>
	/// <param name="_fieldHorizontalOffset">LevelFlowRoot's Field horizontal offset.</param>
    /// <param name="_fieldHeight">LevelFlowRoot's Field height.</param>
	/// <param name="_nodeStyle">LevelFlowRoot's GUIStyle.</param>
    /// <param name="_selectedNodeStyle">Selected GUIStyle.</param>
    /// <param name="onRemoveNode">Action called when the LevelFlowRoot has to be removed.</param>
    /// <param name="onCopyNode">Action called when the LevelFlowRoot has to be copied.</param>
	public LevelFlowRoot(Vector2 _position, float _width, float _height, float _fieldHorizontalOffset, float _fieldHeight, GUIStyle _nodeStyle, GUIStyle _selectedNodeStyle, Action<Node> onRemoveNode, Action<Node, Vector2> onCopyNode)
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

	/// <summary>Shows Tree LevelFlowRoot creation Menu Context.</summary>
	/// <param name="_mousePosition">Position of the mouse when the right click took place.</param>
	public override void ProcessContextMenu(Vector2 _mousePosition)
	{
		GenericMenu nodeContextMenu = new GenericMenu();
        nodeContextMenu.AddItem(new GUIContent(CONTEXT_MENU_REMOVE_NODE), false, ()=>{ OnRemoveNode(this); });             
        nodeContextMenu.ShowAsContext();
	}

	/// <summary>Removes thid LevelFlowRoot from Window Editor's LevelFlowRoot List.</summary>
	public override void OnRemoveNodeSelected()
	{
		OnRemoveNode(this);
	}

	/// <summary>Clones Node into new Node.</summary>
	/// <returns>Clone of this Node.</returns>
	public override Node CloneNode()
	{
		return this;
	}

	/// <summary>Draws Node's Fields.</summary>
	public override void DrawNodeFields()
	{
		base.DrawNodeFields();
		CheckForLayoutUpdate();
	}

	public override ILevelFlowNode ConvertToLevelFlowNodeOfType()
	{
		//return new RootLevelFlowNode(ID);
		return RootLevelFlowNode.NewInstance(ID);
	}
}
}