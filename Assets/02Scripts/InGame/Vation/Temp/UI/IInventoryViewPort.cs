using System;
using UnityEngine;

namespace alpha
{
    public interface IInventoryViewPort
    {
        public event Action OnClickAddInventory;
        
        public void AddAndBindInventorySlotUI(int i);
        public void UpdateSlotUI(ISlotModel slotView);

        public event Func<SlotUIBase, SlotUIBase, bool> OnDragDrop;

        public event Action<ItemSO> OnEquipRequest;
        public event Action<ItemSO> OnUnEquipRequest;
        public event Action<ItemSO> OnUseRequest;
        public event Action<ItemSO> OnDropRequest;
    }
}