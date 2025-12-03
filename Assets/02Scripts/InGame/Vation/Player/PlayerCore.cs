using alpha;
using System;
using UnityEngine;

[RequireComponent(typeof(PlayerAudioManager))]
[RequireComponent(typeof(PlayerInventoryController))]
[RequireComponent(typeof(PlayerGaugeManager))]
[RequireComponent(typeof(PlayerEquipManager))]
[RequireComponent(typeof(PlayerAnimationManager))]
[RequireComponent(typeof(PlayerLocomotion))]
[RequireComponent(typeof(PlayerCombat))]
public class PlayerCore : MonoBehaviour, IPlayerEvents
{
    [Header(" [ Ref Component ] ")]
    public PlayerCameraManger CameraManager;
    public WorldAudioManager AudioManager;
    public RealTimeUIManager RealTimeUIM;
    //public OpenCloseUIManager UIManager;

    public GameObject PlayerObj { get; private set; }
    public PlayerStateContext Context { get; private set; }
    public PlayerStateMachine StateMachine { get; private set; }
    
    public PlayerInputManager InputManager { get; private set; }
    public PlayerAnimationManager AniManager { get; private set; }
    public PlayerLocomotion Locomotion { get; private set; }
    public PlayerCombat Combat { get; private set; }
    public PlayerAudioManager playerAudioManager { get; private set;}
    public PlayerEquipManager EquipmentManager { get; private set; }
    public PlayerInventoryController InventoryController { get; private set; }


    public PlayerIKController IKController { get; private set; }
    public PlayerGaugeManager StatsManager { get; private set; }


    public InputLockedFlagsController<InputLocoLockType> LocomotionFlagsController { get; private set; } = new InputLockedFlagsController<InputLocoLockType>();
    public InputLockedFlagsController<InputCombatLockType> CombatFlagsController { get; private set; } = new InputLockedFlagsController<InputCombatLockType>();
    
    // IPlayerEvents에서 옵저버 패턴을 통해서 다른 클래스에서 받아옴
    public event Action CheckInputAction;

    public bool IsShowInventory { get; private set; }
    [HideInInspector] public bool IsPerformingAction;

    public bool IsCombatLock;

    private void OnEnable()
    {
        InputManager.OnEnable();
    }
    private void OnDisable()
    {
        InputManager.OnDisable();
    }

    private void Awake()
    {
        InputManager = new PlayerInputManager();
        Context = new PlayerStateContext();
        StateMachine = new PlayerStateMachine();
        
        AniManager = GetComponent<PlayerAnimationManager>();
        EquipmentManager = GetComponent<PlayerEquipManager>();
        Locomotion = GetComponent<PlayerLocomotion>();
        Combat = GetComponent<PlayerCombat>();
        StatsManager = GetComponent<PlayerGaugeManager>();
        InventoryController = GetComponent<PlayerInventoryController>();


        IKController = GetComponentInChildren<PlayerIKController>();
        playerAudioManager = GetComponent<PlayerAudioManager>();


        InitializeModule();
        InitializeEvents();
    }

    // TODO : 각 모듈의 경우 필요한 PlayerCore대신 필요한 모듈 매개변수만 받도록 수정
    // 이유 : PlayerCore를 통채로 넘기면 불필요한 것까지 받아 너무 큰단위의 메모리 공간 사용 발생
    private void InitializeModule()
    {
        StateMachine.InitializeMoudle(this);
        AniManager.InitializeModule(Combat);
        Locomotion.InitializeModule(this);
        Combat.InitializeModule(this);
        EquipmentManager.InitializeModule(this);
        StatsManager.InitializeModule(this);
        InventoryController.InitializeModule(InputManager, EquipmentManager, Combat);
    }

    /// <summary>
    /// 옵저버 패턴을 위한 전달(IPlayerEvents)
    /// </summary>
    public void InitializeEvents()
    {
        AniManager.InitializeEvents(this);
        InventoryController.InitializeEvents(this);
        
        Locomotion.InitializeEvents(this);
        Combat.InitializeEvents(this);
    }

    private void Start()
    {
        SwitchLocomotionState(LocomotionStateType.Idle);
        SwitchCombatState(CombatStateType.NonCombat);
    }


    private void Update()
    {
        InputManager.Update();
        StateMachine.Update();
        
        CheckInputAction?.Invoke();
    }
    
    public void SwitchLocomotionState(LocomotionStateType newState)
    {
        StateMachine.SwitchLocomotionState(newState);
    }
    public void SwitchCombatState(CombatStateType newState)
    {
        StateMachine.SwitchCombatState(newState);
    }

    public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        currentCharacterData.CharacterName = gameObject.name;
        
        currentCharacterData.YPos = transform.position.y;
        currentCharacterData.XPos = transform.position.x;
        currentCharacterData.ZPos = transform.position.z;
    }

    public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        Vector3 _myPos = new Vector3(currentCharacterData.XPos, currentCharacterData.YPos, currentCharacterData.ZPos);
        transform.position = _myPos;
    }

    public void UseItem(ItemDataSO data)
    {

    }
}
