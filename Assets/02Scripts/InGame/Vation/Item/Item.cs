using NUnit.Framework.Interfaces;
using UnityEngine;

namespace alpha
{
    public abstract class Item : MonoBehaviour
    {
        public ItemDataSO Data => m_data;
        protected ItemDataSO m_data;

        public void Initialize(ItemDataSO data) => m_data = data;

        public Item CreateItem(Transform parent)
        {
            var itemObj = Instantiate(m_data.ItemPrefab, parent);
            var item = itemObj.GetComponent<Item>();
            item.Initialize(m_data); // 여기서 데이터 세팅
            return item;
        }
    }
}