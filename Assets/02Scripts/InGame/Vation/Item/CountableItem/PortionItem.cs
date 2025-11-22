using UnityEngine;

namespace alpha
{
    public class PortionItem : CountableItem
    {
        public PotionItemDataSO PotionItemData { get; private set; }
        public PortionItem(PotionItemDataSO data, int amount = 1) : base(data, amount)
        {
            PotionItemData = data;
        }

        public bool Use()
        {
            // 임시 : 개수 하나 감소
            Amount--;

            return true;
        }

        protected override CountableItem Clone(int amount)
        {
            return new PortionItem(CountableData as PotionItemDataSO, amount);
        }
    }
}