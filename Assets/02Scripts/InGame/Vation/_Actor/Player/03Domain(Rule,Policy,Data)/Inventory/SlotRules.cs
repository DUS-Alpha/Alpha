namespace alpha
{
    public class SlotRules
    {
        public bool CanAddItem(ISlotModel slotModel, Item item)
        {
            if (slotModel.SlotItemType != item.Data.ItemType)
                return false;

            return true;
        }

        // 제거불가 아이템 매개변수 조건으로
        public void CanRemove() 
        {
            
        }
    }
}