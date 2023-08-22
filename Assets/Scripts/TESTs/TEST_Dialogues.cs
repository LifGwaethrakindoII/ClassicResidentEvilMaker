using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Voidless.REMaker;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class TEST_Dialogues : MonoBehaviour
{
    [NonSerialized, OdinSerialize] public DialogueNode dialogueNode;

    /// <summary>Callback invoked when scene loads, one frame before the first Update's tick.</summary>
    private void Start()
    {
        
    }
}
