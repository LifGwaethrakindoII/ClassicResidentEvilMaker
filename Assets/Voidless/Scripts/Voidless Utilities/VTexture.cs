using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Voidless
{
public static class VTexture
{
	/// <summary>Converts Sprite to Texture2D.</summary>
	/// <param name="_sprite">Sprite to copy pixels from.</param>
	/// <returns>Texture2D with Sprite's pixels.</returns>
	public static Texture2D ToTexture(this Sprite _sprite)
	{
		Texture2D newTexture = new Texture2D((int)_sprite.rect.width, (int)_sprite.rect.height);

		Color[] pixels = _sprite.texture.GetPixels
		( 
			(int)_sprite.textureRect.x, 
            (int)_sprite.textureRect.y, 
            (int)_sprite.textureRect.width, 
            (int)_sprite.textureRect.height
        );

        newTexture.SetPixels(pixels);
        newTexture.Apply();
        return newTexture;
	}

	/// <summary>Marks Texture as readable.</summary>
	/// <param name="_texture">Texture to modify.</param>
	/// <param name="_readable">Mark as Readable? true by default.</param>
	public static void MarkTextureAsReadable(this Texture2D _texture, bool isReadable = true)
	{
#if UNITY_EDITOR
	    if (null == _texture) return;

	    string path = AssetDatabase.GetAssetPath(_texture);
	    Debug.Log("[VTexture] Path: " + path);
	    TextureImporter importer = TextureImporter.GetAtPath(path) as TextureImporter;
	    
	    if(importer != null)
	    {
	    	importer.textureType = TextureImporterType.Default;
	    	importer.isReadable = isReadable;

	    	/*AssetDatabase.ImportAsset(path);
	    	importer.SaveAndReimport();*/
	    	AssetDatabase.Refresh();
	    }
#endif
	}

	/// <returns>Copy of this Texture's pixels into new Texture.</returns>
	public static Texture2D TextureCopy(this Texture2D _texture)
	{
		if(_texture == null) return null;

		_texture.MarkTextureAsReadable();

		int width = _texture.width;
		int height = _texture.width;
		int length = _texture.GetPixels().Length;
		Texture2D newTexture = new Texture2D(width / 2, height / 2);
		Color[] pixels = new Color[length];
		Color[] texturePixels = _texture.GetPixels();

		/*newTexture.alphaIsTransparency = _texture.alphaIsTransparency;
		newTexture.calculatedMipmapLevel = _texture.calculatedMipmapLevel;
		newTexture.desiredMipMapLevel= _texture.desiredMipMapLevel;
		newTexture.format = _texture.format;
		newTexture.loadedMipmapLevel = _texture.loadedMipmapLevel;
		newTexture.loadingMipmapLevel = _texture.loadingMipmapLevel;
		newTexture.minimumMipmapLevel = _texture.minimumMipmapLevel;
		newTexture.requestedMipmapLevel = _texture.requestedMipmapLevel;
		newTexture.streamingMipmaps = _texture.streamingMipmaps;
		newTexture.streamingMipmapsPriority = _texture.streamingMipmapsPriority;
		newTexture.vtOnly = _texture.vtOnly;*/


		for(int i = 0; i < length; i++)
		{
			pixels[i] = texturePixels[i];
		}

		newTexture.MarkTextureAsReadable();

		newTexture.SetPixels(pixels);
		newTexture.Apply();

		return newTexture;
	}

	/// <returns>Duplicated version of given Texture.</returns>
	public static Texture2D Duplicated(this Texture2D _texture)
	{
		if(_texture == null) return null;

		RenderTexture renderTex = RenderTexture.GetTemporary(
			_texture.width,
			_texture.height,
			0,
			RenderTextureFormat.Default,
			RenderTextureReadWrite.Linear
		);

		Graphics.Blit(_texture, renderTex);
		RenderTexture previous = RenderTexture.active;
		RenderTexture.active = renderTex;
		Texture2D newTexture = new Texture2D(_texture.width, _texture.height);

		/*newTexture.alphaIsTransparency = _texture.alphaIsTransparency;
		newTexture.calculatedMipmapLevel = _texture.calculatedMipmapLevel;
		newTexture.desiredMipMapLevel= _texture.desiredMipMapLevel;
		newTexture.format = _texture.format;
		newTexture.loadedMipmapLevel = _texture.loadedMipmapLevel;
		newTexture.loadingMipmapLevel = _texture.loadingMipmapLevel;
		newTexture.minimumMipmapLevel = _texture.minimumMipmapLevel;
		newTexture.requestedMipmapLevel = _texture.requestedMipmapLevel;
		newTexture.streamingMipmaps = _texture.streamingMipmaps;
		newTexture.streamingMipmapsPriority = _texture.streamingMipmapsPriority;
		newTexture.vtOnly = _texture.vtOnly;*/

		newTexture.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
		newTexture.Apply();
		RenderTexture.active = previous;
		RenderTexture.ReleaseTemporary(renderTex);

		return newTexture;

		/*Texture2D newTexture = new Texture2D(_texture.width, _texture.height, _texture.format, _texture.mipmapCount > 1);
    	newTexture.LoadRawTextureData(_texture.GetRawTextureData());
    	newTexture.Apply();

    	return newTexture;*/
	}

	/// <summary>Interpolates 2 Texture2Ds.</summary>
	/// <param name="a">Texture A.</param>
	/// <param name="b">Texture B.</param>
	/// <param name="duration">Interpolation Duration.</param>
	/// <param name="w">Width.</param>
	/// <param name="h">Height.</param>
	/// <param name="onInterpolationEnds">Optional Callback invoked when the interpolation ends.</param>
	/// <param name="f">Optional Normalized Time t function.</param>
	public static IEnumerator<Texture2D> InterpolateToTexture2D(this Texture2D a, Texture2D b, float duration, Action onInterpolationEnds = null, Func<float, float> f = null)
	{
		int lengthA = a.GetPixels().Length;
		int lengthB = b.GetPixels().Length;

		if(lengthA != lengthB) yield break;

		Texture2D texture = new Texture2D(a.width, a.height);
		Color[] pixelsA = a.GetPixels();
		Color[] pixelsB = b.GetPixels();
		Color[] pixels = new Color[lengthA];
		float t = 0.0f;
		float inverseDuration = 1.0f / duration;

		if(f == null) f = VMath.DefaultNormalizedPropertyFunction;

		yield return a;

		while(t < 1.0f)
		{
			for(int i = 0; i < lengthA; i++)
			{
				pixels[i] = Color.Lerp(pixelsA[i], pixelsB[i], f(t));
			}

			texture.SetPixels(pixels);
			texture.Apply();

			t += (Time.deltaTime * inverseDuration);
			yield return texture;
		}

		texture.Apply();

		yield return b;
	}										
}
}