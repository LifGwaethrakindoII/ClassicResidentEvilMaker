using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Voidless
{
[CreateAssetMenu]
public class ScriptableLightMapData : ScriptableObject
{
	[SerializeField] private Texture2D _lightmapColor; 	/// <summary>Lightmap storing color of incoming light.</summary>
	[SerializeField] private Texture2D _lightmapDir; 	/// <summary>Lightmap storing dominant direction of incoming light.</summary>
	[SerializeField] private Texture2D _shadowMask; 	/// <summary>Texture storing occlussion mask per light [ShadowMask, up to four lights].</summary>

	/// <summary>Gets and Sets lightmapColor property.</summary>
	public Texture2D lightmapColor
	{
		get { return _lightmapColor; }
		set { _lightmapColor = value; }
	}

	/// <summary>Gets and Sets lightmapDir property.</summary>
	public Texture2D lightmapDir
	{
		get { return _lightmapDir; }
		set { _lightmapDir = value; }
	}

	/// <summary>Gets and Sets shadowMask property.</summary>
	public Texture2D shadowMask
	{
		get { return _shadowMask; }
		set { _shadowMask = value; }
	}

	/// <summary>Implicit ScriptableLightMapData to LightmapData operator.</summary>
  	public static implicit operator LightmapData(ScriptableLightMapData _asset)
  	{
  		LightmapData data = new LightmapData();

  		data.lightmapColor = _asset.lightmapColor;
  		data.lightmapDir = _asset.lightmapDir;
  		data.shadowMask = _asset.shadowMask;
  		
  		return data;
  	}

  	/// <returns>This asset turned into LightmapData.</returns>
  	public LightmapData ToLightmapData()
  	{
  		LightmapData data = new LightmapData();

  		data.lightmapColor = lightmapColor;
  		data.lightmapDir = lightmapDir;
  		data.shadowMask = shadowMask;
  		
  		return data;	
  	}

  	/// <returns>Texture's Size.</returns>
  	public int GetTextureSize()
  	{
  		return (int)Mathf.Sqrt(lightmapColor.GetPixels().Length);
  	}

  	[Button("Mark Textures as Readable")]
  	/// <summary>Marks Textures as Readable [Editor Only].</summary>
  	public void MarkTexturesAsReadable()
  	{
  		if(lightmapColor != null) lightmapColor.MarkTextureAsReadable();
  		if(lightmapDir != null) lightmapDir.MarkTextureAsReadable();
  		if(shadowMask != null) shadowMask.MarkTextureAsReadable();
  	}
}
}