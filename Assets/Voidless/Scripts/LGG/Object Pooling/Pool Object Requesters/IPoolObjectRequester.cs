using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*===========================================================================
**
** Interface:  IPoolObjectRequester
**
** Purpose: Interface for objects that may request for Pool-Objects.
**
**
** Author: Lîf Gwaethrakindo
**
===========================================================================*/
namespace LGG
{
    public interface IPoolObjectRequester<out T> : IPoolObject where T : IPoolObject
    {
        /// <summary>Gets Requested Pool-Object [this should be the prefab or the template object].</summary>
        T requestedPoolObject { get; }
        /// <summary>Gets Pool-Object [this should be the reference the currently requested Pool-Object].</summary>
        T poolObject { get; }

        /// <summary>Condition Evaluation, this must be true in order to be able to request.</summary>
        /// <returns>Condition's Evaluation.</returns>
        bool RequestCondition();
    }
}