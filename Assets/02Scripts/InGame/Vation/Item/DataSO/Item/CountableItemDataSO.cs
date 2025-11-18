using UnityEngine;
using Unity.Collections;

namespace alpha
{
    public abstract  class CountableItemDataSO : ItemDataSO
    {
        public int MaxAmount => m_maxAmount;
        [SerializeField] private int m_maxAmount = 99;

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            ItemType = EItemTypes.ConuntableItem;
        }
#endif
    }
}