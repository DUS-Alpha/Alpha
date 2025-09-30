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
        else if(m_Combat.IsCombat)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.Upper_InCombat);
        }

        /*if (m_Combat.IsSwapWeapon)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.SwapWeapon);
        }
        else if (m_Combat.IsAttack)    // 우선은 같은 Attack으로 넘어가지만 향후를 위해 구분
        {
            if(m_Combat.CurrentWeaponNum == 1 && !m_Locomotion.IsFlying)
            {
                m_PlayerCore.SwitchCombatState(CombatStateType.Attack);
            }
            else if(m_Combat.CurrentWeaponNum > 1)
            {
                m_PlayerCore.SwitchCombatState(CombatStateType.Aim);
            }
        }
        else if(m_Combat.IsAiming)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.Aim);
        }
        else if(m_Combat.IsReload)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.Reload);
        }*/
    }

    public override void Exit()
    {
        base.Exit();
    }
}
