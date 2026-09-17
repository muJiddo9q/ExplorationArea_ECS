using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct ProjectileSpawnSystem : ISystem
{
    // Player Entity 캐싱
    private Entity playerEntity;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerTag>();
        state.RequireForUpdate<ProjectileSpawnComponent>();

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (playerEntity == Entity.Null || !SystemAPI.Exists(playerEntity))
        {
            // player entity 추출
            playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
        }

        // ProjectileSpawnComponent 추출
        ref var projectileSpawnComponent = ref SystemAPI.GetSingletonRW<ProjectileSpawnComponent>().ValueRW;

        // 현재 시간 가져오기
        var currentTime = SystemAPI.Time.ElapsedTime;

        // 발사 간격 체크
        if (currentTime >= projectileSpawnComponent.nextFireTime)
        {
            var entityCommandBuffer = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            // 다음 발사시간 기록
            projectileSpawnComponent.nextFireTime = (float)currentTime + projectileSpawnComponent.fireInterval;

            //발사 각도 계산 (라디언)
            var angleStep = math.PI * 2f / projectileSpawnComponent.fireCount;

            // 플레이어 위치 추출
            var playerTransform = SystemAPI.GetComponent<LocalTransform>(playerEntity);

            // projectile prefab 추출
            var projectilePrefabData = SystemAPI.GetComponent<ProjectileComponent>(projectileSpawnComponent.projectilePrefab);

            // 발사 횟수만큼 반복해서 Projectile 생성
            for (int i = 0; i < projectileSpawnComponent.fireCount; i++)
            {
                // i번째 발사 각도 계산
                var angle = angleStep * i;

                // 발사좌표 계산 (x, y) = (cos(angle), sin(angle))
                float3 direction = new float3(math.cos(angle), math.sin(angle), 0f);

                // 플레이어 기준으로 발사위치 계산 (플레이어 + 발사좌표 * 최소 발사 반경)
                float3 spawnPosition = playerTransform.Position + direction * projectileSpawnComponent.projectileSpawnOffset;

                // 발사체 엔티티 생성
                Entity projectileEntity = entityCommandBuffer.Instantiate(projectileSpawnComponent.projectilePrefab);

                // 발사체 각도 계산
                quaternion projectileRotation = quaternion.RotateZ(angle - math.PIHALF);

                // 발사체 엔티티의 위치, 각도, 스케일 설정
                entityCommandBuffer.SetComponent(projectileEntity, new LocalTransform
                {
                    Position = spawnPosition,
                    Rotation = projectileRotation,
                    Scale = 1f
                });

                // 발사체 데이터 설정
                projectilePrefabData.direction = direction;
                projectilePrefabData.passedTime = 0f;
                entityCommandBuffer.SetComponent(projectileEntity, projectilePrefabData);


                //entityCommandBuffer.SetComponent(projectileEntity, new ProjectileComponent
                //{
                //    moveSpeed = 10f,
                //    damage = 10f,
                //    lifeTime = 2f,
                //    passedTime = 0f,
                //    direction = direction

                //});
            }
        }

    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
