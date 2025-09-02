using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerCore playerCore) : base(playerCore){}

    public override void Enter()
    {
        m_Locomotion.JumpStart();
    }
  
    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        m_Locomotion.ApplyGravity();

        if (m_Locomotion.Velocity.y <= 0 && !m_Locomotion.IsGrounded)
        {
            m_PlayerCore.SwitchState(new PlayerFallState(m_PlayerCore));
        }
        else if (m_Locomotion.IsFlyUp)
        {
            m_PlayerCore.SwitchState(new PlayerFlyUpStartState(m_PlayerCore));
        }
    }

    public override void Exit()
    {
        m_Locomotion.JumpExit();
    }
}
