using UnityEngine;

namespace alpha {
    public class UseableQuickSlot : SlotBase
    {
        public UseableQuickSlot(int slotIndex) : base(slotIndex){}

        public override EItemTypes SlotItemType => EItemTypes.Useable;

        public override bool CanAccept(ItemDataSO CurrentItemData)
        {
            if(SlotItemType == SlotItemType) return true;
            return false;
        }
    }
}