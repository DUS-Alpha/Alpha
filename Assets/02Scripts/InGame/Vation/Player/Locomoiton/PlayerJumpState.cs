using UnityEngine;

public class PlayerJumpState : PlayerLocomotionState
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
    }

    public override void Exit()
    {
        m_Locomotion.JumpExit();
    }
}
