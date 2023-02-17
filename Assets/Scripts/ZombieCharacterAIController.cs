using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class ZombieCharacterAIController : CharacterAIController<ZombieCharacter>
{
    [SerializeField] private Character target;     /// <summary>Target's reference.</summary>

    /// <summary>Draws Gizmos on Editor mode.</summary>
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }

    /// <summary>Updates CharacterAIController at each time step.</summary>
    protected override void Update()
    {
        base.Update();

        if(currentPath == null) return;

        if(pathIteration != null && pathIteration.MoveNext())
        {
            Vector3 direction = pathIteration.Current - character.transform.position;
            direction.y = 0.0f;
            character.MoveTowards(direction);
        }
    }

    /// <summary>Callback internally invoked when the AI ought to be updated.</summary>
    protected override void OnAIUpdate()
    {
        base.OnAIUpdate();

        if(character == null || target == null) return;

        bool result = GetPath(target.transform.position);
    }
}
}