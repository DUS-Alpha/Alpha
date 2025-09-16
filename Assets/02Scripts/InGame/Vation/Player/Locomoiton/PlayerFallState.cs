using UnityEngine;

public class PlayerFallState : PlayerLocomotionState
{
    protected override InputCombatLockType m_LockOnEnter => InputCombatLockType.Skill;

    protected override InputCombatLockType m_LockOnExit => InputCombatLockType.Skill;

    public PlayerFallState(PlayerCore playerCore) : base(playerCore){}

    public override void Enter()
    {
       base.Enter();

        // Fall 동작 시 2.5정도의 Velocity로 시작해야 조금 부드러움
        m_Locomotion.SetVelocityY(2.5f);
    }
    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        m_Locomotion.AirMovement();
        m_Locomotion.ApplyGravity();

        if (m_Locomotion.IsFlyUp)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.FlyUp);
        }
        else if (m_Locomotion.IsGrounded)
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Landing);
    }

    public override void Exit()
    {
       base.Exit();
    }
}
