using UnityEngine;
using UnityEngine.Windows;
public class PlayerSkillState : PlayerCombatState
{
    public PlayerSkillState(PlayerCore playerCore) : base(playerCore){}
    protected override InputLocoLockType m_LockOnEnter
    {
        get
        {
            if (m_Combat.CurrentWeaponNum == 1) return InputLocoLockType.All;
            else return InputLocoLockType.None;
        }
    }

    protected override InputLocoLockType m_LockOnExit
    {
        get
        {
            if (m_Combat.CurrentWeaponNum == 1) return InputLocoLockType.All;
            else return InputLocoLockType.None;
        }
    }
    private bool m_isMelee;

    public override void Enter()
    {
        base.Enter();

        if (m_Combat.CurrentWeaponNum == 1) m_isMelee = true;

        if (m_isMelee) m_PlayerCore.AniController.SetAnimatorWeight(3,1);
    }
    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        /*var _weapon = m_Combat.CurrentWeapon;

        if (_weapon == null)
        {
            
            m_PlayerCore.SwitchCombatState(CombatStateType.NonCombat);
            return;
        }

        if (m_Locomotion.IsFlying && m_isMelee)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.NonCombat);
            return;
        }

        if (m_Combat.IsSwapWeapon && !m_isMelee)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.SwapWeapon);
        }
        else if (!m_Combat.IsAttack)
        {
            if(!m_Combat.IsAction)
            {
                if (m_Combat.IsAiming) m_PlayerCore.SwitchCombatState(CombatStateType.Aim);
                else m_PlayerCore.SwitchCombatState(CombatStateType.NonCombat);
            }
        }
        else if (m_Combat.IsReload)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.Reload);
        }
        else
        {
            m_Combat.SetAming(m_Combat.IsAttack);
            m_Combat.Attack();
        }*/
    }
    

    public override void Exit()
    {
        base.Exit();
        m_Combat.SetAming(false);
        if (m_isMelee) m_PlayerCore.AniController.SetAnimatorWeight(3, 0);
        m_Combat.ExitAttack();
    }
}
