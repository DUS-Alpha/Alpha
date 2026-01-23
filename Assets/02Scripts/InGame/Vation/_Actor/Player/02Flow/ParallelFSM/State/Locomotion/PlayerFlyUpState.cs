using UnityEngine;

namespace alpha
{
    // 수직 이륙
    public class PlayerFlyUpState : PlayerStateBase
    {
        public PlayerFlyUpState(PlayerCore playerCore) : base(playerCore) { }

        public override void Enter()
        {
            base.Enter();
            m_LocomotionM.SettingsFlyUp();
        }

        public override void Update()
        {
            SetNextStateDelay(1.2f);

            if (!m_CanNextState) return;

            if (Time.time >= m_NextStateDelay)
            {
                bool isVelocityYZero = m_LocomotionM.CheckedVelocityY();

                if (isVelocityYZero)
                {
                    m_StateMachine.SwitchLocomotionState(LocomotionStateType.FlightMove);
                }
            }
        }

        public override void Exit()
        {
            
        }
    }
}