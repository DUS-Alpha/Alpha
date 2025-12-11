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

    [CreateAssetMenu(fileName = "ArmorItem", menuName = "Scriptable Objects/ItemData/ArmorItem")]
    public class ArmorItemDataSO : EquipmentItemDataSO
    {
        public EArmorTypes ArmorType;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            EquipmentType = EEquipmentTypes.Armor;
        }
#endif
    }
}