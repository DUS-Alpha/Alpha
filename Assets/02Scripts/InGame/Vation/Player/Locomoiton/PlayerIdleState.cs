using UnityEngine;

public class PlayerIdleState : PlayerLocomotionState
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
       Vector3 _moveDir = m_Locomotion.LocomotionGroundMovement();

        bool _isJump = m_PlayerCore.InputHandler.IsJump;
        bool _isFly = m_PlayerCore.InputHandler.IsFly;

        if (_isJump)
        {
            if (m_Locomotion.IsGrounded && !m_PlayerCore.isAction)
            {
                m_PlayerCore.SwitchState(new PlayerJumpState(m_PlayerCore));
            }
        }
        else if(_isFly)
        {
            m_PlayerCore.SwitchState(new PlayerFlyUpStartState(m_PlayerCore));
        }
        else if (_moveDir != Vector3.zero)
            m_PlayerCore.SwitchState(new PlayerMoveState(m_PlayerCore));
    }

    public override void Exit()
    {
        
    }
}
