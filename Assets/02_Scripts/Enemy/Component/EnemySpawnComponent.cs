using Unity.Entities;

public struct EnemySpawnComponent : IComponentData
{
    public Entity prefab;
    public int numEnemies;
    public float minSpeed;
    public float maxSpeed;
    public float spawnRadius; //스폰반경
    public uint randomSeed;
}


public struct EnemySpawnTag : IComponentData {}