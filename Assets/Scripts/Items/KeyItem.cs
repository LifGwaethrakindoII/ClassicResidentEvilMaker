using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.REMaker
{
public class KeyItem : Item
{
    protected override void Reset()
    {
        base.Reset();
        interactions = Interactions.KeyDefaultInteractions;
    }
}
}