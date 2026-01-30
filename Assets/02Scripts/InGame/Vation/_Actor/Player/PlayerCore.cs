using alpha;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace alpha
{
    // 외부 Input
    [RequireComponent(typeof(TriggerInputBoundary))]
    [RequireComponent(typeof(PlayerEffectViewManager))]
    [RequireComponent(typeof(PlayerAnimationViewManager))]
    [RequireComponent(typeof(PlayerInputManager))]
    // Module
    [RequireComponent(typeof(InventoryModule))]
    [RequireComponent(typeof(LocomotionModule))]
    [RequireComponent(typeof(CombatModule))]
    public class PlayerCore : MonoBehaviour, IInputActionPort // InputEvent 전달 Port
    {
        // 외부 Input (Self Binding) 객체가 동적 생성이 아닐경우 원래는 Installer에서 주입
        private PlayerInputManager m_inputManager;
        private TriggerInputBoundary m_triggerInput;

        // 내부 OutputAdapter (행위)
        public LocomotionModule LocomotionM;
        public CombatModule CombatM;
        public InventoryModule InventoryM;

        // State
        public PlayerStateMachine StateMachine { get; private set; }

        // Rules
        private LocomotionRules m_locomotionRule;
        private CombatRules m_combatRule;
        public ActionPolicy ActionPolicy;

        // View
        private PlayerAnimationViewManager m_aniViewManager;
        private PlayerEffectViewManager m_effectViewManager;

        public event Action<PlayerInputManager> OnInputAction;
        private void Awake()
        {
            m_locomotionRule = new LocomotionRules();
            m_combatRule = new CombatRules();
            ActionPolicy = new ActionPolicy(m_locomotionRule, m_combatRule);
            StateMachine = new PlayerStateMachine();

            // 외부
            m_inputManager = GetComponent<PlayerInputManager>();
            m_triggerInput = GetComponent<TriggerInputBoundary>();
            
            // 내부
            LocomotionM = GetComponent<LocomotionModule>();
            CombatM = GetComponent<CombatModule>();
            InventoryM = GetComponent<InventoryModule>();
            
            // View
            m_aniViewManager = GetComponent<PlayerAnimationViewManager>();
            m_effectViewManager = GetComponent<PlayerEffectViewManager>();

            LocomotionM.Bind(this, m_aniViewManager, m_effectViewManager);
            m_triggerInput.Bind(InventoryM);
        }
        public void Bind(IInventoryViewPort inventoryViewPort)
        {
            InventoryM.Bind(inventoryViewPort);
        }
        private void Start()
        {
            // 생성자에서 바로 this 넘기면 Awake 전에 호출되어 null 참조 발생
            // 즉 상태는 만들어졌는데 this는 아직 안넘어가져 있어서 코드상 에러 발생되기에 Start에서 호출
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