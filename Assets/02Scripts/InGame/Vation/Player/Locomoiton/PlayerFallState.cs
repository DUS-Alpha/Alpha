using UnityEngine;

public class PlayerFallState : PlayerLocomotionState
{
    public PlayerFallState(PlayerCore playerCore) : base(playerCore){}

    public override void Enter()
    {
        
    }
    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        m_Locomotion.ApplyGravity();
        if(m_Locomotion.IsGrounded)
            m_PlayerCore.SwitchState(new PlayerIdleState(m_PlayerCore));
    }

    public override void Exit()
    {
       
    }
}
