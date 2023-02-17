using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Voidless
{
[Serializable]
public class AudioClipAssetReference : VAssetReferenceT<AudioClip>
{
	/// <summary>AudioClipAssetReference default constructor.</summary>
	public AudioClipAssetReference(string guid) : base(guid) { /*...*/ }
}
}