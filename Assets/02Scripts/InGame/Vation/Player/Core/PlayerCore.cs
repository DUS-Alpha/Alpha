using alpha;
using System;
using UnityEngine;

[RequireComponent(typeof(PlayerAudioManager))]
[RequireComponent(typeof(PlayerGaugeManager))]
[RequireComponent(typeof(PlayerEquipManager))]
[RequireComponent(typeof(PlayerAnimationManager))]
[RequireComponent(typeof(PlayerLocomotionManager))]
[RequireComponent(typeof(PlayerCombatManager))]
public class PlayerCore : MonoBehaviour
{
    [Header(" [ Ref Component ] ")]
    public PlayerCameraManger CameraManager;
    public WorldAudioManager WorldAudioManager;
    public RealTimeUIManager RealTimeUIM;
    public InventoryUI InventoryUI;

    #region ==================== Main Module
    // 입력
    public PlayerInputManager InputManager { get; private set; }
    // 이동
    public PlayerLocomotionManager LocomotionM { get; private set; }
    // 전투
    public PlayerCombatManager Combat { get; private set; }
    // 상태 관리
    public PlayerStateMachineManager StateMachineM { get; private set; }
    // 애니메이션
    public PlayerAnimationManager AniManager { get; private set; }
    // 인벤토리
    public PlayerEquipManager EquipmentManager { get; private set; }
    // 오디오

    public PlayerAudioManager PlayerAudioManager { get; private set; }

    public EffectManager EffectManager { get; private set; }

    #endregion ==================== /Main Module

    public PlayerStateContext Context { get; private set; }
    public PlayerInventoryController InventoryController { get; private set; }
    
    public PlayerIKController IKController { get; private set; }
    public PlayerGaugeManager GaugeManager { get; private set; }

    public bool IsShowInventory { get; private set; }
    [HideInInspector] public bool IsPerformingAction;

    // TODO : LocomotionLock의 경우 Combat의 상태들에 따라 일부만 Lock처리하는 방식으로 변경 필요
    public bool IsCombatLock;
    public bool IsLocomotionLock;

    public event Func<bool> OnCanSwap;

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
        StateMachineM = new PlayerStateMachineManager();

        InventoryController = new PlayerInventoryController();
        
        AniManager = GetComponent<PlayerAnimationManager>();
        EquipmentManager = GetComponent<PlayerEquipManager>();
        LocomotionM = GetComponent<PlayerLocomotionManager>();
        Combat = GetComponent<PlayerCombatManager>();
        GaugeManager = GetComponent<PlayerGaugeManager>();
        
        IKController = GetComponentInChildren<PlayerIKController>();
        PlayerAudioManager = GetComponent<PlayerAudioManager>();
        EffectManager = GetComponentInChildren<EffectManager>();

        InitializeModule();
    }

    // TODO : 각 모듈의 경우 필요한 PlayerCore대신 필요한 모듈 매개변수만 받도록 수정
    // 이유 : PlayerCore를 통채로 넘기면 불필요한 것까지 받아 너무 큰단위의 메모리 공간 사용 발생
    private void InitializeModule()
    {
        StateMachineM.InitializeMoudle(this);
        LocomotionM.InitializeModule(this);
        Combat.InitializeModule(this);
        GaugeManager.InitializeModule(this);
        InventoryController.InitializeModule(EquipmentManager, InventoryUI);
    }

    private void Start()
    {
        SwitchLocomotionState(LocomotionStateType.Idle);
        SwitchCombatState(CombatStateType.NonCombat);
        InventoryController.Start();
    }

    private void Update()
    {
        StateMachineM.OnUpdate();
        LocomotionM.OnUpdate();
        RealTimeUIM.CurrentLocomotionState(StateMachineM.CurrentLocomotion.ToString());
        RealTimeUIM.CurrentCombatState(StateMachineM.CurrentCombat.ToString());
        //CheckInputAction?.Invoke();
    }
    
    public void SwitchLocomotionState(LocomotionStateType newState)
    {
        StateMachineM.SwitchLocomotionState(newState);
    }
    public void SwitchCombatState(CombatStateType newState)
    {
        StateMachineM.SwitchCombatState(newState);
    }

    // Locomotion과 Combat간의 Lock 동기화 처리
    public void SetLockState(bool isCombatLock, bool isLocomotionLock)
    {
        IsCombatLock = isCombatLock;
        IsLocomotionLock = isLocomotionLock;
    }

    // TODO : Save / Load 기능 구현 필요
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

}
