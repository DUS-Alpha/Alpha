using UnityEngine;
using UnityEngine.Windows;
public class PlayerAttackState : PlayerCombatState
{
    public PlayerAttackState(PlayerCore playerCore) : base(playerCore){}
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
        m_Combat.AttackRootMotion(m_isMelee);

        if (m_isMelee) m_PlayerCore.AniController.SetAnimatorWeight(5,1);
    }
    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        var _weapon = m_Combat.CurrentWeapon;

        if (_weapon == null)
        {
            m_Combat.SetAming(true);
            m_PlayerCore.SwitchCombatState(CombatStateType.Idle);
            return;
        }

        /*if(!m_Locomotion.IsGrounded && !m_Locomotion.IsFlying)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.Idle);
            return;
        }*/

        if (m_Locomotion.IsFlying && m_isMelee)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.Idle);
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
                if (m_Combat.IsAim) m_PlayerCore.SwitchCombatState(CombatStateType.Aim);
                else
                m_PlayerCore.SwitchCombatState(CombatStateType.Idle);
            }
        }
        else
        {
            m_Combat.Attack();
        }

        /*if (!m_Combat.CurrentWeapon.IsInAction(m_PlayerCore.AniController))
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.Idle);
        }*/
    }
    

    public override void Exit()
    {
        base.Exit();
        m_Combat.AttackRootMotion(false);
        m_Combat.SetAming(false);
        m_Combat.ExitAttack();
    }
}
