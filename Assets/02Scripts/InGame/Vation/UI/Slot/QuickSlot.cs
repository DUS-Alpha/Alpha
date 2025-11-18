using UnityEngine;

namespace alpha
{
    public class QuickSlot : SlotBase
    {
#if UNITY_EDITOR
        private void OnValidate()
        {
            SlotType = ESlotTypes.Quick;
        }
#endif
        public override bool CanAcceptItem(ItemDataSO itemdata)
        {
            if (itemdata.ItemType == EItemTypes.ConuntableItem) return true;
            return false;
        }
    }
}