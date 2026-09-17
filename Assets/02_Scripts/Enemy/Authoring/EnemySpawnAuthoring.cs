using Unity.Entities;
using UnityEngine;

class EnemySpawnAuthoring : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int enemyCount = 30000;
    public float minSpeed = 0.2f;
    public float maxSpeed = 1.0f;
    public float minSpawnRadius = 10f;
    public float spawnRadius = 50f;
    public float stopDistance = 2.0f;
    public uint randomSeed = 12345;
}

class EnemySpawnAuthoringBaker : Baker<EnemySpawnAuthoring>
{
    public override void Bake(EnemySpawnAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);

        AddComponent(entity, new EnemySpawnComponent{
           prefab = GetEntity(authoring.enemyPrefab, TransformUsageFlags.Dynamic),
           numEnemies = authoring.enemyCount,
           minSpeed = authoring.minSpeed,
           maxSpeed = authoring.maxSpeed,
           minSpawnRadius = authoring.minSpawnRadius,
           spawnRadius = authoring.spawnRadius,
           stopDistance = authoring.stopDistance,
           randomSeed = authoring.randomSeed

        });

        AddComponent<EnemySpawnTag>(entity);

    }
}
