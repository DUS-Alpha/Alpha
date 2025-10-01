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
        if (m_Locomotion.IsAction) return;

        if (m_Combat.IsSwapWeapon())
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.Upper_SwapWeapon);
        }
        else if (m_Combat.IsReload)
        {
            if(m_Combat.CurrentWeaponNum > 1)
            m_PlayerCore.SwitchCombatState(CombatStateType.Upper_Reload);
        }

        if (m_Combat.CurrentWeaponNum == 0) return;
        else if (m_Combat.IsCombat)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.Upper_InCombat);
        }
        else if (m_Combat.SkillQueue.Count != 0)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.Skill);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
