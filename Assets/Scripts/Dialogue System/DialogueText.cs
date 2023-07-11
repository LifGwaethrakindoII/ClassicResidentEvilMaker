using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

namespace Voidless.REMaker
{
[Serializable]
public class DialogueText
{
    [SerializeField][TextArea] private string[] _lines;
    [SerializeField] private VTuple<string, DialogueText>[] _optionTuples;

    /// <summary>Gets and Sets lines property.</summary>
    public string[] lines
    {
        get { return _lines; }
        set { _lines = value; }
    }

    /// <summary>Gets and Sets optionTuples property.</summary>
    public VTuple<string, DialogueText>[] optionTuples
    {
        get { return _optionTuples; }
        set { _optionTuples = value; }
    }
}
}