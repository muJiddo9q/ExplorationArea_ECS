using Unity.Entities;

public struct ProjectileSpawnComponent : IComponentData
{
    public Entity projectilePrefab;
    public float fireInterval;
    public float nextFireTime;
    public int fireCount;
    public int projectileSpawnOffset;
    public int attackRange;
}
