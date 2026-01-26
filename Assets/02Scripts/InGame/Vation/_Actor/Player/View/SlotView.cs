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
        private int m_slotIndex;

        public event Action<EItemTypes, int> OnConnectedSlot;
        public event Action<SlotInfo> OnSetSlot;
        public event Action<int> OnSetItemCount;

        private void OnEnable()
        {
            OnConnectedSlot += ConnectedSlot;
            OnSetSlot += SetSlotView;
            OnSetItemCount += SetItemCount;
        }

        private void OnDisable()
        {
            OnConnectedSlot -= ConnectedSlot;
            OnSetSlot -= SetSlotView;
            OnSetItemCount -= SetItemCount;
        }

        public void ConnectedSlot(EItemTypes itemType, int slotIndex)
        {
            m_slotItemType = itemType;
            m_slotIndex = slotIndex;
        }

        public void SetSlotView(SlotInfo slotInfo)
        {
            m_slotIcon.sprite = slotInfo.ItemData.IconSprite;
            SetItemCount(slotInfo.ItemCount);

        }
        public void SetItemCount(int count)
        {
            m_countTMP.text = count.ToString();
        }

        public void ClearSlot()
        {
            m_slotIcon.sprite = null;
            m_countTMP.text = "";
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