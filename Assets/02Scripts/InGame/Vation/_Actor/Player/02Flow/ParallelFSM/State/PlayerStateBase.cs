using UnityEngine;

namespace alpha
{
    // 상태는 판정하고 상태전이만 할 것
    public abstract class PlayerStateBase
    {
        protected PlayerCore m_Core;
        protected PlayerStateMachine m_StateMachine;
        protected LocomotionModule m_LocomotionM;
        protected CombatModule m_CombatM;
        protected ActionPolicy m_ActionPolicy;

        protected bool m_CanNextState;
        protected float m_NextStateDelay;
        public PlayerStateBase(PlayerCore playerCore)
        {
            m_Core = playerCore;
            m_StateMachine = m_Core.StateMachine;
            m_LocomotionM = m_Core.LocomotionM;
            m_CombatM = m_Core.CombatM;
            m_ActionPolicy = m_Core.ActionPolicy;
        }

        public void DebugCurrentState()
        {
            Debug.Log(GetType().Name);
        }

        public virtual void Enter()
        {
            m_CanNextState = false;
            m_NextStateDelay = 0f;
            DebugCurrentState();
        }
        public abstract void Update();
        public abstract void Exit();

        public void SetNextStateDelay(float delay)
        {
            m_CanNextState = true;
            // 지연시간 설정
            if (m_NextStateDelay == 0)
                m_NextStateDelay = Time.time + delay;
        }
    }
}