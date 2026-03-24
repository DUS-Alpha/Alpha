using alpha;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace alpha
{
    // Boundary
    [RequireComponent(typeof(TriggerInputBoundary))]
    [RequireComponent(typeof(PlayerEffectViewManager))]
    [RequireComponent(typeof(PlayerAnimationViewManager))]
    [RequireComponent(typeof(PlayerInputManager))]
    // Flow
    [RequireComponent(typeof(PlayerStateMachine))]
    // Module
    [RequireComponent(typeof(InventoryModule))]
    [RequireComponent(typeof(LocomotionModule))]
    [RequireComponent(typeof(CombatModule))]
    public class PlayerCore : MonoBehaviour, IInputActionPort // InputEvent 전달 Port
    {
        //외부로부터의 이벤트(객체로의 접근) 관리
        [Header("[ Boundary ]")]
        // 입력 이벤트
        [SerializeField] private PlayerInputManager m_inputManager;
        [SerializeField] private TriggerInputBoundary m_triggerInput;

        //흐름 제어 관리
        [Header("[ Flow ]")]
        public PlayerStateMachine StateMachine;

        //기능 및 실행 관리
        [Header("[ Module ]")]
        // 이동
        public LocomotionModule LocomotionModule;
        // 전투
        public CombatModule CombatModule;
        // 인벤토리
        public InventoryModule InventoryModule;

        // 규칙, 정책
        [Header("[ Rule & Policy ]")]
        private LocomotionRules m_locomotionRule;
        private CombatRules m_combatRule;
        public ActionPolicy ActionPolicy;

        // View
        [Header("[ View ]")]
        private PlayerAnimationViewManager m_aniViewManager;
        private PlayerEffectViewManager m_effectViewManager;

        public event Action<PlayerInputManager> OnInputAction;
        private void Awake()
        {
            m_locomotionRule = new LocomotionRules();
            m_combatRule = new CombatRules();
            ActionPolicy = new ActionPolicy(m_locomotionRule, m_combatRule);

            // Boundary
            m_inputManager = GetComponent<PlayerInputManager>();
            m_triggerInput = GetComponent<TriggerInputBoundary>();
            
            // Flow
            StateMachine = GetComponent<PlayerStateMachine>();

            // Module
            LocomotionModule = GetComponent<LocomotionModule>();
            CombatModule = GetComponent<CombatModule>();
            InventoryModule = GetComponent<InventoryModule>();
            
            // View
            m_aniViewManager = GetComponent<PlayerAnimationViewManager>();
            m_effectViewManager = GetComponent<PlayerEffectViewManager>();

            LocomotionModule.Bind(this, m_aniViewManager, m_effectViewManager);
            m_triggerInput.Bind(InventoryModule);
        }
        public void Bind(IInventoryViewPort inventoryViewPort)
        {
            InventoryModule.Bind(inventoryViewPort);
        }
        private void Start()
        {
            StateMachine.OnStart(this);
        }

        private void Update()
        {
            if(m_inputManager != null)
                OnInputAction?.Invoke(m_inputManager);
            
            StateMachine.OnUpdate();
        }
    }
}