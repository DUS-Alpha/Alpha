using UnityEngine;
using alpha;

public class PlayerAttackState : PlayerCombatStateBase
{
    // Locomotion 상태 Attack에서 관리하는 것 수정 필요
    public PlayerAttackState(PlayerCore playerCore) : base(playerCore){}

    protected override InputLocoLockType m_LockOnEnter => InputLocoLockType.None;

    protected override InputLocoLockType m_LockOnExit => InputLocoLockType.None;
    WeaponItem m_weaponItem;
    public override void Enter()
    {
        base.Enter();

        m_weaponItem = m_Combat.CurrentItem as WeaponItem;
        m_weaponItem.AttackStrategy.StartAttack(m_Combat);

        // 공격 중 이동가능 여부
        m_Combat.SetCanMove(m_weaponItem.AttackStrategy.CanMoveDuringAttack);

        m_Combat.SetIsInCombat(true);
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        base.Update();

        if(m_Combat.CanSwap && m_weaponItem is RangeWeaponItem)
        {
            m_PlayerCore.SwitchCombatState(CombatStateType.Swap);
            m_Combat.AniM.SetAttackBtnAni(false);
            return;
        }
        else if (m_Combat.IsAttackBtn)
        {
            m_weaponItem.AttackStrategy.UpdateAttack(m_Combat);
            m_NextStateDelay = 0;
        }
        else
        {
            if(!m_Combat.IsAction)
                m_PlayerCore.SwitchCombatState(CombatStateType.InCombat);
        }

        m_Combat.AniM.SetAttackBtnAni(m_Combat.IsAttackBtn);
    }
    public override void Exit()
    {
        base.Exit();
        m_weaponItem.AttackStrategy.ExitAttack(m_Combat);
        m_Combat.SetCanMove(true);
    }
}
