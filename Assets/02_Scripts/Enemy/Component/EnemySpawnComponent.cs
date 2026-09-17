using Unity.Entities;

public struct EnemySpawnComponent : IComponentData
{
    public Entity prefab;
    public int numEnemies;
    public float minSpeed;
    public float maxSpeed;
    public float minSpawnRadius;
    public float spawnRadius; //스폰반경
    public float stopDistance;
    public uint randomSeed;
}


public struct EnemySpawnTag : IComponentData {}