
using UnityEngine;

namespace alpha
{
    public class PlayerFlightMoveState : PlayerStateBase
    {
        public PlayerFlightMoveState(PlayerCore playerCore) : base(playerCore) { }
        public override void Enter()
        {
            base.Enter();
            m_LocomotionM.SetFlightMovement();
        }

        public override void Update()
        {
            Vector3 _moveInput = m_LocomotionM.MoveInput;
            bool _isRotLock = m_LocomotionM.IsRotLock;
            bool _isFly = m_LocomotionM.IsFly;

            // TODO : CombatStateData로 전환하기
            bool _isInCombat = m_CombatM.IsInCombat;

            m_LocomotionM.AirMovement(_moveInput, _isRotLock, _isInCombat);

            // ====== 상태 전환
            // TODO : 정책으로 변경
            if(_isFly)
            {
                m_StateMachine.SwitchLocomotionState(LocomotionStateType.Fall);
            }
        }

        public override void Exit()
        {

        }


    }
}