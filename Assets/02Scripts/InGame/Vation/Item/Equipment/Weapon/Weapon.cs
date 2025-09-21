using UnityEngine;

public class Weapon : Equipment, IAttack
{
    public WeaponData WeaponData => (Data as WeaponData);
    public virtual void Attack(bool isAttackInput, PlayerAnimationController anim)
    {
        
    }

    public override void Equip(GameObject user)
    {
        // 무기 장착 로직
    }

    public override void Unequip(GameObject user)
    {
        
    }
    public virtual bool IsInAction(PlayerAnimationController anim)
    {
        return false;
    }
}
