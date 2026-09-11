using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct EnemyMovementSystem : ISystem
{
    // player entity 캐싱
    private Entity playerEntity;
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EnemyTag>();
        state.RequireForUpdate<PlayerTag>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if(playerEntity == Entity.Null || !SystemAPI.Exists(playerEntity))
        {
            // player entity 추출
            playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
        }

        //player entity 포지션값 추출

        var playerPosition = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position;
        
        // Job 병렬처리

        state.Dependency = new EnemyMovementJob
        {
            deltaTime = SystemAPI.Time.DeltaTime,
            playerPosition = playerPosition
        }.ScheduleParallel(state.Dependency);

        //// player entity transform 추출
        //var playerTransform = SystemAPI.GetComponentRO<LocalTransform>(playerEntity);

        //var deltaTime = SystemAPI.Time.DeltaTime;

        //foreach (var (enemyTransform, enemyComponent) in
        //    SystemAPI.Query<RefRW<LocalTransform>, RefRO<EnemyComponent>>())
        //{
        //    // 이동방향 계산
        //    float3 direction = playerTransform.ValueRO.Position - enemyTransform.ValueRO.Position;
        //    if(math.lengthsq(direction) > 0.01f)
        //    {
        //        enemyTransform.ValueRW.Position += math.normalize(direction) * deltaTime * enemyComponent.ValueRO.speed;
        //    }

        //    // Facing 처리
        //    float yRotation = direction.x < 0 ? math.PI : 0f;
        //    if (math.abs(enemyTransform.ValueRO.Rotation.value.y - yRotation) > 0.01f)
        //    {
        //        enemyTransform.ValueRW.Rotation = quaternion.RotateY(yRotation);
        //    }
        //}
    }

    
}
// Enemy 이동처리 Job
// IJobEntity : Entity를 병렬처리하는 고성능 Job
[BurstCompile]
public partial struct EnemyMovementJob : IJobEntity
{
    public float3 playerPosition;
    public float deltaTime;
    public void Execute(ref EnemyComponent enemyComponent, ref LocalTransform enemyTransform, Entity enemyEntity)
    {
        // 먼 Entity는 업데이트 빈도 50% 감소
        float distanceToplayerSq = math.distancesq(enemyTransform.Position, playerPosition);
        if (distanceToplayerSq > 100f && enemyEntity.Index % 2 == 0)
        {
            return;
        }

        // Player 방향의 벡터 계산
        var direction = math.normalize(playerPosition - enemyTransform.Position);

        // Enemy 이동
        enemyTransform.Position += direction * deltaTime * enemyComponent.speed;

        // Facing 처리
        float yRotation = direction.x < 0 ? math.PI : 0f;
        if (math.abs(enemyTransform.Rotation.value.y - yRotation) > 0.01f)
        {
            enemyTransform.Rotation = quaternion.RotateY(yRotation);
        }

    }
}