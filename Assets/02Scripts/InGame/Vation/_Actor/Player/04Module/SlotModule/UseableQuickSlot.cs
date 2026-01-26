using UnityEngine;

namespace alpha {
    public class UseableQuickSlot : SlotModuleBase
    {
        public UseableQuickSlot(int slotIndex) : base(slotIndex){}

        public override EItemTypes SlotItemType => EItemTypes.Useable;

        /*public override bool CanAccept(ItemSO CurrentItemData)
        {
            if(SlotItemType == SlotItemType) return true;
            return false;
        }*/
    }
}