using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace alpha
{
    public class InventorySlot : SlotBase
    {
#if UNITY_EDITOR
        private void OnValidate()
        {
            SlotType = ESlotTypes.Inventory;
        }
#endif

        // 인벤토리슬롯은 모든 아이템 수용가능
        public override bool CanAcceptItem(ItemDataSO item)
        {
            return true;
        }
    }
}