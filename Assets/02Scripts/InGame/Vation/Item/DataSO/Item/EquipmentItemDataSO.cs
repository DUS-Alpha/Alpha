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
        [HideInInspector] public EEquipmentTypes EquipmentType;
        public int Durability;

        // 상속시 EquipmentType값 자동 설정
#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            // 부모에서는 아무것도 안함 (자식에서 설정)
        }
#endif
    }
}