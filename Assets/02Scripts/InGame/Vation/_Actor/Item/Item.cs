using NUnit.Framework.Interfaces;
using UnityEngine;

namespace alpha
{
    public abstract class Item : MonoBehaviour
    {
        public ItemSO Data => m_data;
        protected ItemSO m_data;
        [SerializeField] private bool m_isCountable;
        public bool IsCountable => m_isCountable;

        public void Initialize(ItemSO data) => m_data = data;
    }
}