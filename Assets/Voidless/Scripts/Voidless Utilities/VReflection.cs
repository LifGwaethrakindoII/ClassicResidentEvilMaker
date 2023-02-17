using System.Reflection;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public static class VReflection
{
	/// <summary>Universal Binding's Flags.</summary>
	public const BindingFlags BINDING_FLAGS_UNIVERSAL = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

	/// <summary>Gets a collection of specified signature methods from MonoBehaviour.</summary>
	/// <param name="_mono">MonoBehaviour to extract methods from.</param>
	/// <param name="_returnType">Return type methods criteria.</param>
	/// <param name="_paramTypes">Type of Parameters that the Method signature must have criteria [null if no parameters expected].</param>
	/// <param name="_flags">Methods binding flags.</param>
	/// <returns>List of MethodInfos with the established criteria.</returns>
	public static List<MethodInfo> GetMethods(this MonoBehaviour _mono, Type _returnType, BindingFlags _flags, params Type[] _paramTypes)
	{
		return _mono.GetType().GetMethods(_flags).Where
		(
			method => method.ReturnType == _returnType
		).Select
		(
			method => new
			{
				method, Params = method.GetParameters()
			}
		).Where
		(x =>
			{
				return _paramTypes == null ?
				x.Params.Length == 0 : 
				x.Params.Length == _paramTypes.Length && x.Params
				.Select(p => p.ParameterType).ToArray().IsEqualTo(_paramTypes);
			}
		).Select
		(
			x => x.method
		).ToList();
	}

	/// <summary>[Overload Method] Gets a collection of specified signature methods from MonoBehaviour. Does not care about the specifics of parameters on Method's signature</summary>
	/// <param name="_mono">MonoBehaviour to extract methods from.</param>
	/// <param name="_returnType">Return type methods criteria.</param>
	/// <param name="_flags">Methods binding flags.</param>
	/// <returns>List of MethodInfos with the established criteria.</returns>
	public static List<MethodInfo> GetMethods(this MonoBehaviour _mono, Type _returnType, BindingFlags _flags)
	{
		return _mono.GetType().GetMethods(_flags).Where
		(
			method => method.ReturnType == _returnType
		).ToList();
	}

	/// <summary>[Overload Method] Gets a collection of specified signature methods from MonoBehaviour. Does not care about the specifics of parameters on Method's signature</summary>
	/// <param name="_mono">MonoBehaviour to extract methods from.</param>
	/// <param name="_returnType">Return type methods criteria.</param>
	/// <param name="_flags">Methods binding flags.</param>
	/// <returns>List of MethodInfos with the established criteria.</returns>
	public static List<MethodInfo> GetMethods(this GameObject _gameObject, Type _returnType, BindingFlags _flags)
	{
		MonoBehaviour[] monos = _gameObject.GetComponents<MonoBehaviour>();
		List<MethodInfo> methodsInfo = new List<MethodInfo>();

		foreach(MonoBehaviour mono in monos)
		{
			methodsInfo.AddRange(mono.GetMethods(_returnType, _flags));	
		}

		return methodsInfo;
	}

	/// <summary>Gets a collection of specified signature methods from GameObject.</summary>
	/// <param name="_mono">GameObject to extract methods from.</param>
	/// <param name="_returnType">Return type methods criteria.</param>
	/// <param name="_paramTypes">Type of Parameters that the Method signature must have criteria [null if no parameters expected].</param>
	/// <param name="_flags">Methods binding flags.</param>
	/// <returns>List of MethodInfos with the established criteria.</returns>
	public static List<MethodInfo> GetMethods(this GameObject _gameObject, Type _returnType, BindingFlags _flags, params Type[] _paramTypes)
	{
		MonoBehaviour[] monos = _gameObject.GetComponents<MonoBehaviour>();
		List<MethodInfo> methodsInfo = new List<MethodInfo>();

		foreach(MonoBehaviour mono in monos)
		{
			methodsInfo.AddRange(mono.GetMethods(_returnType, _flags, _paramTypes));	
		}

		return methodsInfo;
	}

	/// \TODO Move this Extension Method to another VoidlessExtension class that is specific for either Lists or IList
	/// <summary>Checks if IList equals another IList.</summary>
	/// <param name="_list">IList to compare.</param>
	/// <param name="_otherList">Other IList to compare this IList.</param>
	/// <returns>If IList equals the other IList.</returns>
	public static bool IsEqualTo<T>(this IList<T> _list, IList<T> _otherList)
	{
		if(_list.Count != _otherList.Count) return false;
		else
		{
			for(int i = 0; i < _list.Count; i++)
			{
				if(!_list[i].Equals(_otherList[i])) return false;	
			}
		}

		return true;
	}
}
}