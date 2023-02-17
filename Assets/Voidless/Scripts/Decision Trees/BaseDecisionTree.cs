using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class BaseDecisionTree : MonoBehaviour
{
#region UnityMethods:
	/// <summary>BaseDecisionTree's' instance initialization.</summary>
	void Awake()
	{
		
	}

	/// <summary>BaseDecisionTree's starting actions before 1st Update frame.</summary>
	void Start ()
	{
		
	}
	
	/// <summary>BaseDecisionTree's tick at each frame.</summary>
	void Update ()
	{
		
	}
#endregion

	public IEnumerator<int> DecisionNode(Func<int> NodeResult, params IEnumerator<int>[] childrenDecisions)
	{
		int result = NodeResult();
		childrenDecisions[result].MoveNext();
		yield return result;
	}
}
}