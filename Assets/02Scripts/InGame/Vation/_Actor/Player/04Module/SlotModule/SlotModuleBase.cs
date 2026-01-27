using UnityEngine;
using UnityEngine.UI;

namespace alpha
{
    public struct SlotInfo
    {
        public Sprite IconSprite;
        public string ItemCountText;
        public int SlotIndex;
        public bool IsEmpty;
    }

    // 인벤토리의 실제 데이터
    public class SlotModuleBase : MonoBehaviour
    {
        // ==================== Ref Component


        // ==================== Config Data
        public virtual EItemTypes SlotItemType { get; set; }
        
        // ==================== Runtime Data
        private int m_slotIndex;
        public ItemSO ItemData { get; private set; }
        public int ItemCount { get; private set; }

        // ==================== State Data
        public bool IsEmpty => ItemData == null;

        public SlotModuleBase(int index)
        {
            SetIndex(index);
        }

        public void SetIndex(int index)
        {
            m_slotIndex = index;

            // View에 인덱스 전달 이벤트

        }

        public void AddItem(ItemSO itemData)
        {
            if(ItemData == null)
                ItemData = itemData;
            ItemCount++;

            // View
        }

        public void SwapSlot(SlotInfo slotInfo)
        {
            

            // View

        }

        public void ClearSlot()
        {
            ItemData = null;
            ItemCount = 0;

            // View
        }
    }
}