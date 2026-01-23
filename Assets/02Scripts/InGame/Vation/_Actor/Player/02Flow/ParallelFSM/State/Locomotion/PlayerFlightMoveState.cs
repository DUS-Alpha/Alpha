
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

            // TODO : CombatStateData로 전환하기
            bool _isInCombat = m_CombatM.IsInCombat;

            m_LocomotionM.AirMovement(_moveInput, _isRotLock, _isInCombat);
        }

        public override void Exit()
        {

        }


    }
}