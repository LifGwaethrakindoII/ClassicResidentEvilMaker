using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public interface ITagsAffecter
{
	string[] affectedTags { get; set; } 	/// <summary>Affected Tags.</summary>
}
}