using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless.EditorNodes
{
[System.Serializable]
public class LevelFlowRepeatNTimes : BaseLevelFlowNode
{
	protected const string NUMBER_OF_TIMES = "# of Times:";

	[SerializeField] private int _numberOfTimes; 	/// <summary>Number of times this node will Tick.</summary>

	/// <summary>Gets and Sets nodeName property.</summary>
	public override string nodeName { get { return "RepeatNTimes"; } }

	/// <summary>Gets and Sets numberOfTimes property.</summary>
	public int numberOfTimes
	{
		get { return _numberOfTimes; }
		set { _numberOfTimes = value; }
	}

	public LevelFlowRepeatNTimes()
	{
		//...
	}

	/// <summary>LevelFlowRepeatNTimes's constructor.</summary>
	/// <param name="_position">LevelFlowRepeatNTimes's Starting Position.</param>
	/// <param name="_width">LevelFlowRepeatNTimes's rect width.</param>
	/// <param name="_height">LevelFlowRepeatNTimes's rect height.</param>
	/// <param name="_fieldHorizontalOffset">LevelFlowRepeatNTimes's Field horizontal offset.</param>
    /// <param name="_fieldHeight">LevelFlowRepeatNTimes's Field height.</param>
	/// <param name="_nodeStyle">LevelFlowRepeatNTimes's GUIStyle.</param>
    /// <param name="_selectedNodeStyle">Selected GUIStyle.</param>
    /// <param name="onRemoveNode">Action called when the LevelFlowRepeatNTimes has to be removed.</param>
    /// <param name="onCopyNode">Action called when the LevelFlowRepeatNTimes has to be copied.</param>
	public LevelFlowRepeatNTimes(Vector2 _position, float _width, float _height, float _fieldHorizontalOffset, float _fieldHeight, GUIStyle _nodeStyle, GUIStyle _selectedNodeStyle, Action<Node> onRemoveNode, Action<Node, Vector2> onCopyNode)
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

	/// <summary>Removes thid LevelFlowRepeatNTimes from Window Editor's LevelFlowRepeatNTimes List.</summary>
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
		GetNewLayoutPosition();
		EditorGUI.LabelField(GetNewLayoutPosition(), NUMBER_OF_TIMES);
		numberOfTimes = EditorGUI.IntField(GetNewLayoutPosition(), NO_TEXT, numberOfTimes);
		CheckForLayoutUpdate();
	}
}
}