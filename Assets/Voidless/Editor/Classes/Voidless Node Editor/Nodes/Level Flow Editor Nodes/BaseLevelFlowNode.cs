using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Voidless.LevelFlowControl;

namespace Voidless.EditorNodes
{
[System.Serializable]
public class BaseLevelFlowNode : Node
{
	private bool _hasID; 	/// <summary>Does this Level Flow node contains ID to be stored on a Blabkboard?.</summary>
	private int _id; 		/// <summary>Level Flow Node's ID [if it has ID].</summary>

	/// <summary>Gets and Sets nodeName property.</summary>
	public override string nodeName { get { return "Level Flow"; } }

	/// <summary>Gets and Sets hasID property.</summary>
	public bool hasID
	{
		get { return _hasID; }
		set { _hasID = value; }
	}

	/// <summary>Gets and Sets ID property.</summary>
	public int ID
	{
		get { return _id; }
		set { _id = value; }
	}

	public BaseLevelFlowNode()
	{
		//...
	}

	/// <summary>BaseLevelFlowNode's constructor.</summary>
	/// <param name="_position">BaseLevelFlowNode's Starting Position.</param>
	/// <param name="_width">BaseLevelFlowNode's rect width.</param>
	/// <param name="_height">BaseLevelFlowNode's rect height.</param>
	/// <param name="_fieldHorizontalOffset">BaseLevelFlowNode's Field horizontal offset.</param>
    /// <param name="_fieldHeight">BaseLevelFlowNode's Field height.</param>
	/// <param name="_nodeStyle">BaseLevelFlowNode's GUIStyle.</param>
    /// <param name="_selectedNodeStyle">Selected GUIStyle.</param>
    /// <param name="onRemoveNode">Action called when the BaseLevelFlowNode has to be removed.</param>
    /// <param name="onCopyNode">Action called when the BaseLevelFlowNode has to be copied.</param>
	public BaseLevelFlowNode(Vector2 _position, float _width, float _height, float _fieldHorizontalOffset, float _fieldHeight, GUIStyle _nodeStyle, GUIStyle _selectedNodeStyle, Action<Node> onRemoveNode, Action<Node, Vector2> onCopyNode)
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

	/// <summary>Removes thid BaseLevelFlowNode from Window Editor's BaseLevelFlowNode List.</summary>
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
		EditorGUI.LabelField(GetNewLayoutPosition(), "Has ID?");
		hasID = EditorGUI.Toggle(GetNewLayoutPosition(), hasID);
		if(hasID)
		{
			EditorGUI.LabelField(GetNewLayoutPosition(), "ID:");
			ID = EditorGUI.IntField(GetNewLayoutPosition(), ID);
		}
	}

	public virtual ILevelFlowNode ConvertToLevelFlowNodeOfType() { return null; }
}
}