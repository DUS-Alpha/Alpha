using System;
using UnityEngine;

namespace alpha
{
    public interface IInventoryModel
    {
        public event Action<InventorySlot> OnChanged;

        InventorySlot TryAddItem(ItemDataSO itemData);
        InventorySlot ExpandSlot();
        bool ExecuteDragDrop(EItemTypes fromType, int fromIndex, EItemTypes toType, int toIndex);
    }
}