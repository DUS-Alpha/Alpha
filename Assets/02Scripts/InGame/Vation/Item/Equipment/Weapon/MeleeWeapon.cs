using UnityEngine;

public class MeleeWeapon : Weapon
{
    private bool m_isCombo;
    public override void Attack(bool isAttackInput, PlayerAnimationController anim)
    {
        m_isCombo = isAttackInput;
        if (isAttackInput) anim.MeleeAttackAni(true);
        else anim.MeleeAttackAni(false);

        // Audio

    }
    public override void Unequip(GameObject user)
    {
        m_isCombo = false;
    }
    public override bool IsInAction(PlayerAnimationController anim)
    {
        return m_isCombo;
    }

}
