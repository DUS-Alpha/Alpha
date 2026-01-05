using UnityEngine;

namespace alpha
{
    // 방어구 슬롯
    public class ArmorSlot : SlotBase
    {
        // new는 SlotBase의 SlotType를 숨김
        public override EItemTypes SlotItemType => EItemTypes.Armor;
        public EArmorTypes ArmorType;

        public ArmorSlot(int slotIndex, EArmorTypes armorType) : base(slotIndex)
        {
            ArmorType = armorType;
        }

        public override bool CanAccept(ItemDataSO itemData)
        {
            if(itemData.ItemType == SlotItemType)
            {
                ArmorItemDataSO _armorData = itemData as ArmorItemDataSO;
                if(ArmorType == _armorData.ArmorType)
                    return true;
            }
            
            return false;
        }
    }
}
