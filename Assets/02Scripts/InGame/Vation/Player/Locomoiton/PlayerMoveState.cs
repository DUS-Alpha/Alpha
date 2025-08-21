using UnityEngine;

public class PlayerMoveState : PlayerLocomotionState
{
    public PlayerMoveState(PlayerCore playerCore) : base(playerCore) {}

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        Vector3 _moveDir = m_Locomotion.LocomotionMovement();

        bool _isJump = m_PlayerCore.InputHandler.IsJump;

        if (_isJump)
        {
            if (m_Locomotion.IsGrounded && !m_PlayerCore.isAction)
            {
                m_PlayerCore.SwitchState(new PlayerJumpState(m_PlayerCore));
            }
        }

        else if (_moveDir == Vector3.zero)
            m_PlayerCore.SwitchState(new PlayerIdleState(m_PlayerCore));
    }
}
