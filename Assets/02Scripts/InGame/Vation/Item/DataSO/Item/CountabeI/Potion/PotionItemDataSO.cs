using UnityEngine;

namespace alpha
{
    [CreateAssetMenu(fileName = "PotionItemData", menuName = "Scriptable Objects/Item/Potion")]
    public class PotionItemDataSO : CountableItemDataSO
    {
        // 채움량 (회복량 hp, mp, stemina 등)
        public int FillAmount => m_fillAmount;
        [SerializeField] private int m_fillAmount;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            CountableType = ECountableTypes.Potion;
        }
#endif


        public override Item CreateItem()
        {
            return new PortionItem(this);
        }

        public override void Used()
        {
            
        }
    }
}