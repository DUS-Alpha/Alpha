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
        if (m_Locomotion.IsCombatStop) return;

        if (m_Combat.IsSwap)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.SwapWeapon);
            return;
        }
        else if (m_Combat.IsReload)
        {
            if(m_Combat.CurrentWeaponNum > 1)
            m_PlayerCore.SwitchCombatState(CombatStateType.Reload);
            return;
        }

        if (m_Combat.CurrentWeaponNum == 0) return;

        else if (m_Combat.IsAttack || m_Combat.IsAim)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.Upper_InCombat);
        }
        else if (m_Combat.IsSkill)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.Skill);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
