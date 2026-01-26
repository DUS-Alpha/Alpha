namespace alpha
{
    public class SlotRules
    {
        public bool CanAddItem(ISlotModel slotModel, ItemModuleBase item)
        {
            // 아이템이 해당 슬롯에 접근 가능한지 판단
            if (slotModel.SlotItemType != item.Data.ItemType)
                return false;
            
            // 스택 가능한 아이템인지 판단


            return true;
        }

        // 제거불가 아이템 매개변수 조건으로
        public void CanRemove() 
        {
            
        }

    }
}