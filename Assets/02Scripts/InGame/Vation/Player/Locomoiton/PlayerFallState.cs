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
        bool _isFly = m_PlayerCore.InputHandler.IsFly;

        m_Locomotion.ApplyGravity();
        if(m_Locomotion.IsGrounded)
            m_PlayerCore.SwitchState(new PlayerIdleState(m_PlayerCore));

        if (_isFly)
        {
            m_PlayerCore.SwitchState(new PlayerFlyState(m_PlayerCore));
        }
    }

    public override void Exit()
    {
       
    }
}
