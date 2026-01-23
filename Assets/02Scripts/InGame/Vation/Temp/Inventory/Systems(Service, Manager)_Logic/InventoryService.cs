using UnityEngine;

namespace alpha
{
    public class InventoryService : IInventoryService
    {
        private InventoryModel m_inventoryModel;

        public InventoryService(InventoryModel inventoryModel)
        {
            m_inventoryModel = inventoryModel;
        }

        public bool AddItem(ItemDataSO itemData)
        {
           var _resultSlot =  m_inventoryModel.TryAddItem(itemData);

            if(_resultSlot == null)return false;
            return true;
        }

        public void ExpandSlot()
        {
            throw new System.NotImplementedException();
        }

        public void RemoveItem(ItemDataSO itemData)
        {
            throw new System.NotImplementedException();
        }
    }
}