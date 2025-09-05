using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerCore playerCore) : base(playerCore){}

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
        m_Locomotion.Movement();

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
        else if (m_Locomotion.MoveDir == Vector3.zero)
            m_PlayerCore.SwitchLocomotionState(LocomotionState.Idle);
    }
}
