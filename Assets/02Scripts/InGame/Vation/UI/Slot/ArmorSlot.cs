using alpha;
using Unity.Collections;
using UnityEngine;

namespace alpha
{

    public class ArmorSlot : SlotBase
    {
        public EArmorTypes ArmorSlotType;

#if UNITY_EDITOR
        protected void OnValidate()
        {
            SlotType = ESlotTypes.Armor;
        }
#endif

        public override bool CanAcceptItem(ItemDataSO itemdata)
        {
            ArmorItemDataSO _armorItemData = itemdata as ArmorItemDataSO;
            if (_armorItemData == null) return false;

            if(_armorItemData.ArmorType == ArmorSlotType) return true;
            return false;
        }
    }
}