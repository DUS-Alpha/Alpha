using UnityEngine;

namespace alpha
{
    public interface ISlotModel
    {
        public EItemTypes SlotItemType { get; }
        public int CurrentSlotIndex { get; }
        public int CurrentItemCount { get; }
        public ItemDataSO CurrentItemData { get; }
        public bool IsEmpty { get; }

        public bool CanAccept(ItemDataSO currentItemData);
        public void AddItem(ItemDataSO itemData);
        public void SwapSlot(SlotInfo slotInfo);
        public void ClearSlot();
    }
}