using Unity.Entities;
using UnityEngine;

class EnemyAuthoring : MonoBehaviour
{
    public float speed = 2f;
}

class EnemyAuthoringBaker : Baker<EnemyAuthoring>
{
    public override void Bake(EnemyAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new EnemyComponent
        {
            speed = authoring.speed
        });

        AddComponent<EnemyTag>(entity);
    }
}
