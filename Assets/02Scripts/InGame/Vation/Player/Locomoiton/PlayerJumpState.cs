using UnityEngine;

public class PlayerJumpState : PlayerLocomotionState
{
    protected override InputCombatLockType m_LockOnEnter => InputCombatLockType.All;

    protected override InputCombatLockType m_LockOnExit => InputCombatLockType.All;

    public PlayerJumpState(PlayerCore playerCore) : base(playerCore){}

    public override void Enter()
    {
        base.Enter();
        m_Locomotion.JumpStart();
        m_Locomotion.SetIsAction(true);
    }
  
    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        m_Locomotion.ApplyGravity();
        m_Locomotion.AirMovement();

        if (m_Locomotion.IsDie)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Die);
            return;
        }
        if (m_Locomotion.Velocity.y <= 0 && !m_Locomotion.IsGrounded)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Fall);
        }
    }

    public override void Exit()
    {
        base.Exit();
        m_Locomotion.JumpExit();
        m_Locomotion.SetIsAction(false);
    }
}
