using UnityEngine;

public class Weapon : Equipment, IAttack
{
    public WeaponDataSO WeaponData => (Data as WeaponDataSO);
    public float m_maxDistance;
    /*public Transform LeftHandIK;
    public Transform RightHandIK;
    public Transform LeftHintIK;
    public Transform RightHintIK;*/
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
