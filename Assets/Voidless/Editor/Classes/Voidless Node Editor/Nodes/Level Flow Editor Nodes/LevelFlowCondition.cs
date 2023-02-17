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
public class LevelFlowCondition : LevelFlowLeaf
{
	/// <summary>Gets and Sets nodeName property.</summary>
	public override string nodeName { get { return "Condition"; } }

	public LevelFlowCondition()
	{
		//...
	}

	/// <summary>LevelFlowCondition's constructor.</summary>
	/// <param name="_position">LevelFlowCondition's Starting Position.</param>
	/// <param name="_width">LevelFlowCondition's rect width.</param>
	/// <param name="_height">LevelFlowCondition's rect height.</param>
	/// <param name="_fieldHorizontalOffset">LevelFlowCondition's Field horizontal offset.</param>
    /// <param name="_fieldHeight">LevelFlowCondition's Field height.</param>
	/// <param name="_nodeStyle">LevelFlowCondition's GUIStyle.</param>
    /// <param name="_selectedNodeStyle">Selected GUIStyle.</param>
    /// <param name="onRemoveNode">Condition called when the LevelFlowCondition has to be removed.</param>
    /// <param name="onCopyNode">Condition called when the LevelFlowCondition has to be copied.</param>
	public LevelFlowCondition(Vector2 _position, float _width, float _height, float _fieldHorizontalOffset, float _fieldHeight, GUIStyle _nodeStyle, GUIStyle _selectedNodeStyle, Action<Node> onRemoveNode, Action<Node, Vector2> onCopyNode)
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

	/// <summary>Removes thid LevelFlowCondition from Window Editor's LevelFlowCondition List.</summary>
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
		EditorGUI.LabelField(GetNewLayoutPosition(), "Game Object:");
		gameObject = EditorGUI.ObjectField(GetNewLayoutPosition(), NO_TEXT, gameObject, typeof(GameObject), true) as GameObject;

		if(gameObject != null)
		{
			GetNewLayoutPosition();
			monos = gameObject.GetComponents<MonoBehaviour>();

			if(monos != null)
			{
				monosNames = monos.Select(m => m.GetType().Name).ToArray();
				EditorGUI.LabelField(GetNewLayoutPosition(), SCRIPT_LABEL);
				monoIndex = EditorGUI.Popup(GetNewLayoutPosition(), monoIndex, monosNames);
				methodsInfo = monos[monoIndex].GetMethods(typeof(bool), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, typeof(int));
				methodsNames = methodsInfo.Select(p => p.Name).ToArray();
				GetNewLayoutPosition();
				EditorGUI.LabelField(GetNewLayoutPosition(), METHOD_LABEL);
				index = EditorGUI.Popup(GetNewLayoutPosition(), NO_TEXT, index, methodsNames);
				parametersInfo = methodsInfo[index].GetParameters();
				arguments = arguments.InitializeArray<object>(parametersInfo.Length);

				for(int i = 0; i < parametersInfo.Length; i++)
				{
					EditorGUI.LabelField(GetNewIndentedLayoutPosition(), parametersInfo[i].Name);

					if(parametersInfo[i].ParameterType == typeof(int))
					{
						argumentIndex = (int)EditorGUI.IntField(GetNewIndentedLayoutPosition(), NO_TEXT, argumentIndex);
						arguments[i] = argumentIndex;
					}

					methodsInfo[index].Invoke((object)monos[monoIndex], arguments);
				}
			}
			else
			{
				gameObject = null;
				EditorUtility.DisplayDialog
				(
					DISPLAY_NO_SCRIPT_TITLE,
					DISPLAY_NO_SCRIPT_MESSAGE,
					DISPLAY_ACCEPT_ANSWER

				);
			}	
		}
		else
		{

		}

		CheckForLayoutUpdate();
	}
}
}