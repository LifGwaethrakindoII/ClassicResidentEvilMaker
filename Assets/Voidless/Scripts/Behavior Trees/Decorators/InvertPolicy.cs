using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public struct InvertPolicy
{
	public Predicate<TreeState> TreeStateConditions; 	/// <summary>Conditions applied to a given TreeState.</summary>
	public TreeState invertedState; 					/// <summary>TreeState to invert if the conditions are met.</summary>

	/// <summary>InvertPolicy constructor.</summary>
	/// <param name="treeStateConditions">Conditions applied to TreeState.</param>
	/// <param name="_invertedState">Inverted TreeState.</param>
	public InvertPolicy(Predicate<TreeState> treeStateConditions, TreeState _invertedState)
	{
		TreeStateConditions = treeStateConditions;
		invertedState = _invertedState;
	}

	/// <summary>Evaluates the Policy terms, to then return a TreeState.</summary>
	/// <param name="_treeState">TreeState to evaluate the conditions on.</param>
	/// <returns>The inverted state if the conditions were met, given state otherwise.</returns>
	public TreeState EvaluatePolicyTerms(TreeState _treeState)
	{
		return TreeStateConditions(_treeState) ? invertedState : _treeState;
	}
}
}