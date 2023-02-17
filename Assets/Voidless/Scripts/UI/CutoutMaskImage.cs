using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Voidless
{
public class CutoutMaskImage : Image
{
	private Material cutoutMaterial;

	public override Material materialForRendering
	{
		get
		{
			if(cutoutMaterial == null)
			{
				cutoutMaterial = new Material(base.materialForRendering);
				cutoutMaterial.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
			}

			return cutoutMaterial;
		}
	}
}
}