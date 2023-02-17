using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[CreateAssetMenu(menuName = VString.PATH_SCRIPTABLE_OBJECTS + " / Grid Level Map Data")]
public class LevelMapData : ScriptableObject
{

#region Properties:
	[SerializeField] private LevelMapPair[] _levelMapDictionary; 	/// <summary>Level Map's Dictionary.</summary>
	[Space(5f)]
	[SerializeField] private LevelMapPlane[] _levelPlanes; 			/// <summary>Level Map's levels.</summary>
#endregion

#region Getters:
	/// <summary>Gets levelPlanes property.</summary>
	public LevelMapPlane[] levelPlanes { get { return _levelPlanes; } }

	/// <summary>Gets levelMapDictionary property.</summary>
	public LevelMapPair[] levelMapDictionary { get { return _levelMapDictionary; } }
#endregion

}

[System.Serializable]
public class LevelMapPlane
{
	[SerializeField] private Texture2D _mapInformation; 			/// <summary>Level Map's information.</summary>
	[Space(5f)]	
	[SerializeField] private Vector3 _levelOffset; 					/// <summary>Offset from previous level [from reference point if it is the first level].</summary>
	[Space(5f)]
	[SerializeField] private bool _ignoreAlpha; 					/// <summary>Does the generator ignore Alpha information?.</summary>

	/// <summary>Gets mapInformation property.</summary>
	public Texture2D mapInformation { get { return _mapInformation; } }

	/// <summary>Gets levelOffset property.</summary>
	public Vector3 levelOffset { get { return _levelOffset; } }

	/// <summary>Gets ignoreAlpha property.</summary>
	public bool ignoreAlpha { get { return _ignoreAlpha; } }
	
}

[System.Serializable]
public class LevelMapPair : SerializableKeyValuePair<Color, GameObject>{ /*...*/ }
}