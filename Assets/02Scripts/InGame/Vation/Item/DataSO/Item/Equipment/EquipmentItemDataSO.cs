using Unity.Collections;
using UnityEngine;
/*
상속
EquipmentItemDataSO
    ㄴWeaponItemDataSO
        ㄴMeleeWeaponItemDtaSO
        ㄴMainRangeWeaponItemDataSO
        ㄴSubRangeWeaponItemDataSO
*/

namespace alpha
{
    public enum EEquipmentTypes
    {
        Armor,
        Weapon
    }
    public abstract class EquipmentItemDataSO : ItemDataSO
    {
        [Space(10)]
        [ReadOnly] public EEquipmentTypes EquipmentType;
        /// <summary>
        /// 내구성
        /// </summary>
        public int Durability;

        // 상속시 EquipmentType값 자동 설정
#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            ItemType = EItemTypes.EquipmentItem;
            // 부모에서는 아무것도 안함 (자식에서 설정)
        }
#endif
    }
}