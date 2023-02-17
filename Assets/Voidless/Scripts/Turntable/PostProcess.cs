using UnityEngine;

namespace Voidless
{
[ExecuteInEditMode]
public class PostProcess : MonoBehaviour
{
	[SerializeField] private Material _material; 	/// <summary>Camera's Material.</summary>

	/// <summary>Gets and Sets material property.</summary>
	public Material material
	{
		get { return _material; }
		set { _material = value; }
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
	  	Graphics.Blit(source, destination, material);
	}
}
}

///Resolution 320 x 240