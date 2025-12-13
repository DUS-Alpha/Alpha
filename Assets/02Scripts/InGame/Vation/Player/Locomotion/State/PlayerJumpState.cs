using UnityEngine;

public class PlayerJumpState : PlayerLocomotionStateBase
{
    public PlayerJumpState(PlayerCore playerCore) : base(playerCore){}

    public override void Enter()
    {
        // 마지막 방향으로 회전
        m_Locomotion.ForcedRotate(m_Locomotion.LastMoveInputDir);

        // 지면 체크 비활성화
        m_Locomotion.SetCheckedGround(false);

        // 점프 시 수직힘 적용
        m_Locomotion.SetForceVerticallMoveValue(m_Locomotion.JumpHeight);

        /// 애니메이션
        m_AniM.JumpAni();

        // 오디오
        m_Audio.PlayLocomotionAudio(0, SFX_LomotionType.Jump);

        // 낙하 타입 설정
        m_Locomotion.SetFallType(EFallType.NormalFall);

        // Comabat 잠금
        m_Core.SetLockState(true, false);
    }

    public override void Update()
    {
        // 점프 이동
        m_Locomotion.ForceMove(m_Locomotion.LastMoveInputDir, m_Locomotion.CurrentVelocity.y);

        // 중력 적용
        m_Locomotion.ApplyGravity();

        // 상태 전환
        if (m_Locomotion.CurrentVelocity.y <= 0f)   // Y속력이 0이하(아래로 떨어지는 시점)
        {
            m_Locomotion.SetCheckedGround(true);
            if(!m_Locomotion.IsGrounded)
            {
                m_Core.SwitchLocomotionState(LocomotionStateType.Fall);
                return;
            }
        }
    }

    public override void Exit()
    {
        m_Core.SetLockState(false, false);
    }
}
