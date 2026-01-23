using UnityEngine;

namespace alpha
{
    public interface IInventoryService
    {
        bool AddItem(ItemSO itemData);
        void RemoveItem(ItemSO itemData);
        void ExpandSlot();
    }
}