using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public interface ICameraVisibleHandler
{
	bool seenByCamera { get; set; } 	/// <summary>Is this object already seen by the Camera?.</summary>
}
}