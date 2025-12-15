using UnityEngine;

namespace alpha
{
    public class PotionItem : CountableItem
    {
        public PotionItemDataSO PotionData => (PotionItemDataSO)Data;
        
        public bool Use()
        {
            // 임시 : 개수 하나 감소
            Amount--;

            return true;
        }
    }
}