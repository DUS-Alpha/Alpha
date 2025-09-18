using UnityEngine;

public enum RangeTypes
{
    Pistol,
    Rifle,
    Sniper,
    Machinegun
}

public class RangeWeapon : Weapon
{
    public RangeTypes RangeType;

    private float m_nextFire = 0f;
    private bool m_isFire;
    
    public override void Attack(bool isAttackInput, PlayerAnimationController anim)
    {
        m_isFire = isAttackInput;
        if (!isAttackInput) return;
        anim.RangeShootingAni();
    }
    public override bool IsInAction(PlayerAnimationController anim)
    {
        return m_isFire;
    }
}
