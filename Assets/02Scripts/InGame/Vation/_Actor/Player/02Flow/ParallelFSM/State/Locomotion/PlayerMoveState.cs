using UnityEngine;

namespace alpha
{
    public class PlayerMoveState : PlayerStateBase
    {
        public PlayerMoveState(PlayerCore playerCore) : base(playerCore){}

        public override void Enter()
        {
            base.Enter();
            m_LocomotionM.SettingsGroundMove();
        }

        public override void Update()
        {
            Vector3 _moveInput = m_LocomotionM.MoveInput;
            bool _isGrounded = m_LocomotionM.IsGrounded;
            bool _isRotLock = m_LocomotionM.IsRotLock;

            // TODO : CombatStateData로 전환하기
            bool _isInCombat = m_CombatM.IsInCombat;    

            // 이동 행위 실행
            m_LocomotionM.GroundMovement(_moveInput, _isRotLock, _isInCombat);
            
            // ====== 상태 전환 ======
            // 점프
            if(m_Core.ActionPolicy.CanJump(_isGrounded,m_CombatM.CombatStateData))
            {
                if(m_LocomotionM.IsJump)
                {
                    m_StateMachine.SwitchLocomotionState(LocomotionStateType.Jump);
                    return;
                }
            }

            // 대시
            if (m_Core.ActionPolicy.CanDash(100, m_CombatM.CombatStateData))
            {
                if(m_LocomotionM.IsDash)
                {
                    m_StateMachine.SwitchLocomotionState(LocomotionStateType.Dash);
                    return;
                }
            }

            // 비행 상승
            if (m_Core.ActionPolicy.CanFlyUp(m_CombatM.CombatStateData))
            {
                if (m_LocomotionM.IsFly)
                {
                    m_StateMachine.SwitchLocomotionState(LocomotionStateType.FlyUp);
                    return;
                }
            }

            // 이동 중 낙하
            if (_isGrounded == false)
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