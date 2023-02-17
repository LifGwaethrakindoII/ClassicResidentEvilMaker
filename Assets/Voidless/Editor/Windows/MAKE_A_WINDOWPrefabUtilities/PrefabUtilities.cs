using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Voidless
{
[ExecuteInEditMode]
public class PrefabUtilities : MonoBehaviour
{
#if UNITY_EDITOR
	[SerializeField] private GameObject _prefab; 			/// <summary>Prefab to substitute for given GameObjects.</summary>
	[SerializeField] private GameObject[] _gameObjects; 	/// <summary>GameObjects to swap for prefab.</summary>

	/// <summary>Gets and Sets prefab property.</summary>
	public GameObject prefab
	{
		get { return _prefab; }
		set { _prefab = value; }
	}

	/// <summary>Gets and Sets gameObjects property.</summary>
	public GameObject[] gameObjects
	{
		get { return _gameObjects; }
		set { _gameObjects = value; }
	}

	/// <summary>PrefabUtilities's' instance initialization.</summary>
	void Awake()
	{
		//gameObject.SetActive(false);
	}

	public void SubstituteGameObjectsForPrefab()
	{
		if(/*gameObjects.Length > 0*/Selection.gameObjects.Length > 0 && prefab != null)
		{
//#if UNITY_EDITOR
			for(int i = 0; i < Selection.gameObjects.Length; i++)
			{
				Vector3 recycledPosition = Selection.gameObjects[i].transform.position;
				Quaternion recycledRotation = Selection.gameObjects[i].transform.rotation;

				GameObject obj = PrefabUtility.ConnectGameObjectToPrefab(Selection.gameObjects[i], prefab);
				
				obj.transform.position = recycledPosition;
				obj.transform.rotation = recycledRotation;		
			}

			/*for(int i = 0; i < gameObjects.Length; i++)
			{

				PrefabUtility.ConnectGameObjectToPrefab(gameObjects[i], prefab);
			}*/
//#endif
			//StartCoroutine(SubstituteGameObjectsForPrefabAtEndOfFrame());
		}
	}

	private IEnumerator SubstituteGameObjectsForPrefabAtEndOfFrame()
	{
		yield return new WaitForEndOfFrame();

		for(int i = 0; i < gameObjects.Length; i++)
		{
			yield return new WaitForEndOfFrame();
			Instantiate(_prefab, gameObjects[i].transform.position, gameObjects[i].transform.rotation);
			gameObjects[i].DestroyOnEditor();
		}
	}
#endif
}
}