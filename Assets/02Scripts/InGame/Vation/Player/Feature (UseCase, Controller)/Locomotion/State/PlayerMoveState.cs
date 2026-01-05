using UnityEngine;

namespace alpha
{
    public class PlayerMoveState : PlayerLocomotionStateBase
    {
        public PlayerMoveState(PlayerCore playerCore) : base(playerCore) { }

        public override void Enter()
        {
            m_AniM.SetMoveType(m_Combat.IsInCombat);
        }

        public override void Update()
        {
            // 입력 확인
            bool _isJump = m_InputM.IsJump;
            bool _isFlyUp = m_InputM.IsFlyUp;
            bool _isCombat = m_Combat.IsInCombat;
            Vector2 _moveInput = m_InputM.MoveDirInput;
            Vector2 _lookInput = m_InputM.LookInput;

            // 이동 동작 및 중력 적용
            m_Locomotion.Movement(true, _moveInput, m_InputM.LookInput, false, _isCombat);
            m_Locomotion.ApplyGravity();

            // 애니메이션
            m_AniM.MoveAni(_moveInput.x, _moveInput.y, _isCombat);

            // 오디오


            // 상태 전환
            /*if (_isJump)
                m_Core.SwitchLocomotionState(LocomotionStateType.Jump);
            else if (_isFlyUp)
            {
                if (m_Locomotion.ActionGauge > 0.1f)
                    m_Core.SwitchLocomotionState(LocomotionStateType.FlyUp);
            }*/

        }
        public override void Exit()
        {

        }
    }
}