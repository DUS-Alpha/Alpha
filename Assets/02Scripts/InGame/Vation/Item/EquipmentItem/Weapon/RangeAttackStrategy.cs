using alpha;
using UnityEngine;

public class RangeAttackStrategy : IAttackStrategy
{
    public bool CanMoveDuringAttack => true;
    private RangeWeaponItem m_rangeItem;
    private float m_nextFireTime;
    public void StartAttack(PlayerCombat combat)
    {
        m_rangeItem = combat.CurrentItem as RangeWeaponItem;
        combat.AniM.RangeShootingAni();

        m_nextFireTime = 0;
    }

    public void UpdateAttack(PlayerCombat combat)
    {
        // 주기에 따른 발사 적용
        if (Time.time < m_nextFireTime)
            return;

        combat.AniM.RangeShootingAni();
        m_nextFireTime = Time.time + m_rangeItem.RangeData.FireRate;
    }

    public void ExitAttack(PlayerCombat combat)
    {
        m_nextFireTime = 0;

    }
}
