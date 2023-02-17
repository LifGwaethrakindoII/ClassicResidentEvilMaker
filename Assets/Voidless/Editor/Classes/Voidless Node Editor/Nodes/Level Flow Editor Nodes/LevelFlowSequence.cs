using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless.EditorNodes
{
[System.Serializable]
public class LevelFlowSequence : BaseLevelFlowNode
{
	/// <summary>Gets and Sets nodeName property.</summary>
	public override string nodeName { get { return "Sequence"; } }

	public LevelFlowSequence()
	{
		//...
	}

	/// <summary>LevelFlowSequence's constructor.</summary>
	/// <param name="_position">LevelFlowSequence's Starting Position.</param>
	/// <param name="_width">LevelFlowSequence's rect width.</param>
	/// <param name="_height">LevelFlowSequence's rect height.</param>
	/// <param name="_fieldHorizontalOffset">LevelFlowSequence's Field horizontal offset.</param>
    /// <param name="_fieldHeight">LevelFlowSequence's Field height.</param>
	/// <param name="_nodeStyle">LevelFlowSequence's GUIStyle.</param>
    /// <param name="_selectedNodeStyle">Selected GUIStyle.</param>
    /// <param name="onRemoveNode">Action called when the LevelFlowSequence has to be removed.</param>
    /// <param name="onCopyNode">Action called when the LevelFlowSequence has to be copied.</param>
	public LevelFlowSequence(Vector2 _position, float _width, float _height, float _fieldHorizontalOffset, float _fieldHeight, GUIStyle _nodeStyle, GUIStyle _selectedNodeStyle, Action<Node> onRemoveNode, Action<Node, Vector2> onCopyNode)
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

	/// <summary>Removes thid LevelFlowSequence from Window Editor's LevelFlowSequence List.</summary>
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
}
}