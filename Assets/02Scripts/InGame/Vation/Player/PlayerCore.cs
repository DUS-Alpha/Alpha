using alpha;
using System;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayerInventoryController))]
[RequireComponent(typeof(PlayerStatsManager))]
[RequireComponent(typeof(PlayerEquipManager))]
[RequireComponent(typeof(PlayerAnimationManager))]
[RequireComponent(typeof(PlayerInputManager))]
[RequireComponent(typeof(PlayerLocomotion))]
[RequireComponent(typeof(PlayerCombat))]
public class PlayerCore : MonoBehaviour, IPlayerEvents
{
    [Header(" [ Ref Component ] ")]
    public PlayerCameraManger CameraM;
    public WorldAudioManager AudioManager;
    
    //public OpenCloseUIManager UIManager;

    public GameObject PlayerObj { get; private set; }
    public PlayerInputManager InputManager { get; private set; }
    public PlayerAnimationManager AniManager { get; private set; }
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerLocomotion Locomotion { get; private set; }
    public PlayerCombat Combat { get; private set; }
    public PlayerAudioController playerAudio { get; private set;}
    public PlayerEquipManager EquipmentManager { get; private set; }
    public PlayerIKController IKController { get; private set; }
    public PlayerStatsManager StatsManager { get; private set; }
    public PlayerInventoryController InventoryController { get; private set; }

    public InputLockedFlagsController<InputLocoLockType> LocomotionFlagsController { get; private set; } = new InputLockedFlagsController<InputLocoLockType>();
    public InputLockedFlagsController<InputCombatLockType> CombatFlagsController { get; private set; } = new InputLockedFlagsController<InputCombatLockType>();
    
    // IPlayerEvents에서 옵저버 패턴을 통해서 다른 클래스에서 받아옴
    public event Action CheckInputAction;

    public bool IsShowInventory { get; private set; }
    [HideInInspector] public bool IsPerformingAction;

    public bool IsCombatLock;

    private void Awake()
    {
        InputManager = GetComponent<PlayerInputManager>();
        AniManager = GetComponent<PlayerAnimationManager>();
        EquipmentManager = GetComponent<PlayerEquipManager>();
        Locomotion = GetComponent<PlayerLocomotion>();
        Combat = GetComponent<PlayerCombat>();
        StatsManager = GetComponent<PlayerStatsManager>();
        InventoryController = GetComponent<PlayerInventoryController>();

        IKController = GetComponentInChildren<PlayerIKController>();
        playerAudio = GetComponent<PlayerAudioController>();


        StateMachine = new PlayerStateMachine();
        InitializeModule();
        InitializeEvents();
    }

    // TODO : 각 모듈의 경우 필요한 PlayerCore대신 필요한 모듈 매개변수만 받도록 수정
    // 이유 : PlayerCore를 통채로 넘기면 불필요한 것까지 받아 너무 큰단위의 메모리 공간 사용 발생
    private void InitializeModule()
    {
        StateMachine.InitializeMoudle(this);
        AniManager.InitializeModule(Combat);
        InputManager.InitializeModule(Combat, LocomotionFlagsController, CombatFlagsController);
        Locomotion.InitializeModule(AniManager, CameraM,AudioManager);
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
        StateMachine.Update();
        CheckInputAction?.Invoke();

        //IsCombatLock = InputManager.IsRotLock || WindowUIManager.Instance.IsWindowUI;
        //playerAudio.FootStepAudio();
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
