using UnityEngine;

namespace alpha
{
    [CreateAssetMenu(fileName = "PortionItemData", menuName = "Scriptable Objects/Item/Portion")]
    public class PortionItemDataSO : CountableItemDataSO
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