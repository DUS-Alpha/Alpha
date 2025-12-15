using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace alpha
{
    public class DragSlot : SlotBase
    {
#if UNITY_EDITOR
        protected void OnValidate()
        {
            SlotType = ESlotTypes.Drag;
        }
#endif
        public override void ApplySlotInfo(int slotNum, Sprite icon, int itemCount, ItemDataSO itemData)
        {
            base.ApplySlotInfo(slotNum, icon, itemCount, itemData);
        }
        public override bool CanAcceptItem(ItemDataSO itemData)
        {
            return true;
        }
    }
}