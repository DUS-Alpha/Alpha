using UnityEngine;

public class PlayerIdleState : PlayerState
{
    // playerCore 부모생성자 생성, 플레이어의 컴포넌트 하나로 쓰는
    // m_PlayerCore, m_Locomotion 사용하면됨
    public PlayerIdleState(PlayerCore playerCore) : base(playerCore) {}

    public override void Enter()
    {
        
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        if (m_Combat.IsCombatProgressing) return;

        m_Locomotion.Movement(m_Combat.IsAim);

        if (m_Locomotion.IsJump)
        {
            if (m_Locomotion.IsGrounded)
            {
                m_PlayerCore.SwitchLocomotionState(LocomotionState.Jump);
            }
        }
        else if (m_Locomotion.IsFlyUp)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionState.FlyStartUp);
        }
        else if (m_Locomotion.MoveDir != Vector3.zero)
            m_PlayerCore.SwitchLocomotionState(LocomotionState.Move);
    }

    public override void Exit()
    {
        
    }
}
