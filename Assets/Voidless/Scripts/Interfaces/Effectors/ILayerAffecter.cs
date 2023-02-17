using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public interface ILayerAffecter
{
	LayerMask affectedLayer { get; set; } 	/// <summary>Affected LayerMasks.</summary>
}
}