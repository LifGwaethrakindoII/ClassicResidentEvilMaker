using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;
using Sirenix.OdinInspector;

public class TEST_GameObjectPooling : MonoBehaviour
{
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private Transform effectSpawnPoint;
    [SerializeField] private GameObjectPool<VProjectile> projectilesPool;
    [SerializeField] private GameObjectPool<ParticleEffect> effectsPool;

    /// <summary>TEST_GameObjectPooling's instance initialization when loaded [Before scene loads].</summary>
    private void Awake()
    {
        projectilesPool.Initialize();
        effectsPool.Initialize();
    }

    [Button("Spawn Projectile")]
    /// <summary>Spawns Projectile.</summary>
    private void RecycleProjectile()
    {
        if(Application.isPlaying)
        projectilesPool.Recycle(projectileSpawnPoint.position, projectileSpawnPoint.rotation);
    }

    [Button("Spawn Particle-Effect")]
    /// <summary>Spawns Particle-Effect.</summary>
    private void RecycleParticleEffect()
    {
        if(Application.isPlaying)
        effectsPool.Recycle(effectSpawnPoint.position, effectSpawnPoint.rotation);
    }
}