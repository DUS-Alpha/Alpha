using UnityEngine;

public enum EFallType
{
    None,
    NormalFall,
    FlyFall
}

public class PlayerFallState : PlayerLocomotionStateBase
{
    public PlayerFallState(PlayerCore playerCore) : base(playerCore){}

    public override void Enter()
    {
        // 캐릭터 정면 바라보며 떨어지기
        if (m_Locomotion.CurrentFallType == EFallType.FlyFall)
        {
            Vector3 dir = m_Locomotion.transform.forward;
            dir.y = 0;
            m_Locomotion.ForcedRotate(dir);
        }
        else
            m_Locomotion.ForcedRotate(m_Locomotion.LastMoveInputDir);

        // 애니메이션
        m_AniM.FallAni(m_Locomotion.CurrentFallType);

        // Combat 잠금
        m_Core.SetLockState(true, false);

        // 오디오
    }

    public override void Update()
    {
        bool _isFlyUp = m_InputM.IsFlyUp;

        m_Locomotion.ApplyGravity();

        if (_isFlyUp && m_Locomotion.ActionGauge > 0)
            m_Core.SwitchLocomotionState(LocomotionStateType.FlyUp);
        else if (m_Locomotion.IsGrounded)
            m_Core.SwitchLocomotionState(LocomotionStateType.Land);
    }

    public override void Exit()
    {
        m_Core.SetLockState(false, false);
    }
}
