using UnityEngine;

namespace alpha
{
    [CreateAssetMenu(fileName = "PotionItemData", menuName = "Scriptable Objects/CurrentItemData/Potion")]
    public class PotionItemDataSO : UseableSO
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
    }
}