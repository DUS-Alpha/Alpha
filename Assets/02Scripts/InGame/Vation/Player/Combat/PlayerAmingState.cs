using UnityEngine;

public class PlayerAmingState : PlayerCombatState
{
    public PlayerAmingState(PlayerCore playerCore) : base(playerCore){}

    protected override InputLocoLockType m_LockOnEnter => InputLocoLockType.Look;

    protected override InputLocoLockType m_LockOnExit => InputLocoLockType.Look;
    public override void Enter()
    {
        base.Enter();
        m_Combat.Aming(true);
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        // TODO : Combat에서 처리
        if (m_Locomotion.IsJump || m_Locomotion.IsDodge || m_Locomotion.IsFlyUp)
        {
            m_Combat.Aming(false);
            m_PlayerCore.SwitchCombatState(CombatStateType.Idle);
        }
        else if (m_Combat.IsRangeShooting)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.RangeShooting);
        }
        else if (m_Combat.IsAim)
        {
            m_Combat.Aming(m_Combat.IsAim);
        }
        else if (!m_Combat.IsAim)
        {
            m_Combat.Aming(false);
            m_PlayerCore.SwitchCombatState(CombatStateType.Idle);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
