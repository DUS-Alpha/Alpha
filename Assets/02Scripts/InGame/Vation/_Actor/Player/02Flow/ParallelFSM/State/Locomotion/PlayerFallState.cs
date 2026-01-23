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
            m_LocomotionM.SettingsFallMovement(EFallType.NormalFall);
        }

        public override void Update()
        {
            bool _isGrounded = m_LocomotionM.IsGrounded;

            // ====== 상태 전환 ======
            // 낙하 상태 전환
            if(_isGrounded)
            {
                m_StateMachine.SwitchLocomotionState(LocomotionStateType.Land);
                return;
            }

            if(m_Core.ActionPolicy.CanFlyUp(m_CombatM.CombatStateData))
            {
                if (m_LocomotionM.IsFlyUp)
                {
                    m_StateMachine.SwitchLocomotionState(LocomotionStateType.FlyUp);
                    return;
                }
            }

            /* bool _isFlyUp = m_InputM.IsFlyUp;

             m_Locomotion.ApplyGravity();*/

            /*if (_isFlyUp && m_Locomotion.ActionGauge > 0)
                m_Core.SwitchLocomotionState(LocomotionStateType.FlyUp);
            else if (m_Locomotion.IsGrounded)
                m_Core.SwitchLocomotionState(LocomotionStateType.Land);*/
        }

        public override void Exit()
        {
            //m_Core.SetLockState(false, false);
        }
    }
}