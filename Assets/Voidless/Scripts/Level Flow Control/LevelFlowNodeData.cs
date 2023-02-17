using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.LevelFlowControl
{
[Serializable]
public class LevelFlowNodeData : BaseNodeData<ILevelFlowNode> 
{
	public override ILevelFlowNode GetNodeFlow()
	{
		return default(ILevelFlowNode);
	}
}
}