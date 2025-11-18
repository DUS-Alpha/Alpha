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

        public override bool CanAcceptItem(ItemDataSO itemdata)
        {
            return true;
        }
    }
}