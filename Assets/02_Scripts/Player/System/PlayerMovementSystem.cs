using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct PlayerMovementSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        // 시스템이 시작될 때 1회 호출
        // OnUpdate가 실행될 조건 명시
        state.RequireForUpdate<PlayerTag>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // 입력 데이터 가져오기
        var inputData = SystemAPI.GetSingleton<PlayerInputComponent>();
        var deltaTime = SystemAPI.Time.DeltaTime;

        // 이동중이 아니면 종료
        if (!inputData.IsMoving) return;

        // 플레이어 엔티티, 컴포넌트 추출
        var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
        // ECS LocalTransform 컴포넌트 가져오기
        var playerTransform = SystemAPI.GetComponentRW<LocalTransform>(playerEntity);
        // 플레이어 컴포넌트 추출
        var playerComponent = SystemAPI.GetComponentRO<PlayerComponent>(playerEntity);

        // 이동 방향 벡터 계산
        float3 direction = new float3(inputData.Movement.x, inputData.Movement.y, 0f);

        // 이동처리
        if (math.lengthsq(direction) > 0.01f)
        {
            playerTransform.ValueRW.Position += math.normalize(direction) * deltaTime * playerComponent.ValueRO.speed;
        }

        // Facing 처리
        float yRotation = direction.x < 0 ? math.PI : 0f;

        if (math.abs(playerTransform.ValueRO.Rotation.value.y - yRotation) > 0.01f)
        {
            playerTransform.ValueRW.Rotation = quaternion.RotateY(yRotation);

        }

    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
