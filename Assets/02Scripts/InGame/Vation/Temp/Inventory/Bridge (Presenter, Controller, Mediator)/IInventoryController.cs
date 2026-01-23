using System;
using UnityEngine;

namespace alpha
{
    public interface IInventoryController
    {
        // 인벤토리슬롯 확장
        public ISlotModel ExpandInventorySlot();

        /*// 인벤토리에 아이템 추가
        public event Action<ISlotModel> OnUpdateSlotUI;

        public bool ExecuteDragDrop(EItemTypes fromType, int fromIndex, EItemTypes toType, int toIndex);

        public void DestoryItem(ItemDataSO itemData);

        // Equip
        public event Action<ItemDataSO> OnEquipItem;
        public event Action<ItemDataSO> OnUnEquipItem;*/
    }
}