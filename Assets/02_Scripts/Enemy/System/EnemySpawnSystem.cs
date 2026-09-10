using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct EnemySpawnSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerTag>();
        state.RequireForUpdate<EnemySpawnTag>();
        state.RequireForUpdate<EnemySpawnComponent>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // 한번말 실행하도록
        state.Enabled = false;

        // EntityCommandBuffer 생성
        var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);

        var enemySpawnComponent = SystemAPI.GetSingleton<EnemySpawnComponent>();
        
        // 랜덤생성기 초기화
        var random = new Random(enemySpawnComponent.randomSeed);

        using(var enemies = state.EntityManager.Instantiate(
            enemySpawnComponent.prefab, 
            enemySpawnComponent.numEnemies, 
            state.WorldUpdateAllocator
        ))
        {
            // 생성된 Enemy에 대한 초기화
            for (int i = 0; i < enemies.Length; i++)
            {
                var enemyEntity = enemies[i];

                // 랜덤한 위치 생성
                var angle = random.NextFloat(0, math.PI * 2f);
                var distance = random.NextFloat(10f, enemySpawnComponent.spawnRadius);
                var position = new float3(
                    math.cos(angle) * distance,
                    math.sin(angle) * distance,
                    0f);

                // Enemy에 컴포넌트 데이터 추가
                var enemyData = new EnemyComponent
                {
                    speed = random.NextFloat(enemySpawnComponent.minSpeed, enemySpawnComponent.maxSpeed)
                };

                entityCommandBuffer.AddComponent(enemyEntity, enemyData);

                entityCommandBuffer.SetComponent(enemyEntity, LocalTransform.FromPositionRotationScale(position, quaternion.identity, 2f));
                
            }
        }   
        
        entityCommandBuffer.Playback(state.EntityManager);
        entityCommandBuffer.Dispose();
    }
}
