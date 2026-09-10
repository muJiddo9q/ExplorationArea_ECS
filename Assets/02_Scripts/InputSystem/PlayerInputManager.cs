using Unity.Entities;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public struct PlayerInputComponent : IComponentData
{
    public float2 Movement;
    public bool IsMoving;
}

public class PlayerInputManager : MonoBehaviour
{
    private EntityManager entityManager;
    private Entity playerInputEntity;

    private InputSystem_Actions inputSystem;
    private InputAction moveAction;

    private void Awake()
    {
        inputSystem = new InputSystem_Actions();
        moveAction = inputSystem.Player.Move;
        moveAction.Enable();
    }

    private void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        //playerInputEntity = entityManager.CreateEntity();
        //entityManager.AddComponentData(playerInputEntity, new PlayerInputComponent());

        playerInputEntity = entityManager.CreateEntity(typeof(PlayerInputComponent));
    }

    private void OnEnable()
    {
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;
    }

    private void OnDisable()
    {
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        float h = context.ReadValue<Vector2>().x;
        float v = context.ReadValue<Vector2>().y;

        entityManager.SetComponentData(playerInputEntity, new PlayerInputComponent
        {
            Movement = new float2(h, v),
            IsMoving = context.phase == InputActionPhase.Performed
        });
    }

}
