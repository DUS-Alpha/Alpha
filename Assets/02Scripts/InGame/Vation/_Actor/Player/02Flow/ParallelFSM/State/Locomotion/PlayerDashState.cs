using UnityEngine;

namespace alpha
{
    public class PlayerDashState : PlayerStateBase
    {
        public PlayerDashState(alpha.PlayerCore playerCore) : base(playerCore) { }

        public override void Enter()
        {
            base.Enter();
            m_LocomotionM.SettingsDash();
        }

        public override void Update()
        {
            // 애니메이션 종료 시점으로 판정해봤으나 이상하게 되어 시간으로 해결
            SetNextStateDelay(0.5f);
            
            if (!m_CanNextState) return;

            if (Time.time >= m_NextStateDelay)
                m_StateMachine.SwitchLocomotionState(LocomotionStateType.Idle);
        }

        public override void Exit()
        {

        }
    }
}