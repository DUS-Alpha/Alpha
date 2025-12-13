using alpha;
using UnityEngine;

public class RangeWeaponStrategy : IAttackStrategy
{
    public bool CanMoveDuringAttack => true;
    private RangeWeaponItem m_rangeItem;
    private float m_nextFireTime;

    private void PerformAttack(PlayerCombatManager combat)
    {

        m_rangeItem.PlayEffect();
        combat.AudioM.PlayRangeAttack(m_rangeItem.RangeData.AudioClip);
        
        // Gauge
        combat.InvokeResetTimer();

        // Ani
        combat.AniM.RangeShootingAni();
    }

    public void StartAttack(PlayerCombatManager combat)
    {
        m_rangeItem = combat.CurrentItem as RangeWeaponItem;
        //combat.InvokeResetTimer();

        //combat.InvokeDecreaseGauge(m_rangeItem.WeaponData.AttackCost);

        m_nextFireTime = 0;
    }

    public void UpdateAttack(PlayerCombatManager combat)
    {
        float _currentGauge = combat.GetCurrentGauge();


        if (_currentGauge < m_rangeItem.WeaponData.AttackCost) return;

        // 주기에 따른 발사 적용
        if (Time.time < m_nextFireTime)
            return;

        PerformAttack(combat);
        combat.InvokeDecreaseGauge(m_rangeItem.WeaponData.AttackCost);

        m_nextFireTime = Time.time + m_rangeItem.RangeData.FireRate;
    }

    public void ExitAttack(PlayerCombatManager combat)
    {
        m_nextFireTime = 0;
    }
}
