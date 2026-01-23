using System;
using System.Collections.Generic;
using UnityEngine;

namespace alpha
{
    // 인벤토리 창 전체 상태 + 데이터 + 도메인 규칙
    // 데이터 저장 함수, 인벤토리가 가지는 규칙
    public class InventoryModel : IInventoryModel
    {
        private int m_verticalInventorySlotCount;        // 슬롯 세로 개수
        private int m_horizontalInventorySlotCount;      // 슬롯 가로 개수(인스펙터에서 간격에 의한 설정이기에)

        public List<InventorySlot> InventorySlotList;
        public event Action<InventorySlot> OnChanged;
        public InventoryModel(int n, int m)
        {
            // 슬롯 생성 및 초기화
            InventorySlotList = new List<InventorySlot>();

            m_verticalInventorySlotCount = n;
            m_horizontalInventorySlotCount = m;
        }

        public InventorySlot TryAddItem(ItemDataSO itemData)
        {
            foreach (InventorySlot slot in InventorySlotList)
            {
                if(slot.IsEmpty)
                {
                    slot.AddItem(itemData);
                    OnChanged?.Invoke(slot);
                    return slot;
                }
            }
            return null;
        }

        /// <summary>
        /// 인벤토리 확장(생성)
        /// </summary>
        /// <returns></returns>
        public InventorySlot ExpandSlot()
        {
            // TODO : 확장 가능 규칙

            // 확장
            int _n = m_verticalInventorySlotCount * m_horizontalInventorySlotCount;

            InventorySlotList.Add(new InventorySlot(InventorySlotList.Count));
            // Count 증가된 상태이기에 -1
            return InventorySlotList[InventorySlotList.Count - 1];
        }

        public bool ExecuteDragDrop(EItemTypes fromType, int fromIndex, EItemTypes toType, int toIndex)
        {
            if (fromType == toType && fromIndex == toIndex) return true;


            return false;
        }
    }
}