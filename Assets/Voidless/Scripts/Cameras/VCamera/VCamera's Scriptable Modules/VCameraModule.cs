using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*===========================================================================
**
** Class:  VCameraModule
**
** Purpose: ScriptableObject-derived classes that ought to work as modules
** for a VCamera.
**
**
** Author: Lîf Gwaethrakindo
**
===========================================================================*/
namespace Voidless
{
[CreateAssetMenu(menuName = PATH_ROOT)]
public abstract class VCameraModule : ScriptableObject
{
	public const string PATH_ROOT = "Voidless / Camera / Scriptable Modules /";

	/// <summary>Resets ScriptableObject.</summary>
	public virtual void Reset() { /*...*/ }

	/// <summary>Resets Module.</summary>
	public virtual void ResetModule() { /*...*/ }

	/// <summary>Draws Gizmos taking the VCamera's argument into account.</summary>
	/// <param name="_camera">VCamera's requesting to Draw Gizmos.</param>
	public virtual void DrawGizmos(VCamera _camera) { /*...*/ }

	/// <summary>Initializes this component taking the VCamera's argument into account.</summary>
	/// <param name="_camera">VCamera requesting for initialization.</param>
	public virtual void Initialize(VCamera _camera) { /*...*/ }

	/// <summary>Updates this component taking the VCamera's argument into account.</summary>
	/// <param name="_camera">VCamera requesting for an update.</param>
	public virtual void UpdateModule(VCamera _camera) { /*...*/ }
}
}