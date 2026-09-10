using Unity.Entities;

public struct PlayerComponent : IComponentData
{
    public float speed;
    public float currentHp;
    public float maxHp;
}

public struct PlayerTag : IComponentData
{
    // This struct is intentionally left empty to serve as a tag component.
}