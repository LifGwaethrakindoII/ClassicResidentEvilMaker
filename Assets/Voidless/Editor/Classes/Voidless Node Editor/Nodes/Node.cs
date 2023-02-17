using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless.EditorNodes
{
[System.Serializable]
public class Node : BaseNode<Node>
{
	/// <summary>Gets and Sets nodeName property.</summary>
	public override string nodeName { get { return "Node"; } }

	/// <summary>Default Node constructor.</summary>
	public Node()
	{
		//...
	}

	/// <summary>Node's constructor.</summary>
	/// <param name="_position">Node's Starting Position.</param>
	/// <param name="_width">Node's rect width.</param>
	/// <param name="_height">Node's rect height.</param>
	/// <param name="_fieldHorizontalOffset">Node's Field horizontal offset.</param>
    /// <param name="_fieldHeight">Node's Field height.</param>
	/// <param name="_nodeStyle">Node's GUIStyle.</param>
    /// <param name="_selectedNodeStyle">Selected GUIStyle.</param>
    /// <param name="onRemoveNode">Action called when the Node has to be removed.</param>
    /// <param name="onCopyNode">Action called when the Node has to be copied.</param>
	public Node(Vector2 _position, float _width, float _height, float _fieldHorizontalOffset, float _fieldHeight, GUIStyle _nodeStyle, GUIStyle _selectedNodeStyle, Action<Node> onRemoveNode, Action<Node, Vector2> onCopyNode)
	{
		childNodes = new List<Node>();
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

	/// <summary>Node ScriptableObject's constructor.</summary>
	/// <param name="_position">Node's Starting Position.</param>
	/// <param name="_width">Node's rect width.</param>
	/// <param name="_height">Node's rect height.</param>
	/// <param name="_fieldHorizontalOffset">Node's Field horizontal offset.</param>
    /// <param name="_fieldHeight">Node's Field height.</param>
	/// <param name="_nodeStyle">Node's GUIStyle.</param>
    /// <param name="_selectedNodeStyle">Selected GUIStyle.</param>
    /// <param name="onRemoveNode">Action called when the Node has to be removed.</param>
    /// <param name="onCopyNode">Action called when the Node has to be copied.</param>
	/// <returns>Instance of this ScriptableClass.</returns>
	new public static Node NewInstance(Vector2 _position, float _width, float _height, float _fieldHorizontalOffset, float _fieldHeight, GUIStyle _nodeStyle, GUIStyle _selectedNodeStyle, Action<Node> onRemoveNode, Action<Node, Vector2> onCopyNode)
	{
		Node newNode = CreateInstance<Node>();

		newNode.rect = new Rect(_position.x, _position.y, _width, _height);
		newNode.fieldHorizontalOffset = _fieldHorizontalOffset;
		newNode.fieldHeight = _fieldHeight;
		newNode.originalHeight = _height;
		newNode.nodeStyle = _nodeStyle;
		newNode.defaultNodeStyle = _nodeStyle;
		newNode.selectedNodeStyle = _selectedNodeStyle;
		newNode.OnRemoveNode = onRemoveNode;
        newNode.OnCopyNode = onCopyNode;

        return newNode;
	}

	/// <summary>Shows Tree Node creation Menu Context.</summary>
	/// <param name="_mousePosition">Position of the mouse when the right click took place.</param>
	public override void ProcessContextMenu(Vector2 _mousePosition)
	{
		GenericMenu nodeContextMenu = new GenericMenu();

        nodeContextMenu.AddItem(new GUIContent(CONTEXT_MENU_REMOVE_NODE), false, ()=>{ OnRemoveNode(this); });    
        nodeContextMenu.AddItem(new GUIContent(CONTEXT_MENU_COPY_NODE), false, ()=>{ OnCopyNode(this, _mousePosition); });    
        
        nodeContextMenu.ShowAsContext();
	}

	/// <summary>Removes thid Node from Window Editor's Node List.</summary>
	public override void OnRemoveNodeSelected()
	{

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
		currentLayoutY = 0;
		EditorGUI.LabelField(GetNewLayoutPosition(), nodeName);
		GetNewLayoutPosition();
	}
}
}