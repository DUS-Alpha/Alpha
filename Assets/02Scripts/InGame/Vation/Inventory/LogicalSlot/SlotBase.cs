using System;
using Unity.Collections;
using UnityEngine;

namespace alpha
{
    public struct SlotInfo
    {
        public ItemDataSO ItemData;
        public int ItemCount;
    }

    // 슬롯의 기본 클래스
    public abstract class SlotBase : ISlotModel
    {
        public virtual EItemTypes SlotItemType { get; private set; }
        public int CurrentSlotIndex => m_SlotIndex;
        protected int m_SlotIndex;

        public ItemDataSO CurrentItemData => m_ItemData;
        protected ItemDataSO m_ItemData;

        public int CurrentItemCount => m_ItemCount;
        protected int m_ItemCount;

        public bool IsEmpty => CurrentItemData == null;

        protected SlotBase(int slotIndex)
        {
            m_SlotIndex = slotIndex;
        }

        // 해당 슬롯에 아이템 넣기
        public abstract bool CanAccept(ItemDataSO CurrentItemData);

        public void AddItem(ItemDataSO itemData)
        {
            if(m_ItemData == null)
                m_ItemData = itemData;
            m_ItemCount++;
        }

        public void StackItem(int stackCount)
        {
            m_ItemCount += stackCount;
        }

        public void SwapSlot(SlotInfo slotInfo)
        {
            m_ItemData = slotInfo.ItemData;
            m_ItemCount = slotInfo.ItemCount;
        }

        public void ClearSlot()
        {
            m_ItemData = null;
            m_ItemCount = 0;
        }

    }
}