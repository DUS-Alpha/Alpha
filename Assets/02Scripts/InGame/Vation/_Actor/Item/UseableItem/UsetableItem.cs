using UnityEngine;

namespace alpha {
    public abstract class UsetableItem : Item
    {
        public UseableItemDataSO CountableData => (UseableItemDataSO)Data;

        public void InitializeCountable(UseableItemDataSO data, int amount = 1)
        {
            SetAmount(amount);
        }

        // 현재 아이템 개수
        public int Amount { get; protected set; }

        // 하나의 슬롯이 가질 수 있는 최대 개수(기본 99)
        public int MaxAmount => CountableData.MaxAmount;

        public bool IsMax => Amount >= CountableData.MaxAmount;

        // 개수가 없는지 여부
        public bool IsEmpty => Amount <= 0;

        /// <summary> 개수 지정(범위 제한) </summary>
        public void SetAmount(int amount)
        {
            Amount = Mathf.Clamp(amount, 0, MaxAmount);
        }
    }
}