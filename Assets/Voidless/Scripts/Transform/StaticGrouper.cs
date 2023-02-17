using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Voidless
{
[ExecuteInEditMode]
public class StaticGrouper : MonoBehaviour
{
#if UNITY_EDITOR
	private static readonly string[,] messages; 	/// <summary>Error messages for display logs.</summary>

	/// <summary>StaticGrouper's Static constructor.</summary>
	static StaticGrouper()
	{
		messages = new string[,]
		{
			{ "Error", "Do not modify this GameObject's Transform since it is treated as a group. Move child GameObjects instead", "Okay" },
			{ "You Are a Disobedient One, Aren't You?", "Why do you try to upset the order of things? Do you feel empowered by challenging the order? You are not so special...", "I know, but I have daddy issues" },
			{ "Whoa!", "Sonic says NO to some (all) minority rights and NO to moving GameObjects intended to be groups", "Chaos Control" },
			{ "Error (En Español)", "En caso de que seas de los extraños individuos que esta desarrollando juegos sin saber ingles: No come la taco en la trabajar y no se toca la GameObject con la componente", "Bien gracias y tu?" },
			{ "Hey VSauce, Michael Here!", "Are you trying to move this GameObject's Transform as a means to change the World space of the children? Why don't you instead displace all teh universe and not the displacer?", "What?" },
			{ "Congratulations!", "You don't follow orders. My ass-cheeks applaud in bewilderment", "Thank you" }
		};
	}

	/// <summary>Updates StaticGrouper's instance at each frame.</summary>
	private void Update()
	{
		if(gameObject.scene == null) return;
		EvaluateGrouper();
	}

	/// <summary>Evaluates this GameObject's Position.</summary>
	private void EvaluateGrouper()
	{
		if(transform.position != Vector3.zero || transform.rotation != Quaternion.identity || transform.localScale != Vector3.one)
		{
			int index = Random.Range(0, messages.GetLength(0));
			
			if(EditorUtility.DisplayDialog(messages[index, 0], messages[index, 1], messages[index, 2]))
			{
				transform.position = Vector3.zero;
				transform.rotation = Quaternion.identity;
				transform.localScale = Vector3.one;
			}
		}
	}

#endif
}
}