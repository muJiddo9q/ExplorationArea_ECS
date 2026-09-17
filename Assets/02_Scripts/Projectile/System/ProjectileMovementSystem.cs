using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

partial struct ProjectileMovementSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<ProjectileComponent>();
        state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // EntityCommandBuffer 생성
        var entityCommandBuffer = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged);
        var deltaTime = SystemAPI.Time.DeltaTime;
        foreach (var (procjectileComponent, velocity, entity) in
            SystemAPI.Query<RefRW<ProjectileComponent>, RefRW<PhysicsVelocity>>().WithEntityAccess())
        {
            velocity.ValueRW.Linear = procjectileComponent.ValueRO.direction * procjectileComponent.ValueRO.moveSpeed;
            velocity.ValueRW.Angular = float3.zero;

            // 생존시간 체크
            procjectileComponent.ValueRW.passedTime += deltaTime;
            if (procjectileComponent.ValueRO.passedTime >= procjectileComponent.ValueRO.lifeTime)
            {
                // 화살 삭제
                entityCommandBuffer.DestroyEntity(entity);
            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
