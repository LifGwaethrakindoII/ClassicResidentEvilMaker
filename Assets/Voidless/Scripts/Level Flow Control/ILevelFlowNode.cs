using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.LevelFlowControl
{
public interface ILevelFlowNode : INode<ILevelFlowNode, bool>
{
	bool hasIDAssigned { get; set; } 		/// <summary>Does this Node have ID assigned? [Not the default 0].</summary>
}
}