using System;
using Unity.Collections;
using UnityEngine;

namespace alpha
{
    public enum EWeaponTypes
    {
        Melee,
        MainRange,
        SubRange
    }

    public class WeaponItemDataSO : ItemDataSO
    {
        [ReadOnly] public EWeaponTypes WeaponType;

        // 상속시 EquipmentType값 자동 설정
#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            ItemType = EItemTypes.Weapon;
        }
#endif

        [Header("[ WeaponItemData ]"), Space(10)]
        public int Damage;
        public float AttackCost;
        public AudioClip AudioClip;
    }
}
