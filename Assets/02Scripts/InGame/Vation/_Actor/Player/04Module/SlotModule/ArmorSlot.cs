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

        public override bool CanAccept(ItemSO itemData)
        {
            if(itemData.ItemType == SlotItemType)
            {
                ArmorSO _armorData = itemData as ArmorSO;
                if(ArmorType == _armorData.ArmorType)
                    return true;
            }
            
            return false;
        }
    }
}
