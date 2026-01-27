using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace alpha
{
    public class DragSlotView : MonoBehaviour
    {
        [SerializeField] private GameObject m_backGO;   //백그라운드
        [SerializeField] private Image m_icon;

        public SlotInfo SlotInfo { get; private set; }  // 정보 전달
        private void Awake()
        {
            m_backGO.SetActive(false);
        }

        public void SetSlotView(SlotInfo slotInfo)
        {
            SlotInfo = slotInfo;

            m_icon.sprite = slotInfo.IconSprite;
            m_backGO.SetActive(true);
        }

        public void Clear()
        {
            m_backGO.SetActive(false);
        }
    }
}