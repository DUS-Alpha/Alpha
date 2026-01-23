using UnityEngine;
/*
상속
ArmorItemDataSO
    ㄴHeadItemDtaSO
    ㄴGlovesItemDataSO
    ㄴBootsItemDataSO
*/

namespace alpha
{
    public enum EArmorTypes
    {
        Head,
        UpperBody,
        LowerBody,
        Gloves,
        Boots
    }

    [CreateAssetMenu(fileName = "Armor", menuName = "Scriptable Objects/Item/Armor")]
    public class ArmorSO : ItemSO
    {
        public EArmorTypes ArmorType;

        // 상속시 EquipmentType값 자동 설정
#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            ItemType = EItemTypes.Armor;
            // 부모에서는 아무것도 안함 (자식에서 설정)
        }
#endif
    }
}