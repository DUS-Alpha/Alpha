using UnityEngine;

namespace alpha
{
    public class PlayerInventorySystem : MonoBehaviour
    {
        /// <summary> 아이템 수용 한도 </summary>
        public int Capacity { get; private set; }

        // 초기 수용 한도
        [SerializeField, Range(8, 64)]
        private int m_initalCapacity = 32;

        // 최대 수용 한도(아이템 배열 크기)
        [SerializeField, Range(8, 64)]
        private int m_maxCapacity = 64;

        [SerializeField]
        private InventoryUI m_inventoryUI; // 연결된 인벤토리 UI

        /// <summary> 아이템 목록 </summary>
        [SerializeField]
        private Item[] _items;


    }
}