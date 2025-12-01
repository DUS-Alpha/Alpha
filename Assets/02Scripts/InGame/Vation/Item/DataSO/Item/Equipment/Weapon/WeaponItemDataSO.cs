using System;
using Unity.Collections;
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

    public class WeaponItemDataSO : EquipmentItemDataSO
    {
        [ReadOnly] public EWeaponTypes WeaponType;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            EquipmentType = EEquipmentTypes.Weapon;
        }
#endif
        [Header("[ WeaponItemData ]"), Space(10)]
        public int Damage;
        public float AttackCost;
        public AudioClip AudioClip;
    }
}
