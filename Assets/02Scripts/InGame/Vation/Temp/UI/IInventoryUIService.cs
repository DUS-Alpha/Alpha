using System;
using UnityEngine;

namespace alpha
{
    public interface IInventoryUIService
    {
        public event Action OnClickAddInventory;
        
        public void AddAndBindInventorySlotUI(int i);
        public void UpdateSlotUI(ISlotModel slotView);

        public event Func<SlotUIBase, SlotUIBase, bool> OnDragDrop;

        public event Action<ItemDataSO> OnEquipRequest;
        public event Action<ItemDataSO> OnUnEquipRequest;
        public event Action<ItemDataSO> OnUseRequest;
        public event Action<ItemDataSO> OnDropRequest;
    }
}