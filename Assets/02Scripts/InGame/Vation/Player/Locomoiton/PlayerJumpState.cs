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
            m_PlayerCore.SwitchLocomotionState(LocomotionState.Fall);
        }
        else if (m_Locomotion.IsFlyUp)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionState.FlyStartUp);
        }
    }

    public override void Exit()
    {
        m_Locomotion.JumpExit();
    }
}
