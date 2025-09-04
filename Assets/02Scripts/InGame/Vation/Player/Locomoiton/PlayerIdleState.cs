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
        m_Locomotion.Movement();
        m_Combat.SwapWeapon();

        if (m_Locomotion.IsJump)
        {
            if (m_Locomotion.IsGrounded)
            {
                m_PlayerCore.SwitchState(new PlayerJumpState(m_PlayerCore));
            }
        }
        else if (m_Locomotion.IsFlyUp)
        {
            m_PlayerCore.SwitchState(new PlayerFlyUpStartState(m_PlayerCore));
        }
        else if (m_Locomotion.MoveDir != Vector3.zero)
            m_PlayerCore.SwitchState(new PlayerMoveState(m_PlayerCore));
        else if (m_Combat.IsAttack)
        {
            m_PlayerCore.SwitchState(new PlayerAttackState(m_PlayerCore));
        }
    }

    public override void Exit()
    {
        
    }
}
