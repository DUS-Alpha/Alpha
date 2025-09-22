using UnityEngine;

public class PlayerMoveState : PlayerLocomotionState
{
    protected override InputCombatLockType m_LockOnEnter => InputCombatLockType.None;

    protected override InputCombatLockType m_LockOnExit => InputCombatLockType.None;

    public PlayerMoveState(PlayerCore playerCore) : base(playerCore){}

    public override void Enter()
    {
        base.Enter();
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        m_Locomotion.Movement();
        m_Locomotion.ApplyGravity();

        if (m_Locomotion.IsJump)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Jump);
        }
        else if (m_Locomotion.IsDodge)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Dodge);
        }
        else if (m_Locomotion.IsFlyUp)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.FlyUp);
        }
        else if (m_Locomotion.MoveDir == Vector3.zero)
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Idle);
        /*else if(!m_Locomotion.IsGrounded)
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Fall);*/
        // else if(IsDie)
    }
    public override void Exit()
    {
        base.Exit();
    }
}
