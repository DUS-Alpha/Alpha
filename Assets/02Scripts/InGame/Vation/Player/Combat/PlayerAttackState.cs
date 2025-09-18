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
    private bool m_isRange;

    public override void Enter()
    {
        base.Enter();
        if (m_Combat.CurrentWeaponNum == 1) m_isMelee = true;
        else
        {
            m_isRange = true;
        }
        // 무기 Swap시 마다 스나이퍼 같은 총의 경우 바로 발사를 하면 안되기에 계속 현재 무기값으로
        m_Combat.SetNextAttackDelay(m_Combat.CurrentWeapon.WeaponData.AttackDelay);
        m_Combat.AttackRootMotion(m_isMelee);
    }
    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        var _weapon = m_Combat.CurrentWeapon;

        if (_weapon == null)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.Idle);
            return;
        }
        
        m_Combat.Attack();
        m_Combat.Aming(m_Combat.IsAim);

        if (m_Combat.IsSwapWeapon)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.SwapWeapon);
        }
        // 공격이 끝났으면 Idle로
        else if (!_weapon.IsInAction(m_PlayerCore.AniController))
        {
            // TODO 딜레이 조금 줄지 고민
            if (m_isRange && m_Combat.IsAim)
            {
                m_PlayerCore.SwitchCombatState(CombatStateType.Aim);
            }
            else
            {
                m_PlayerCore.SwitchCombatState(CombatStateType.Idle);
            }
        }

        
    }

    public override void Exit()
    {
        base.Exit();
        m_Combat.AttackRootMotion(false);
    }
}
