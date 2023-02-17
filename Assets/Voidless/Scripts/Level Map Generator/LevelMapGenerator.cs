using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Voidless
{
public enum OffsetFrom
{
	Origin,
	TextureCoordinates
}

//
public class LevelMapGenerator : MonoBehaviour
{
	private const string GROUP_TAG = "Group_"; 								/// <summary>Group's Tag.</summary>

	[HideInInspector] public List<GameObject> _groupParents; 				/// <summary>Stores the Prefab's group parents for bot Remove and Clear methods.</summary>
	[HideInInspector] public List<GameObject> _mapPrefabs;
	[SerializeField] private LevelMapData data; 							/// <summary>Level Map Data.</summary>
	//[SerializeField] private FloorTerrain floorTerrain; 					/// <summary>Floor Terrain.</summary>
	[SerializeField] private Axes3D ignore; 							/// <summary>Coordinate Axis to ignore.</summary>
	[SerializeField] private OffsetFrom offsetFrom; 						/// <summary>Offset from either Origin [0,0,0] or Texture's Coordinates [Width / 2, Height / 2].</summary>
	[SerializeField] private bool debugUnmatchingColors; 					/// <summary>Debug Colors that unmatch the map dictionary keys color keys?.</summary>

	/// <summary>Gets and Sets groupParents property.</summary>
	public List<GameObject> groupParents
	{
		get { return _groupParents; }
		private set { _groupParents = value; }
	}

	/// <summary>Generates Tiles given a collection of textures to interpret.</summary>
	public void GenerateMap()
	{
		//floorTerrain.AdaptToDimensions(new Vector3(data.mapInformation.width, data.mapInformation.height, 1f));
		/// O(ixy), Edu approves.

		/// Stores a record of the prefab's instances on this method's scope so they can be grouped in a common parent.
		Dictionary<int, GameObject> parentsRegistry = new Dictionary<int, GameObject>();
		int id = 0; 								/// Prefab's ID as a unique key to the Dictionary.
		GameObject newObject; 						/// Stores the instantiated Prefab.
		Vector3 newPosition; 						/// Instantiated Prefab's new position.
		Vector3 accumulatedOffset = Vector3.zero; 	/// Accumulated offset of all LevelMapPlanes.
		Vector3 originAnchor; 						/// Origin anchor.

		for(int i = 0; i < data.levelPlanes.Length; i++)
		{
			accumulatedOffset += data.levelPlanes[i].levelOffset;
			originAnchor = (offsetFrom == OffsetFrom.Origin ?
				Vector3.zero :
				ignore == Axes3D.Y ?
					new Vector3(-(data.levelPlanes[i].mapInformation.width * 0.5f), 0f, -(data.levelPlanes[i].mapInformation.height * 0.5f)) :
					new Vector3(-(data.levelPlanes[i].mapInformation.width * 0.5f), -(data.levelPlanes[i].mapInformation.height * 0.5f), 0f));

			for(int x = 0; x < data.levelPlanes[i].mapInformation.width; x++)
			{
				for(int y = 0; y < data.levelPlanes[i].mapInformation.height; y++)
				{
					Color pixelColor = data.levelPlanes[i].mapInformation.GetPixel(x, y);
					//Vector3 originAnchor = floorTerrain.GetOriginAnchor();

					for(int j = 0; j < data.levelMapDictionary.Length; j++)
					{
						if(data.levelMapDictionary[j].key.Equals(pixelColor))
						{
							id = data.levelMapDictionary[j].value.GetInstanceID();
							newPosition = (originAnchor + (ignore == Axes3D.Y ? new Vector3(x, 0f, y) : new Vector3(x, y, 0f)) + accumulatedOffset);
							newObject = Instantiate(data.levelMapDictionary[j].value, newPosition, Quaternion.identity) as GameObject;
								
							//if(mapPrefabs == null) mapPrefabs = new Dictionary<int, List<GameObject>>();
							//if(parentsRegistry == null) parentsRegistry = new Dictionary<int, GameObject>();

							//if(!mapPrefabs.ContainsKey(id)) mapPrefabs.Add(id, new List<GameObject>());
							if(!parentsRegistry.ContainsKey(id)) parentsRegistry.Add(id, new GameObject(GROUP_TAG + data.levelMapDictionary[j].value.name + id));

							newObject.transform.parent = parentsRegistry[id].transform;
							//mapPrefabs[id].Add(newObject);
						}else if(debugUnmatchingColors)
						{	
							Debug.Log("[LevelMapGenerator] Picked Color not registered on dictionary: " + pixelColor +
									"\n[LevelMapGenerator] Map Data of index [" + i + "] Color: " + data.levelMapDictionary[j].key);
						}
					}
				}	
			}
		}	
	}

	/// <summary>Resets prefabs instantiated by this Object.</summary>
	public void Reset()
	{
		if(groupParents != null)
		for(int i = 0; i < groupParents.Count; i++)
		{
			/// \TODO Overload KillAllChilds() method that extends GameObject, and maybe MonoBehaviour.
			groupParents[i].transform.KillAllChilds();
			Destroy(groupParents[i]);
		}	
	}
}
}