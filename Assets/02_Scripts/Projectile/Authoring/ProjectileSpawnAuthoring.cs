using Unity.Entities;
using UnityEngine;

class ProjectileSpawnAuthoring : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float fireInterval = 0.5f;
    public float nextFireTime = 0f;
    public int fireCount = 10;
    public int projectileSpawnOffset = 1;
}

class ProjectileSpawnAuthoringBaker : Baker<ProjectileSpawnAuthoring>
{
    public override void Bake(ProjectileSpawnAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);

        AddComponent(entity, new ProjectileSpawnComponent
        {
            projectilePrefab = GetEntity(authoring.projectilePrefab, TransformUsageFlags.Dynamic),
            fireInterval = authoring.fireInterval,
            fireCount = authoring.fireCount,
            nextFireTime = authoring.nextFireTime,
            projectileSpawnOffset = authoring.projectileSpawnOffset
        });
    }
}
