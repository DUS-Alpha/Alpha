using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace alpha
{
    public class SlotView : MonoBehaviour
    {
        // ==================== Config Data
        [Header("[ Slot Config ]")]
        [SerializeField] private RectTransform m_rectTr;
        [SerializeField] private Image m_slotIcon;
        [SerializeField] private TextMeshProUGUI m_countTMP;
        private EItemTypes m_slotItemType;

        // ==================== Runtime Data
        public SlotInfo SlotInfo { get; private set; }

        // ==================== State Data
        public bool IsEmpty { get; private set; }

        public event Action<SlotInfo> OnSetSlot;
        public event Action<string> OnSetItemCount;

        private void Awake()
        {
            m_slotIcon.gameObject.SetActive(false);
        }
        private void OnEnable()
        {
            OnSetSlot += SetSlotView;
            OnSetItemCount += SetItemCount;
        }

        private void OnDisable()
        {
            OnSetSlot -= SetSlotView;
            OnSetItemCount -= SetItemCount;
        }

        // 슬롯뷰 셋팅
        public void SetSlotView(SlotInfo slotInfo)
        {
            SlotInfo = slotInfo;
            if (slotInfo.IsEmpty) return;
            m_slotIcon.sprite = slotInfo.IconSprite;
            SetItemCount(slotInfo.ItemCountText);
            m_slotIcon.gameObject.SetActive(true);
        }

        public void SetItemCount(string countText)
        {
            m_countTMP.text = countText;
        }

        public void ClearSlot()
        {
            m_slotIcon.sprite = null;
            m_countTMP.text = "";
            IsEmpty = false;

            m_slotIcon.gameObject.SetActive(false);
        }

        #region ========== ICON DRAG AREA ==========
        private float m_xMin;
        public float XMin   // 좌
        {
            get
            {
                m_xMin = transform.position.x - m_rectTr.rect.width * 0.5f;
                return m_xMin;
            }
        }

        private float m_xMax;   // 우
        public float XMax
        {
            get
            {
                m_xMax = transform.position.x + m_rectTr.rect.width * 0.5f;
                return m_xMax;
            }
        }

        private float m_yMin;
        public float YMin
        {
            get
            {
                m_yMin = transform.position.y - m_rectTr.rect.height * 0.5f;
                return m_yMin;
            }
        }

        private float m_yMax;
        public float YMax
        {
            get
            {
                m_yMax = transform.position.y + m_rectTr.rect.height * 0.5f;
                return m_yMax;
            }
        }

        public bool IsInRect(Vector2 pos)
        {
            if (pos.x >= XMin && pos.x <= XMax && pos.y >= YMin && pos.y <= YMax)
                return true;

            return false;
        }
        #endregion ========== /Icon Area ==========
    }
}