using alpha;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace alpha
{
    [RequireComponent(typeof(PlayerEffectViewManager))]
    [RequireComponent(typeof(PlayerAnimationViewManager))]
    [RequireComponent(typeof(PlayerInputManager))]
    [RequireComponent(typeof(CombatModule))]
    [RequireComponent(typeof(LocomotionModule))]
    // InputAdapter어댑터 역할
    public class PlayerCore : MonoBehaviour, IInputActionPort // InputEvent 전달 Port
    {
        // 외부 Input (Self Binding) 객체가 동적 생성이 아닐경우 원래는 Installer에서 주입
        private PlayerInputManager m_inputManager;
        public event Action<PlayerInputManager> OnInputAction;

        // 내부 OutputAdapter (행위)
        public LocomotionModule LocomotionM;
        public CombatModule CombatM;

        // State
        public PlayerStateMachine StateMachine { get; private set; }

        // Rules
        private LocomotionRules m_locomotionRule;
        private CombatRules m_combatRule;
        public ActionPolicy ActionPolicy;

        // View
        private PlayerAnimationViewManager m_aniViewM;
        private PlayerEffectViewManager m_effectViewM;

        private void Awake()
        {
            m_locomotionRule = new LocomotionRules();
            m_combatRule = new CombatRules();
            ActionPolicy = new ActionPolicy(m_locomotionRule, m_combatRule);
            StateMachine = new PlayerStateMachine();

            LocomotionM = GetComponent<LocomotionModule>();
            CombatM = GetComponent<CombatModule>();
            m_inputManager = GetComponent<PlayerInputManager>();
            m_aniViewM = GetComponent<PlayerAnimationViewManager>();
            m_effectViewM = GetComponent<PlayerEffectViewManager>();

            LocomotionM.BInd(this, m_aniViewM, m_effectViewM);
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