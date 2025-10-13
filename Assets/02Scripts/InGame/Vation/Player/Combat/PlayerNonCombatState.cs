using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerNonCombatState : PlayerCombatState
{
    protected override InputLocoLockType m_LockOnEnter => InputLocoLockType.None;

    protected override InputLocoLockType m_LockOnExit => InputLocoLockType.None;

    public PlayerNonCombatState(PlayerCore playerCore) : base(playerCore){}

    public override void Enter()
    {
        base.Enter();
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        if (m_Combat.IsSwap)
            m_PlayerCore.SwitchCombatState(CombatStateType.SwapWeapon);
        else if (m_Combat.IsReload)
            m_PlayerCore.SwitchCombatState(CombatStateType.Reload);
        else if (m_Combat.IsAttack || m_Combat.IsAim)
        {
            m_Combat.SetAming(m_Combat.IsAim);
            m_PlayerCore.SwitchCombatState(CombatStateType.InCombat);
        }
        else if (m_Combat.IsSkill)
            m_PlayerCore.SwitchCombatState(CombatStateType.Skill);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
