using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
[ExecuteInEditMode]
public class ImageEffectShaderApplier : MonoBehaviour
{
	[SerializeField] private Material _material; 	/// <summary>Source Material.</summary>

	/// <summary>Gets material property.</summary>
	public Material material { get { return _material; } }

	/// <summary>Event function that Unity calls after a Camera has finished rendering, that allows you to modify the Camera's final image.</summary>
	/// <param name="source">A RenderTexture containing the source image.</param>
	/// <param name="destination">The RenderTexture to update with the modified image.</param>
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, material);
	}
}
}