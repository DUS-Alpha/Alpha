using UnityEngine;

namespace alpha
{
    public interface ISlotModel
    {
        public EItemTypes SlotItemType { get; }
        public int CurrentSlotIndex { get; }
        public int CurrentItemCount { get; }
        public ItemSO CurrentItemData { get; }
        public bool IsEmpty { get; }

        public bool CanAccept(ItemSO currentItemData);
        public void AddItem(ItemSO itemData);
        public void SwapSlot(SlotInfo slotInfo);
        public void ClearSlot();
    }
}