using UnityEngine;

namespace alpha
{
    public class PlayerJumpState : PlayerStateBase
    {
        public PlayerJumpState(PlayerCore playerCore) : base(playerCore) { }

        public override void Enter()
        {
            base.Enter();
            m_LocomotionM.SettingsJumpMovement();
        }

        public override void Update()
        {
            // 점프 중 Y 속도가 음수인지 체크하여 Fall로 상태 전환
            bool _isVelocityYZero = m_LocomotionM.CheckedVelocityY();

            // ====== 상태 전환 ======
            // 낙하 상태 전환
            if(_isVelocityYZero)
            {
                m_StateMachine.SwitchLocomotionState(LocomotionStateType.Fall);
                return;
            }
        }

        public override void Exit()
        {
           
        }
    }
}