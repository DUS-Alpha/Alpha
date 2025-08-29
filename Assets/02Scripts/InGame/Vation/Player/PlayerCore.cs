using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayerAnimationController))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInventorySystem))]
[RequireComponent(typeof(PlayerEquipmentSystem))]
[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerStateMachine))]
[RequireComponent(typeof(PlayerLocomotion))]
[RequireComponent(typeof(PlayerCombat))]
public class PlayerCore : MonoBehaviour
{
    [Header(" [ Ref Component ] ")]
    public GameObject player;
    public PlayerInputHandler InputHandler;
    public PlayerAnimationController AniController;
    public PlayerStateMachine StateMachine;
    public PlayerLocomotion Locomotion;
    public PlayerCombat Combat;
    public CharacterController PlayerCharacterController;
    public PlayerEquipmentSystem EquipmentSystem;
    public PlayerInventorySystem InventorySystem;
    public bool isAction = false;

    private void Awake()
    {
        InputHandler = GetComponent<PlayerInputHandler>();
        AniController = GetComponent<PlayerAnimationController>();
        Locomotion = GetComponent<PlayerLocomotion>();
        Combat = GetComponent<PlayerCombat>();
        PlayerCharacterController = GetComponent<CharacterController>();
        StateMachine = GetComponent<PlayerStateMachine>();
        EquipmentSystem = GetComponent<PlayerEquipmentSystem>();
        InventorySystem = GetComponent<PlayerInventorySystem>();
        Initialize();
    }

    public void Initialize()
    {
        Locomotion.Initialize(this);
        Combat.Initialize(this);
        StateMachine.Initialize(this);
    }

    void Start()
    {
        SwitchState(new PlayerIdleState(this));
    }

    public void SwitchState(PlayerState playerState)
    {
        StateMachine.SwitchState(playerState);
    }

}
