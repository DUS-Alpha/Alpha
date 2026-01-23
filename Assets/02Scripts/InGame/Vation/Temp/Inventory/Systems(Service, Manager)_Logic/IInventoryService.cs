using UnityEngine;

namespace alpha
{
    public interface IInventoryService
    {
        bool AddItem(ItemDataSO itemData);
        void RemoveItem(ItemDataSO itemData);
        void ExpandSlot();
    }
}