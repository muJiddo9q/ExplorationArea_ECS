using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class ProjectileAuthoring : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float damage = 10f;
    public float lifeTime = 2f;
    public float passedTime = 0f;
    public float3 direction = new float3(0, 1, 0);
}

class ProjectileAuthoringBaker : Baker<ProjectileAuthoring>
{
    public override void Bake(ProjectileAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new ProjectileComponent
        {
            moveSpeed = authoring.moveSpeed,
            damage = authoring.damage,
            lifeTime = authoring.lifeTime,
            passedTime = authoring.passedTime,
            direction = authoring.direction
        });
    }
}
