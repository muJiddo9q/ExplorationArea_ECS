using Unity.Entities;
using UnityEngine;

class PlayerAuthoring : MonoBehaviour
{
    public float speed = 5f;
    public float currentHp = 100f;
    public float maxHp = 100f;
}

class PlayerAuthoringBaker : Baker<PlayerAuthoring>
{
    public override void Bake(PlayerAuthoring authoring)
    {
        //player의 엔티티 가져오기
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);

        //player엔티티에 컴포넌트 추가
        AddComponent(entity, new PlayerComponent
        {
            speed = authoring.speed,
            currentHp = authoring.currentHp,
            maxHp = authoring.maxHp
        });

        //player태그 추가
        AddComponent<PlayerTag>(entity);
    }
}
