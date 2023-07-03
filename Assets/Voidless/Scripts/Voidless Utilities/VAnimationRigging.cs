using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

/*===========================================================================
**
** Class:  VAnimationRigging
**
** Purpose: Static methods & functions for Unity's Animation Rigging namespace.
**
**
** Author: Lîf Gwaethrakindo
**
===========================================================================*/

namespace Voidless
{
public static class VAnimationRigging
{
    /// <summary>Sets weight to all provided RigConstraints [as IRigConstraint].</summary>
    /// <param name="w">Target weight.</param>
    /// <param name="_constraints">IRigConstraints.</param>
    public static void SetRigConstraintsWeights(float w, params IRigConstraint[] _constraints)
    {
        if(_constraints == null || _constraints.Length == 0) return;

        foreach(IRigConstraint constraint in _constraints)
        {
            if(constraint != null) constraint.weight = w;
        }
    }

    /// <summary>Gets target [source object] from MultiAimConstraint.</summary>
    /// <param name="_constraint">MultiAimConstraint's reference.</param>
    /// <param name="_index">Source Object's index [0 by default].</param>
    public static Transform GetSourceObject(this MultiAimConstraint _constraint, int _index = 0)
    {
        return _constraint.data.sourceObjects[_index].transform;
    }

    /// <summary>Gets target from TwoBoneIKConstraint.</summary>
    /// <param name="_constraint">TwoBoneIKConstraint's reference.</param>
    public static Transform GetSourceObject(this TwoBoneIKConstraint _constraint)
    {
        return _constraint.data.target;
    }
}
}