using UnityEngine;

public class PlayerFallState : PlayerLocomotionStateBase
{
    protected override InputCombatLockType m_LockOnEnter => InputCombatLockType.All;

    protected override InputCombatLockType m_LockOnExit => InputCombatLockType.All;

    public PlayerFallState(PlayerCore playerCore) : base(playerCore){}

    public override void Enter()
    {
       base.Enter();
        m_Ani.SetAnimatorWeight(1,0);
    }
    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        m_Locomotion.AirMovement();
        m_Locomotion.ApplyGravity();

        if (m_Locomotion.IsDie)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Die);
            return;
        }
        if (m_Locomotion.IsFlyUp && m_Locomotion.ActionGauge > 0)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.FlyUp);
        }
        else if (m_Locomotion.IsGrounded)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Land);
        }
    }

    public override void Exit()
    {
       base.Exit();
    }
}
