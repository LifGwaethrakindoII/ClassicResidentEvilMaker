using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless.REMaker
{
public class ScriptableDialogueNode : ScriptableObject
{
    [SerializeField][TextArea] private string _text;
    [SerializeField] private List<ScriptableDialogueNode> _children;
}
}