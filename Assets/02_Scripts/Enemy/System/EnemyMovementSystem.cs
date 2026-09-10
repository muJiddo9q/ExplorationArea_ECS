using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct EnemyMovementSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EnemyTag>();
        state.RequireForUpdate<PlayerTag>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // player entity 추출
        var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
        // player entity transform 추출
        var playerTransform = SystemAPI.GetComponentRO<LocalTransform>(playerEntity);
        
        var deltaTime = SystemAPI.Time.DeltaTime;

        foreach (var (enemyTransform, enemyComponent) in
            SystemAPI.Query<RefRW<LocalTransform>, RefRO<EnemyComponent>>())
        {
            // 이동방향 계산
            float3 direction = playerTransform.ValueRO.Position - enemyTransform.ValueRO.Position;
            if(math.lengthsq(direction) > 0.01f)
            {
                enemyTransform.ValueRW.Position += math.normalize(direction) * deltaTime * enemyComponent.ValueRO.speed;
            }

            // Facing 처리
            float yRotation = direction.x < 0 ? math.PI : 0f;
            if (math.abs(enemyTransform.ValueRO.Rotation.value.y - yRotation) > 0.01f)
            {
                enemyTransform.ValueRW.Rotation = quaternion.RotateY(yRotation);
            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
