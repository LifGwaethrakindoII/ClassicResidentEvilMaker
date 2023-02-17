using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class AsynchronousBehaviorsManager : Singleton<AsynchronousBehaviorsManager>
{
	private List<Behavior> asynchronousBehaviors;

#region UnityMethods:
	/// <summary>AsynchronousBehaviorsManager's' instance initialization.</summary>
	void Awake()
	{
		if(Instance != this) Destroy(gameObject);
		else DontDestroyOnLoad(gameObject);

		if(asynchronousBehaviors == null) asynchronousBehaviors = new List<Behavior>();
	}

	/// <summary>AsynchronousBehaviorsManager's starting actions before 1st Update frame.</summary>
	void Start ()
	{
		
	}
	
	/// <summary>AsynchronousBehaviorsManager's tick at each frame.</summary>
	void Update ()
	{
		ManageAsynchronousBehaviors();
	}
#endregion

	private void ManageAsynchronousBehaviors()
	{
		for(int i = 0; i < asynchronousBehaviors.Count; i++)
		{
			Behavior asynchronousBehavior = asynchronousBehaviors[i];
			if(asynchronousBehavior != null)
			{
				switch(asynchronousBehavior.state)
				{
					case BehaviorState.Running:
					if(!asynchronousBehavior.monoBehaviourDependency)
					{
						if(asynchronousBehavior.monoBehaviour != null)
						asynchronousBehavior.MoveNext();
					}
					else asynchronousBehavior.MoveNext();
					break;

					case BehaviorState.Paused:
					break;

					case BehaviorState.Finished:
					asynchronousBehaviors.RemoveAt(i);
					break;
				}
			}
		}
	}
}
}