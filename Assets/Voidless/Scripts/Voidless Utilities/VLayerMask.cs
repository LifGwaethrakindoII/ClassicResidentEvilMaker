using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VLayerMask
{
	public const int LAYER_BIT_DEFAULT = 0;
	public const int LAYER_BIT_TRANSPARENT_FX = 1;
	public const int LAYER_BIT_IGNORE_RAYCAST = 2;
	public const int LAYER_BIT_WATER = 4;
	public const int LAYER_BIT_UI = 5;
	public const int LAYER_VALUE_DEFAULT = 1 << LAYER_BIT_DEFAULT;
	public const int LAYER_VALUE_TRANSPARENT_FX = 1 << LAYER_BIT_TRANSPARENT_FX;
	public const int LAYER_VALUE_IGNORE_RAYCAST = 1 << LAYER_BIT_IGNORE_RAYCAST;
	public const int LAYER_VALUE_WATER = 1 << LAYER_BIT_WATER;
	public const int LAYER_VALUE_UI = 1 << LAYER_BIT_UI;
	public const int LAYER_VALUE_ALL = ~0;
	public const int LENGTH_LAYER_MASK = 32;

	public static List<string> GetRegisteredLayerMasks()
	{
		List<string> layersList = new List<string>();

		for(int i = 0; i < LENGTH_LAYER_MASK; i++)
		{
			string layerName = LayerMask.LayerToName(i);
			if(!string.IsNullOrEmpty(layerName))
			{
				layersList.Add(layerName);
			}
		}

		return layersList;
	}

	public static bool IsValueALayerBit(this int _layer)
	{
		//int bitsCount = _layer.GetActiveFlagsCount();
		return (/*(bitsCount == 0 || bitsCount == 1) && _layer >= 0 && */_layer < LENGTH_LAYER_MASK);
	}

	/// <summary>Evaluates if GameObject's Layer Value is on LayerMask.</summary>
	/// <param name="_value">Value to evaluate.</param>
	/// <param name="_layer">LayerMask.</param>
	/// <returns>True if value is on LayerMask.</returns>
	public static bool IsOnLayerMask(this GameObject _gameObject, LayerMask _layer)
	{
		return ((_layer | (1 << _gameObject.layer)) == _layer);	
	}

	/// <summary>Evaluates if Layer Value is on LayerMask.</summary>
	/// <param name="_value">Value to evaluate.</param>
	/// <param name="_layer">LayerMask.</param>
	/// <returns>True if value is on LayerMask.</returns>
	public static bool HasLayerValue(int _value, LayerMask _layer)
	{
		return ((_layer | (1 << _value)) == _layer);	
	}

	public static LayerMask WithAddedValue(this LayerMask _layerMask, int _value)
	{
		if(_value.IsValueALayerBit()) _layerMask |= (1 << _value);
		return _layerMask;
	}

	public static int NameToIndex(string _name)
	{
		List<string> layers = GetRegisteredLayerMasks();
		int index = 0;

		for(int i = 0; i < layers.Count; i++)
		{
			if(_name == layers[i])
			{
				index = i;
				break;
			}
		}

		return index;
	}

	public static void SetLayerRecursively(this GameObject _gameObject, int _layer, bool _includeAnyOfItsChildren = true, bool _includeActive = true)
	{
		if(_gameObject == null) return;
		
		_gameObject.layer = _layer;

		if(_includeAnyOfItsChildren) foreach(Transform transform in _gameObject.GetComponentsInChildren<Transform>())
		{
			transform.gameObject.layer = _layer;	
		}
		else foreach(Transform transform in _gameObject.transform)
		{
			transform.gameObject.layer = _layer;
		}
	}

	/*public static bool MyInterfaceFilter(Type typeObj, System.Object criteriaObj)
    {
        // 1. "typeObj" is a Type object of an interface supported by class B.
        // 2. "criteriaObj" will be a Type object of the base class of B : 
        // i.e. the Type object of class A.
        Type baseClassType = (Type)criteriaObj;
        // Obtain an array of the interfaces supported by the base class A.
        Type[] interfaces_array = baseClassType.GetInterfaces();
        for (int i = 0; i < interfaces_array.Length; i++)
        {
            // If typeObj is an interface supported by the base class, skip it.
            if (typeObj.ToString() == interfaces_array[i].ToString())
                return false;
        }

        return true;
    }

	public void DebugInterfaces()
	{
		TypeFilter filter = new TypeFilter(MyInterfaceFilter);
		Type[] implementedInterfaces = _gameObject.transform.GetType().FindInterfaces(filter, typeof(Transform));
		Debug.Log("[VLayerMask] SIZE: " + implementedInterfaces.Length);

		foreach(Type type in implementedInterfaces)
		{
			Debug.Log("[VLayerMask] Interface: " + type.ToString());
		}

		Type[] interfaces = typeof(Transform).GetInterfaces();

		foreach(Type interfaceType in interfaces)
		{
			Debug.Log("[VLayerMask] Interface: " + interfaceType.ToString());
		}
	}*/
}
}