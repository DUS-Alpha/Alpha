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
        public override void ApplySlotInfo(int slotNum, Sprite icon, int itemCount, ItemDataSO info)
        {
            base.ApplySlotInfo(slotNum, icon, itemCount, info);
        }
        public override bool CanAcceptItem(ItemDataSO itemdata)
        {
            return true;
        }
    }
}