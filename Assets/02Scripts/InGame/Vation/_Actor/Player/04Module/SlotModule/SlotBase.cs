namespace alpha
{
    public struct SlotInfo
    {
        public ItemSO ItemData;
        public int ItemCount;
    }

    // 인벤토리의 실제 데이터
    public abstract class SlotBase : ISlotModel
    {
        public virtual EItemTypes SlotItemType { get; private set; }
        public int CurrentSlotIndex { get; private set; }

        public ItemSO CurrentItemData { get; private set; }

        public int CurrentItemCount { get; private set; }

        public bool IsEmpty => CurrentItemData == null;

        protected SlotBase(int slotIndex)
        {
            CurrentSlotIndex = slotIndex;
        }

        // 해당 슬롯에 아이템 넣기
        public abstract bool CanAccept(ItemSO CurrentItemData);

        public void AddItem(ItemSO itemData)
        {
            if(CurrentItemData == null)
                CurrentItemData = itemData;
            CurrentItemCount++;
        }

        public void StackItem(int stackCount)
        {
            CurrentItemCount += stackCount;
        }

        public void SwapSlot(SlotInfo slotInfo)
        {
            CurrentItemData = slotInfo.ItemData;
            CurrentItemCount = slotInfo.ItemCount;
        }

        public void ClearSlot()
        {
            CurrentItemData = null;
            CurrentItemCount = 0;
        }
    }
}