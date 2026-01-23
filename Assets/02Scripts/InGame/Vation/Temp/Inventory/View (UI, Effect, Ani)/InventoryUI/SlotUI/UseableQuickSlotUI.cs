using UnityEngine;

namespace alpha
{
    public class UseableQuickSlotUI : SlotUIBase
    {
#if UNITY_EDITOR
        private void OnValidate()
        {
            SlotItemType = EItemTypes.Useable;
        }
#endif
    }
}