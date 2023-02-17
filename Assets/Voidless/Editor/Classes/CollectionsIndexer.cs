using System.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

using Object = UnityEngine.Object;

namespace Voidless
{
[Serializable]
public class CollectionsIndexer
{
	public const string PATH_COUNT_OBJECTS = "CollectionsIndexer_Objects_Count";
	public const string PATH_OBJECT = "CollectionsIndexer_Object_AssetPath_";

	public static string[] paths; 																	/// <summary>Persistent Object's Paths.</summary>
	public static List<Object> objects; 															/// <summary>Persistent Objects.</summary>
	public static Dictionary<int, List<VTuple<string, IEnumerable<Object>>>> objectsCollections; 	/// <summary>Objects' Collections.</summary>
	public static bool loaded;

	/// <summary>Initializes Collections' Indexer.</summary>
	public static void Initialize()
	{
		objectsCollections = new Dictionary<int,List<VTuple<string, IEnumerable<Object>>>>();
	}

	/// [UnityEditor.Callbacks.DidReloadScripts]
	public static void Update()
	{
		if(objects == null) return;

		StringBuilder builder = new StringBuilder();
		FieldInfo[] fieldsInfo = null;
		List<VTuple<string, IEnumerable<Object>>> tupleCollections = null;
		int ID = 0;

		foreach(Object obj in objects)
		{
			if(obj == null) continue;
			
			fieldsInfo = obj.GetType().GetFields(VReflection.BINDING_FLAGS_UNIVERSAL);
			tupleCollections = new List<VTuple<string, IEnumerable<Object>>>();
			ID = obj.GetInstanceID();

			foreach(FieldInfo info in fieldsInfo)
			{
				object fieldValue = info.GetValue(obj);
				VTuple<string, IEnumerable<Object>> tuple = new VTuple<string, IEnumerable<Object>>(null, null);

				if(fieldValue == null) continue;

				if(fieldValue is IEnumerable<Object>)
				{
					tuple.Item1 = info.Name;
					tuple.Item2 = fieldValue as IEnumerable<Object>;
				}

				if(tuple.Item2 != null)
				{
					tupleCollections.Add(tuple);
					foreach(Object asset in tuple.Item2)
					{
						builder.AppendLine(asset != null ? asset.name : "NULL");
					}
				}
			}

			if(objectsCollections.ContainsKey(ID)) objectsCollections[ID] = tupleCollections;
			else objectsCollections.Add(ID, tupleCollections);
		}
	}

	/// <summary>Loads Assets from VoidlessEditor's Data.</summary>
	public static void Load()
	{
		if(loaded) return;

		Initialize();

		int count = VEditorData.LoadInt(PATH_COUNT_OBJECTS);

		if(count <= 0) return;

		objects = new List<Object>(count);

		for(int i = 0; i < count; i++)
		{
			string path = VEditorData.LoadString(PATH_OBJECT + i);
			Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(Object)) as Object;
			if(obj != null) objects.Add(obj);
		}

		loaded = true;
	}

	/// <returns>Objects' Names to an Array.</returns>
	public static string[] GetObjectsNames()
	{
		List<string> objectsNames = new List<string>();
		
		foreach(Object obj in objects)
		{
			objectsNames.Add(obj.name);
		}

		return objectsNames.ToArray();
	}

	/// <summary>Gets Object's Collections' Names.</summary>
	/// <param name="_index">Index of the object.</param>
	/// <returns>Collections' Names.</returns>
	public static string[] GetObjectCollectionsNames(int _index)
	{
		if(objects == null) return null;

		_index = Mathf.Clamp(_index, 0, objects.Count - 1);

		Object obj = objects[_index];

		if(obj == null) return null;

		int ID = obj.GetInstanceID();
		List<string> collectionNames = new List<string>();

		if(objectsCollections.ContainsKey(ID))
		{
			foreach(VTuple<string, IEnumerable<Object>> tuple in objectsCollections[ID])
			{
				collectionNames.Add(tuple.Item1.ToInspectorFormat());
			}
		}

		return collectionNames.ToArray();
	}

	/// <summary>Gets Object's Collections' Items.</summary>
	/// <param name="_objectIndex">Index of the object.</param>
	/// <param name="_collectionIndeex">Index of the Collection.</param>
	/// <returns>Collections' Items.</returns>
	public static string[] GetObjectCollectionsItems(int _objectIndex, int _collectionIndex)
	{
		int ID = objects[_objectIndex].GetInstanceID();
		List<string> itemNames = new List<string>();

		if(objectsCollections.ContainsKey(ID))
		{
			foreach(Object obj in objectsCollections[ID][_collectionIndex].Item2)
			{
				if(obj != null)
				itemNames.Add(obj.name);
			}
		}

		return itemNames.ToArray();
	}

	public static IEnumerator<VTuple<string, IEnumerable<Object>>> GetEnumerator(int _index)
	{
		int ID = objects[_index].GetInstanceID();

		if(objectsCollections != null && objectsCollections.ContainsKey(ID))
		{
			foreach(VTuple<string, IEnumerable<Object>> tuple in objectsCollections[ID])
			{
				yield return tuple;
			}
		}
	}
}
}