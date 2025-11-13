using UnityEngine;


namespace alpha
{
    public abstract  class CountableItemDataSO : ItemDataSO
    {
        public int MaxAmount => m_maxAmount;
        [SerializeField] private int m_maxAmount = 99;
    }
}