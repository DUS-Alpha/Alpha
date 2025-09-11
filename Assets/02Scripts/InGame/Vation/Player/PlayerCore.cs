using System;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayerEquipmentController))]
[RequireComponent(typeof(PlayerInventoryManager))]
[RequireComponent(typeof(PlayerAnimationController))]
[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerStateMachine))]
[RequireComponent(typeof(PlayerLocomotion))]
[RequireComponent(typeof(PlayerCombat))]
public class PlayerCore : MonoBehaviour, IPlayerEvents
{
    public GameObject PlayerObj { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public PlayerAnimationController AniController { get; private set; }
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerLocomotion Locomotion { get; private set; }
    public PlayerCombat Combat { get; private set; }
    public PlayerInventoryManager InventoryManager { get; private set; }
    public PlayerEquipmentController EquipmentController { get; private set; }
    public CombatFlagsController CombatFlagsController { get; private set; } = new CombatFlagsController();

    [Header(" [ Ref Component ] ")]
    public PlayerCameraManger CameraManger;

    // IPlayerEvents에서 옵저버 패턴을 통해서 다른 클래스에서 받아옴
    public event Action CheckInputAction;
    public event Action<int> SwapWeaponAction;

    private void Awake()
    {
        InputHandler = GetComponent<PlayerInputHandler>();
        AniController = GetComponent<PlayerAnimationController>();
        Locomotion = GetComponent<PlayerLocomotion>();
        Combat = GetComponent<PlayerCombat>();
        StateMachine = GetComponent<PlayerStateMachine>();
        InventoryManager = GetComponent<PlayerInventoryManager>();
        EquipmentController = GetComponent<PlayerEquipmentController>();
        InitializeModule();
        InitializeEvents();
    }

    // TODO : 각 모듈의 경우 필요한 PlayerCore대신 필요한 모듈 매개변수만 받도록 수정
    // 이유 : PlayerCore를 통채로 넘기면 불필요한 것까지 받아 너무 큰단위의 메모리 공간 사용 발생
    private void InitializeModule()
    {
        StateMachine.InitializeMoudle(this);
        InventoryManager.InitializeModule(this);
        EquipmentController.InitializeModule();
        InputHandler.InitializeModule(StateMachine);
        Locomotion.InitializeModule(InputHandler, AniController);
        Combat.InitializeModule(InputHandler, CombatFlagsController, AniController);
    }

    /// <summary>
    /// 옵저버 패턴을 위한 전달(IPlayerEvents)
    /// </summary>
    public void InitializeEvents()
    {
        Locomotion.InitializeEvents(this);
        EquipmentController.InitializeEvents(this);
        AniController.InitializeEvents(this);
        Combat.InitializeEvents(this);
    }

    private void Start()
    {
        SwapWeaponAction(0);

        Combat.SetSwapAction(SwapWeaponAction);
    }

    public void SwitchLocomotionState(LocomotionStateType newState)
    {
        StateMachine.SwitchLocomotionState(newState);
    }
    public void SwitchCombatFullState(CombatFullStateType newState)
    {
        StateMachine.SwitchCombatFullState(newState);
    }

    private void Update()
    {
        CheckInputAction?.Invoke();
    }
}
