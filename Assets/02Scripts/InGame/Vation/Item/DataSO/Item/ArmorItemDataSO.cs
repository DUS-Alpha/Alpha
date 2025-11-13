using alpha;
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
        Gloves,
    }
    public abstract class ArmorItemDataSO : EquipmentItemDataSO
    {
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            EquipmentType = EEquipmentTypes.Armor;
        }
#endif
    }
}