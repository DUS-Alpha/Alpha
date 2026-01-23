using UnityEngine;

namespace alpha
{
    public class PickupController
    {
        IInventoryService m_inventoryService;

        public void Inject(IInventoryService inventoryService)
        {
            m_inventoryService = inventoryService;
        }

        public void Pickup(ItemSO itemData)
        {
            m_inventoryService.AddItem(itemData);
        }
    }
}