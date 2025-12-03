using UnityEngine;

public class PlayerMoveState : PlayerLocomotionStateBase
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

        m_Locomotion.Movement(m_Combat.CanMove, m_Combat.IsInCombat);

        m_Locomotion.ApplyGravity();
        if (m_Locomotion.IsDie)
        {
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Die);
            return;
        }

        if (m_Locomotion.IsJump)
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Jump);
        /*else if (m_Locomotion.IsDash && m_Locomotion.ActionGauge >= 40)
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Dash);*/
        else if (m_Locomotion.IsVerticalTakeOff)
        {
            if(m_Locomotion.ActionGauge > 0.1f)
                m_PlayerCore.SwitchLocomotionState(LocomotionStateType.VerticalTakeOff);
        }
        else if (!m_Locomotion.IsMove)
            m_PlayerCore.SwitchLocomotionState(LocomotionStateType.Idle);

        
    }
    public override void Exit()
    {
        base.Exit();
    }
}
