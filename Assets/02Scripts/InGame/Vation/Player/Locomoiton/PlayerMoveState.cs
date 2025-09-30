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
        if(m_Locomotion.IsDie)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Die);
            return;
        }

        m_Locomotion.Movement(m_Combat.IsCombating, m_Combat.IsSniper);
        m_Locomotion.ApplyGravity();

        if (m_Locomotion.IsJump)
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Jump);
        else if (m_Locomotion.IsDodge)
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Dash);
        else if (m_Locomotion.IsFlyUp)
        {
            if(m_Locomotion.FlyingGauge > 0)
                m_PlayerCore.SwitchLocomotionState(LocomotionStateType.FlyUp);
        }
        else if (m_Locomotion.MoveDir == Vector3.zero)
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Idle);

        if(m_Locomotion.MoveDir.sqrMagnitude > 0.1f)
        {
            m_Locomotion.MoveEffect();
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
