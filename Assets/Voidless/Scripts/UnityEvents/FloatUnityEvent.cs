using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class FloatUnityEvent : UnityEvent<float>
{

}

[Serializable]
public class StringGameObjectUnityEvent : UnityEvent<string, GameObject>
{

}