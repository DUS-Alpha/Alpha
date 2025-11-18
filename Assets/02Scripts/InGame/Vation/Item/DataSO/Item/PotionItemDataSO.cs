using UnityEngine;

namespace alpha
{
    [CreateAssetMenu(fileName = "PotionItemData", menuName = "Scriptable Objects/Item/Potion")]
    public class PotionItemDataSO : CountableItemDataSO
    {
        // 효과량(회복량 hp, mp, stemina 등)
        public float Value => m_value;
        [SerializeField] private float m_value;
        public override Item CreateItem()
        {
            return new PortionItem(this);
        }
    }
}