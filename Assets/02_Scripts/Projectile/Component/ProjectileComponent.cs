using Unity.Entities;
using Unity.Mathematics;

public struct ProjectileComponent : IComponentData
{
    public float moveSpeed;
    public float damage;
    public float lifeTime;
    public float passedTime;
    public float3 direction;

}
