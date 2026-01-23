using UnityEngine;

namespace alpha
{
    public enum EFallType
    {
        NormalFall,
        FlyFall
    }

    public class PlayerFallState : PlayerStateBase
    {
        public PlayerFallState(PlayerCore playerCore) : base(playerCore) { }

        public override void Enter()
        {
            base.Enter();
            m_LocomotionM.SettingsFallMovement();
        }

        public override void Update()
        {
            bool _isGrounded = m_LocomotionM.IsGrounded;
            bool _isFly = m_LocomotionM.IsFly;

            // ====== 상태 전환 ======
            // 낙하 상태 전환
            if (_isGrounded)
            {
                m_StateMachine.SwitchLocomotionState(LocomotionStateType.Land);
                return;
            }

            if(m_Core.ActionPolicy.CanFlyUp(m_CombatM.CombatStateData))
            {
                if (_isFly)
                {
                    m_StateMachine.SwitchLocomotionState(LocomotionStateType.FlyUp);
                    return;
                }
            }
        }

        public override void Exit()
        {
            
        }
    }
}