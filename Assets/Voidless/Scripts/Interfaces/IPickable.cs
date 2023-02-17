using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public enum PickableState
{
	Unpicked, 												/// <summary>Unpicked State.</summary>
	Picked, 												/// <summary>Picked State.</summary>
	Dropped 												/// <summary>Dropped State.</summary>
}

public interface IPickable: IFiniteStateMachine<PickableState>
{
	//void OnPickRequest();
}
}