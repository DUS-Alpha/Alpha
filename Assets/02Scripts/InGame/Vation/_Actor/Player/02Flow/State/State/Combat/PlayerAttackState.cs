using UnityEngine;
using alpha;

public class PlayerAttackState : PlayerStateBase
{
    // Locomotion 상태 Attack에서 관리하는 것 수정 필요
    public PlayerAttackState(alpha.PlayerCore playerCore) : base(playerCore){}
    
    WeaponItem m_weaponItem;
    public override void Enter()
    {
        //m_weaponItem = m_Combat.CurrentItem as WeaponItem;
        //m_weaponItem.AttackStrategy.StartAttack(m_Combat);

        //m_Combat.SetIsInCombat(true);
    }

    public override void Update()
    {
        /*if(m_Combat.CanSwap && m_weaponItem is RangeWeaponItem)
        {
            m_Core.SwitchCombatState(CombatStateType.Swap);
            m_Combat.AniM.SetAttackBtnParameter(false);
            return;
        }
        else if (m_Combat.IsAttackBtn)
        {
            m_weaponItem.AttackStrategy.UpdateAttack(m_Combat);
            m_NextStateDelay = 0;
        }
        else
        {
            m_Combat.InvokeRegenerateGauge();
            if (!m_Combat.IsAction)
                m_Core.SwitchCombatState(CombatStateType.CombatReady);
        }
        m_Combat.AniM.SetAttackBtnParameter(m_Combat.IsAttackBtn);*/
    }
    public override void Exit()
    {
        //m_weaponItem.AttackStrategy.ExitAttack(m_Combat);
    }
}
