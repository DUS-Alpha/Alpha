using NUnit.Framework.Interfaces;
using UnityEngine;

namespace alpha
{
    public abstract class Item : MonoBehaviour
    {
        public ItemDataSO Data => m_data;
        protected ItemDataSO m_data;

        public void Initialize(ItemDataSO data) => m_data = data;
    }
}