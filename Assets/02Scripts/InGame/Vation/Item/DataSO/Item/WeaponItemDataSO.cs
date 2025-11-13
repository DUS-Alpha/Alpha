using System;
using UnityEngine;
/*
상속
WeaponItemDataSO
    ㄴMeleeItemDtaSO
    ㄴRangeMainItemDataSO
    ㄴRangeSubItemDataSO
*/

namespace alpha
{
    public enum EWeaponTypes
    {
        Melee,
        MainRange,
        SubRange
    }

    public class WeaponItemDataSO : EquipmentItemDataSO, IAttack
    {

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            EquipmentType = EEquipmentTypes.Weapon;
        }
#endif
        [HideInInspector] public EWeaponTypes WeaponType;

        [Header("[ WeaponItemDataSO Requirements ]"), Space(10)]
        public int StrengthREQ = 0;
        public int DexREQ = 0;
        public int IntREQ = 0;
        [Tooltip("장착 최소 수치")]
        public int FaithREQ = 0;

        [Header("[ WeaponItemDataSO Base Damage ]"), Space(10)]
        public int Damage = 0;
        public int MagicDamage = 0;
        public int FireDamage = 0;
        public int HolyDamage = 0;
        public int LightningDamage = 0;     // 연쇄 데미지

        [Header("[ WeaponItemDataSO Poise ]"), Space(10)]
        [Tooltip("공격 받을 시 밀리는 값")]
        public float PoiseDamage = 10;

        [Header("[ Stamina Costs ]"), Space(10)]
        public int BaseStaminaCost = 20;

        public virtual void Attack(bool isAttackInput, PlayerAnimationController anim)
        {

        }

        public override Item CreateItem()
        {
            throw new NotImplementedException();
        }
    }
}
