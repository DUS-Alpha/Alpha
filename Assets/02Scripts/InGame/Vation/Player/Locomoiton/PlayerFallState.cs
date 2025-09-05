using UnityEngine;

public class PlayerFallState : PlayerState
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
        if (m_Locomotion.IsFlyUp)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionState.FlyStartUp);
        }
        else if (m_Locomotion.IsGrounded)
            m_PlayerCore.SwitchLocomotionState(LocomotionState.Idle);
    }

    public override void Exit()
    {
       
    }
}
