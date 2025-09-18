using UnityEngine;

public class MeleeWeapon : Weapon
{
    private bool m_isCombo;

    public override void Attack(bool isAttackInput, PlayerAnimationController anim)
    {
        bool isTag = anim.CheckComboAnimation();
        if (isAttackInput)
        {
            if (!m_isCombo && isTag)
            {
                m_isCombo = true;
                return;
            }
            anim.MeleeAttackAni(true);
        }
        else
        {
            anim.MeleeAttackAni(false);
        }

        if (m_isCombo && !isTag)
            m_isCombo = false;
    }
    public override void Unequip(GameObject user)
    {
        m_isCombo = false;
    }
    public override bool IsInAction(PlayerAnimationController anim)
    {
        return m_isCombo || anim.CheckComboAnimation();
    }

}
