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
                    enemyEntity.Index * 0.001f );// Z축으로 약간씩 이동

                // Enemy에 컴포넌트 데이터 추가 (랜덤한 속도설정)
                var enemyData = new EnemyComponent
                {
                    speed = random.NextFloat(enemySpawnComponent.minSpeed, enemySpawnComponent.maxSpeed)
                };

                entityCommandBuffer.AddComponent(enemyEntity, enemyData);

                entityCommandBuffer.SetComponent(enemyEntity, LocalTransform.FromPositionRotationScale(position, quaternion.identity, 2f));
                
                // 애니메이션 오프셋 값 설정
                var spawnTimeDate = new EnemySpawnTime
                {
                    value = random.NextFloat(0f, 10f)
                };
                entityCommandBuffer.AddComponent(enemyEntity, spawnTimeDate);
            }
        }   
        
        entityCommandBuffer.Playback(state.EntityManager);
        entityCommandBuffer.Dispose();
    }
}
