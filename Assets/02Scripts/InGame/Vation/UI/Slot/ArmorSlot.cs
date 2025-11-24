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
        public override void ApplySlotInfo(int slotNum, Sprite icon, int itemCount, ItemDataSO itemData)
        {
            base.ApplySlotInfo(slotNum, icon, itemCount, itemData);
        }
        public override bool CanAcceptItem(ItemDataSO itemData)
        {
            ArmorItemDataSO _armorItemData = itemData as ArmorItemDataSO;
            if (_armorItemData == null) return false;

            if(_armorItemData.ArmorType == ArmorSlotType) return true;
            return false;
        }
    }
}