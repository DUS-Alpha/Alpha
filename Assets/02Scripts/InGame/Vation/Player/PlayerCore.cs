using System;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayerEquipmentManager))]
[RequireComponent(typeof(PlayerInventoryManager))]
[RequireComponent(typeof(PlayerAnimationController))]
[RequireComponent(typeof(CharacterController))]
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
    public PlayerInventoryManager InventorySystem;
    public PlayerEquipmentManager EquipmentManager;
    public event Action CheckInputAction;

    private void Awake()
    {
        InputHandler = GetComponent<PlayerInputHandler>();
        AniController = GetComponent<PlayerAnimationController>();
        Locomotion = GetComponent<PlayerLocomotion>();
        Combat = GetComponent<PlayerCombat>();
        PlayerCharacterController = GetComponent<CharacterController>();
        StateMachine = GetComponent<PlayerStateMachine>();
        InventorySystem = GetComponent<PlayerInventoryManager>();
        EquipmentManager = GetComponent<PlayerEquipmentManager>();
        Initialize();
    }

    public void Initialize()
    {
        Locomotion.Initialize(this);
        Combat.Initialize(this);
        StateMachine.Initialize(this);
        InventorySystem.Initialize(this);
    }

    void Start()
    {
        SwitchState(new PlayerIdleState(this));
    }

    public void SwitchState(PlayerState playerState)
    {
        StateMachine.SwitchState(playerState);
    }
    
    private void Update()
    {
        CheckInputAction?.Invoke();
        //Combat.SwapWeapon();
    }

}
