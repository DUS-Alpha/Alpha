using UnityEngine;

public class Weapon : Equipment
{
    public WeaponData WeaponData => m_Data as WeaponData;

    public override void Equip(GameObject user)
    {
        // 무기 장착 로직
    }

    public void Attack(GameObject target)
    {
        // WeaponData.Damage 참조하여 공격 처리
    }
}
