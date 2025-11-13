using UnityEngine;

namespace alpha {
    public abstract class CountableItem : Item
    {
        public CountableItemDataSO CountableData { get; private set; }

        // 현재 아이템 개수
        public int Amount { get; protected set; }

        // 하나의 슬롯이 가질 수 있는 최대 개수(기본 99)
        public int MaxAmount => CountableData.MaxAmount;

        public bool IsMax => Amount >= CountableData.MaxAmount;

        // 개수가 없는지 여부
        public bool IsEmpty => Amount <= 0;

        public CountableItem(CountableItemDataSO data, int amount = 1) : base(data)
        {
            CountableData = data;
            SetAmount(amount);
        }

        /// <summary> 개수 지정(범위 제한) </summary>
        public void SetAmount(int amount)
        {
            Amount = Mathf.Clamp(amount, 0, MaxAmount);
        }

        /// <summary> 개수 추가 및 최대치 초과량 반환(초과량 없을 경우 0) </summary>
        public int AddAmountAndGetExcess(int amount)
        {
            int _nextAmount = Amount + amount;
            SetAmount(_nextAmount);

            return (_nextAmount > MaxAmount) ? (_nextAmount - MaxAmount) : 0;
        }

        /// <summary> 수량을 가질 수 있는 아이템 개수를 나누어 복제 </summary>
        public CountableItem SeperateAndClone(int amount)
        {
            // 수량이 한개 이하일 경우, 복제 불가
            if (Amount <= 1) return null;

            // 수량을 분리하는것이기에 원본에는 하나의 개수는 존재해야함 그래서 -1
            if (amount > Amount - 1) amount = Amount - 1;

            // 원본 수량에서 분리 수량만큼 감소
            Amount -= amount;
            return Clone(amount);
        }
        protected abstract CountableItem Clone(int amount);
    }
}