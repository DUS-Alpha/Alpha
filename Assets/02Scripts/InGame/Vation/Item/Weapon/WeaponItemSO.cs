using System;
using UnityEngine;

public enum WeaponTypes
{
    Melee,
    MainRange,
    SubRange
}

public class WeaponItemSO : ItemSO, IAttack
{
    [Header("[ === WeaponItemSO Info === ]"), Space(10)]
    public WeaponTypes WeaponType;
    public GameObject WeaponPrefab;


    [Header("[ WeaponItemSO Requirements ]"), Space(10)]
    public int StrengthREQ = 0;
    public int DexREQ = 0;
    public int IntREQ = 0;
    public int FaithREQ = 0;    // 장착 최수 수치

    [Header("[ WeaponItemSO Base Damage ]"), Space(10)]
    public int Damage = 0;
    public int MagicDamage = 0;
    public int FireDamage = 0;
    public int HolyDamage = 0;
    public int LightningDamage = 0;     // 연쇄 데미지

    [Header("[ WeaponItemSO Poise ]"), Space(10)]
    public float PoiseDamage = 10;      // 공격 받을 시 밀리는 값

    [Header("[ Stamina Costs ]"), Space(10)]
    public int BaseStaminaCost = 20;


    public virtual void Attack(bool isAttackInput, PlayerAnimationController anim)
    {

    }

    /*public WeaponDataSO WeaponData => (Data as WeaponDataSO);
    public float m_maxDistance;
    

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
    }*/
}
